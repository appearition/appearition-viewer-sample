// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: ImageRecognitionHandler.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using Appearition.Common;
using Appearition.ImageRecognition.API;
using Appearition.Internal;

namespace Appearition.ImageRecognition
{
    /// <summary>
    /// Handles Image Recognition related API requests, which include getting and editing the Data Store entries.
    /// Those Data Store entries are related to the Image Recognition settings of external providers as on the EMS.
    /// When an Image Recognition is configured on the EMS, the future TargetImages will be synced with said provider.
    /// </summary>
    public sealed class ImageRecognitionHandler : BaseHandler
    {
        #region ArProvider DataStores

        /// <summary>
        /// Fetches all the ArProviders available for the selected channel.
        ///
        /// API Requirement: Session Token. Offline Capability.
        /// </summary>
        /// <param name="onSuccess">Contains all the ArProvider DataStores relevant to this channel. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void GetAllAvailableProviders(Action<List<DataStore>> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            GetAllAvailableProviders(AppearitionGate.Instance.CurrentUser.selectedChannel, onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Fetches all the ArProviders available for the selected channel.
        ///
        /// API Requirement: Session Token. Offline Capability.
        /// </summary>
        /// <param name="onSuccess">Contains all the ArProvider DataStores relevant to this channel. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator GetAllAvailableProvidersProcess(Action<List<DataStore>> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            yield return GetAllAvailableProvidersProcess(AppearitionGate.Instance.CurrentUser.selectedChannel, onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Fetches all the ArProviders available for a given channel by ids.
        ///
        /// API Requirement: Session Token. Offline Capability.
        /// </summary>
        /// <param name="channelId">The id of the targeted channel.</param>
        /// <param name="onSuccess">Contains all the ArProvider DataStores relevant to this channel. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void GetAllAvailableProviders(int channelId, Action<List<DataStore>> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(GetAllAvailableProvidersProcess(channelId, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Fetches all the ArProviders available for a given channel by ids.
        ///
        /// API Requirement: Session Token. Offline Capability.
        /// </summary>
        /// <param name="channelId">The id of the targeted channel.</param>
        /// <param name="onSuccess">Contains all the ArProvider DataStores relevant to this channel. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator GetAllAvailableProvidersProcess(int channelId, Action<List<DataStore>> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            //Online request
            var listAvailableProvidersRequest = AppearitionRequest<ImageRecognition_ListAvailable>.LaunchAPICall_GET<ImageRecognitionHandler>
                (channelId, GetReusableApiRequest<ImageRecognition_ListAvailable>());

            while (!listAvailableProvidersRequest.IsDone)
                yield return null;

            //Debug result
            if (listAvailableProvidersRequest.RequestResponseObject.IsSuccess)
            {
                if (listAvailableProvidersRequest.CurrentState == AppearitionBaseRequest<ImageRecognition_ListAvailable>.RequestState.SuccessOnline)
                    AppearitionLogger.LogInfo("ArProvider data fetched successfully!");
                else
                    AppearitionLogger.LogInfo("ArProvider DataStores successfully loaded offline!");
            }
            else
                AppearitionLogger.LogError("An issue occured when trying fetch all available ArProviders' settings. " + listAvailableProvidersRequest.Errors);

            //Finally, callback
            if (onSuccess != null && listAvailableProvidersRequest.RequestResponseObject.IsSuccess)
                onSuccess(listAvailableProvidersRequest.RequestResponseObject.Data.dataStores);

            if (onFailure != null && listAvailableProvidersRequest.RequestResponseObject.Errors != null && listAvailableProvidersRequest.RequestResponseObject.Errors.Length > 0)
                onFailure(listAvailableProvidersRequest.Errors);

            if (onComplete != null)
                onComplete(listAvailableProvidersRequest.RequestResponseObject.IsSuccess);
        }

        /// <summary>
        /// Creates or updates a ArProvider entry in the EMS using the given data.
        /// Do note that the provider name field will be used to match data with an existing entry.
        ///
        /// API Requirement: Session Token.
        /// </summary>
        /// <param name="providerData">ArProvider DataStore you wish to create or update.</param>
        /// <param name="onSuccess">Contains all the ArProvider DataStores relevant to this channel. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void SetProviderData(DataStore providerData, Action onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            SetProviderData(AppearitionGate.Instance.CurrentUser.selectedChannel, providerData, onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Creates or updates a ArProvider entry in the EMS using the given data.
        /// Do note that the provider name field will be used to match data with an existing entry.
        ///
        /// API Requirement: Session Token.
        /// </summary>
        /// <param name="providerData">ArProvider DataStore you wish to create or update.</param>
        /// <param name="onSuccess">Contains all the ArProvider DataStores relevant to this channel. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator SetProviderDataProcess(DataStore providerData, Action onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            yield return SetProviderDataProcess(AppearitionGate.Instance.CurrentUser.selectedChannel, providerData, onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Creates or updates a ArProvider entry in the EMS using the given data.
        /// Do note that the provider name field will be used to match data with an existing entry.
        ///
        /// API Requirement: Session Token.
        /// </summary>
        /// <param name="channelId"></param>
        /// <param name="providerData">ArProvider DataStore you wish to create or update.</param>
        /// <param name="onSuccess">Contains all the ArProvider DataStores relevant to this channel. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void SetProviderData(int channelId, DataStore providerData, Action onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(SetProviderDataProcess(channelId, providerData, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Creates or updates a ArProvider entry in the EMS using the given data.
        /// Do note that the provider name field will be used to match data with an existing entry.
        ///
        /// API Requirement: Session Token.
        /// </summary>
        /// <param name="channelId">The id of the targeted channel.</param>
        /// <param name="providerData">ArProvider DataStore you wish to create or update.</param>
        /// <param name="onSuccess">Contains all the ArProvider DataStores relevant to this channel. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator SetProviderDataProcess(int channelId, DataStore providerData, Action onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            //Online request
            var saveDataStoreRequest = AppearitionRequest<ImageRecognition_SaveDataStore>.LaunchAPICall_POST<ImageRecognitionHandler>
                (channelId, GetReusableApiRequest<ImageRecognition_SaveDataStore>(), providerData);

            while (!saveDataStoreRequest.IsDone)
                yield return null;

            //Debug result
            if (saveDataStoreRequest.RequestResponseObject.IsSuccess)
            {
                AppearitionLogger.LogInfo("ArProvider data updated successfully!");
            }
            else
                AppearitionLogger.LogError("An issue occured when trying to update the given ArProvider's data. " + saveDataStoreRequest.Errors);

            //Finally, callback
            if (onSuccess != null && saveDataStoreRequest.RequestResponseObject.IsSuccess)
                onSuccess();

            if (onFailure != null && saveDataStoreRequest.RequestResponseObject.Errors != null && saveDataStoreRequest.RequestResponseObject.Errors.Length > 0)
                onFailure(saveDataStoreRequest.Errors);

            if (onComplete != null)
                onComplete(saveDataStoreRequest.RequestResponseObject.IsSuccess);
        }

        #endregion
    }
}