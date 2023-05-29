// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: UserProfile.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using UnityEngine;
using Appearition.API;
using Appearition.Internal.EndPoints;
using System.Linq;
using Appearition.AccountAndAuthentication;

namespace Appearition.Profile
{
    /// <inheritdoc />
    /// <summary>
    /// Basic user that contains all the ApiData for storing purpose.
    /// </summary>
    [System.Serializable]
    [CreateAssetMenu(menuName = "Appearition/Create Basic User")]
    public class UserProfile : ScriptableObject
    {
        //Serialized login ApiData
        /// <summary>
        /// Nickname that can be used for profile selection.
        /// </summary>
        public string nickname;

        /// <summary>
        /// User main ID. Often is the same as user email.
        /// </summary>
        public string username;
        public string firstName;
        public string lastName;
        public string emailAddress;

        /// <summary>
        /// EMS portal token. Can be found on the portal under Settings>Developer after selecting a tenant.
        /// </summary>
        public string applicationToken;

        /// <summary>
        /// Contains all the tokens currently active on this profile.
        /// If your application uses external cloud authentication with other tokens, feel free to store them here with a key of your choice.
        /// </summary>
        public Dictionary<string, string> authenticationTokens = new Dictionary<string, string>();

        /// <summary>
        /// Storage for the end point URL. Also used to fetch the selected end point.
        /// </summary>
        public string selectedEndPointName;

        /// <summary>
        /// The tenant index selected by the user.
        /// </summary>
        public string selectedTenant;

        /// <summary>
        /// Contains all the tenants available for this profile.
        /// Do note that by default, it only contains the selected tenant.
        /// If you want to see all tenants, trigger the AccountHandler.GetProfileData() request, and this list will be refreshed upon completion.
        /// </summary>
        public List<TenantData> allTenantsAvailable = new List<TenantData>();

        /// <summary>
        /// The product ID selected by the user. This corresponds to the channel ID.
        /// </summary>
        public int selectedChannel;

        #region Request Utilities

        #region Token

        /// <summary>
        /// Defines whether or not the user has logged in and is in the middle of a session. 
        /// </summary>
        public bool IsUserLoggedIn
        {
            get { return !string.IsNullOrEmpty(SessionToken); }
        }

        /// <summary>
        /// Session Token, used by a logged-in user to execute requests which require a higher level of authentication.
        /// </summary>
        public string SessionToken
        {
            get
            {
                if (authenticationTokens.ContainsKey(AppearitionConstants.PROFILE_SESSION_TOKEN_NAME))
                    return authenticationTokens[AppearitionConstants.PROFILE_SESSION_TOKEN_NAME];
                return "";
            }
        }

        /// <summary>
        /// Fetches the most relevant Authentication Token, regardless the user's login state.
        /// </summary>
        public string GetHighestLevelAuthenticationToken
        {
            get
            {
                if (!string.IsNullOrEmpty(SessionToken))
                    return SessionToken;
                return applicationToken;
            }
        }

        /// <summary>
        /// Adds or creates a token entry based on a token name (key) and a token value (authentication token).
        /// </summary>
        /// <param name="tokenName"></param>
        /// <param name="token"></param>
        public void AddOrModifyAuthenticationToken(string tokenName, string token)
        {
            //if (authenticationTokens.ContainsKey(tokenName))
            //    authenticationTokens[tokenName] = token;
            //else
            //    authenticationTokens.Add(tokenName, token);

            if (authenticationTokens.ContainsKey(tokenName))
                authenticationTokens.Remove(tokenName);
            authenticationTokens.Add(tokenName, token);
        }

        /// <summary>
        /// Tries to find a matching token in the currently loaded tokens and remove said entry.
        /// </summary>
        /// <param name="token"></param>
        public void RemoveTokenByValueIfExisting(string token)
        {
            string key = authenticationTokens.FirstOrDefault(o => o.Value == token).Key;

            if (!string.IsNullOrEmpty(key))
                authenticationTokens.Remove(key);
        }

        /// <summary>
        /// Tries to find an entry with a matching token key (not the actual token, but the dictionary key), and removes it.
        /// </summary>
        /// <param name="tokenKey"></param>
        public void RemoveTokenByNameIfExisting(string tokenKey)
        {
            if (!string.IsNullOrEmpty(tokenKey) && authenticationTokens.ContainsKey(tokenKey))
                authenticationTokens.Remove(tokenKey);
        }

        #endregion

        #region Tenant

        /// <summary>
        /// Returns a list of all the roles this authenticated user has access to in the current tenant.
        /// </summary>
        /// <returns></returns>
        public List<string> GetAvailableRolesForSelectedTenant()
        {
            if (allTenantsAvailable == null || allTenantsAvailable.Count == 0)
                return new List<string>();

            TenantData tmpTenant = allTenantsAvailable.FirstOrDefault(o => o.tenantKey.Equals(selectedTenant, StringComparison.InvariantCultureIgnoreCase));

            if (tmpTenant == null || tmpTenant.roles == null)
                return new List<string>();
            return tmpTenant.roles;
        }

        /// <summary>
        /// Whether this authenticated user has a given role.
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public bool ContainsRoleForSelectedTenant(string role)
        {
            if (allTenantsAvailable == null || allTenantsAvailable.Count == 0)
                return false;

            TenantData tmpTenant = allTenantsAvailable.FirstOrDefault(o => o.tenantKey.Equals(selectedTenant, StringComparison.InvariantCultureIgnoreCase));

            if (tmpTenant == null || tmpTenant.roles == null)
                return false;
            return tmpTenant.roles.Any(o => o.Equals(role, StringComparison.InvariantCultureIgnoreCase));
        }

        #endregion

        /// <summary>
        /// Returns the user-selected end point URL.
        /// </summary>
        /// <value>The selected end point.</value>
        public EndPoint SelectedEndPoint
        {
            get { return EndPointUtility.GetEndPointFromDisplayName(selectedEndPointName); }
        }

        /// <summary>
        /// Gets the authentication headers for WebRequest used during API calls according to user-selected way of authentication.
        /// </summary>
        /// <value>The get authentication headers.</value>
        public Dictionary<string, string> GetAuthenticationHeaders(AuthenticationOverrideType requiredType, BaseApi requestSample = null)
        {
            Dictionary<string, string> output = new Dictionary<string, string>();

            switch (requiredType)
            {
                case AuthenticationOverrideType.None:
                    output.Add("authentication-token", GetHighestLevelAuthenticationToken);
                    break;

                case AuthenticationOverrideType.ApplicationToken:
                    output.Add("authentication-token", applicationToken);
                    break;

                case AuthenticationOverrideType.SessionToken:
                    output.Add("authentication-token", SessionToken);
                    break;

                case AuthenticationOverrideType.Custom:
                    if (requestSample == null)
                        AppearitionLogger.LogError("A custom token was requested but the provided request is null.");
                    else if (!authenticationTokens.ContainsKey(requestSample.AuthenticationTokenCustomKey))
                        AppearitionLogger.LogError("A custom token was requested but the profile does not contain a matching key. Key required: " + requestSample.AuthenticationTokenCustomKey);
                    else
                        output.Add("authentication-token", authenticationTokens[requestSample.AuthenticationTokenCustomKey]);
                    break;
            }

            return output;
        }

        #endregion

        #region Constructors 

        /// <summary>
        /// Pseudo copy constructor. Creates a runtime copy from a given user profile.
        /// </summary>
        /// <param name="cc"></param>
        /// <returns></returns>
        public static T CreateCopy<T>(UserProfile cc) where T : UserProfile
        {
            T newUser = CreateInstance<T>();
            newUser.nickname = cc.nickname;
            newUser.selectedEndPointName = cc.selectedEndPointName;
            newUser.selectedTenant = cc.selectedTenant;
            newUser.allTenantsAvailable = new List<TenantData> {new TenantData {tenantKey = cc.selectedTenant}};
            newUser.applicationToken = cc.applicationToken;
            newUser.authenticationTokens = new Dictionary<string, string>
                {{AppearitionConstants.PROFILE_APPLICATION_TOKEN_NAME, cc.applicationToken}};
            newUser.selectedChannel = cc.selectedChannel;
            return newUser;
        }

        #endregion
    }
}