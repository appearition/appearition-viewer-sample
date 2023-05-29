// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: AccountHandler.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using System.Collections;
using UnityEngine;
using System;
using System.Collections.Generic;
using Appearition.AccountAndAuthentication.API;
using Appearition.Common;

namespace Appearition.AccountAndAuthentication
{
    /// <summary>
    /// Handler in charge of taking care of any Account or portal related operations. 
    /// Offers an easy to use access login and profile handling system.
    /// </summary>
    public sealed partial class AccountHandler : BaseHandler
    {
        #region Login Handling 

        /// <summary>
        /// Tries to login to the EMS with the credentials stored in the current profile. Do note that this is an optional step to access other requests.
        ///
        /// API Requirement: Application Token.
        /// </summary>
        /// <param name="username">The username entered by the user.</param>
        /// <param name="password">The password entered by the user.</param>
        /// <param name="onSuccess">Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void Login(string username, string password, Action onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(LoginProcess(username, password, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Tries to login to the EMS with the credentials stored in the current profile. Do note that this is an optional step to access other requests.
        ///
        /// API Requirement: Application Token.
        /// </summary>
        /// <param name="username">The username entered by the user.</param>
        /// <param name="password">The password entered by the user.</param>
        /// <param name="onSuccess">Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator LoginProcess(string username, string password, Action onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            //Online request
            var authenticateParam = new Account_Authenticate.PostData() {
                username = username,
                password = password,
                #if UNITY_WEBGL || UNITY_STANDALONE_WIN
                appId = AppearitionGate.Instance.appBundleIdentifier
                #else
                appId = Application.identifier
                #endif
            };

            var authenticateRequest = AppearitionRequest<Account_Authenticate>.LaunchAPICall_POST<AccountHandler>
                (AppearitionGate.Instance.CurrentUser.selectedChannel, GetReusableApiRequest<Account_Authenticate>(), authenticateParam);

            while (!authenticateRequest.IsDone)
                yield return null;

            //Debug result
            if (authenticateRequest.RequestResponseObject.IsSuccess)
            {
                AppearitionGate.Instance.CurrentUser.username = username;
                AppearitionLogger.LogInfo("Authentication success !");
                if (!string.IsNullOrEmpty(authenticateRequest.RequestResponseObject.Data.sessionToken))
                    AppearitionGate.Instance.CurrentUser.AddOrModifyAuthenticationToken(AppearitionConstants.PROFILE_SESSION_TOKEN_NAME, authenticateRequest.RequestResponseObject.Data.sessionToken);

                //Upon success, get my profile data
                yield return GetProfileDataProcess();
            }
            else
                AppearitionLogger.LogError("Authentication Failure.");

            //Finally, callback
            if (onSuccess != null && authenticateRequest.RequestResponseObject.IsSuccess)
                onSuccess();

            if (onFailure != null && authenticateRequest.RequestResponseObject.Errors != null && authenticateRequest.RequestResponseObject.Errors.Length > 0)
                onFailure(authenticateRequest.Errors);

            if (onComplete != null)
                onComplete(authenticateRequest.RequestResponseObject.IsSuccess);
        }

        /// <summary>
        /// Using the information stored in the current user and its session token, ends the current session with the EMS and resets token to its global one.
        /// 
        /// API Requirement: Session Token.
        /// </summary>
        /// <param name="revertUserToDefault">Whether or not all the </param>
        /// <param name="onSuccess">Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void Logout(bool revertUserToDefault, Action onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(LogoutProcess(revertUserToDefault, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Using the information stored in the current user and its session token, ends the current session with the EMS and resets token to its global one.
        /// 
        /// API Requirement: Session Token.
        /// </summary>
        /// <param name="revertUserToDefault"></param>
        /// <param name="onSuccess">Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator LogoutProcess(bool revertUserToDefault, Action onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            if (!AppearitionGate.Instance.CurrentUser.authenticationTokens.ContainsKey(AppearitionConstants.PROFILE_SESSION_TOKEN_NAME) ||
                AppearitionGate.Instance.CurrentUser.SessionToken.Equals(AppearitionGate.Instance.CurrentUser.applicationToken))
            {
                AppearitionLogger.LogWarning("Tried to log out without any session token.");
                onFailure?.Invoke(new EmsError("Tried to log out without any session token."));
                onComplete?.Invoke(false);
                AppearitionGate.Instance.CurrentUser.RemoveTokenByNameIfExisting(AppearitionConstants.PROFILE_SESSION_TOKEN_NAME);
                yield break;
            }

            //Online request
            var logoutRequest = AppearitionRequest<Account_Logout>.LaunchAPICall_GET<AccountHandler>
                (AppearitionGate.Instance.CurrentUser.selectedChannel, GetReusableApiRequest<Account_Logout>());

            while (!logoutRequest.IsDone)
                yield return null;

            //Reset the token to the global one, regardless of the outcome.
            //If the logout fails, it most likely means that the token was already expired on the EMS anyway.

            //Reset to default
            if (revertUserToDefault)
                AppearitionGate.Instance.RevertCurrentUserToDefault();
            else
            {
                if (AppearitionGate.Instance.CurrentUser.authenticationTokens.ContainsKey(AppearitionConstants.PROFILE_SESSION_TOKEN_NAME))
                    AppearitionGate.Instance.CurrentUser.authenticationTokens.Remove(AppearitionConstants.PROFILE_SESSION_TOKEN_NAME);
                AppearitionGate.Instance.CurrentUser.username = "";
            }

            if (logoutRequest.RequestResponseObject.IsSuccess)
            {
                AppearitionLogger.LogInfo("User logged out successfully. Session token: " +
                                          AppearitionGate.Instance.CurrentUser.SessionToken +
                                          ", Application Token: " + AppearitionGate.Instance.CurrentUser.applicationToken);

                if (onSuccess != null)
                    onSuccess();
            }
            else
            {
                AppearitionLogger.LogError("User was unable to log out.");
                if (onFailure != null)
                    onFailure(logoutRequest.Errors);
            }

            if (onComplete != null)
                onComplete(logoutRequest.RequestResponseObject.IsSuccess);
        }

        #endregion

        #region Profile Handling 

        /// <summary>
        /// Fetches all the information related to the profile of a logged in user.
        ///
        /// API Requirement: Application Token.
        /// </summary>
        /// <param name="onSuccess">Contains the profile data as on the EMS. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void GetProfileData(Action<ExtendedProfile> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(GetProfileDataProcess(onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Fetches all the information related to the profile of a logged in user.
        ///
        /// API Requirement: Application Token.
        /// </summary>
        /// <param name="onSuccess">Contains the profile data as on the EMS. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator GetProfileDataProcess(Action<ExtendedProfile> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            //Online request
            var myProfileRequest = AppearitionRequest<Account_MyProfile>.LaunchAPICall_GET<AccountHandler>
                (AppearitionGate.Instance.CurrentUser.selectedChannel, GetReusableApiRequest<Account_MyProfile>());

            while (!myProfileRequest.IsDone)
                yield return null;

            //Debug result
            if (myProfileRequest.RequestResponseObject.IsSuccess)
            {
                AppearitionLogger.LogInfo("Profile content successfully fetched!");

                //Update the tenants
                AppearitionGate.Instance.CurrentUser.username = myProfileRequest.RequestResponseObject.Data.emailAddress;
                AppearitionGate.Instance.CurrentUser.firstName = myProfileRequest.RequestResponseObject.Data.firstName;
                AppearitionGate.Instance.CurrentUser.lastName = myProfileRequest.RequestResponseObject.Data.lastName;
                AppearitionGate.Instance.CurrentUser.emailAddress = myProfileRequest.RequestResponseObject.Data.emailAddress;

                AppearitionGate.Instance.CurrentUser.allTenantsAvailable.Clear();
                AppearitionGate.Instance.CurrentUser.allTenantsAvailable.AddRange(myProfileRequest.RequestResponseObject.Data.tenants);
            }
            else
                AppearitionLogger.LogError("An issue occured when trying to fetch the profile settings. " + myProfileRequest.Errors);

            //Finally, callback
            if (onSuccess != null && myProfileRequest.RequestResponseObject.IsSuccess)
                onSuccess(myProfileRequest.RequestResponseObject.Data);

            if (onFailure != null && myProfileRequest.RequestResponseObject.Errors != null && myProfileRequest.RequestResponseObject.Errors.Length > 0)
                onFailure(myProfileRequest.Errors);

            if (onComplete != null)
                onComplete(myProfileRequest.RequestResponseObject.IsSuccess);
        }

        /// <summary>
        /// Updates the information of the logged in user. Should be used to update name, email or any custom attribute.
        /// If wanting to remove an attribute, do set said attribute doDeleteAttribute to true.
        ///
        /// API Requirement: Application Token.
        /// </summary>
        /// <param name="profileData">The user's profile as on the EMS. Can be fetched using GetProfileData.</param>
        /// <param name="onSuccess">Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void UpdateProfileSettings(Profile profileData, Action onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(UpdateProfileSettingsProcess(profileData, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Updates the information of the logged in user. Should be used to update name, email or any custom attribute.
        /// If wanting to remove an attribute, do set said attribute doDeleteAttribute to true.
        ///
        /// API Requirement: Application Token.
        /// </summary>
        /// <param name="profileData">The user's profile as on the EMS. Can be fetched using GetProfileData.</param>
        /// <param name="onSuccess">Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator UpdateProfileSettingsProcess(Profile profileData, Action onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            //Online request
            var updateMyProfileRequest = AppearitionRequest<Account_UpdateMyProfile>.LaunchAPICall_POST<AccountHandler>
                (AppearitionGate.Instance.CurrentUser.selectedChannel, GetReusableApiRequest<Account_UpdateMyProfile>(), profileData);

            while (!updateMyProfileRequest.IsDone)
                yield return null;

            //Debug result
            if (updateMyProfileRequest.RequestResponseObject.IsSuccess)
            {
                AppearitionLogger.LogInfo("Profile data updated successfully!");
            }
            else
                AppearitionLogger.LogError("An issue occured when trying to update the profile settings. " + updateMyProfileRequest.Errors);

            //Finally, callback
            if (onSuccess != null && updateMyProfileRequest.RequestResponseObject.IsSuccess)
                onSuccess();

            if (onFailure != null && updateMyProfileRequest.RequestResponseObject.Errors != null && updateMyProfileRequest.RequestResponseObject.Errors.Length > 0)
                onFailure(updateMyProfileRequest.Errors);

            if (onComplete != null)
                onComplete(updateMyProfileRequest.RequestResponseObject.IsSuccess);
        }

        #endregion

        #region Account Handling

        /* In order to complete an OAUTH registration, there are a few steps to take. Each step is a process on its own.
         * 1- Get the list of OAuth Client Types from the EMS using FetchAvailableOAuthClientTypes( ).
         * 2- Display all the available OAuth clients for the user to pick one.
         * 3- Fetch the OAuth Client's URI from the EMS using FetchOAuthClientUri( ).
         * 4- Send a request using that URI and wait for its response.
         * 5- Forward the OAuth Client's URI response to the EMS using OAuthCallback
         * 6- Complete the registration by filling the RegistrationForm and calling SubmitRegistration( );.
         */

        #region OAuth

        #region Fetch OAuth Clients 

        public static void FetchAvailableOAuthClientTypes(Action<List<OAuthClient>> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            FetchAvailableOAuthClientTypes(AppearitionGate.Instance.CurrentUser.selectedChannel, onSuccess, onFailure, onComplete);
        }

        public static IEnumerator FetchAvailableOAuthClientTypesProcess(Action<List<OAuthClient>> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            yield return FetchAvailableOAuthClientTypesProcess(AppearitionGate.Instance.CurrentUser.selectedChannel, onSuccess, onFailure, onComplete);
        }

        public static void FetchAvailableOAuthClientTypes(int channelId, Action<List<OAuthClient>> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(FetchAvailableOAuthClientTypesProcess(channelId, onSuccess, onFailure, onComplete));
        }

        public static IEnumerator FetchAvailableOAuthClientTypesProcess(int channelId, Action<List<OAuthClient>> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            //Online request. Launch Request
            var getOAuthClientsRequest =
                AppearitionRequest<OAuth_AvailableClients>.LaunchAPICall_GET<AccountHandler>(channelId, GetReusableApiRequest<OAuth_AvailableClients>());

            while (!getOAuthClientsRequest.IsDone)
                yield return null;

            //Handle response
            if (getOAuthClientsRequest.RequestResponseObject.IsSuccess)
            {
                List<OAuthClient> clients = new List<OAuthClient>();

                if (getOAuthClientsRequest.RequestResponseObject.Data?.Length > 0)
                    clients.AddRange(getOAuthClientsRequest.RequestResponseObject.Data);

                AppearitionLogger.LogInfo("OAuth Clients successfully fetched!");

                if (onSuccess != null)
                    onSuccess(clients);
            }
            else
            {
                AppearitionLogger.LogError(string.Format("An error occured when trying to fetch the OAuth Clients for the channel of id {0}", channelId));

                if (onFailure != null)
                    onFailure(getOAuthClientsRequest.Errors);
            }

            if (onComplete != null)
                onComplete(getOAuthClientsRequest.RequestResponseObject.IsSuccess);
        }

        #endregion

        #region Fetch OAuth Client URI 

        public static void FetchOAuthClientUri(OAuthClient selectedClient, Action<string> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            FetchOAuthClientUri(AppearitionGate.Instance.CurrentUser.selectedChannel, selectedClient, onSuccess, onFailure, onComplete);
        }

        public static IEnumerator FetchOAuthClientUriProcess(OAuthClient selectedClient, Action<string> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            yield return FetchOAuthClientUriProcess(AppearitionGate.Instance.CurrentUser.selectedChannel, selectedClient, onSuccess, onFailure, onComplete);
        }

        public static void FetchOAuthClientUri(int channelId, OAuthClient selectedClient, Action<string> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(FetchOAuthClientUriProcess(channelId, selectedClient, onSuccess, onFailure, onComplete));
        }

        public static IEnumerator FetchOAuthClientUriProcess(int channelId, OAuthClient selectedClient, Action<string> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            //Online request. Launch Request
            var getOAuthClientUriRequest =
                AppearitionRequest<OAuth_OAuthUri>.LaunchAPICall_GET<AccountHandler>(channelId, GetReusableApiRequest<OAuth_OAuthUri>(), new OAuth_OAuthUri.RequestContent {clientType = selectedClient.Name});

            while (!getOAuthClientUriRequest.IsDone)
                yield return null;

            //Handle response
            if (getOAuthClientUriRequest.RequestResponseObject.IsSuccess && !string.IsNullOrEmpty(getOAuthClientUriRequest.RequestResponseObject.Data))
            {
                AppearitionLogger.LogInfo("OAuth Client URI successfully fetched!");

                if (onSuccess != null)
                    onSuccess(getOAuthClientUriRequest.RequestResponseObject.Data);
            }
            else if (string.IsNullOrEmpty(getOAuthClientUriRequest.RequestResponseObject.Data))
            {
                AppearitionLogger.LogError("The OAuth Client URI was successfully fetched but is empty. If you think this shouldn't happen, contact us on our developer website.");

                if (onFailure != null)
                    onFailure(getOAuthClientUriRequest.Errors);
            }
            else
            {
                AppearitionLogger.LogError(string.Format("An error occured when trying to fetch the OAuth Client URI for the channel of id {0}", channelId));

                if (onFailure != null)
                    onFailure(getOAuthClientUriRequest.Errors);
            }

            if (onComplete != null)
                onComplete(getOAuthClientUriRequest.RequestResponseObject.IsSuccess);
        }

        #endregion

        #region OAuth Client Callback

        public static IEnumerator OAuthCallbackProcess(int channelId, string content, Action<string> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            //Online request. Launch Request
            var oauthCallbackRequest =
                AppearitionRequest<OAuth_OAuthCallback>.LaunchAPICall_GET<AccountHandler>(channelId, GetReusableApiRequest<OAuth_OAuthCallback>(), new OAuth_OAuthCallback.RequestContent {clientType = content});

            while (!oauthCallbackRequest.IsDone)
                yield return null;

            //Handle response
            if (oauthCallbackRequest.RequestResponseObject.IsSuccess && !string.IsNullOrEmpty(oauthCallbackRequest.RequestResponseObject.Data))
            {
                AppearitionLogger.LogInfo("OAuth Client callback complete! " + oauthCallbackRequest.RequestResponseObject.Data);

                if (onSuccess != null)
                    onSuccess(oauthCallbackRequest.RequestResponseObject.Data);
            }
            else
            {
                AppearitionLogger.LogError(string.Format("An error occured when firing the OAuth Callback for the channel of id {0}", channelId));

                if (onFailure != null)
                    onFailure(oauthCallbackRequest.Errors);
            }

            if (onComplete != null)
                onComplete(oauthCallbackRequest.RequestResponseObject.IsSuccess);
        }

        #endregion

        #endregion

        #region Registration 

        /// <summary>
        /// Begin the first part of the registration process. Submits a form to the EMS containing all the information required to create a new account.
        /// </summary>
        /// <param name="form"></param>
        /// <param name="onSuccess"></param>
        /// <param name="onFailure"></param>
        /// <param name="onComplete"></param>
        public static void SubmitRegistration(RegistrationForm form, Action<AccountStatus> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            SubmitRegistration(AppearitionGate.Instance.CurrentUser.selectedChannel, form, onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Begin the first part of the registration process. Submits a form to the EMS containing all the information required to create a new account.
        /// </summary>
        /// <param name="form"></param>
        /// <param name="onSuccess"></param>
        /// <param name="onFailure"></param>
        /// <param name="onComplete"></param>
        /// <returns></returns>
        public static IEnumerator SubmitRegistrationProcess(RegistrationForm form, Action<AccountStatus> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            yield return SubmitRegistrationProcess(AppearitionGate.Instance.CurrentUser.selectedChannel, form, onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Begin the first part of the registration process. Submits a form to the EMS containing all the information required to create a new account.
        /// </summary>
        /// <param name="channelId"></param>
        /// <param name="form"></param>
        /// <param name="onSuccess"></param>
        /// <param name="onFailure"></param>
        /// <param name="onComplete"></param>
        public static void SubmitRegistration(int channelId, RegistrationForm form, Action<AccountStatus> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(SubmitRegistrationProcess(channelId, form, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Begin the first part of the registration process. Submits a form to the EMS containing all the information required to create a new account.
        /// </summary>
        /// <param name="channelId"></param>
        /// <param name="form"></param>
        /// <param name="onSuccess"></param>
        /// <param name="onFailure"></param>
        /// <param name="onComplete"></param>
        /// <returns></returns>
        public static IEnumerator SubmitRegistrationProcess(int channelId, RegistrationForm form, Action<AccountStatus> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            //Online request. Launch Request
            var submitRegistrationFormRequest =
                AppearitionRequest<Registration_Register>.LaunchAPICall_POST<AccountHandler>(channelId, GetReusableApiRequest<Registration_Register>(), form);

            while (!submitRegistrationFormRequest.IsDone)
                yield return null;

            //Handle response
            if (submitRegistrationFormRequest.RequestResponseObject.IsSuccess)
            {
                AppearitionLogger.LogInfo("Account successfully registered!");

                if (onSuccess != null)
                    onSuccess(submitRegistrationFormRequest.RequestResponseObject.Data);
            }
            else
            {
                AppearitionLogger.LogError("An error occured when trying to register the account of given credentials.");

                if (onFailure != null)
                    onFailure(submitRegistrationFormRequest.Errors);
            }

            if (onComplete != null)
                onComplete(submitRegistrationFormRequest.RequestResponseObject.IsSuccess);
        }

        /// <summary>
        /// Checks the registration status of a specific user to find out whether the account is successfully registered, locked and/or verified.  
        /// </summary>
        /// <param name="username"></param>
        /// <param name="onSuccess"></param>
        /// <param name="onFailure"></param>
        /// <param name="onComplete"></param>
        public static void CheckRegistrationStatus(string username, Action<AccountStatus> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(CheckRegistrationStatusProcess(username, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Checks the registration status of a specific user to find out whether the account is successfully registered, locked and/or verified.  
        /// </summary>
        /// <param name="username"></param>
        /// <param name="onSuccess"></param>
        /// <param name="onFailure"></param>
        /// <param name="onComplete"></param>
        /// <returns></returns>
        public static IEnumerator CheckRegistrationStatusProcess(string username, Action<AccountStatus> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            var requestContent = new Registration_Status.RequestContent() {
                username = username
            };

            //Online request. Launch Request
            var checkRegistrationStatusRequest =
                AppearitionRequest<Registration_Status>.LaunchAPICall_GET<AccountHandler>(AppearitionGate.Instance.CurrentUser.selectedChannel, GetReusableApiRequest<Registration_Status>(), requestContent);

            while (!checkRegistrationStatusRequest.IsDone)
                yield return null;

            //Handle response
            if (checkRegistrationStatusRequest.RequestResponseObject.IsSuccess)
            {
                AppearitionLogger.LogInfo("Account registration status successfully fetched!");

                if (onSuccess != null)
                    onSuccess(checkRegistrationStatusRequest.RequestResponseObject.Data);
            }
            else
            {
                AppearitionLogger.LogError("An error occured when trying to fetch the account registration status for user " + username + ".");

                if (onFailure != null)
                    onFailure(checkRegistrationStatusRequest.Errors);
            }

            if (onComplete != null)
                onComplete(checkRegistrationStatusRequest.RequestResponseObject.IsSuccess);
        }

        #endregion

        #region Password Forgot

        /// <summary>
        /// Submits a password reset request to the EMS with a given username.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="onSuccess"></param>
        /// <param name="onFailure"></param>
        /// <param name="onComplete"></param>
        public static void SubmitPasswordResetRequest(string username, Action onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(SubmitPasswordResetRequestProcess(username, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Submits a password reset request to the EMS with a given username.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="onSuccess"></param>
        /// <param name="onFailure"></param>
        /// <param name="onComplete"></param>
        public static IEnumerator SubmitPasswordResetRequestProcess(string username, Action onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            //Online request
            var postData = new Security_RequestPasswordReset.RequestContent {username = username};

            var requestResetPasswordRequest = AppearitionRequest<Security_RequestPasswordReset>.LaunchAPICall_POST<AccountHandler>
                (AppearitionGate.Instance.CurrentUser.selectedChannel, GetReusableApiRequest<Security_RequestPasswordReset>(), null, postData);

            while (!requestResetPasswordRequest.IsDone)
                yield return null;

            if (requestResetPasswordRequest.RequestResponseObject.IsSuccess)
            {
                AppearitionLogger.LogInfo("The password reset request for username " + username + " was successfully submitted!");

                if (onSuccess != null)
                    onSuccess();
            }
            else
            {
                AppearitionLogger.LogError("Error happened when trying to request password reset for username " + username + ".");
                if (onFailure != null)
                    onFailure(requestResetPasswordRequest.Errors);
            }

            if (onComplete != null)
                onComplete(requestResetPasswordRequest.RequestResponseObject.IsSuccess);
        }

        #endregion

        #endregion
    }
}