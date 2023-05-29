// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: AppearitionAPIData.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics;

namespace Appearition.API
{
    #region Consts

    public static class ApiConstants
    {
        //API CALL
        public const string API_CALL_SUCCESS = "200";
        public const string API_CALL_FORBIDDEN = "401";
        public const string API_CALL_NOT_FOUND = "404";

        //API LOG MESSAGES
        public const string REQUEST_NO_USER_GIVEN = "Please use a valid profile. Ensure that the End Point, Tenant and Channel ID are selected properly in order to proceed.";
        public const string REQUEST_AUTHENTICATION_LEVEL_TOO_LOW = "The request you are trying to execute requires a higher authentication level. Please login and try again.";
    }

    /// <summary>
    /// List of the different kind of authentication methods that can be enforced by a type of API.
    /// </summary>
    public enum AuthenticationOverrideType
    {
        None,
        ApplicationToken,
        SessionToken,
        Custom
    }

    /// <summary>
    /// Type of API post request (ie single form, multiple form, etc).
    /// </summary>
    public enum TypeOfPost
    {
        SingleForm,
        MultiForms
    }

    /// <summary>
    /// Storage method of the API
    /// </summary>
    public enum TypeOfApiStorage
    {
        None,
        Response,
        Custom
    }

    #endregion

    /// <summary>
    /// Base class for any GET API call, or any API call response.
    /// </summary>
    [System.Serializable]
    public abstract class BaseApi
    {
        //Inherited members
        public bool IsSuccess;
        public string[] Errors;

        /// <summary>
        /// Determines whether this API request object is currently being used or is available.
        /// The purpose of this flag is for re-usability of API objects, marking whether this API object can be reused or not.
        /// </summary>
        /// <value><c>true</c> if is being used; otherwise, <c>false</c>.</value>
        public virtual bool IsBeingUsed { get; set; }

        #region Standard API Settings

        /// <summary>
        /// Returns the end point part corresponding to this request. Format: api/RequestCategory/RequestName.
        /// </summary>
        /// <value>The get EMS call.</value>
        public virtual string GetEmsCall
        {
            get { return "api/" + this.GetType().Name.Replace('_', '/'); }
        }

        /// <summary>
        /// API Version used by this request. Default value is 1.
        /// </summary>
        /// <value>The AP i version.</value>
        public virtual int ApiVersion
        {
            get { return 1; }
        }

        /// <summary>
        /// Provides a template for additional headers that should be included in the API call.
        /// </summary>
        /// <value>The extra URL headers.</value>
        protected virtual Dictionary<string, string> ExtraRequestUrlHeaders
        {
            get { return new Dictionary<string, string>(); }
        }

        /// <summary>
        /// Access the API's additional request headers.
        /// </summary>
        /// <returns></returns>
        public virtual Dictionary<string, string> GetRequestExtraHeaders()
        {
            return ExtraRequestUrlHeaders;
        }

        #endregion

        #region Communication Settings & Overrides

        /// <summary>
        /// Whether or not the given request is forced to go to a specific end point instead of the main one.
        /// </summary>
        public virtual string ForcedEndPoint
        {
            get { return null; }
        }

        /// <summary>
        /// Whether or not this request should have a tenant as part of the request URL. True by default.
        /// </summary>
        public virtual bool RequiresTenant
        {
            get { return true; }
        }

        /// <summary>
        /// Whether or not this specific API requires a certain channel id. If null, will use the one in the Appearition Gate.
        /// </summary>
        public virtual int? ChannelIdOverride
        {
            get { return default(int?); }
        }

        /// <summary>
        /// Whether or not the authentication in the request header should be overrode. If overrode, include your own.
        /// </summary>
        public virtual bool OverrideAuthenticationHeader
        {
            get { return false; }
        }


        /// <summary>
        /// Override of the authentication type used for a specific API
        /// </summary>
        /// <value>The type of the authentication override.</value>
        public virtual AuthenticationOverrideType AuthenticationOverride
        {
            get { return AuthenticationOverrideType.None; }
        }

        /// <summary>
        /// If the authentication override type is set to custom, this value will be used to access the UserProfile authentication token dictionary to fetch the desired value.
        /// If none existing, the anonymous token will most likely be used.
        /// </summary>
        public virtual string AuthenticationTokenCustomKey
        {
            get { return ""; }
        }

        /// <summary>
        /// Whether this API can bypass the internet connectivity check.
        /// </summary>
        public virtual bool BypassInternetCheck => false;

        /// <summary>
        /// A dictionary of string to be replaced from one to another.
        /// This can be used especially if a key has a name which cannot be used, such as "params", but is required by the EMS.
        /// </summary>
        public virtual Dictionary<string, string> JsonReplaceKvp { get; } = new Dictionary<string, string>();

        #endregion

        #region Save Settings

        /// <summary>
        /// How the API should be saved locally for offline capability.
        /// In order for the file to be saved, make sure AppearitionConstant.enableApiResponseStorage is true.
        /// Lastly, if using a CUSTOM api storage type, override GetCustomOfflineJsonContent and UpdateCustomOfflineJsonContent.
        /// </summary>
        public virtual TypeOfApiStorage ResponseSaveType
        {
            get => TypeOfApiStorage.Response;
        }

        /// <summary>
        /// When the ResponseSaveType is set to Custom, override this method to return a valid json.
        /// </summary>
        /// <returns></returns>
        public virtual string GetCustomOfflineJsonContent(int channelId, object postBodyIfAny, BaseRequestContent requestContentIfAny)
        {
            return "";
        }

        /// <summary>
        /// When the ResponseSaveType is set to Custom, override this method to update the local JSOn using live data.
        /// </summary>
        public virtual void UpdateCustomOfflineJsonContent(BaseApi api, int channelId, object postBodyIfAny, BaseRequestContent requestContentIfAny)
        { 
        }

        public virtual string GetJsonFileExtraParams(object postBodyIfAny, BaseRequestContent requestContentIfAny)
        {
            return "";
        }

        #endregion

        //Static utilities
        /// <summary>
        /// Returns the API call of a given class
        /// </summary>
        /// <returns>The API call.</returns>
        /// <param name="source">Source.</param>
        /// <param name="tenantName">Tenant name.</param>
        /// <param name="id">Identifier.</param>
        /// <param name="requestContent">Request content.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static string GetApiCall<T>(string source, string tenantName = "", int id = -1, BaseRequestContent requestContent = null) where T : BaseApi
        {
            BaseApi member = (BaseApi) System.Activator.CreateInstance(typeof(T));
            string output = "";

            string forcedSource = member.ForcedEndPoint;

            if (string.IsNullOrEmpty(forcedSource))
                output += source + "/";
            else
                output += forcedSource + "/";

            //Add tenant information
            if (member.RequiresTenant)
                output += (tenantName.Length > 1 ? tenantName + "/" : "");

            //Add the type of EMS call
            output += member.GetEmsCall;

            //Add the ID
            if (member.ChannelIdOverride.HasValue)
                output += member.ChannelIdOverride.Value >= 0 ? "/" + member.ChannelIdOverride.Value : "/";
            else
                output += id >= 0 ? "/" + id : "";

            //Add the URL extra params
            output += (requestContent != null ? requestContent.GetUrlExtraParameters() : "");

            return output;
        }

        /// <summary>
        /// Must contain information on how to reset this object to default.
        /// This system is made so that 
        /// </summary>
        public virtual void ResetToDefault<T>() where T : BaseApi
        {
            IsSuccess = false;
            Errors = null;
            IsBeingUsed = false;
        }
    }
}