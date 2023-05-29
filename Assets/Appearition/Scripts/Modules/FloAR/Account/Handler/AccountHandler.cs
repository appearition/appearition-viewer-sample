// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: AccountHandler.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using Appearition.AccountAndAuthentication.API;
using Appearition.Common;
using UnityEngine;

namespace Appearition.AccountAndAuthentication
{
    //FloAR Login implementation
    public sealed partial class AccountHandler : BaseHandler
    {
        /// <summary>
        /// A login method targeting a FloAR-like EMS structure.
        /// 
        /// API Requirement: Anonymous Token.
        /// </summary>
        /// <param name="username">The username entered by the user.</param>
        /// <param name="password">The password entered by the user.</param>
        /// <param name="onSuccess">Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void RegisterDevice(string username, string password, Action onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(RegisterDeviceProcess(username, password, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// A login method targeting a FloAR-like EMS structure.
        ///
        /// API Requirement: Anonymous Token.
        /// </summary>
        /// <param name="username">The username entered by the user.</param>
        /// <param name="password">The password entered by the user.</param>
        /// <param name="onSuccess">Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator RegisterDeviceProcess(string username, string password, Action onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            //Online request
            var deviceRegisterParam = new Account_RegisterDevice.PostData() {
                #if UNITY_WEBGL
				DeviceInformation = AppearitionGate.Instance.webGL_appId
                #else
                DeviceInformation = Application.identifier
                #endif
            };

            var reusableRequest = GetReusableApiRequest<Account_RegisterDevice>();
            reusableRequest.username = username;
            reusableRequest.password = password;

            var authenticateRequest = AppearitionRequest<Account_RegisterDevice>.LaunchAPICall_POST<AccountHandler>
                (AppearitionGate.Instance.CurrentUser.selectedChannel, reusableRequest, deviceRegisterParam);

            while (!authenticateRequest.IsDone)
                yield return null;

            //Debug result
            if (authenticateRequest.RequestResponseObject.IsSuccess)
            {
                AppearitionLogger.LogInfo("Authentication success !");
                if (!string.IsNullOrEmpty(authenticateRequest.RequestResponseObject.Data.Token))
                {
                    AppearitionGate.Instance.CurrentUser.AddOrModifyAuthenticationToken(FloARConstants.PROFILE_FLOAR_TOKEN_NAME, authenticateRequest.RequestResponseObject.Data.Token);
                    Debug.LogError("Token set!: " + authenticateRequest.RequestResponseObject.Data.Token);
                }
            }
            else
                AppearitionLogger.LogError("Authentication Failure.");

            //For security purpose, reset the fields
            reusableRequest.username = reusableRequest.password = "";

            //Finally, callback
            if (onSuccess != null && authenticateRequest.RequestResponseObject.IsSuccess)
                onSuccess();

            if (onFailure != null && authenticateRequest.RequestResponseObject.Errors != null && authenticateRequest.RequestResponseObject.Errors.Length > 0)
                onFailure(authenticateRequest.Errors);

            if (onComplete != null)
                onComplete(authenticateRequest.RequestResponseObject.IsSuccess);
        }

        /// <summary>
        /// Fetches all the information related to the profile of a logged in user targeting a FloAR-like EMS structure.
        ///
        /// API Requirement: Anonymous Token.
        /// </summary>
        /// <param name="onSuccess">Contains the user profile information as on the EMS. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void GetRegisteredUserProfileData(Action<UserProfile> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(GetRegisteredUserProfileDataProcess(onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Fetches all the information related to the profile of a logged in user targeting a FloAR-like EMS structure.
        ///
        /// API Requirement: Anonymous Token.
        /// </summary>
        /// <param name="onSuccess">Contains the user profile information as on the EMS. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator GetRegisteredUserProfileDataProcess(Action<UserProfile> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            //Online request
            var getMyProfileRequest = AppearitionRequest<Account_GetUserProfile>.LaunchAPICall_GET<AccountHandler>
                (AppearitionGate.Instance.CurrentUser.selectedChannel, GetReusableApiRequest<Account_GetUserProfile>());

            while (!getMyProfileRequest.IsDone)
                yield return null;

            //Debug result
            if (getMyProfileRequest.RequestResponseObject.IsSuccess)
            {
                AppearitionLogger.LogInfo("UserProfile data was successfully fetched!");
            }
            else
                AppearitionLogger.LogError("An error happened when trying to fetch the logged in UserProfile data.");

            //Finally, callback
            if (onSuccess != null && getMyProfileRequest.RequestResponseObject.IsSuccess)
                onSuccess(getMyProfileRequest.RequestResponseObject.Data);

            if (onFailure != null && getMyProfileRequest.RequestResponseObject.Errors != null && getMyProfileRequest.RequestResponseObject.Errors.Length > 0)
                onFailure(getMyProfileRequest.Errors);

            if (onComplete != null)
                onComplete(getMyProfileRequest.RequestResponseObject.IsSuccess);
        }
    }
}