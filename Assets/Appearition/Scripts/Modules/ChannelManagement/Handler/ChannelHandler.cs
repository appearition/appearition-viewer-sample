// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: ChannelHandler.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using System;
using Appearition.Common;
using Appearition.ChannelManagement.API;
using Appearition.Internal;
using UnityEngine;

namespace Appearition.ChannelManagement
{
    /// <summary>
    /// Handles Channel related API requests, which include handling channel settings, as well as MediaType management.
    /// </summary>
    public sealed class ChannelHandler : BaseHandler
    {
        #region Handler Settings

        /// <summary>
        /// Path to a Channel (or any data extending Channel).
        /// </summary>
        /// <param name="channel"></param>
        /// <returns></returns>
        public static string GetPathToChannelDirectory(Channel channel)
        {
            return string.Format("{0}/{1}", GetHandlerStoragePath<ChannelHandler>(), channel.name.Trim());
        }

        /// <summary>
        /// Path to the channel image directory of a channel.
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="channelImage"></param>
        /// <returns></returns>
        public static string GetPathToChannelImageDirectory(Channel channel, ChannelImage channelImage)
        {
            return GetPathToChannelDirectory(channel);
        }

        /// <summary>
        /// Path to channel image.
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="channelImage"></param>
        /// <returns></returns>
        public static string GetPathToChannelImage(Channel channel, ChannelImage channelImage)
        {
            return string.Format("{0}/{1}", GetPathToChannelImageDirectory(channel, channelImage), channelImage.fileName);
        }

        #endregion

        #region Channel Management

        #region Channel Listing

        /// <summary>
        /// Fetches the channel's information using the Channel Id stored in the Current User.
        /// 
        /// API Requirement: Session Token.
        /// </summary>
        /// <param name="onSuccess">Contains the channel ApiData of the selected channel. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void GetSingleChannelSettings(Action<Channel> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            GetSingleChannelSettings(AppearitionGate.Instance.CurrentUser.selectedChannel, onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Fetches the channel's information using the Channel Id stored in the Current User.
        /// 
        /// API Requirement: Session Token.
        /// </summary>
        /// <param name="onSuccess">Contains the channel ApiData of the selected channel. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator GetSingleChannelSettingsProcess(Action<Channel> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            yield return GetSingleChannelSettingsProcess(AppearitionGate.Instance.CurrentUser.selectedChannel, onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Fetches the channel's information using a given Id.
        /// 
        /// API Requirement: Session Token.
        /// </summary>
        /// <param name="channelId">The id of the targeted channel.</param>
        /// <param name="onSuccess">Contains the channel ApiData of the selected channel. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void GetSingleChannelSettings(int channelId, Action<Channel> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(GetSingleChannelSettingsProcess(channelId, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Fetches the channel's information using a given Id.
        /// 
        /// API Requirement: Session Token.
        /// </summary>
        /// <param name="channelId">The id of the targeted channel.</param>
        /// <param name="onSuccess">Contains the channel ApiData of the selected channel. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator GetSingleChannelSettingsProcess(int channelId, Action<Channel> onSuccess, Action<EmsError> onFailure, Action<bool> onComplete)
        {
            //Online request
            var channelGetRequest = AppearitionRequest<Channel_Get>.LaunchAPICall_GET<ChannelHandler>(channelId, GetReusableApiRequest<Channel_Get>());

            while (!channelGetRequest.IsDone)
                yield return null;

            if (channelGetRequest.RequestResponseObject.IsSuccess)
            {
                if (channelGetRequest.CurrentState == AppearitionBaseRequest<Channel_Get>.RequestState.SuccessOnline)
                    AppearitionLogger.LogInfo("Single channel successfully fetched!");
                else
                    AppearitionLogger.LogInfo("Single channel successfully fetched offline!");

                if (onSuccess != null)
                    onSuccess(channelGetRequest.RequestResponseObject.Data);
            }
            else
            {
                AppearitionLogger.LogError(string.Format("An error occured when trying to fetch the settings of channel of id {0}", channelId));

                if (onFailure != null)
                    onFailure(new EmsError(channelGetRequest.RequestResponseObject.Errors));
            }


            if (onComplete != null)
                onComplete(channelGetRequest.RequestResponseObject.IsSuccess);
        }

        /// <summary>
        /// Fetches all the channels' information present on the selected tenant.
        /// 
        /// API Requirement: None.
        /// </summary>
        /// <param name="onSuccess">Contains the channel ApiData of all the channels in this tenant. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void GetAllChannels(Action<List<Channel>> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            GetAllChannels(null, onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Fetches all the channels' information present on the selected tenant.
        /// 
        /// API Requirement: None.
        /// </summary>
        /// <param name="onSuccess">Contains the channel ApiData of all the channels in this tenant. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator GetAllChannelsProcess(Action<List<Channel>> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            yield return GetAllChannelsProcess(null, onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Fetches all the channels' information present on the selected tenant.
        /// Variation allowing an existing list to populate.
        ///
        /// API Requirement: None.
        /// </summary>
        /// <param name="reusableChannelList">Reusable channel list.</param>
        /// <param name="onSuccess">Contains the channel ApiData of all the channels in this tenant. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void GetAllChannels(List<Channel> reusableChannelList, Action<List<Channel>> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(GetAllChannelsProcess(reusableChannelList, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Fetches all the channels' information present on the selected tenant.
        /// Variation allowing an existing list to populate.
        ///
        /// API Requirement: None.
        /// </summary>
        /// <param name="reusableChannelList">Reusable channel list.</param>
        /// <param name="onSuccess">Contains the channel ApiData of all the channels in this tenant. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator GetAllChannelsProcess(List<Channel> reusableChannelList,
            Action<List<Channel>> onSuccess, Action<EmsError> onFailure, Action<bool> onComplete)
        {
            //Online request
            var channelListRequest = AppearitionRequest<Channel_List>.LaunchAPICall_GET<ChannelHandler>(0, GetReusableApiRequest<Channel_List>());

            while (!channelListRequest.IsDone)
                yield return null;

            if (channelListRequest.RequestResponseObject.IsSuccess)
            {
                if (reusableChannelList != null)
                    reusableChannelList.Clear();
                else
                    reusableChannelList = new List<Channel>();

                if (channelListRequest.RequestResponseObject.Data.channels != null && channelListRequest.RequestResponseObject.Data.channels.Length > 0)
                    reusableChannelList.AddRange(channelListRequest.RequestResponseObject.Data.channels);

                if (channelListRequest.CurrentState == AppearitionBaseRequest<Channel_List>.RequestState.SuccessOnline)
                    AppearitionLogger.LogInfo("All channels successfully fetched!");
                else
                    AppearitionLogger.LogInfo("All channels successfully fetched offline!");

                if (onSuccess != null)
                    onSuccess(reusableChannelList);
            }
            else if (!channelListRequest.RequestResponseObject.IsSuccess)
            {
                AppearitionLogger.LogError("An error occured when trying to fetch the settings of all the channels of the selected tenant.");

                if (onFailure != null)
                    onFailure(new EmsError(channelListRequest.RequestResponseObject.Errors));
            }

            if (onComplete != null)
                onComplete(channelListRequest.RequestResponseObject.IsSuccess);
        }

        #endregion

        #region Settings Handling

        /// <summary>
        /// Get the channel settings of the channel selected in the current user.
        ///
        /// API Requirement: Application Token.
        /// </summary>
        /// <param name="onSuccess">Contains the channel settings of the selected channel. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void GetChannelSettings(Action<Dictionary<string, string>> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            GetChannelSettings(AppearitionGate.Instance.CurrentUser.selectedChannel, onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Get the channel settings of the channel selected in the current user.
        ///
        /// API Requirement: Application Token.
        /// </summary>
        /// <param name="onSuccess">Contains the channel settings of the selected channel. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator GetChannelSettingsProcess(Action<Dictionary<string, string>> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            yield return GetChannelSettingsProcess(AppearitionGate.Instance.CurrentUser.selectedChannel, onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Get the channel settings of the channel of given id. A reusable list can be provided for the sake of optimizing performance.
        /// 
        /// API Requirement: Application Token.
        /// </summary>
        /// <param name="channelId">The id of the targeted channel.</param>
        /// <param name="onSuccess">Contains the channel settings of the selected channel. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void GetChannelSettings(int channelId, Action<Dictionary<string, string>> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(GetChannelSettingsProcess(channelId, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Get the channel settings of the channel of given id. A reusable list can be provided for the sake of optimizing performance.
        /// 
        /// API Requirement: Application Token.
        /// </summary>
        /// <param name="channelId">The id of the targeted channel.</param>
        /// <param name="onSuccess">Contains the channel settings of the selected channel. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator GetChannelSettingsProcess(int channelId, Action<Dictionary<string, string>> onSuccess, Action<EmsError> onFailure, Action<bool> onComplete)
        {
            //Online request. Launch Request
            var getChannelSettingsRequest =
                AppearitionRequest<Channel_GetSettings>.LaunchAPICall_GET<ChannelHandler>(channelId, GetReusableApiRequest<Channel_GetSettings>());

            while (!getChannelSettingsRequest.IsDone)
                yield return null;

            //Handle response
            if (getChannelSettingsRequest.RequestResponseObject.IsSuccess)
            {
                var outcome = new Dictionary<string, string>();

                if (getChannelSettingsRequest.RequestResponseObject.Data.settings != null)
                {
                    for (int i = 0; i < getChannelSettingsRequest.RequestResponseObject.Data.settings.Length; i++)
                    {
                        if (!outcome.ContainsKey(getChannelSettingsRequest.RequestResponseObject.Data.settings[i].key))
                            outcome.Add(getChannelSettingsRequest.RequestResponseObject.Data.settings[i].key,
                                getChannelSettingsRequest.RequestResponseObject.Data.settings[i].value);
                    }
                }

                if (getChannelSettingsRequest.CurrentState == AppearitionBaseRequest<Channel_GetSettings>.RequestState.SuccessOnline)
                    AppearitionLogger.LogInfo("Channel settings successfully fetched!");
                else
                    AppearitionLogger.LogInfo("Channel settings successfully fetched offline!");

                if (onSuccess != null)
                    onSuccess(outcome);
            }
            else
            {
                AppearitionLogger.LogError(string.Format("An error occured when trying to fetch the channel settings for the channel of id {0}", channelId));

                if (onFailure != null)
                    onFailure(getChannelSettingsRequest.Errors);
            }

            if (onComplete != null)
                onComplete(getChannelSettingsRequest.RequestResponseObject.IsSuccess);
        }

        /// <summary>
        /// Updates the settings of the channel currently selected by the current user.
        ///
        /// API Requirement: Session Token.
        /// </summary>
        /// <param name="newSettings">Should contains the settings to apply to the selected channel.</param>
        /// <param name="onSuccess">Contains the channel settings of the selected channel. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void UpdateChannelSettings(List<Setting> newSettings, Action onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            UpdateChannelSettings(AppearitionGate.Instance.CurrentUser.selectedChannel, newSettings, onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Updates the settings of the channel currently selected by the current user.
        ///
        /// API Requirement: Session Token.
        /// </summary>
        /// <param name="newSettings">Should contains the settings to apply to the selected channel.</param>
        /// <param name="onSuccess">Contains the channel settings of the selected channel. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator UpdateChannelSettingsProcess(List<Setting> newSettings, Action onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            yield return UpdateChannelSettingsProcess(AppearitionGate.Instance.CurrentUser.selectedChannel, newSettings, onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Updates the settings of the channel from a given channel id.
        ///
        /// API Requirement: Session Token.
        /// </summary>
        /// <param name="channelId">The id of the targeted channel.</param>
        /// <param name="newSettings">Should contains the settings to apply to the selected channel.</param>
        /// <param name="onSuccess">Contains the channel settings of the selected channel. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void UpdateChannelSettings(int channelId, List<Setting> newSettings, Action onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(UpdateChannelSettingsProcess(channelId, newSettings, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Updates the settings of the channel from a given channel id.
        ///
        /// API Requirement: Session Token.
        /// </summary>
        /// <param name="channelId">The id of the targeted channel.</param>
        /// <param name="newSettings">Should contains the settings to apply to the selected channel.</param>
        /// <param name="onSuccess">Contains the channel settings of the selected channel. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator UpdateChannelSettingsProcess(int channelId, List<Setting> newSettings, Action onSuccess, Action<EmsError> onFailure, Action<bool> onComplete)
        {
            //Online request. Prepare the request
            var updateChannelSettingsPostData = new Channel_UpdateSettings.PostData() {settings = newSettings.ToArray()};

            var updateChannelSettingsRequest = AppearitionRequest<Channel_UpdateSettings>.LaunchAPICall_POST<ChannelHandler>
                (channelId, GetReusableApiRequest<Channel_UpdateSettings>(), updateChannelSettingsPostData);

            //Launch the request!
            while (!updateChannelSettingsRequest.IsDone)
                yield return null;

            //Handle the response
            if (updateChannelSettingsRequest.RequestResponseObject.IsSuccess)
            {
                AppearitionLogger.LogInfo("Channel settings successfully updated!");

                if (onSuccess != null)
                    onSuccess();
            }
            else
            {
                AppearitionLogger.LogError("An error occured when trying to update the channel's settings.");

                if (onFailure != null)
                    onFailure(updateChannelSettingsRequest.Errors);
            }

            if (onComplete != null)
                onComplete(updateChannelSettingsRequest.RequestResponseObject.IsSuccess);
        }

        #endregion

        #endregion

        #region MediaType management

        /// <summary>
        /// Fetch the MediaTypes from the channel chosen by the current user.
        ///
        /// API Requirement: Application Token. Offline capability.
        /// </summary>
        /// <param name="onSuccess">Contains the the MediaTypes linked to the selected channel. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void GetMediaTypesFromEms(Action<List<MediaType>> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            GetMediaTypesFromEms(AppearitionGate.Instance.CurrentUser.selectedChannel, null, onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Fetch the MediaTypes from the channel chosen by the current user.
        ///
        /// API Requirement: Application Token. Offline capability.
        /// </summary>
        /// <param name="onSuccess">Contains the the MediaTypes linked to the selected channel. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator GetMediaTypesFromEmsProcess(Action<List<MediaType>> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            yield return GetMediaTypesFromEmsProcess(AppearitionGate.Instance.CurrentUser.selectedChannel, null, onSuccess, onFailure, onComplete);
        }


        /// <summary>
        /// Fetch the MediaTypes from the channel of a given id.
        ///
        /// API Requirement: Application Token. Offline capability.
        /// </summary>
        /// <param name="channelId">The id of the targeted channel.</param>
        /// <param name="onSuccess">Contains the the MediaTypes linked to the selected channel. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void GetMediaTypesFromEms(int channelId, Action<List<MediaType>> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            GetMediaTypesFromEms(channelId, null, onSuccess, onFailure, onComplete);
        }


        /// <summary>
        /// Fetch the MediaTypes from the channel of a given id.
        ///
        /// API Requirement: Application Token. Offline capability.
        /// </summary>
        /// <param name="channelId">The id of the targeted channel.</param>
        /// <param name="onSuccess">Contains the the MediaTypes linked to the selected channel. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator GetMediaTypesFromEmsProcess(int channelId, Action<List<MediaType>> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            yield return GetMediaTypesFromEmsProcess(channelId, null, onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Fetch the MediaTypes from the channel of a given id.
        /// A reusable list can be provided for the sake of optimizing performance.
        ///
        /// API Requirement: Application Token. Offline capability.
        /// </summary>
        /// <param name="channelId">The id of the targeted channel.</param>
        /// <param name="reusableMediaTypeList">A reusable MediaType list.</param>
        /// <param name="onSuccess">Contains the the MediaTypes linked to the selected channel. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void GetMediaTypesFromEms(int channelId, List<MediaType> reusableMediaTypeList, Action<List<MediaType>> onSuccess = null,
            Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(GetMediaTypesFromEmsProcess(channelId, reusableMediaTypeList, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Fetch the MediaTypes from the channel of a given id.
        /// A reusable list can be provided for the sake of optimizing performance.
        ///
        /// API Requirement: Application Token. Offline capability.
        /// </summary>
        /// <param name="channelId">The id of the targeted channel.</param>
        /// <param name="reusableMediaTypeList">A reusable MediaType list.</param>
        /// <param name="onSuccess">Contains the the MediaTypes linked to the selected channel. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator GetMediaTypesFromEmsProcess(int channelId, List<MediaType> reusableMediaTypeList,
            Action<List<MediaType>> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            //Get the request ready
            var getMediaTypesRequest = AppearitionRequest<Channel_ListMediaTypes>.LaunchAPICall_GET<ChannelHandler>(channelId, GetReusableApiRequest<Channel_ListMediaTypes>());

            //Handle the request
            while (!getMediaTypesRequest.IsDone)
                yield return null;

            //Callback
            if (getMediaTypesRequest.RequestResponseObject.IsSuccess)
            {
                //Handle success
                if (getMediaTypesRequest.RequestResponseObject.Data != null && getMediaTypesRequest.RequestResponseObject.Data.mediaTypes != null)
                {
                    reusableMediaTypeList.AddRange(getMediaTypesRequest.RequestResponseObject.Data.mediaTypes);

                    if (getMediaTypesRequest.CurrentState == AppearitionBaseRequest<Channel_ListMediaTypes>.RequestState.SuccessOnline)
                        AppearitionLogger.LogInfo("MediaTypes successfully fetched!");
                    else
                        AppearitionLogger.LogInfo(string.Format("MediaTypes of the channel {0} have been successfully fetched!", channelId));
                }
                else
                {
                    AppearitionLogger.LogError("The MediaType fetch request completed successfully but no MediaTypes could be obtained from it.");
                }

                if (onSuccess != null)
                    onSuccess(reusableMediaTypeList);
            }
            else
            {
                AppearitionLogger.LogError("An error occured when trying to fetch the MediaTypes from the selected channel.");

                if (onFailure != null)
                    onFailure(new EmsError(getMediaTypesRequest.RequestResponseObject.Errors));
            }

            if (onComplete != null)
                onComplete(getMediaTypesRequest.RequestResponseObject.IsSuccess);
        }

        #endregion

        #region Feedback

        public static void SendFeedback(FeedbackContent feedback, Action onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            SendFeedback(AppearitionGate.Instance.CurrentUser.selectedChannel, feedback, onSuccess, onFailure, onComplete);
        }

        public static IEnumerator SendFeedbackProcess(FeedbackContent feedback, Action onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            yield return SendFeedbackProcess(AppearitionGate.Instance.CurrentUser.selectedChannel, feedback, onSuccess, onFailure, onComplete);
        }

        public static void SendFeedback(int channelId, FeedbackContent feedback, Action onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(SendFeedbackProcess(channelId, feedback, onSuccess, onFailure, onComplete));
        }

        public static IEnumerator SendFeedbackProcess(int channelId, FeedbackContent feedback, Action onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            //Online request
            var postContent = new Feedback_Submit.PostData(feedback) {
                productId = channelId,
                os = SystemInfo.operatingSystem,
                device = SystemInfo.deviceName + ", " + SystemInfo.deviceModel,
            };

            var sendFeedbackRequest = AppearitionRequest<Feedback_Submit>.LaunchAPICall_POST<ChannelHandler>(channelId, GetReusableApiRequest<Feedback_Submit>(), postContent);

            while (!sendFeedbackRequest.IsDone)
                yield return null;

            if (sendFeedbackRequest.RequestResponseObject.IsSuccess)
            {
                AppearitionLogger.LogInfo("User feedback successfully uploaded!");

                if (onSuccess != null)
                    onSuccess();
            }
            else
            {
                AppearitionLogger.LogError(string.Format("An error occured when trying send user feedback."));

                if (onFailure != null)
                    onFailure(new EmsError(sendFeedbackRequest.RequestResponseObject.Errors));
            }


            if (onComplete != null)
                onComplete(sendFeedbackRequest.RequestResponseObject.IsSuccess);
        }

        #endregion
    }
}