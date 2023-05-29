// // -----------------------------------------------------------------------
// // Company:"Appearition Pty Ltd"
// // File: ArTargetHandler.cs
// // Copyright (c) 2019. All rights reserved.
// // -----------------------------------------------------------------------

#pragma warning disable 0162

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using Appearition.ArTargetImageAndMedia.API;
using Appearition.Common;
using Appearition.ContentLibrary;
using Appearition.Internal;

namespace Appearition.ArTargetImageAndMedia
{
    /// <summary>
    /// Handler in charge of taking care of any ArTarget, Media and TargetImage related operations.
    /// Offers an easy to use access to the CRUD features for those operations.
    /// </summary>
    public class ArTargetHandler : BaseHandler
    {
        #region Handler Settings

        /// <summary>
        /// Path to an Asset (or any data extending Asset).
        /// </summary>
        /// <param name="asset"></param>
        /// <returns></returns>
        public static string GetPathToAssetDirectory(Asset asset)
        {
            return string.Format("{0}/{1}", GetHandlerStoragePath<ArTargetHandler>(), asset.assetId.Trim());
        }

        /// <summary>
        /// Path to the Media directory (or any data extending MediaFile), usually contained inside Asset directory.
        /// </summary>
        /// <param name="asset"></param>
        /// <param name="media"></param>
        /// <returns></returns>
        public static string GetPathToMediaDirectory(Asset asset, MediaFile media)
        {
            if (media.IsContentLibraryItem)
                return ContentLibraryHandler.GetPathToContentFileDirectory(media.contentItemProviderName, GetKeyFromAssetAndMedia(asset, media));
            return GetPathToAssetDirectory(asset);
        }

        /// <summary>
        /// Path to a Media file.
        /// </summary>
        /// <param name="asset"></param>
        /// <param name="media"></param>
        public static string GetPathToMedia(Asset asset, MediaFile media)
        {
            if (media.IsContentLibraryItem)
                return ContentLibraryHandler.TryToFindFirstItemWithContentKey(media.contentItemProviderName, GetKeyFromAssetAndMedia(asset, media));
            return string.Format("{0}/{1}", GetPathToMediaDirectory(asset, media), media.fileName.Trim());
        }

        /// <summary>
        /// Path to a TargetImage (or any data extending TargetImage), usually contained inside Assets.
        /// </summary>
        /// <param name="asset"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static string GetPathToTargetImageDirectory(Asset asset, TargetImage target)
        {
            return string.Format("{0}/{1}", GetPathToAssetDirectory(asset), target.arImageId);
        }

        /// <summary>
        /// Path to a Target Image file.
        /// </summary>
        /// <param name="asset"></param>
        /// <param name="targetImage"></param>
        /// <returns></returns>
        public static string GetPathToTargetImage(Asset asset, TargetImage targetImage)
        {
            return string.Format("{0}/{1}", GetPathToTargetImageDirectory(asset, targetImage), targetImage.fileName.Trim());
        }

        static string GetKeyFromAssetAndMedia(Asset asset, MediaFile media)
        {
            return $"{asset.assetId}_{asset.mediaFiles.FindIndex(o => o == media)}";
        }

        #endregion

        #region Get Channel's Experiences And Assets

        #region Get Experiences

        /// <summary>
        /// Fetches all the experiences from the channel sorted inside the given user ApiData. Returns them as a callback whenever they're ready.
        ///
        /// Offline capability.
        /// </summary>
        /// <param name="onSuccess">Contains all the Asset ApiData stored in the selected. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void GetChannelExperiences(Action<List<Asset>> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            GetChannelExperiences
                (AppearitionGate.Instance.CurrentUser.selectedChannel, null, false, false, false, onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Fetches all the experiences from the channel sorted inside the given user ApiData. Returns them as a callback whenever they're ready.
        ///
        /// Offline capability.
        /// </summary>
        /// <param name="onSuccess">Contains all the Asset ApiData stored in the selected. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator GetChannelExperiencesProcess(Action<List<Asset>> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            yield return GetChannelExperiencesProcess
                (AppearitionGate.Instance.CurrentUser.selectedChannel, null, false, false, false, onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Fetches all the experiences from the channel sorted inside the given user ApiData. Returns them as a callback whenever they're ready.
        /// A filename can be provided for the system to store the request's content locally, enabling an offline capability.
        /// Each type of ApiData can also be chose to be downloaded locally.
        ///
        /// Offline capability.
        /// </summary>
        /// <param name="downloadTargetImages">Whether or not the Target Images should be downloaded. Do note that the EMS needs to allow this feature as well.</param>
        /// <param name="downloadContent">Whether or not the Medias should be downloaded. Do note that the EMS needs to allow this feature as well.</param>
        /// <param name="downloadPreDownloads">Whether or not the Medias marked as pre-download should be downloaded. If downloadContent is set to true, the medias will get downloaded anyway.</param>
        /// <param name="onSuccess">Contains all the Asset ApiData stored in the selected. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void GetChannelExperiences(bool downloadTargetImages, bool downloadContent, bool downloadPreDownloads,
            Action<List<Asset>> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            GetChannelExperiences(AppearitionGate.Instance.CurrentUser.selectedChannel,
                null, downloadTargetImages, downloadContent, downloadPreDownloads, onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Fetches all the experiences from the channel sorted inside the given user ApiData. Returns them as a callback whenever they're ready.
        /// A filename can be provided for the system to store the request's content locally, enabling an offline capability.
        /// Each type of ApiData can also be chose to be downloaded locally.
        ///
        /// Offline capability.
        /// </summary>
        /// <param name="downloadTargetImages">Whether or not the Target Images should be downloaded. Do note that the EMS needs to allow this feature as well.</param>
        /// <param name="downloadContent">Whether or not the Medias should be downloaded. Do note that the EMS needs to allow this feature as well.</param>
        /// <param name="downloadPreDownloads">Whether or not the Medias marked as pre-download should be downloaded. If downloadContent is set to true, the medias will get downloaded anyway.</param>
        /// <param name="onSuccess">Contains all the Asset ApiData stored in the selected. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator GetChannelExperiencesProcess(bool downloadTargetImages, bool downloadContent, bool downloadPreDownloads,
            Action<List<Asset>> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            yield return GetChannelExperiencesProcess(AppearitionGate.Instance.CurrentUser.selectedChannel,
                null, downloadTargetImages, downloadContent, downloadPreDownloads, onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Fetches all the experiences from the channel of the given id.
        /// A filename can be provided for the system to store the request's content locally, enabling an offline capability.
        /// Each type of ApiData can also be chose to be downloaded locally.
        ///
        /// Offline capability.
        /// </summary>
        /// <param name="channelId">The id of the targeted channel.</param>
        /// <param name="downloadTargetImages">Whether or not the Target Images should be downloaded. Do note that the EMS needs to allow this feature as well.</param>
        /// <param name="downloadContent">Whether or not the Medias should be downloaded. Do note that the EMS needs to allow this feature as well.</param>
        /// <param name="downloadPreDownloads">Whether or not the Medias marked as pre-download should be downloaded. If downloadContent is set to true, the medias will get downloaded anyway.</param>
        /// <param name="onSuccess">Contains all the Asset ApiData stored in the selected. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void GetChannelExperiences(int channelId, bool downloadTargetImages, bool downloadContent, bool downloadPreDownloads,
            Action<List<Asset>> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            GetChannelExperiences
                (channelId, null, downloadTargetImages, downloadContent, downloadPreDownloads, onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Fetches all the experiences from the channel of the given id.
        /// A filename can be provided for the system to store the request's content locally, enabling an offline capability.
        /// Each type of ApiData can also be chose to be downloaded locally.
        ///
        /// Offline capability.
        /// </summary>
        /// <param name="channelId">The id of the targeted channel.</param>
        /// <param name="downloadTargetImages">Whether or not the Target Images should be downloaded. Do note that the EMS needs to allow this feature as well.</param>
        /// <param name="downloadContent">Whether or not the Medias should be downloaded. Do note that the EMS needs to allow this feature as well.</param>
        /// <param name="downloadPreDownloads">Whether or not the Medias marked as pre-download should be downloaded. If downloadContent is set to true, the medias will get downloaded anyway.</param>
        /// <param name="onSuccess">Contains all the Asset ApiData stored in the selected. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator GetChannelExperiencesProcess(int channelId, bool downloadTargetImages, bool downloadContent, bool downloadPreDownloads,
            Action<List<Asset>> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            yield return GetChannelExperiencesProcess
                (channelId, null, downloadTargetImages, downloadContent, downloadPreDownloads, onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Fetches all the experiences from the channel sorted inside the given user ApiData.
        /// A filename can be provided for the system to store the request's content locally, enabling an offline capability.
        /// Each type of ApiData can also be chose to be downloaded locally.
        /// 
        /// Offline capability.
        /// </summary>
        /// <param name="reusableList">Reusable list of assets.</param>
        /// <param name="downloadTargetImages">If set to <c>true</c> download target images.</param>
        /// <param name="downloadContent">If set to <c>true</c> download content.</param>
        /// <param name="downloadPreDownloads">If set to <c>true</c> download pre downloads.</param>
        /// <param name="onSuccess">Contains all the Asset ApiData stored in the selected. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        /// <param name="downloadTransferStatus"></param>
        public static void GetChannelExperiences(List<Asset> reusableList, bool downloadTargetImages, bool downloadContent, bool downloadPreDownloads,
            Action<List<Asset>> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null, DataTransferStatus downloadTransferStatus = null)
        {
            GetChannelExperiences(AppearitionGate.Instance.CurrentUser.selectedChannel,
                reusableList, downloadTargetImages, downloadContent, downloadPreDownloads, onSuccess, onFailure, onComplete, downloadTransferStatus);
        }

        /// <summary>
        /// Fetches all the experiences from the channel sorted inside the given user ApiData.
        /// A filename can be provided for the system to store the request's content locally, enabling an offline capability.
        /// Each type of ApiData can also be chose to be downloaded locally.
        /// 
        /// Offline capability.
        /// </summary>
        /// <param name="reusableList">Reusable list of assets.</param>
        /// <param name="downloadTargetImages">If set to <c>true</c> download target images.</param>
        /// <param name="downloadContent">If set to <c>true</c> download content.</param>
        /// <param name="downloadPreDownloads">If set to <c>true</c> download pre downloads.</param>
        /// <param name="onSuccess">Contains all the Asset ApiData stored in the selected. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        /// <param name="downloadTransferStatus"></param>
        public static IEnumerator GetChannelExperiencesProcess(List<Asset> reusableList, bool downloadTargetImages, bool downloadContent, bool downloadPreDownloads,
            Action<List<Asset>> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null, DataTransferStatus downloadTransferStatus = null)
        {
            yield return GetChannelExperiencesProcess(AppearitionGate.Instance.CurrentUser.selectedChannel,
                reusableList, downloadTargetImages, downloadContent, downloadPreDownloads, onSuccess, onFailure, onComplete, downloadTransferStatus);
        }

        /// <summary>
        /// Fetches all the experiences from the channel of the given id.
        /// A filename can be provided for the system to store the request's content locally, enabling an offline capability.
        /// Each type of ApiData can also be chose to be downloaded locally.
        /// 
        /// Offline capability.
        /// </summary>
        /// <param name="channelId">The id of the targeted channel.</param>
        /// <param name="reusableList">Reusable list of assets.</param>
        /// <param name="downloadTargetImages">Whether or not the Target Images should be downloaded. Do note that the EMS needs to allow this feature as well.</param>
        /// <param name="downloadContent">Whether or not the Medias should be downloaded. Do note that the EMS needs to allow this feature as well.</param>
        /// <param name="downloadPreDownloads">Whether or not the Medias marked as pre-download should be downloaded. If downloadContent is set to true, the medias will get downloaded anyway.</param>
        /// <param name="onSuccess">Contains all the Asset ApiData stored in the selected. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        /// <param name="downloadTransferStatus"></param>
        public static void GetChannelExperiences(int channelId, List<Asset> reusableList, bool downloadTargetImages, bool downloadContent, bool downloadPreDownloads,
            Action<List<Asset>> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null,
            DataTransferStatus downloadTransferStatus = null)
        {
            AppearitionGate.Instance.StartCoroutine(GetChannelExperiencesProcess
                (channelId, reusableList, downloadTargetImages, downloadContent, downloadPreDownloads, onSuccess, onFailure, onComplete, downloadTransferStatus));
        }

        /// <summary>
        /// Fetches all the experiences from the channel of the given id.
        /// A filename can be provided for the system to store the request's content locally, enabling an offline capability.
        /// Each type of ApiData can also be chose to be downloaded locally.
        /// 
        /// Offline capability.
        /// </summary>
        /// <param name="channelId">The id of the targeted channel.</param>
        /// <param name="reusableList">Reusable list of assets.</param>
        /// <param name="downloadTargetImages">Whether or not the Target Images should be downloaded. Do note that the EMS needs to allow this feature as well.</param>
        /// <param name="downloadContent">Whether or not the Medias should be downloaded. Do note that the EMS needs to allow this feature as well.</param>
        /// <param name="downloadPreDownloads">Whether or not the Medias marked as pre-download should be downloaded. If downloadContent is set to true, the medias will get downloaded anyway.</param>
        /// <param name="onSuccess">Contains all the Asset ApiData stored in the selected. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        /// <param name="downloadTransferStatus"></param>
        public static IEnumerator GetChannelExperiencesProcess(int channelId, List<Asset> reusableList, bool downloadTargetImages, bool downloadContent, bool downloadPreDownloads,
            Action<List<Asset>> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null,
            DataTransferStatus downloadTransferStatus = null)
        {
            //Wait for internet check, or handle offline.
            while (!AppearitionGate.HasInternetAccessToEms.HasValue)
                yield return null;

            //Prepare the list
            if (reusableList == null)
                reusableList = new List<Asset>();
            else
                reusableList.Clear();

            Asset_List reusableApiContainer = GetReusableApiRequest<Asset_List>();
            Asset_List.PostData query = new Asset_List.PostData(ArTargetConstant.GetDefaultAssetListQuery());

            //Launch request
            var listAllExperiencesRequest =
                AppearitionRequest<Asset_List>.LaunchAPICall_POST<ArTargetHandler>(channelId, reusableApiContainer, query, null, obj => { reusableApiContainer = obj; });

            //Wait for request..
            while (!listAllExperiencesRequest.IsDone)
                yield return null;

            //All done!
            if (reusableApiContainer != null && reusableApiContainer.IsSuccess)
            {
                if (listAllExperiencesRequest.CurrentState == AppearitionBaseRequest<Asset_List>.RequestState.SuccessOnline)
                {
                    //Success ! If required, download the sub-content.
                    if (downloadTargetImages || downloadPreDownloads || downloadContent)
                        yield return DownloadAssetsContent(reusableApiContainer.Data.assets, downloadTargetImages, downloadPreDownloads, downloadContent, downloadTransferStatus);

                    AppearitionLogger.LogInfo(string.Format(ArTargetConstant.GET_CHANNEL_EXPERIENCES_SUCCESS, channelId));
                }
                else
                {
                    if (downloadTargetImages)
                        yield return DownloadAssetsContent(reusableApiContainer.Data.assets, true, false, false, downloadTransferStatus);

                    AppearitionLogger.LogInfo(string.Format(ArTargetConstant.GET_CHANNEL_EXPERIENCES_SUCCESS_OFFLINE, channelId));
                }

                if (reusableApiContainer.Data.assets != null && reusableApiContainer.Data.assets.Count > 0)
                    reusableList.AddRange(reusableApiContainer.Data.assets);
                //Callback it out ~
                if (onSuccess != null)
                    onSuccess(reusableList);
            }
            else
            {
                AppearitionLogger.LogError(string.Format(ArTargetConstant.GET_CHANNEL_EXPERIENCES_FAILURE, channelId));

                //Request failed =(
                if (onFailure != null)
                    onFailure(listAllExperiencesRequest.Errors);
            }

            if (onComplete != null)
                onComplete(listAllExperiencesRequest.RequestResponseObject.IsSuccess);
        }

        #endregion

        #region Get Media By Query

        /// <summary>
        /// Fetches all the Medias from an asset of a given Id using a dictionary of key value pair to query the desired content.
        /// </summary>
        /// <param name="assetId">The id of the asset you wish to query.</param>
        /// <param name="channelId">The id of the targeted channel.</param>
        /// <param name="onSuccess">Contains all the MediaFiles found from the given query. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void GetSpecificMediasInAssetByQuery(int channelId, string assetId, Action<List<MediaFile>> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            GetSpecificMediasInAssetByQuery(channelId, assetId, true, onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Fetches all the Medias from an asset of a given Id using a dictionary of key value pair to query the desired content.
        /// </summary>
        /// <param name="assetId">The id of the asset you wish to query.</param>
        /// <param name="channelId">The id of the targeted channel.</param>
        /// <param name="onSuccess">Contains all the MediaFiles found from the given query. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator GetSpecificMediasInAssetByQueryProcess(int channelId, string assetId,
            Action<List<MediaFile>> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            yield return GetSpecificMediasInAssetByQueryProcess
                (channelId, assetId, true, onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Fetches all the Medias from an asset of a given Id using a dictionary of key value pair to query the desired content.
        /// </summary>
        /// <param name="assetId">The id of the asset you wish to query.</param>
        /// <param name="channelId">The id of the targeted channel.</param>
        /// <param name="downloadAllContent">Whether or not the Medias should be downloaded. Do note that the EMS needs to allow this feature as well.</param>
        /// <param name="onSuccess">Contains all the MediaFiles found from the given query. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        /// <param name="downloadTransferStatus"></param>
        public static void GetSpecificMediasInAssetByQuery(int channelId, string assetId, bool downloadAllContent,
            Action<List<MediaFile>> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null, DataTransferStatus downloadTransferStatus = null)
        {
            AppearitionGate.Instance.StartCoroutine(GetSpecificMediasInAssetByQueryProcess
                (channelId, assetId, downloadAllContent, onSuccess, onFailure, onComplete, downloadTransferStatus));
        }

        /// <summary>
        /// Fetches all the Medias from an asset of a given Id using a dictionary of key value pair to query the desired content.
        /// </summary>
        /// <param name="assetId">The id of the asset you wish to query.</param>
        /// <param name="channelId">The id of the targeted channel.</param>
        /// <param name="downloadAllContent">Whether or not the Medias should be downloaded. Do note that the EMS needs to allow this feature as well.</param>
        /// <param name="onSuccess">Contains all the MediaFiles found from the given query. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        /// <param name="downloadTransferStatus"></param>
        public static IEnumerator GetSpecificMediasInAssetByQueryProcess(int channelId, string assetId, bool downloadAllContent,
            Action<List<MediaFile>> onSuccess, Action<EmsError> onFailure, Action<bool> onComplete, DataTransferStatus downloadTransferStatus = null)
        {
            if (string.IsNullOrWhiteSpace(assetId))
            {
                AppearitionLogger.LogError(ArTargetConstant.GET_SPECIFIC_MEDIAS_IN_ASSET_BY_QUERY_EMPTY_ID);
                if (onFailure != null)
                    onFailure(new EmsError(ArTargetConstant.GET_SPECIFIC_MEDIAS_IN_ASSET_BY_QUERY_EMPTY_ID));
                if (onComplete != null)
                    onComplete(false);
                yield break;
            }

            //Online request
            var postData = new Asset_MediaByAsset.PostData() {assetId = assetId};

            var assetMediaByAssetRequest =
                AppearitionRequest<Asset_MediaByAsset>.LaunchAPICall_POST<ArTargetHandler>(channelId, GetReusableApiRequest<Asset_MediaByAsset>(), postData);

            while (!assetMediaByAssetRequest.IsDone)
                yield return null;

            if (assetMediaByAssetRequest.RequestResponseObject.IsSuccess && assetMediaByAssetRequest.RequestResponseObject.Data != null)
            {
                if (assetMediaByAssetRequest.RequestResponseObject.Data.Count > 0)
                {
                    //Success ! If required, download the sub-content.
                    if (assetMediaByAssetRequest.CurrentState == AppearitionBaseRequest<Asset_MediaByAsset>.RequestState.SuccessOnline)
                    {
                        if (downloadAllContent)
                            yield return DownloadAssetsContent(new List<Asset>()
                                {new Asset() {assetId = assetId, mediaFiles = assetMediaByAssetRequest.RequestResponseObject.Data}}, false, false, true, downloadTransferStatus);
                    }

                    AppearitionLogger.LogInfo(string.Format(ArTargetConstant.GET_SPECIFIC_MEDIAS_IN_ASSET_BY_QUERY_SUCCESS,
                        assetId, assetMediaByAssetRequest.RequestResponseObject.Data.Count));
                }
                else
                {
                    AppearitionLogger.LogInfo(string.Format(ArTargetConstant.GET_SPECIFIC_MEDIAS_IN_ASSET_BY_QUERY_SUCCESS_NO_RESULTS, assetId));
                }

                if (onSuccess != null)
                    onSuccess(assetMediaByAssetRequest.RequestResponseObject.Data);
            }
            else
            {
                AppearitionLogger.LogError(string.Format(ArTargetConstant.GET_SPECIFIC_MEDIAS_IN_ASSET_BY_QUERY_FAILURE, assetId));

                if (onFailure != null)
                    onFailure(assetMediaByAssetRequest.Errors);
            }

            if (onComplete != null)
                onComplete(assetMediaByAssetRequest.RequestResponseObject.IsSuccess);
        }

        #endregion

        #region Get Experience By Query

        /// <summary>
        /// Submits a query to find specific experiences in the selected channel. Check out the query class for more info.
        /// The paging and quantities returned by the query can be changed within the query class.
        /// </summary>
        /// <param name="query">The given query</param>
        /// <param name="downloadTargetImages">Whether to download the target images of the experiences found.</param>
        /// <param name="downloadContent">Whether to download all the medias from the experiences found.</param>
        /// <param name="downloadPreDownloads">Whether to only download the medias from the experiences found with the pre-download flag on them.</param>
        /// <param name="onSuccess">Contains all the Experiences found from the given query. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        /// <param name="downloadTransferStatus">Transfer progress.</param>
        public static void GetSpecificExperiencesByQuery(Asset_List.QueryContent query,
            bool downloadTargetImages, bool downloadContent, bool downloadPreDownloads,
            Action<List<Asset>> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null, DataTransferStatus downloadTransferStatus = null)
        {
            AppearitionGate.Instance.StartCoroutine(GetSpecificExperiencesByQueryProcess(AppearitionGate.Instance.CurrentUser.selectedChannel, query, downloadTargetImages, downloadContent,
                downloadPreDownloads, onSuccess,
                onFailure, onComplete, downloadTransferStatus));
        }

        /// <summary>
        /// Submits a query to find specific experiences in the selected channel. Check out the query class for more info.
        /// The paging and quantities returned by the query can be changed within the query class.
        /// </summary>
        /// <param name="query">The given query</param>
        /// <param name="downloadTargetImages">Whether to download the target images of the experiences found.</param>
        /// <param name="downloadContent">Whether to download all the medias from the experiences found.</param>
        /// <param name="downloadPreDownloads">Whether to only download the medias from the experiences found with the pre-download flag on them.</param>
        /// <param name="onSuccess">Contains all the Experiences found from the given query. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        /// <param name="downloadTransferStatus">Transfer progress.</param>
        public static IEnumerator GetSpecificExperiencesByQueryProcess(Asset_List.QueryContent query,
            bool downloadTargetImages, bool downloadContent, bool downloadPreDownloads,
            Action<List<Asset>> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null, DataTransferStatus downloadTransferStatus = null)
        {
            yield return GetSpecificExperiencesByQueryProcess(AppearitionGate.Instance.CurrentUser.selectedChannel, query, downloadTargetImages, downloadContent, downloadPreDownloads, onSuccess,
                onFailure, onComplete, downloadTransferStatus);
        }

        /// <summary>
        /// Submits a query to find specific experiences in the selected channel. Check out the query class for more info.
        /// The paging and quantities returned by the query can be changed within the query class.
        /// </summary>
        /// <param name="channelId">The target channel ID</param>
        /// <param name="query">The given query</param>
        /// <param name="downloadTargetImages">Whether to download the target images of the experiences found.</param>
        /// <param name="downloadContent">Whether to download all the medias from the experiences found.</param>
        /// <param name="downloadPreDownloads">Whether to only download the medias from the experiences found with the pre-download flag on them.</param>
        /// <param name="onSuccess">Contains all the Experiences found from the given query. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        /// <param name="downloadTransferStatus">Transfer progress.</param>
        public static void GetSpecificExperiencesByQuery(int channelId, Asset_List.QueryContent query,
            bool downloadTargetImages, bool downloadContent, bool downloadPreDownloads,
            Action<List<Asset>> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null, DataTransferStatus downloadTransferStatus = null)
        {
            AppearitionGate.Instance.StartCoroutine(GetSpecificExperiencesByQueryProcess(channelId, query, downloadTargetImages, downloadContent,
                downloadPreDownloads, onSuccess, onFailure, onComplete, downloadTransferStatus));
        }

        /// <summary>
        /// Submits a query to find specific experiences in the selected channel.  Check out the query class for more info.
        /// The paging and quantities returned by the query can be changed within the query class.
        /// </summary>
        /// <param name="channelId">The target channel ID</param>
        /// <param name="query">The given query</param>
        /// <param name="downloadTargetImages">Whether to download the target images of the experiences found.</param>
        /// <param name="downloadContent">Whether to download all the medias from the experiences found.</param>
        /// <param name="downloadPreDownloads">Whether to only download the medias from the experiences found with the pre-download flag on them.</param>
        /// <param name="onSuccess">Contains all the Experiences found from the given query. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        /// <param name="downloadTransferStatus">Transfer progress.</param>
        public static IEnumerator GetSpecificExperiencesByQueryProcess(int channelId, Asset_List.QueryContent query,
            bool downloadTargetImages, bool downloadContent, bool downloadPreDownloads,
            Action<List<Asset>> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null, DataTransferStatus downloadTransferStatus = null)
        {
            if (query == null || (string.IsNullOrWhiteSpace(query.Name) && query.Tags.Count == 0 && string.IsNullOrWhiteSpace(query.CreatedByUsername) && query.RecordsPerPage == 0))
            {
                AppearitionLogger.LogError(ArTargetConstant.GET_SPECIFIC_EXPERIENCES_BY_QUERY_FAILURE_EMPTY_QUERY);
                if (onFailure != null)
                    onFailure(new EmsError(ArTargetConstant.GET_SPECIFIC_EXPERIENCES_BY_QUERY_FAILURE_EMPTY_QUERY));
                if (onComplete != null)
                    onComplete(false);
                yield break;
            }

            var assetListRequest =
                AppearitionRequest<Asset_List>.LaunchAPICall_POST<ArTargetHandler>(channelId, GetReusableApiRequest<Asset_List>(), new Asset_List.PostData(query));

            while (!assetListRequest.IsDone)
                yield return null;

            //All done!
            if (assetListRequest.RequestResponseObject.IsSuccess && assetListRequest.RequestResponseObject.Data != null)
            {
                if (assetListRequest.RequestResponseObject.Data.assets == null || assetListRequest.RequestResponseObject.Data.assets.Count == 0)
                {
                    AppearitionLogger.LogWarning(ArTargetConstant.GET_SPECIFIC_EXPERIENCES_BY_QUERY_SUCCESS_EMPTY);

                    //Callback it out ~
                    if (onSuccess != null)
                        onSuccess(new List<Asset>());
                }
                else
                {
                    if (assetListRequest.CurrentState == AppearitionBaseRequest<Asset_List>.RequestState.SuccessOnline)
                    {
                        //Success ! If required, download the sub-content.
                        if (downloadTargetImages || downloadPreDownloads || downloadContent)
                            yield return DownloadAssetsContent(assetListRequest.RequestResponseObject.Data.assets, downloadTargetImages, downloadPreDownloads, downloadContent, downloadTransferStatus);

                        AppearitionLogger.LogInfo(string.Format(ArTargetConstant.GET_SPECIFIC_EXPERIENCES_BY_QUERY_SUCCESS, assetListRequest.RequestResponseObject.Data.totalRecords));
                    }
                    else
                    {
                        if (downloadTargetImages)
                            yield return DownloadAssetsContent(assetListRequest.RequestResponseObject.Data.assets, true, false, false, downloadTransferStatus);
                        AppearitionLogger.LogInfo(string.Format(ArTargetConstant.GET_SPECIFIC_EXPERIENCES_BY_QUERY_SUCCESS_OFFLINE, assetListRequest.RequestResponseObject.Data.totalRecords));
                    }

                    //Callback it out ~
                    if (onSuccess != null)
                        onSuccess(assetListRequest.RequestResponseObject.Data.assets);
                }
            }
            else
            {
                AppearitionLogger.LogError(ArTargetConstant.GET_SPECIFIC_EXPERIENCES_BY_QUERY_FAILURE);

                //Request failed =(
                if (onFailure != null)
                    onFailure(assetListRequest.Errors);
            }

            if (onComplete != null)
                onComplete(assetListRequest.RequestResponseObject.IsSuccess);
        }

        #endregion

        #region Get Data Integration Result

        /// <summary>
        /// Get the latest Data Integration results for a given media.
        /// Optionally, a dictionary of parameters can be provided, which will be swapped with the query's [[]] parameters.
        /// </summary>
        /// <param name="asset">Associated Asset</param>
        /// <param name="media">Media of type query</param>
        /// <param name="onSuccess"></param>
        /// <param name="onFailure"></param>
        /// <param name="onComplete"></param>
        /// <returns></returns>
        public static void GetDataIntegrationResultP(Asset asset, MediaFile media,
            Action<string> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(GetDataIntegrationResultProcess(asset, media, null, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Get the latest Data Integration results for a given media.
        /// Optionally, a dictionary of parameters can be provided, which will be swapped with the query's [[]] parameters.
        /// </summary>
        /// <param name="asset">Associated Asset</param>
        /// <param name="media">Media of type query</param>
        /// <param name="onSuccess"></param>
        /// <param name="onFailure"></param>
        /// <param name="onComplete"></param>
        /// <returns></returns>
        public static IEnumerator GetDataIntegrationResultProcess(Asset asset, MediaFile media,
            Action<string> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            yield return GetDataIntegrationResultProcess(asset, media, null, onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Get the latest Data Integration results for a given media.
        /// Optionally, a dictionary of parameters can be provided, which will be swapped with the query's [[]] parameters.
        /// </summary>
        /// <param name="asset">Associated Asset</param>
        /// <param name="media">Media of type query</param>
        /// <param name="parameters">Additional parameters for the query</param>
        /// <param name="onSuccess"></param>
        /// <param name="onFailure"></param>
        /// <param name="onComplete"></param>
        /// <returns></returns>
        public static void GetDataIntegrationResultP(Asset asset, MediaFile media, Dictionary<string, string> parameters,
            Action<string> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(GetDataIntegrationResultProcess(asset, media, parameters, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Get the latest Data Integration results for a given media.
        /// Optionally, a dictionary of parameters can be provided, which will be swapped with the query's [[]] parameters.
        /// </summary>
        /// <param name="asset">Associated Asset</param>
        /// <param name="media">Media of type query</param>
        /// <param name="parameters">Additional parameters for the query</param>
        /// <param name="onSuccess"></param>
        /// <param name="onFailure"></param>
        /// <param name="onComplete"></param>
        /// <returns></returns>
        public static IEnumerator GetDataIntegrationResultProcess(Asset asset, MediaFile media, Dictionary<string, string> parameters,
            Action<string> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            //Ref Check
            if (asset == null || media == null)
            {
                AppearitionLogger.LogError(string.Format(ArTargetConstant.GET_DATA_INTEGRATION_RESULTS_NULL, asset, media));
                if (onFailure != null)
                    onFailure(new EmsError(string.Format(ArTargetConstant.GET_DATA_INTEGRATION_RESULTS_NULL, asset, media)));
                if (onComplete != null)
                    onComplete(false);
                yield break;
            }

            //Online request. Create content
            var settings = new List<Setting>();

            if (parameters != null)
            {
                foreach (var kvp in parameters)
                    settings.Add(new Setting(kvp.Key, kvp.Value));
            }

            var postContent = new Asset_DataByAsset.PostData {
                assetId = asset.assetId,
                arMediaId = media.arMediaId,
                parameters = settings
            };
            Debug.LogError(AppearitionConstants.SerializeJson(postContent));

            //Launch publish !
            AppearitionRequest<Asset_DataByAsset> dataIntegrationResultRequest =
                AppearitionRequest<Asset_DataByAsset>.LaunchAPICall_POST<ArTargetHandler>(asset.productId, GetReusableApiRequest<Asset_DataByAsset>(), postContent);

            while (!dataIntegrationResultRequest.IsDone)
                yield return null;

            //Debug output
            if (dataIntegrationResultRequest.RequestResponseObject != null && dataIntegrationResultRequest.RequestResponseObject.IsSuccess)
            {
                AppearitionLogger.LogInfo(string.Format(ArTargetConstant.GET_DATA_INTEGRATION_RESULTS_SUCCESS, media.arMediaId));
                if (onSuccess != null)
                    onSuccess(dataIntegrationResultRequest.RequestResponseObject.Data);
            }
            else
            {
                AppearitionLogger.LogError(string.Format(ArTargetConstant.GET_DATA_INTEGRATION_RESULTS_FAILURE, media.arMediaId));

                if (onFailure != null)
                    onFailure(dataIntegrationResultRequest.Errors);
            }

            if (onComplete != null)
                onComplete(dataIntegrationResultRequest.RequestResponseObject != null && dataIntegrationResultRequest.RequestResponseObject.IsSuccess);
        }

        #endregion

        #endregion

        #region Create AR Target / Experience

        /// <summary>
        /// Create a new experience in the current user's tenant and channel.
        /// </summary>
        /// <param name="experienceName">The name of the experience you wish to create.</param>
        /// <param name="onSuccess">Contains the ArTarget object freshly created and synced with the cloud. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void CreateExperience(string experienceName, Action<ArTarget> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(CreateExperienceProcess(AppearitionGate.Instance.CurrentUser.selectedChannel, experienceName, null, null, true, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Create a new experience in the current user's tenant and channel.
        /// </summary>
        /// <param name="experienceName">The name of the experience you wish to create.</param>
        /// <param name="onSuccess">Contains the ArTarget object freshly created and synced with the cloud. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator CreateExperienceProcess(string experienceName, Action<ArTarget> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            yield return CreateExperienceProcess(AppearitionGate.Instance.CurrentUser.selectedChannel, experienceName, null, null, true, onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Create a new experience in the current user's tenant and a given channel id.
        /// </summary>
        /// <param name="channelId">The id of the targeted channel.</param>
        /// <param name="experienceName">The name of the experience you wish to create.</param>
        /// <param name="onSuccess">Contains the ArTarget object freshly created and synced with the cloud. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void CreateExperience(int channelId, string experienceName, Action<ArTarget> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(CreateExperienceProcess(channelId, experienceName, null, null, true, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Create a new experience in the current user's tenant and a given channel id.
        /// </summary>
        /// <param name="channelId">The id of the targeted channel.</param>
        /// <param name="experienceName">The name of the experience you wish to create.</param>
        /// <param name="onSuccess">Contains the ArTarget object freshly created and synced with the cloud. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator CreateExperienceProcess(int channelId, string experienceName, Action<ArTarget> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            yield return CreateExperienceProcess(channelId, experienceName, null, null, true, onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Create a new experience in the current user's tenant and a given channel id, using provided experience name, targetImage and filename.
        /// Optionally, this experience can be published once created. This will make it visible to Asset processes.
        /// </summary>
        /// <param name="experienceName">The name of the experience you wish to create.</param>
        /// <param name="publishExperience">Whether or not this experience should be published once created.</param>
        /// <param name="onSuccess">Contains the ArTarget object freshly created and synced with the cloud. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void CreateExperience(string experienceName, bool publishExperience,
            Action<ArTarget> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(CreateExperienceProcess(AppearitionGate.Instance.CurrentUser.selectedChannel, experienceName, null, null, publishExperience, onSuccess, onFailure,
                onComplete));
        }

        /// <summary>
        /// Create a new experience in the current user's tenant and a given channel id, using provided experience name, targetImage and filename.
        /// Optionally, this experience can be published once created. This will make it visible to Asset processes.
        /// </summary>
        /// <param name="experienceName">The name of the experience you wish to create.</param>
        /// <param name="publishExperience">Whether or not this experience should be published once created.</param>
        /// <param name="onSuccess">Contains the ArTarget object freshly created and synced with the cloud. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator CreateExperienceProcess(string experienceName, bool publishExperience,
            Action<ArTarget> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            yield return CreateExperienceProcess(AppearitionGate.Instance.CurrentUser.selectedChannel, experienceName, null, null, publishExperience, onSuccess, onFailure,
                onComplete);
        }

        /// <summary>
        /// Create a new experience in the current user's tenant and a given channel id, using provided experience name, targetImage and filename.
        /// Optionally, this experience can be published once created. This will make it visible to Asset processes.
        /// </summary>
        /// <param name="channelId">The id of the targeted channel.</param>
        /// <param name="experienceName">The name of the experience you wish to create.</param>
        /// <param name="publishExperience">Whether or not this experience should be published once created.</param>
        /// <param name="onSuccess">Contains the ArTarget object freshly created and synced with the cloud. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void CreateExperience(int channelId, string experienceName, bool publishExperience,
            Action<ArTarget> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(CreateExperienceProcess(channelId, experienceName, null, null, publishExperience, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Create a new experience in the current user's tenant and a given channel id, using provided experience name, targetImage and filename.
        /// Optionally, this experience can be published once created. This will make it visible to Asset processes.
        /// </summary>
        /// <param name="channelId">The id of the targeted channel.</param>
        /// <param name="experienceName">The name of the experience you wish to create.</param>
        /// <param name="publishExperience">Whether or not this experience should be published once created.</param>
        /// <param name="onSuccess">Contains the ArTarget object freshly created and synced with the cloud. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator CreateExperienceProcess(int channelId, string experienceName, bool publishExperience,
            Action<ArTarget> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            yield return CreateExperienceProcess(channelId, experienceName, null, null, publishExperience, onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Create a new experience in the current user's tenant and channel, using provided experience name, targetImage and filename.
        /// By default, this experience will be published once created, making it available to Asset processes.
        /// </summary>
        /// <param name="experienceName">The name of the experience you wish to create.</param>
        /// <param name="newTargetImage">The TargetImage of the ArTarget being created.</param>
        /// <param name="filename">The name of the TargetImage being uploaded.</param>
        /// <param name="onSuccess">Contains the ArTarget object freshly created and synced with the cloud. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void CreateExperience(string experienceName, Texture newTargetImage, string filename,
            Action<ArTarget> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(CreateExperienceProcess(AppearitionGate.Instance.CurrentUser.selectedChannel, experienceName, newTargetImage, filename, true, onSuccess, onFailure,
                onComplete));
        }

        /// <summary>
        /// Create a new experience in the current user's tenant and channel, using provided experience name, targetImage and filename.
        /// By default, this experience will be published once created, making it available to Asset processes.
        /// </summary>
        /// <param name="experienceName">The name of the experience you wish to create.</param>
        /// <param name="newTargetImage">The TargetImage of the ArTarget being created.</param>
        /// <param name="filename">The name of the TargetImage being uploaded.</param>
        /// <param name="onSuccess">Contains the ArTarget object freshly created and synced with the cloud. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator CreateExperienceProcess(string experienceName, Texture newTargetImage, string filename,
            Action<ArTarget> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            yield return CreateExperienceProcess(AppearitionGate.Instance.CurrentUser.selectedChannel, experienceName, newTargetImage, filename, true, onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Create a new experience in the current user's tenant and a given channel id, using provided experience name, targetImage and filename.
        /// By default, this experience will be published once created, making it available to Asset processes.
        /// </summary>
        /// <param name="channelId">The id of the targeted channel.</param>
        /// <param name="experienceName">The name of the experience you wish to create.</param>
        /// <param name="newTargetImage">The TargetImage of the ArTarget being created.</param>
        /// <param name="filename">The name of the TargetImage being uploaded.</param>
        /// <param name="onSuccess">Contains the ArTarget object freshly created and synced with the cloud. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void CreateExperience(int channelId, string experienceName, Texture newTargetImage, string filename,
            Action<ArTarget> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(CreateExperienceProcess(channelId, experienceName, newTargetImage, filename, true, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Create a new experience in the current user's tenant and a given channel id, using provided experience name, targetImage and filename.
        /// By default, this experience will be published once created, making it available to Asset processes.
        /// </summary>
        /// <param name="channelId">The id of the targeted channel.</param>
        /// <param name="experienceName">The name of the experience you wish to create.</param>
        /// <param name="newTargetImage">The TargetImage of the ArTarget being created.</param>
        /// <param name="filename">The name of the TargetImage being uploaded.</param>
        /// <param name="onSuccess">Contains the ArTarget object freshly created and synced with the cloud. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator CreateExperienceProcess(int channelId, string experienceName, Texture newTargetImage, string filename,
            Action<ArTarget> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            yield return CreateExperienceProcess(channelId, experienceName, newTargetImage, filename, true, onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Create a new experience in the current user's tenant and a given channel id, using provided experience name, targetImage and filename.
        /// Optionally, this experience can be published once created. This will make it visible to Asset processes.
        /// </summary>
        /// <param name="experienceName">The name of the experience you wish to create.</param>
        /// <param name="newTargetImage">The TargetImage of the ArTarget being created.</param>
        /// <param name="filename">The name of the TargetImage being uploaded.</param>
        /// <param name="publishExperience">Whether or not this experience should be published once created.</param>
        /// <param name="onSuccess">Contains the ArTarget object freshly created and synced with the cloud. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        /// <param name="uploadTransferStatus"></param>
        public static void CreateExperience(string experienceName, Texture newTargetImage, string filename, bool publishExperience,
            Action<ArTarget> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null, DataTransferStatus uploadTransferStatus = null)
        {
            AppearitionGate.Instance.StartCoroutine(CreateExperienceProcess(AppearitionGate.Instance.CurrentUser.selectedChannel, experienceName, newTargetImage, filename, publishExperience,
                onSuccess, onFailure, onComplete, uploadTransferStatus));
        }

        /// <summary>
        /// Create a new experience in the current user's tenant and a given channel id, using provided experience name, targetImage and filename.
        /// Optionally, this experience can be published once created. This will make it visible to Asset processes.
        /// </summary>
        /// <param name="experienceName">The name of the experience you wish to create.</param>
        /// <param name="newTargetImage">The TargetImage of the ArTarget being created.</param>
        /// <param name="filename">The name of the TargetImage being uploaded.</param>
        /// <param name="publishExperience">Whether or not this experience should be published once created.</param>
        /// <param name="onSuccess">Contains the ArTarget object freshly created and synced with the cloud. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        /// <param name="uploadTransferStatus"></param>
        public static IEnumerator CreateExperienceProcess(string experienceName, Texture newTargetImage, string filename, bool publishExperience,
            Action<ArTarget> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null, DataTransferStatus uploadTransferStatus = null)
        {
            yield return CreateExperienceProcess(AppearitionGate.Instance.CurrentUser.selectedChannel, experienceName, newTargetImage, filename, publishExperience,
                onSuccess, onFailure, onComplete, uploadTransferStatus);
        }


        /// <summary>
        /// Create a new experience in the current user's tenant and a given channel id, using provided experience name, targetImage and filename.
        /// Optionally, this experience can be published once created. This will make it visible to Asset processes.
        /// </summary>
        /// <param name="channelId">The id of the targeted channel.</param>
        /// <param name="experienceName">The name of the experience you wish to create.</param>
        /// <param name="newTargetImage">The TargetImage of the ArTarget being created.</param>
        /// <param name="filename">The name of the TargetImage being uploaded.</param>
        /// <param name="publishExperience">Whether or not this experience should be published once created.</param>
        /// <param name="onSuccess">Contains the ArTarget object freshly created and synced with the cloud. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        /// <param name="uploadTransferStatus"></param>
        public static void CreateExperience(int channelId, string experienceName, Texture newTargetImage, string filename, bool publishExperience,
            Action<ArTarget> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null, DataTransferStatus uploadTransferStatus = null)
        {
            AppearitionGate.Instance.StartCoroutine(CreateExperienceProcess(channelId, experienceName, newTargetImage, filename, publishExperience, onSuccess, onFailure, onComplete,
                uploadTransferStatus));
        }

        /// <summary>
        /// Create a new experience in the current user's tenant and a given channel id, using provided experience name, targetImage and filename.
        /// Optionally, this experience can be published once created. This will make it visible to Asset processes.
        /// </summary>
        /// <param name="channelId">The id of the targeted channel.</param>
        /// <param name="experienceName">The name of the experience you wish to create.</param>
        /// <param name="newTargetImage">The TargetImage of the ArTarget being created.</param>
        /// <param name="filename">The name of the TargetImage being uploaded.</param>
        /// <param name="publishExperience">Whether or not this experience should be published once created.</param>
        /// <param name="onSuccess">Contains the ArTarget object freshly created and synced with the cloud. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        /// <param name="uploadTransferStatus"></param>
        public static IEnumerator CreateExperienceProcess(int channelId, string experienceName, Texture newTargetImage, string filename, bool publishExperience,
            Action<ArTarget> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null, DataTransferStatus uploadTransferStatus = null)
        {
            //Online request. Assume that the user is logged in. First, launch a CREATE post
            ArTarget_Create.PostData createPostData = new ArTarget_Create.PostData() {
                name = experienceName,
                productId = channelId
            };

            var createRequest = AppearitionRequest<ArTarget_Create>.LaunchAPICall_POST<ArTargetHandler>(channelId, GetReusableApiRequest<ArTarget_Create>(), createPostData);

            while (!createRequest.IsDone)
            {
                yield return null;
            }

            ArTarget workingArTarget = null;

            if (createRequest.RequestResponseObject != null && createRequest.RequestResponseObject.IsSuccess)
            {
                AppearitionLogger.LogDebug(string.Format(ArTargetConstant.CREATE_EXPERIENCE_SUCCESS,
                    createRequest.ResponseCode, createRequest.RequestResponseObject.Data.arTargetId));

                workingArTarget = createRequest.RequestResponseObject.Data;
            }
            else
            {
                AppearitionLogger.LogError(string.Format(ArTargetConstant.CREATE_EXPERIENCE_FAILURE, experienceName));
                if (onFailure != null)
                    onFailure(createRequest.Errors);

                if (onComplete != null)
                    onComplete(false);
                yield break;
            }

            //If there is an image attached, upload it as well !
            if (newTargetImage != null)
            {
                bool success = false;
                yield return AddTargetImageToArTargetProcess(createRequest.RequestResponseObject.Data, filename, newTargetImage, null, onFailure,
                    isTargetImageUploadSuccess => { success = isTargetImageUploadSuccess; }, uploadTransferStatus);

                if (!success)
                {
                    if (onComplete != null)
                        onComplete(false);
                    yield break;
                }
            }

            if (publishExperience)
            {
                bool publishSuccess = false;
                yield return PublishExperienceProcess(workingArTarget, null, onFailure, isPublishSuccess => { publishSuccess = isPublishSuccess; });

                if (publishSuccess)
                    workingArTarget.isPublished = true;
                else
                {
                    if (onComplete != null)
                        onComplete(false);
                    yield break;
                }
            }

            if (onSuccess != null)
                onSuccess(workingArTarget);

            if (onComplete != null)
                onComplete(createRequest.RequestResponseObject.IsSuccess);
        }

        #endregion

        #region Create Media

        /// <summary>
        /// Adds a new Media to the experience of a given ArTargetId.
        /// </summary>
        /// <param name="arTarget">The experience you wish to add a media to.</param>
        /// <param name="data">The template MediaFile ApiData to upload on the given ArTarget.</param>
        /// <param name="onSuccess">Contains the MediaFile object freshly synced with the cloud. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void AddMediaToExistingExperience(ArTarget arTarget, MediaFile data, Action<ArTarget, MediaFile> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(AddMediaToExistingExperienceProcess(arTarget, data, null, false, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Adds a new Media to the experience of a given ArTargetId.
        /// </summary>
        /// <param name="arTarget">The experience you wish to add a media to.</param>
        /// <param name="data">The template MediaFile ApiData to upload on the given ArTarget.</param>
        /// <param name="onSuccess">Contains the MediaFile object freshly synced with the cloud. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator AddMediaToExistingExperienceProcess(ArTarget arTarget, MediaFile data, Action<ArTarget, MediaFile> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            yield return AddMediaToExistingExperienceProcess(arTarget, data, null, false, onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Adds a new Media to the experience of a given ArTargetId.
        /// Once complete, you can choose to publish this experience, making it visible when using any Asset processes.
        /// If the experience was already published, there is no need to publish it again.
        /// </summary>
        /// <param name="arTarget">The experience you wish to add a media to.</param>
        /// <param name="data">The template MediaFile ApiData to upload on the given ArTarget.</param>
        /// <param name="publishOnComplete">Whether the experience should be published once the media has been successfully uploaded.</param>
        /// <param name="onSuccess">Contains the MediaFile object freshly synced with the cloud. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void AddMediaToExistingExperience(ArTarget arTarget, MediaFile data, bool publishOnComplete,
            Action<ArTarget, MediaFile> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(AddMediaToExistingExperienceProcess(arTarget, data, null, publishOnComplete, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Adds a new Media to the experience of a given ArTargetId.
        /// Once complete, you can choose to publish this experience, making it visible when using any Asset processes.
        /// If the experience was already published, there is no need to publish it again.
        /// </summary>
        /// <param name="arTarget">The experience you wish to add a media to.</param>
        /// <param name="data">The template MediaFile ApiData to upload on the given ArTarget.</param>
        /// <param name="publishOnComplete">Whether the experience should be published once the media has been successfully uploaded.</param>
        /// <param name="onSuccess">Contains the MediaFile object freshly synced with the cloud. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator AddMediaToExistingExperienceProcess(ArTarget arTarget, MediaFile data, bool publishOnComplete,
            Action<ArTarget, MediaFile> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            yield return AddMediaToExistingExperienceProcess(arTarget, data, null, publishOnComplete, onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Adds a new Media to the experience of a given ArTargetId.
        /// Optionally, a file can be uploaded and attached to this media.
        /// </summary>
        /// <param name="arTarget">The experience you wish to add a media to.</param>
        /// <param name="data">The template MediaFile ApiData to upload on the given ArTarget.</param>
        /// <param name="mediaToUpload">Media object to upload and attach to the MediaFile.</param>
        /// <param name="onSuccess">Contains the MediaFile object freshly synced with the cloud. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void AddMediaToExistingExperience(ArTarget arTarget, MediaFile data, object mediaToUpload,
            Action<ArTarget, MediaFile> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(AddMediaToExistingExperienceProcess(arTarget, data, mediaToUpload, false, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Adds a new Media to the experience of a given ArTargetId.
        /// Optionally, a file can be uploaded and attached to this media.
        /// </summary>
        /// <param name="arTarget">The experience you wish to add a media to.</param>
        /// <param name="data">The template MediaFile ApiData to upload on the given ArTarget.</param>
        /// <param name="mediaToUpload">Media object to upload and attach to the MediaFile.</param>
        /// <param name="onSuccess">Contains the MediaFile object freshly synced with the cloud. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator AddMediaToExistingExperienceProcess(ArTarget arTarget, MediaFile data, object mediaToUpload,
            Action<ArTarget, MediaFile> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            yield return AddMediaToExistingExperienceProcess(arTarget, data, mediaToUpload, false, onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Adds a new Media to the experience of a given ArTargetId.
        /// Optionally, a file can be uploaded and attached to this media.
        /// Once complete, you can choose to publish this experience, making it visible when using any Asset processes.
        /// If the experience was already published, there is no need to publish it again.
        /// </summary>
        /// <param name="arTarget">The experience you wish to add a media to.</param>
        /// <param name="data">The template MediaFile ApiData to upload on the given ArTarget.</param>
        /// <param name="mediaToUpload">Media object to upload and attach to the MediaFile.</param>
        /// <param name="publishOnComplete">Whether the experience should be published once the media has been successfully uploaded.</param>
        /// <param name="onSuccess">Contains the MediaFile object freshly synced with the cloud. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        /// <param name="uploadTransferStatus"></param>
        public static void AddMediaToExistingExperience(ArTarget arTarget, MediaFile data, object mediaToUpload, bool publishOnComplete,
            Action<ArTarget, MediaFile> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null, DataTransferStatus uploadTransferStatus = null)
        {
            AppearitionGate.Instance.StartCoroutine(AddMediaToExistingExperienceProcess(arTarget, data, mediaToUpload, publishOnComplete, onSuccess, onFailure, onComplete,
                uploadTransferStatus));
        }

        /// <summary>
        /// Adds a new Media to the experience of a given ArTargetId.
        /// Optionally, a file can be uploaded and attached to this media.
        /// Once complete, you can choose to publish this experience, making it visible when using any Asset processes.
        /// If the experience was already published, there is no need to publish it again.
        /// </summary>
        /// <param name="arTarget">The experience you wish to add a media to.</param>
        /// <param name="data">The template MediaFile ApiData to upload on the given ArTarget.</param>
        /// <param name="mediaToUpload">Media object to upload and attach to the MediaFile.</param>
        /// <param name="publishOnComplete">Whether the experience should be published once the media has been successfully uploaded.</param>
        /// <param name="onSuccess">Contains the MediaFile object freshly synced with the cloud. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        /// <param name="uploadTransferStatus"></param>
        public static IEnumerator AddMediaToExistingExperienceProcess(ArTarget arTarget, MediaFile data, object mediaToUpload, bool publishOnComplete,
            Action<ArTarget, MediaFile> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null, DataTransferStatus uploadTransferStatus = null)
        {
            //Ref Check
            if (arTarget == null)
            {
                AppearitionLogger.LogError(ArTargetConstant.ADD_MEDIA_TO_EXISTING_EXPERIENCE_NULL);
                if (onFailure != null)
                    onFailure(new EmsError(ArTargetConstant.ADD_MEDIA_TO_EXISTING_EXPERIENCE_NULL));
                if (onComplete != null)
                    onComplete(false);
                yield break;
            }

            //Prepare outcome variables
            MediaFile outcomeMediaFile;

            //Online request. If the user is trying to upload an empty media, call CreateMedia instead.
            if (mediaToUpload == null)
            {
                //Online request. Create the ApiData
                var createMediaPostData = new ArTarget_CreateMedia.PostData(data);
                var createMediaRequestContent = new ArTarget_CreateMedia.RequestContent() {arTargetId = arTarget.arTargetId};

                //Launch the request !
                var createMediaRequest = AppearitionRequest<ArTarget_CreateMedia>.LaunchAPICall_POST<ArTargetHandler>
                    (arTarget.productId, GetReusableApiRequest<ArTarget_CreateMedia>(), createMediaPostData, createMediaRequestContent);

                while (!createMediaRequest.IsDone)
                    yield return null;

                //Handle creation _errors
                if (createMediaRequest.RequestResponseObject != null && createMediaRequest.RequestResponseObject.IsSuccess)
                {
                    AppearitionLogger.LogInfo(ArTargetConstant.ADD_MEDIA_TO_EXISTING_EXPERIENCE_SUCCESS);
                    outcomeMediaFile = createMediaRequest.RequestResponseObject.Data;
                }
                else
                {
                    AppearitionLogger.LogError(ArTargetConstant.ADD_MEDIA_TO_EXISTING_EXPERIENCE_FAILURE);
                    if (onFailure != null)
                        onFailure(createMediaRequest.Errors);
                    if (onComplete != null)
                        onComplete(false);
                    yield break;
                }
            }
            else
            {
                //Create the upload multipart form ApiData
                List<MultiPartFormParam> multiPartParam = new List<MultiPartFormParam>() {
                    //Don't convert if it's a Texture or Texture2D. Those are not serializable for.. reasons..
                    new MultiPartFormParam(data.fileName + "_Media", mediaToUpload, data.fileName, data.mimeType)
                };

                //Create the URL extra params
                var requestContent = new ArTarget_UploadMedia.RequestContent() {
                    arTargetId = arTarget.arTargetId
                };

                //Launch the request
                var uploadRequest =
                    AppearitionRequest<ArTarget_UploadMedia>.LaunchAPICall_MultiPartPOST<ArTargetHandler>(arTarget.productId, GetReusableApiRequest<ArTarget_UploadMedia>(), requestContent,
                        multiPartParam,
                        data.mediaType);

                string uploadItemKey = new Guid().ToString();
                bool isTrackingUpload = uploadTransferStatus != null;
                if (isTrackingUpload)
                    uploadTransferStatus.AddNewItemProgress(uploadItemKey, data.fileName,
                        uploadRequest.SizeOfFileBeingUploaded > 0 ? uploadRequest.SizeOfFileBeingUploaded : ArTargetConstant.FILE_UPLOAD_PLACEHOLDER_SIZE);

                while (!uploadRequest.IsDone)
                {
                    if (isTrackingUpload)
                        uploadTransferStatus.UpdateItemProgress(uploadItemKey, uploadRequest.Progress);
                    yield return null;
                }

                //Debug output
                if (uploadRequest.RequestResponseObject == null || !uploadRequest.RequestResponseObject.IsSuccess)
                {
                    AppearitionLogger.LogError(ArTargetConstant.ADD_MEDIA_TO_EXISTING_EXPERIENCE_FAILURE);

                    if (onFailure != null)
                        onFailure(uploadRequest.Errors);
                    if (onComplete != null)
                        onComplete(false);
                    yield break;
                }

                //For now, once uploaded, update its content based on the given content. This will be fixed at later date.
                MediaFile recentlyUploadedMediaFile = uploadRequest.RequestResponseObject.Data;
                MediaFile workingMediaFile = new MediaFile(data) {
                    arMediaId = recentlyUploadedMediaFile.arMediaId,
                    checksum = recentlyUploadedMediaFile.checksum,
                    url = recentlyUploadedMediaFile.url,
                    fileName = recentlyUploadedMediaFile.fileName,
                    mediaType = recentlyUploadedMediaFile.mediaType,
                    mimeType = recentlyUploadedMediaFile.mimeType
                };

                var updateMediaPostData = new ArTarget_UpdateMediaSettings.PostData(workingMediaFile);
                var updateMediaRequestContent = new ArTarget_UpdateMediaSettings.RequestContent() {
                    arTargetId = arTarget.arTargetId,
                    arMediaId = workingMediaFile.arMediaId
                };

                var updateMediaRequest =
                    AppearitionRequest<ArTarget_UpdateMediaSettings>.LaunchAPICall_POST<ArTargetHandler>(arTarget.productId, GetReusableApiRequest<ArTarget_UpdateMediaSettings>(), updateMediaPostData,
                        updateMediaRequestContent);

                while (!updateMediaRequest.IsDone)
                    yield return null;

                //Handle update media outcome
                if (updateMediaRequest.RequestResponseObject != null && updateMediaRequest.RequestResponseObject.IsSuccess)
                {
                    AppearitionLogger.LogInfo(ArTargetConstant.ADD_MEDIA_TO_EXISTING_EXPERIENCE_SUCCESS);
                    outcomeMediaFile = workingMediaFile;
                }
                else
                {
                    AppearitionLogger.LogError(ArTargetConstant.ADD_MEDIA_TO_EXISTING_EXPERIENCE_FAILURE);

                    if (onFailure != null)
                        onFailure(updateMediaRequest.Errors);
                    if (onComplete != null)
                        onComplete(false);
                    yield break;
                }
            }

            //Publish if requested and if required.
            if (publishOnComplete)
            {
                bool publishSuccess = false;
                yield return PublishExperienceProcess(arTarget, null, onFailure, isPublishSuccess => { publishSuccess = isPublishSuccess; });

                if (publishSuccess)
                    arTarget.isPublished = true;
                else
                {
                    if (onComplete != null)
                        onComplete(false);
                    yield break;
                }
            }

            //Lastly, update the ArTarget
            bool isRefreshSuccess = false;
            yield return GetSingleArTargetProcess(arTarget.productId, arTarget.arTargetId, false, false, onRefreshSuccess => arTarget = onRefreshSuccess, onFailure,
                onRefreshComplete => isRefreshSuccess = onRefreshComplete, uploadTransferStatus);

            //Finally, callback
            if (!isRefreshSuccess)
            {
                if (onComplete != null)
                    onComplete(false);
            }
            else
            {
                if (onSuccess != null)
                    onSuccess(arTarget, outcomeMediaFile);
                if (onComplete != null)
                    onComplete(isRefreshSuccess);
            }
        }

        #endregion

        #region Manage Experience

        #region Handle ArTarget

        #region Get and List

        /// <summary>
        /// Fetches a single ArTarget from the EMS, using a given ArTargetId.
        /// Do note that this request requires the user to be logged in.
        /// </summary>
        /// <param name="arTargetId">The Id of the desired ArTarget.</param>
        /// <param name="onSuccess">Contains the ArTarget of a given ArTargetId, if found in the given channel. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void GetSingleArTarget(int arTargetId, Action<ArTarget> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            GetSingleArTarget(AppearitionGate.Instance.CurrentUser.selectedChannel, arTargetId, true, true, onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Fetches a single ArTarget from the EMS, using a given ArTargetId.
        /// Do note that this request requires the user to be logged in.
        /// </summary>
        /// <param name="arTargetId">The Id of the desired ArTarget.</param>
        /// <param name="onSuccess">Contains the ArTarget of a given ArTargetId, if found in the given channel. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator GetSingleArTargetProcess(int arTargetId, Action<ArTarget> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            yield return GetSingleArTargetProcess
                (AppearitionGate.Instance.CurrentUser.selectedChannel, arTargetId, true, true, onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Fetches a single ArTarget from the EMS, using a given ArTargetId.
        /// Do note that this request requires the user to be logged in.
        /// </summary>
        /// <param name="channelId">The id of the targeted channel.</param>
        /// <param name="arTargetId">The Id of the desired ArTarget.</param>
        /// <param name="onSuccess">Contains the ArTarget of a given ArTargetId, if found in the given channel. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void GetSingleArTarget(int channelId, int arTargetId, Action<ArTarget> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            GetSingleArTarget(channelId, arTargetId, true, true, onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Fetches a single ArTarget from the EMS, using a given ArTargetId.
        /// Do note that this request requires the user to be logged in.
        /// </summary>
        /// <param name="channelId">The id of the targeted channel.</param>
        /// <param name="arTargetId">The Id of the desired ArTarget.</param>
        /// <param name="onSuccess">Contains the ArTarget of a given ArTargetId, if found in the given channel. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator GetSingleArTargetProcess(int channelId, int arTargetId, Action<ArTarget> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            yield return GetSingleArTargetProcess(channelId, arTargetId, true, true, onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Fetches a single ArTarget from the EMS, using a given ArTargetId.
        /// Do note that this request requires the user to be logged in.
        /// </summary>
        /// <param name="arTargetId">The Id of the desired ArTarget.</param>
        /// <param name="downloadTargetImages">Whether or not the Target Image should be downloaded. Do note that the EMS needs to allow this feature as well.</param>
        /// <param name="downloadContent">Whether or not the Medias should be downloaded. Do note that the EMS needs to allow this feature as well.</param>
        /// <param name="onSuccess">Contains the ArTarget of a given ArTargetId, if found in the given channel. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        /// <param name="downloadTransferStatus"></param>
        public static void GetSingleArTarget(int arTargetId, bool downloadTargetImages, bool downloadContent, Action<ArTarget> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null, DataTransferStatus downloadTransferStatus = null)
        {
            GetSingleArTarget(AppearitionGate.Instance.CurrentUser.selectedChannel, arTargetId,
                downloadTargetImages, downloadContent, onSuccess, onFailure, onComplete, downloadTransferStatus);
        }

        /// <summary>
        /// Fetches a single ArTarget from the EMS, using a given ArTargetId.
        /// Do note that this request requires the user to be logged in.
        /// </summary>
        /// <param name="arTargetId">The Id of the desired ArTarget.</param>
        /// <param name="downloadTargetImages">Whether or not the Target Image should be downloaded. Do note that the EMS needs to allow this feature as well.</param>
        /// <param name="downloadContent">Whether or not the Medias should be downloaded. Do note that the EMS needs to allow this feature as well.</param>
        /// <param name="onSuccess">Contains the ArTarget of a given ArTargetId, if found in the given channel. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        /// <param name="downloadTransferStatus"></param>
        public static IEnumerator GetSingleArTargetProcess(int arTargetId, bool downloadTargetImages, bool downloadContent, Action<ArTarget> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null, DataTransferStatus downloadTransferStatus = null)
        {
            yield return GetSingleArTargetProcess(AppearitionGate.Instance.CurrentUser.selectedChannel, arTargetId,
                downloadTargetImages, downloadContent, onSuccess, onFailure, onComplete, downloadTransferStatus);
        }

        /// <summary>
        /// Fetches a single ArTarget from the EMS, using a given ArTargetId.
        /// Do note that this request requires the user to be logged in.
        /// </summary>
        /// <param name="channelId">The id of the targeted channel.</param>
        /// <param name="arTargetId">The Id of the desired ArTarget.</param>
        /// <param name="downloadTargetImages">Whether or not the Target Image should be downloaded. Do note that the EMS needs to allow this feature as well.</param>
        /// <param name="downloadContent">Whether or not the Medias should be downloaded. Do note that the EMS needs to allow this feature as well.</param>
        /// <param name="onSuccess">Contains the ArTarget of a given ArTargetId, if found in the given channel. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        /// <param name="downloadTransferStatus"></param>
        public static void GetSingleArTarget(int arTargetId, int channelId, bool downloadTargetImages, bool downloadContent,
            Action<ArTarget> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null, DataTransferStatus downloadTransferStatus = null)
        {
            AppearitionGate.Instance.StartCoroutine(GetSingleArTargetProcess
                (channelId, arTargetId, downloadTargetImages, downloadContent, onSuccess, onFailure, onComplete, downloadTransferStatus));
        }

        /// <summary>
        /// Fetches a single ArTarget from the EMS, using a given ArTargetId.
        /// Do note that this request requires the user to be logged in.
        /// </summary>
        /// <param name="channelId">The id of the targeted channel.</param>
        /// <param name="arTargetId">The Id of the desired ArTarget.</param>
        /// <param name="downloadTargetImages">Whether or not the Target Image should be downloaded. Do note that the EMS needs to allow this feature as well.</param>
        /// <param name="downloadContent">Whether or not the Medias should be downloaded. Do note that the EMS needs to allow this feature as well.</param>
        /// <param name="onSuccess">Contains the ArTarget of a given ArTargetId, if found in the given channel. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        /// <param name="downloadTransferStatus"></param>
        public static IEnumerator GetSingleArTargetProcess(int channelId, int arTargetId, bool downloadTargetImages, bool downloadContent,
            Action<ArTarget> onSuccess, Action<EmsError> onFailure, Action<bool> onComplete, DataTransferStatus downloadTransferStatus = null)
        {
            ArTarget_Get.RequestContent requestContent = new ArTarget_Get.RequestContent() {arTargetId = arTargetId};

            var arTargetRequestGet =
                AppearitionRequest<ArTarget_Get>.LaunchAPICall_GET<ArTargetHandler>(channelId, GetReusableApiRequest<ArTarget_Get>(), requestContent);

            while (!arTargetRequestGet.IsDone)
                yield return null;

            if (arTargetRequestGet.RequestResponseObject.IsSuccess && arTargetRequestGet.RequestResponseObject.Data != null)
            {
                //Success ! If required, download the sub-content.
                if (downloadTargetImages || downloadContent)
                    yield return DownloadAssetsContent(new List<ArTarget>() {arTargetRequestGet.RequestResponseObject.Data},
                        downloadTargetImages, false, downloadContent, downloadTransferStatus);

                AppearitionLogger.LogInfo(string.Format(ArTargetConstant.GET_SINGLE_ARTARGET_SUCCESS, arTargetId));

                if (onSuccess != null)
                    onSuccess(arTargetRequestGet.RequestResponseObject.Data);
            }
            else
            {
                AppearitionLogger.LogError(string.Format(ArTargetConstant.GET_SINGLE_ARTARGET_FAILURE, arTargetId));

                if (onFailure != null)
                    onFailure(arTargetRequestGet.Errors);
            }

            if (onComplete != null)
                onComplete(arTargetRequestGet.RequestResponseObject.IsSuccess);
        }

        /// <summary>
        /// Fetches all the ArTargets from the channel and tenant entered in the current user.
        /// Do note that this request requires the user to be logged in.
        ///
        /// Offline capability.
        /// </summary>
        /// <param name="onSuccess">Contains all the ArTargets from the selected channel. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void GetChannelArTargets(Action<List<ArTarget>> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            GetChannelArTargets(AppearitionGate.Instance.CurrentUser.selectedChannel, false, false, onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Fetches all the ArTargets from the channel and tenant entered in the current user.
        /// Do note that this request requires the user to be logged in.
        ///
        /// Offline capability.
        /// </summary>
        /// <param name="onSuccess">Contains all the ArTargets from the selected channel. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator GetChannelArTargetsProcess(Action<List<ArTarget>> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            yield return GetChannelArTargetsProcess(AppearitionGate.Instance.CurrentUser.selectedChannel, false, false, onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Fetches all the ArTargets from the channel and tenant entered in the current user.
        /// Do note that this request requires the user to be logged in.
        /// Variant allowing a file name for the request to be saved for offline handling.
        /// Variant also allowing to download the target images, the MediaFile's content, or both.
        ///
        /// Offline capability.
        /// </summary>
        /// <param name="downloadTargetImages">Whether or not the Target Images should be downloaded. Do note that the EMS needs to allow this feature as well.</param>
        /// <param name="downloadContent">Whether or not the Medias should be downloaded. Do note that the EMS needs to allow this feature as well.</param>
        /// <param name="onSuccess">Contains all the ArTargets from the selected channel. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void GetChannelArTargets(bool downloadTargetImages, bool downloadContent,
            Action<List<ArTarget>> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            GetChannelArTargets(AppearitionGate.Instance.CurrentUser.selectedChannel, downloadTargetImages, downloadContent, onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Fetches all the ArTargets from the channel and tenant entered in the current user.
        /// Do note that this request requires the user to be logged in.
        /// Variant allowing a file name for the request to be saved for offline handling.
        /// Variant also allowing to download the target images, the MediaFile's content, or both.
        ///
        /// Offline capability.
        /// </summary>
        /// <param name="downloadTargetImages">Whether or not the Target Images should be downloaded. Do note that the EMS needs to allow this feature as well.</param>
        /// <param name="downloadContent">Whether or not the Medias should be downloaded. Do note that the EMS needs to allow this feature as well.</param>
        /// <param name="onSuccess">Contains all the ArTargets from the selected channel. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator GetChannelArTargetsProcess(bool downloadTargetImages, bool downloadContent,
            Action<List<ArTarget>> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            yield return GetChannelArTargetsProcess(AppearitionGate.Instance.CurrentUser.selectedChannel, downloadTargetImages, downloadContent, onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Fetches all the ArTargets from the channel and tenant entered in the given user.
        /// Do note that this request requires the user to be logged in.
        /// Variant allowing a file name for the request to be saved for offline handling.
        /// 
        /// Offline capability.
        /// </summary>
        /// <param name="channelId">The id of the targeted channel.</param>
        /// <param name="onSuccess">Contains all the ArTargets the channel of the given id. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        /// <param name="downloadTransferStatus"></param>
        public static void GetChannelArTargets(int channelId,
            Action<List<ArTarget>> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null, DataTransferStatus downloadTransferStatus = null)
        {
            GetChannelArTargets(channelId, false, false, onSuccess, onFailure, onComplete, downloadTransferStatus);
        }

        /// <summary>
        /// Fetches all the ArTargets from the channel and tenant entered in the given user.
        /// Do note that this request requires the user to be logged in.
        /// Variant allowing a file name for the request to be saved for offline handling.
        /// 
        /// Offline capability.
        /// </summary>
        /// <param name="channelId">The id of the targeted channel.</param>
        /// <param name="onSuccess">Contains all the ArTargets the channel of the given id. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        /// <param name="downloadTransferStatus"></param>
        public static IEnumerator GetChannelArTargetsProcess(int channelId,
            Action<List<ArTarget>> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null, DataTransferStatus downloadTransferStatus = null)
        {
            yield return GetChannelArTargetsProcess(channelId, false, false, onSuccess, onFailure, onComplete, downloadTransferStatus);
        }

        /// <summary>
        /// Fetches all the ArTargets from the channel and tenant entered in the given user.
        /// Do note that this request requires the user to be logged in.
        /// Variant allowing a file name for the request to be saved for offline handling.
        /// Variant also allowing to download the target images, the MediaFile's content, or both.
        /// 
        /// Offline capability.
        /// </summary>
        /// <param name="channelId">The id of the targeted channel.</param>
        /// <param name="downloadTargetImages">Whether or not the Target Images should be downloaded. Do note that the EMS needs to allow this feature as well.</param>
        /// <param name="downloadContent">Whether or not the Medias should be downloaded. Do note that the EMS needs to allow this feature as well.</param>
        /// <param name="onSuccess">Contains all the ArTargets the channel of the given id. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        /// <param name="downloadTransferStatus"></param>
        public static void GetChannelArTargets(int channelId, bool downloadTargetImages, bool downloadContent,
            Action<List<ArTarget>> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null, DataTransferStatus downloadTransferStatus = null)
        {
            AppearitionGate.Instance.StartCoroutine
                (GetChannelArTargetsProcess(channelId, downloadTargetImages, downloadContent, onSuccess, onFailure, onComplete, downloadTransferStatus));
        }

        /// <summary>
        /// Fetches all the ArTargets from the channel and tenant entered in the given user.
        /// Do note that this request requires the user to be logged in.
        /// Variant allowing a file name for the request to be saved for offline handling.
        /// Variant also allowing to download the target images, the MediaFile's content, or both.
        /// 
        /// Offline capability.
        /// </summary>
        /// <param name="channelId">The id of the targeted channel.</param>
        /// <param name="downloadTargetImages">Whether or not the Target Images should be downloaded. Do note that the EMS needs to allow this feature as well.</param>
        /// <param name="downloadContent">Whether or not the Medias should be downloaded. Do note that the EMS needs to allow this feature as well.</param>
        /// <param name="onSuccess">Contains all the ArTargets the channel of the given id. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        /// <param name="downloadTransferStatus"></param>
        public static IEnumerator GetChannelArTargetsProcess(int channelId, bool downloadTargetImages, bool downloadContent,
            Action<List<ArTarget>> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null, DataTransferStatus downloadTransferStatus = null)
        {
            //Query
            ArTarget_List.PostData query = new ArTarget_List.PostData(ArTargetConstant.GetDefaultArTargetListQuery());

            //Launch request
            var arTargetListRequest = AppearitionRequest<ArTarget_List>.LaunchAPICall_POST<ArTargetHandler>(channelId, GetReusableApiRequest<ArTarget_List>(), query);

            //Wait for request..
            while (!arTargetListRequest.IsDone)
                yield return null;

            //All done!
            if (arTargetListRequest.RequestResponseObject.IsSuccess)
            {
                if (arTargetListRequest.CurrentState == AppearitionBaseRequest<ArTarget_List>.RequestState.SuccessOnline)
                {
                    //Success ! If required, download the sub-content.
                    if (downloadTargetImages || downloadContent)
                        yield return DownloadAssetsContent(arTargetListRequest.RequestResponseObject.Data.ArTargets, downloadTargetImages, false, downloadContent, downloadTransferStatus);

                    AppearitionLogger.LogInfo(ArTargetConstant.GET_CHANNEL_ARTARGET_SUCCESS);
                }
                else
                {
                    if (downloadTargetImages)
                        yield return DownloadAssetsContent(arTargetListRequest.RequestResponseObject.Data.ArTargets, true, false, false, downloadTransferStatus);

                    AppearitionLogger.LogInfo(ArTargetConstant.GET_CHANNEL_ARTARGET_SUCCESS_OFFLINE);
                }

                //Callback it out ~
                if (onSuccess != null)
                    onSuccess(arTargetListRequest.RequestResponseObject.Data.ArTargets);
            }
            else
            {
                AppearitionLogger.LogError(string.Format(ArTargetConstant.GET_CHANNEL_ARTARGET_FAILURE, channelId));

                //Request failed =(
                if (onFailure != null)
                    onFailure(arTargetListRequest.Errors);
            }

            if (onComplete != null)
                onComplete(arTargetListRequest.RequestResponseObject.IsSuccess);
        }

        #endregion

        #region Get By Query

        /// <summary>
        /// Submits a query to find specific ArTarget in the selected channel. Check out the query class for more info.
        /// The paging and quantities returned by the query can be changed within the query class.
        /// </summary>
        /// <param name="query">The given query</param>
        /// <param name="onSuccess">Contains all the Experiences found from the given query. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void GetSpecificArTargetByQuery(ArTarget_List.QueryContent query,
            Action<List<ArTarget>> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(
                GetSpecificArTargetByQueryProcess(AppearitionGate.Instance.CurrentUser.selectedChannel, query, false, false, onSuccess, onFailure, onComplete, null));
        }

        /// <summary>
        /// Submits a query to find specific ArTarget in the selected channel. Check out the query class for more info.
        /// The paging and quantities returned by the query can be changed within the query class.
        /// </summary>
        /// <param name="query">The given query</param>
        /// <param name="onSuccess">Contains all the Experiences found from the given query. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator GetSpecificArTargetByQueryProcess(ArTarget_List.QueryContent query,
            Action<List<ArTarget>> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            yield return GetSpecificArTargetByQueryProcess(AppearitionGate.Instance.CurrentUser.selectedChannel, query, false, false, onSuccess, onFailure, onComplete, null);
        }

        /// <summary>
        /// Submits a query to find specific ArTarget in the selected channel. Check out the query class for more info.
        /// The paging and quantities returned by the query can be changed within the query class.
        /// </summary>
        /// <param name="channelId">The target channel ID</param>
        /// <param name="query">The given query</param>
        /// <param name="onSuccess">Contains all the Experiences found from the given query. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void GetSpecificArTargetByQuery(int channelId, ArTarget_List.QueryContent query,
            Action<List<ArTarget>> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(GetSpecificArTargetByQueryProcess(channelId, query, false, false, onSuccess, onFailure, onComplete, null));
        }

        /// <summary>
        /// Submits a query to find specific ArTarget in the selected channel. Check out the query class for more info.
        /// The paging and quantities returned by the query can be changed within the query class.
        /// </summary>
        /// <param name="channelId">The target channel ID</param>
        /// <param name="query">The given query</param>
        /// <param name="onSuccess">Contains all the Experiences found from the given query. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator GetSpecificArTargetByQueryProcess(int channelId, ArTarget_List.QueryContent query,
            Action<List<ArTarget>> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            yield return GetSpecificArTargetByQueryProcess(channelId, query, false, false, onSuccess, onFailure, onComplete, null);
        }

        /// <summary>
        /// Submits a query to find specific ArTarget in the selected channel. Check out the query class for more info.
        /// The paging and quantities returned by the query can be changed within the query class.
        /// </summary>
        /// <param name="query">The given query</param>
        /// <param name="downloadTargetImages">Whether to download the target images of the experiences found.</param>
        /// <param name="downloadContent">Whether to download all the medias from the experiences found.</param>
        /// <param name="onSuccess">Contains all the Experiences found from the given query. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        /// <param name="downloadTransferStatus">Transfer progress.</param>
        public static void GetSpecificArTargetByQuery(ArTarget_List.QueryContent query,
            bool downloadTargetImages, bool downloadContent,
            Action<List<ArTarget>> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null, DataTransferStatus downloadTransferStatus = null)
        {
            AppearitionGate.Instance.StartCoroutine(GetSpecificArTargetByQueryProcess(AppearitionGate.Instance.CurrentUser.selectedChannel, query, downloadTargetImages, downloadContent, onSuccess,
                onFailure, onComplete,
                downloadTransferStatus));
        }

        /// <summary>
        /// Submits a query to find specific ArTarget in the selected channel. Check out the query class for more info.
        /// The paging and quantities returned by the query can be changed within the query class.
        /// </summary>
        /// <param name="query">The given query</param>
        /// <param name="downloadTargetImages">Whether to download the target images of the experiences found.</param>
        /// <param name="downloadContent">Whether to download all the medias from the experiences found.</param>
        /// <param name="onSuccess">Contains all the Experiences found from the given query. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        /// <param name="downloadTransferStatus">Transfer progress.</param>
        public static IEnumerator GetSpecificArTargetByQueryProcess(ArTarget_List.QueryContent query,
            bool downloadTargetImages, bool downloadContent,
            Action<List<ArTarget>> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null, DataTransferStatus downloadTransferStatus = null)
        {
            yield return GetSpecificArTargetByQueryProcess(AppearitionGate.Instance.CurrentUser.selectedChannel, query, downloadTargetImages, downloadContent, onSuccess, onFailure, onComplete,
                downloadTransferStatus);
        }

        /// <summary>
        /// Submits a query to find specific ArTarget in the selected channel. Check out the query class for more info.
        /// The paging and quantities returned by the query can be changed within the query class.
        /// </summary>
        /// <param name="channelId">The target channel ID</param>
        /// <param name="query">The given query</param>
        /// <param name="downloadTargetImages">Whether to download the target images of the experiences found.</param>
        /// <param name="downloadContent">Whether to download all the medias from the experiences found.</param>
        /// <param name="onSuccess">Contains all the Experiences found from the given query. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        /// <param name="downloadTransferStatus">Transfer progress.</param>
        public static void GetSpecificArTargetByQuery(int channelId, ArTarget_List.QueryContent query,
            bool downloadTargetImages, bool downloadContent,
            Action<List<ArTarget>> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null, DataTransferStatus downloadTransferStatus = null)
        {
            AppearitionGate.Instance.StartCoroutine(GetSpecificArTargetByQueryProcess(AppearitionGate.Instance.CurrentUser.selectedChannel, query, downloadTargetImages, downloadContent, onSuccess,
                onFailure, onComplete,
                downloadTransferStatus));
        }

        /// <summary>
        /// Submits a query to find specific ArTarget in the selected channel. Check out the query class for more info.
        /// The paging and quantities returned by the query can be changed within the query class.
        /// </summary>
        /// <param name="channelId">The target channel ID</param>
        /// <param name="query">The given query</param>
        /// <param name="downloadTargetImages">Whether to download the target images of the experiences found.</param>
        /// <param name="downloadContent">Whether to download all the medias from the experiences found.</param>
        /// <param name="onSuccess">Contains all the Experiences found from the given query. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        /// <param name="downloadTransferStatus">Transfer progress.</param>
        public static IEnumerator GetSpecificArTargetByQueryProcess(int channelId, ArTarget_List.QueryContent query,
            bool downloadTargetImages, bool downloadContent,
            Action<List<ArTarget>> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null, DataTransferStatus downloadTransferStatus = null)
        {
            if (query == null || (string.IsNullOrWhiteSpace(query.Name) && query.Tags.Count == 0 && string.IsNullOrWhiteSpace(query.CreatedByUsername) && query.RecordsPerPage == 0))
            {
                AppearitionLogger.LogError(ArTargetConstant.GET_SPECIFIC_EXPERIENCES_BY_QUERY_FAILURE_EMPTY_QUERY);
                if (onFailure != null)
                    onFailure(new EmsError(ArTargetConstant.GET_SPECIFIC_EXPERIENCES_BY_QUERY_FAILURE_EMPTY_QUERY));
                if (onComplete != null)
                    onComplete(false);
                yield break;
            }

            var arTargetListRequest =
                AppearitionRequest<ArTarget_List>.LaunchAPICall_POST<ArTargetHandler>(channelId, GetReusableApiRequest<ArTarget_List>(), new ArTarget_List.PostData(query));

            while (!arTargetListRequest.IsDone)
                yield return null;

            //All done!
            if (arTargetListRequest.RequestResponseObject.IsSuccess && arTargetListRequest.RequestResponseObject.Data != null)
            {
                if (arTargetListRequest.RequestResponseObject.Data.ArTargets == null || arTargetListRequest.RequestResponseObject.Data.ArTargets.Count == 0)
                {
                    AppearitionLogger.LogWarning(ArTargetConstant.GET_SPECIFIC_EXPERIENCES_BY_QUERY_SUCCESS_EMPTY);

                    //Callback it out ~
                    if (onSuccess != null)
                        onSuccess(new List<ArTarget>());
                }
                else
                {
                    if (arTargetListRequest.CurrentState == AppearitionBaseRequest<ArTarget_List>.RequestState.SuccessOnline)
                    {
                        //Success ! If required, download the sub-content.
                        if (downloadTargetImages || downloadContent)
                            yield return DownloadAssetsContent(arTargetListRequest.RequestResponseObject.Data.ArTargets, downloadTargetImages, false, downloadContent,
                                downloadTransferStatus);

                        AppearitionLogger.LogInfo(string.Format(ArTargetConstant.GET_SPECIFIC_EXPERIENCES_BY_QUERY_SUCCESS, arTargetListRequest.RequestResponseObject.Data.TotalRecords));
                    }
                    else
                    {
                        if (downloadTargetImages)
                            yield return DownloadAssetsContent(arTargetListRequest.RequestResponseObject.Data.ArTargets, true, false, false,
                                downloadTransferStatus);

                        AppearitionLogger.LogInfo(string.Format(ArTargetConstant.GET_SPECIFIC_EXPERIENCES_BY_QUERY_SUCCESS_OFFLINE, arTargetListRequest.RequestResponseObject.Data.TotalRecords));
                    }

                    //Callback it out ~
                    if (onSuccess != null)
                        onSuccess(arTargetListRequest.RequestResponseObject.Data.ArTargets);
                }
            }
            else
            {
                AppearitionLogger.LogError(ArTargetConstant.GET_SPECIFIC_EXPERIENCES_BY_QUERY_FAILURE);

                //Request failed =(
                if (onFailure != null)
                    onFailure(arTargetListRequest.Errors);
            }

            if (onComplete != null)
                onComplete(arTargetListRequest.RequestResponseObject.IsSuccess);
        }

        #endregion

        #region Publish and Unpublish

        /// <summary>
        /// Publishes an ArTarget using a given ArTarget, making it visible to Asset / Experience APIs. 
        /// </summary>
        /// <param name="arTarget">The target ArTarget to publish.</param>
        /// <param name="onSuccess">Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void PublishExperience(ArTarget arTarget, Action<ArTarget> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(PublishExperienceProcess(arTarget, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Publishes an ArTarget using a given ArTarget, making it visible to Asset / Experience APIs. 
        /// /// </summary>
        /// <param name="arTarget">The target ArTarget to publish.</param>
        /// <param name="onSuccess">Contains the updated ArTarget. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator PublishExperienceProcess(ArTarget arTarget, Action<ArTarget> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            //Ref Check
            if (arTarget == null)
            {
                AppearitionLogger.LogError(ArTargetConstant.PUBLISH_EXPERIENCE_NULL);
                if (onFailure != null)
                    onFailure(new EmsError(ArTargetConstant.PUBLISH_EXPERIENCE_NULL));
                if (onComplete != null)
                    onComplete(false);
                yield break;
            }

            //Online request. Create content
            ArTarget_Publish.RequestContent publishContent = new ArTarget_Publish.RequestContent() {
                arTargetId = arTarget.arTargetId
            };

            //Launch publish !
            AppearitionRequest<ArTarget_Publish> publishRequest =
                AppearitionRequest<ArTarget_Publish>.LaunchAPICall_POST<ArTargetHandler>(arTarget.productId, GetReusableApiRequest<ArTarget_Publish>(), null, publishContent);

            while (!publishRequest.IsDone)
                yield return null;

            //Debug output
            if (publishRequest.RequestResponseObject != null && publishRequest.RequestResponseObject.IsSuccess)
            {
                arTarget.isPublished = true;
                AppearitionLogger.LogInfo(ArTargetConstant.PUBLISH_EXPERIENCE_SUCCESS);
                if (onSuccess != null)
                    onSuccess(arTarget);
            }
            else
            {
                AppearitionLogger.LogError(ArTargetConstant.PUBLISH_EXPERIENCE_FAILURE);

                if (onFailure != null)
                    onFailure(publishRequest.Errors);
            }

            if (onComplete != null)
                onComplete(publishRequest.RequestResponseObject != null && publishRequest.RequestResponseObject.IsSuccess);
        }

        /// <summary>
        /// Unpublishes an ArTarget using a given ArTarget, making it invisible to Asset / Experience APIs. 
        /// </summary>
        /// <param name="arTarget">The target ArTarget to unpublish.</param>
        /// <param name="onSuccess">Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void UnpublishExperience(ArTarget arTarget, Action<ArTarget> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(UnpublishExperienceProcess(arTarget, onSuccess, onFailure, onComplete));
        }


        /// <summary>
        /// Unpublishes an ArTarget using a given ArTarget, making it invisible to Asset / Experience APIs. 
        /// </summary>
        /// <param name="arTarget">The target ArTarget to unpublish.</param>
        /// <param name="onSuccess">Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator UnpublishExperienceProcess(ArTarget arTarget, Action<ArTarget> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            //Ref check
            if (arTarget == null)
            {
                AppearitionLogger.LogError(ArTargetConstant.UNPUBLISH_EXPERIENCE_NULL);
                if (onFailure != null)
                    onFailure(new EmsError(ArTargetConstant.UNPUBLISH_EXPERIENCE_NULL));
                if (onComplete != null)
                    onComplete(false);
                yield break;
            }

            //Online request. Create content
            var unpublishContent = new ArTarget_Unpublish.RequestContent() {
                arTargetId = arTarget.arTargetId
            };

            //Launch unpublish !
            var unpublishRequest = AppearitionRequest<ArTarget_Unpublish>.LaunchAPICall_POST<ArTargetHandler>(arTarget.productId, GetReusableApiRequest<ArTarget_Unpublish>(), null, unpublishContent);

            while (!unpublishRequest.IsDone)
                yield return null;

            //Debug output
            if (unpublishRequest.RequestResponseObject != null && unpublishRequest.RequestResponseObject.IsSuccess)
            {
                AppearitionLogger.LogInfo(ArTargetConstant.UNPUBLISH_EXPERIENCE_SUCCESS);
                arTarget.isPublished = true;

                if (onSuccess != null)
                    onSuccess(arTarget);
            }
            else
            {
                AppearitionLogger.LogError(ArTargetConstant.UNPUBLISH_EXPERIENCE_FAILURE);

                if (onFailure != null)
                    onFailure(unpublishRequest.Errors);
            }

            if (onComplete != null)
                onComplete(unpublishRequest.RequestResponseObject != null && unpublishRequest.RequestResponseObject.IsSuccess);
        }

        #endregion

        #region Delete

        /// <summary>
        /// Deletes the given experience from the EMS.
        /// </summary>
        /// <param name="arTarget">ArTarget to delete.</param>
        /// <param name="onSuccess">Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void DeleteExperience(ArTarget arTarget, Action onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(DeleteExperienceProcess(arTarget, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Deletes the given experience from the EMS.
        /// </summary>
        /// <param name="arTarget">ArTarget to delete.</param>
        /// <param name="onSuccess">Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator DeleteExperienceProcess(ArTarget arTarget, Action onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            //Ref Check
            if (arTarget == null)
            {
                AppearitionLogger.LogError(ArTargetConstant.DELETE_EXPERIENCE_NULL);
                if (onFailure != null)
                    onFailure(new EmsError(ArTargetConstant.DELETE_EXPERIENCE_NULL));
                if (onComplete != null)
                    onComplete(false);
                yield break;
            }

            //Online request. Create content
            ArTarget_Publish.RequestContent deleteContent = new ArTarget_Publish.RequestContent() {
                arTargetId = arTarget.arTargetId
            };

            //Launch publish !
            AppearitionRequest<ArTarget_Delete> deleteTarget =
                AppearitionRequest<ArTarget_Delete>.LaunchAPICall_POST<ArTargetHandler>(arTarget.productId, GetReusableApiRequest<ArTarget_Delete>(), null, deleteContent);

            while (!deleteTarget.IsDone)
                yield return null;

            //Debug output
            if (deleteTarget.RequestResponseObject != null && deleteTarget.RequestResponseObject.IsSuccess)
            {
                arTarget.isPublished = true;
                AppearitionLogger.LogInfo(ArTargetConstant.DELETE_EXPERIENCE_SUCCESS);
                if (onSuccess != null)
                    onSuccess();
            }
            else
            {
                AppearitionLogger.LogError(ArTargetConstant.DELETE_EXPERIENCE_FAILURE);

                if (onFailure != null)
                    onFailure(deleteTarget.Errors);
            }

            if (onComplete != null)
                onComplete(deleteTarget.RequestResponseObject != null && deleteTarget.RequestResponseObject.IsSuccess);
        }

        #endregion

        #endregion

        #region Handle Ar Image (Target Image)

        #region Add / Upload

        /// <summary>
        /// Add a new Target Image to an existing ArTarget.
        /// </summary>
        /// <param name="arTarget">The ArTarget you wish to add a TargetImage onto.</param>
        /// <param name="newTargetImageFileName">The filename (including extension) of the new Target Image you're willing to add. If left blank, a name will be generated.</param>
        /// <param name="newTargetImage">The texture of the new Target Image you're willing to add.</param>
        /// <param name="onSuccess">Contains the refreshed ArTarget containing the TargetImage newly added to it. Only called if the request has succeeded..</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        /// <param name="uploadTransferStatus">Transfer progress.</param>
        public static void AddTargetImageToArTarget(ArTarget arTarget, string newTargetImageFileName, Texture newTargetImage, Action<ArTarget> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null, DataTransferStatus uploadTransferStatus = null)
        {
            AppearitionGate.Instance.StartCoroutine(AddTargetImageToArTargetProcess(arTarget, newTargetImageFileName, newTargetImage, onSuccess, onFailure, onComplete, uploadTransferStatus));
        }

        /// <summary>
        /// Add a new Target Image to an existing ArTarget.
        /// </summary>
        /// <param name="arTarget">The ArTarget you wish to add a TargetImage onto.</param>
        /// <param name="newTargetImageFileName">The filename (including extension) of the new Target Image you're willing to add. If left blank, a name will be generated.</param>
        /// <param name="newTargetImage">The texture of the new Target Image you're willing to add.</param>
        /// <param name="onSuccess">Contains the refreshed ArTarget containing the TargetImage newly added to it. Only called if the request has succeeded..</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        /// <param name="uploadTransferStatus">Transfer progress.</param>
        public static IEnumerator AddTargetImageToArTargetProcess(ArTarget arTarget, string newTargetImageFileName, Texture newTargetImage, Action<ArTarget> onSuccess = null,
            Action<EmsError> onFailure = null, Action<bool> onComplete = null, DataTransferStatus uploadTransferStatus = null)
        {
            //Ref Check
            if (newTargetImage == null)
            {
                AppearitionLogger.LogError(ArTargetConstant.ADD_TARGET_IMAGE_TO_ARTARGET_NULL);

                if (onFailure != null)
                    onFailure(new EmsError(ArTargetConstant.ADD_TARGET_IMAGE_TO_ARTARGET_NULL));
                if (onComplete != null)
                    onComplete(false);
                yield break;
            }

            //Create the URL params for the upload request
            var uploadContent = new ArTarget_UploadTargetImage.RequestContent() {
                arTargetId = arTarget.arTargetId
            };

            //Create the multipart form ApiData
            if (string.IsNullOrWhiteSpace(newTargetImageFileName))
                newTargetImageFileName = "NewTargetImage.jpg";

            string fullFileName = newTargetImageFileName + (Path.HasExtension(newTargetImageFileName) ? "" : ".jpg");
            List<MultiPartFormParam> multiPartParam = new List<MultiPartFormParam>() {
                new MultiPartFormParam() {
                    fileName = fullFileName,
                    value = newTargetImage,
                    mimeType = string.Format("image/{0}", Path.GetExtension(fullFileName).Substring(1))
                }
            };

            //Create the request itself
            var uploadTargetImageRequest = AppearitionRequest<ArTarget_UploadTargetImage>.LaunchAPICall_MultiPartPOST<ArTargetHandler>(
                arTarget.productId, GetReusableApiRequest<ArTarget_UploadTargetImage>(), uploadContent, multiPartParam);

            string uploadItemKey = new Guid().ToString();
            bool isTrackingUpload = uploadTransferStatus != null;
            if (isTrackingUpload)
                uploadTransferStatus.AddNewItemProgress(uploadItemKey, fullFileName,
                    uploadTargetImageRequest.SizeOfFileBeingUploaded > 0 ? uploadTargetImageRequest.SizeOfFileBeingUploaded : ArTargetConstant.FILE_UPLOAD_PLACEHOLDER_SIZE);

            while (!uploadTargetImageRequest.IsDone)
            {
                if (isTrackingUpload)
                    uploadTransferStatus.UpdateItemProgress(uploadItemKey, uploadTargetImageRequest.Progress);
                yield return null;
            }

            if (uploadTargetImageRequest.RequestResponseObject != null && uploadTargetImageRequest.RequestResponseObject.IsSuccess)
            {
                AppearitionLogger.LogDebug(string.Format(ArTargetConstant.ADD_TARGET_IMAGE_TO_ARTARGET_SUCCESS, arTarget.arTargetId));
            }
            else
            {
                AppearitionLogger.LogError(ArTargetConstant.ADD_TARGET_IMAGE_TO_ARTARGET_FAILURE);
                if (onFailure != null)
                    onFailure(uploadTargetImageRequest.Errors);
                if (onComplete != null)
                    onComplete(false);
                yield break;
            }

            //Now, refresh the ArTarget.
            yield return GetSingleArTargetProcess(arTarget.productId, arTarget.arTargetId, onSuccess, onFailure, onComplete);
        }

        #endregion

        #region Remove / Unlink

        /// <summary>
        /// Removes a TargetImage from a given ArTarget.
        /// </summary>
        /// <param name="arTarget">The target ArTarget</param>
        /// <param name="targetImage">The given TargetImage to remove.</param>
        /// <param name="deleteTargetImageFromLibrary">Whether or not the TargetImage should be removed from the EMS.</param>
        /// <param name="onSuccess">Contains the refreshed ArTarget with the given TargetImage removed.. Only called if the request has succeeded..</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void RemoveTargetImageFromArTarget(ArTarget arTarget, TargetImage targetImage, bool deleteTargetImageFromLibrary, Action<ArTarget> onSuccess = null,
            Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(RemoveTargetImageFromArTargetProcess(arTarget, targetImage, deleteTargetImageFromLibrary, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Removes a TargetImage from a given ArTarget.
        /// </summary>
        /// <param name="arTarget">The target ArTarget</param>
        /// <param name="targetImage">The given TargetImage to remove.</param>
        /// <param name="deleteTargetImageFromLibrary">Whether or not the TargetImage should be removed from the EMS.</param>
        /// <param name="onSuccess">Contains the refreshed ArTarget with the given TargetImage removed.. Only called if the request has succeeded..</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator RemoveTargetImageFromArTargetProcess(ArTarget arTarget, TargetImage targetImage, bool deleteTargetImageFromLibrary, Action<ArTarget> onSuccess = null,
            Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            //Ref Check
            if (arTarget == null)
            {
                AppearitionLogger.LogError(ArTargetConstant.REMOVE_TARGET_IMAGE_FROM_ARTARGET_NULL_ARTARGET);

                if (onFailure != null)
                    onFailure(new EmsError(ArTargetConstant.REMOVE_TARGET_IMAGE_FROM_ARTARGET_NULL_ARTARGET));
                if (onComplete != null)
                    onComplete(false);
                yield break;
            }

            //Make sure it contains some TargetImage if null
            if (targetImage == null)
            {
                if (arTarget.targetImages != null && arTarget.targetImages.Count != 0)
                {
                    targetImage = arTarget.targetImages[0];
                }
                else
                {
                    AppearitionLogger.LogError(ArTargetConstant.REMOVE_TARGET_IMAGE_FROM_ARTARGET_NULL_TARGETIMAGE);

                    if (onFailure != null)
                        onFailure(new EmsError(ArTargetConstant.REMOVE_TARGET_IMAGE_FROM_ARTARGET_NULL_TARGETIMAGE));
                    if (onComplete != null)
                        onComplete(false);
                    yield break;
                }
            }

            //Online request. Create content
            ArTarget_UnlinkArImage.RequestContent unlinkRequestContent = new ArTarget_UnlinkArImage.RequestContent() {
                arTargetId = arTarget.arTargetId,
                arImageId = targetImage.arImageId
            };

            //Launch publish !
            AppearitionRequest<ArTarget_UnlinkArImage> unlinkTargetImageRequest =
                AppearitionRequest<ArTarget_UnlinkArImage>.LaunchAPICall_POST<ArTargetHandler>(arTarget.productId, GetReusableApiRequest<ArTarget_UnlinkArImage>(), null, unlinkRequestContent);

            while (!unlinkTargetImageRequest.IsDone)
                yield return null;

            //Debug output
            if (unlinkTargetImageRequest.RequestResponseObject != null && unlinkTargetImageRequest.RequestResponseObject.IsSuccess)
            {
                AppearitionLogger.LogError(ArTargetConstant.REMOVE_TARGET_IMAGE_FROM_ARTARGET_SUCCESS);
            }
            else
            {
                AppearitionLogger.LogError(ArTargetConstant.REMOVE_TARGET_IMAGE_FROM_ARTARGET_FAILURE);

                if (onFailure != null)
                    onFailure(unlinkTargetImageRequest.Errors);
                if (onComplete != null)
                    onComplete(false);
                yield break;
            }

            if (deleteTargetImageFromLibrary)
            {
                ArImage_Delete.RequestContent deleteRequestContent = new ArImage_Delete.RequestContent() {
                    arImageId = targetImage.arImageId
                };

                //Delete from library.
                AppearitionRequest<ArImage_Delete> arImageDeleteRequest =
                    AppearitionRequest<ArImage_Delete>.LaunchAPICall_POST<ArTargetHandler>(arTarget.productId, GetReusableApiRequest<ArImage_Delete>(), null, deleteRequestContent);

                while (!arImageDeleteRequest.IsDone)
                    yield return null;

                if (arImageDeleteRequest.RequestResponseObject.IsSuccess)
                    AppearitionLogger.LogError(ArTargetConstant.REMOVE_TARGET_IMAGE_FROM_ARTARGET_DELETE_SUCCESS);
                else
                {
                    AppearitionLogger.LogError(ArTargetConstant.REMOVE_TARGET_IMAGE_FROM_ARTARGET_DELETE_FAILURE);

                    if (onFailure != null)
                        onFailure(unlinkTargetImageRequest.Errors);
                    if (onComplete != null)
                        onComplete(false);
                    yield break;
                }
            }

            yield return GetSingleArTargetProcess(arTarget.productId, arTarget.arTargetId, onSuccess, onFailure, onComplete);
        }

        #endregion

        #region Replace

        /// <summary>
        /// Removes a TargetImage from a given ArTarget.
        /// </summary>
        /// <param name="arTarget">The target ArTarget</param>
        /// <param name="targetImageToRemove">The given TargetImage to remove.</param>
        /// <param name="newTargetImageFilename">The filename of the new TargetImage to add.</param>
        /// <param name="newTargetImage">The texture of the new TargetImage to add.</param>
        /// <param name="deleteTargetImageFromLibrary">Whether or not the TargetImage should be removed from the EMS.</param>
        /// <param name="onSuccess">Contains the refreshed ArTarget with the given TargetImage removed.. Only called if the request has succeeded..</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        /// <param name="uploadTransferStatus"></param>
        public static void ReplaceTargetImage(ArTarget arTarget, TargetImage targetImageToRemove, string newTargetImageFilename, Texture newTargetImage,
            bool deleteTargetImageFromLibrary, Action<ArTarget> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null, DataTransferStatus uploadTransferStatus = null)
        {
            AppearitionGate.Instance.StartCoroutine(ReplaceTargetImageProcess(arTarget, targetImageToRemove, newTargetImageFilename,
                newTargetImage,
                deleteTargetImageFromLibrary, onSuccess, onFailure, onComplete, uploadTransferStatus));
        }

        /// <summary>
        /// Removes a TargetImage from a given ArTarget.
        /// </summary>
        /// <param name="arTarget">The target ArTarget</param>
        /// <param name="targetImageToRemove">The given TargetImage to remove.</param>
        /// <param name="newTargetImageFilename">The filename of the new TargetImage to add.</param>
        /// <param name="newTargetImage">The texture of the new TargetImage to add.</param>
        /// <param name="deleteTargetImageFromLibrary">Whether or not the TargetImage should be removed from the EMS.</param>
        /// <param name="onSuccess">Contains the refreshed ArTarget with the given TargetImage removed.. Only called if the request has succeeded..</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        /// <param name="uploadTransferStatus"></param>
        public static IEnumerator ReplaceTargetImageProcess(ArTarget arTarget, TargetImage targetImageToRemove, string newTargetImageFilename, Texture newTargetImage,
            bool deleteTargetImageFromLibrary, Action<ArTarget> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null, DataTransferStatus uploadTransferStatus = null)
        {
            //Ref Check
            if (arTarget == null)
            {
                AppearitionLogger.LogError(ArTargetConstant.REPLACE_TARGET_IMAGE_NULL_ARTARGET);

                if (onFailure != null)
                    onFailure(new EmsError(ArTargetConstant.REPLACE_TARGET_IMAGE_NULL_ARTARGET));
                if (onComplete != null)
                    onComplete(false);
                yield break;
            }

            if (newTargetImage == null)
            {
                AppearitionLogger.LogError(ArTargetConstant.REPLACE_TARGET_IMAGE_NULL_IMAGE);

                if (onFailure != null)
                    onFailure(new EmsError(ArTargetConstant.REPLACE_TARGET_IMAGE_NULL_IMAGE));
                if (onComplete != null)
                    onComplete(false);
                yield break;
            }

            //Firstly, remove.
            bool isRemoveSuccessful = false;
            yield return RemoveTargetImageFromArTargetProcess(arTarget, targetImageToRemove, deleteTargetImageFromLibrary, deleteSuccess => arTarget = deleteSuccess, onFailure,
                deleteComplete => isRemoveSuccessful = deleteComplete);

            if (!isRemoveSuccessful)
            {
                if (onComplete != null)
                    onComplete(false);
                yield break;
            }

            //Now, add.
            yield return AddTargetImageToArTargetProcess(arTarget, newTargetImageFilename, newTargetImage, onSuccess, onFailure, onComplete, uploadTransferStatus);
        }

        #endregion

        #endregion

        #region Rename ArTarget

        /// <summary>
        /// Changes the name of the experience as displayed on the EMS.
        /// Simply requires the ArTargetId and the new name in order to work.
        /// Callbacks can be provided as well.
        /// </summary>
        /// <param name="arTarget">The ArTarget to rename.</param>
        /// <param name="newName">New name for the Experience.</param>
        /// <param name="onSuccess">Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void ChangeExperienceName(ArTarget arTarget, string newName, Action<ArTarget> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(ChangeExperienceNameProcess(arTarget, newName, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Changes the name of the experience as displayed on the EMS.
        /// Simply requires the ArTargetId and the new name in order to work.
        /// Callbacks can be provided as well.
        /// </summary>
        /// <param name="arTarget">The ArTarget to rename.</param>
        /// <param name="newName">New name for the Experience.</param>
        /// <param name="onSuccess">Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator ChangeExperienceNameProcess(ArTarget arTarget, string newName, Action<ArTarget> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            //Ref Check
            if (arTarget == null)
            {
                AppearitionLogger.LogError(ArTargetConstant.ARTARGET_NULL);
                if (onFailure != null)
                    onFailure(new EmsError(ArTargetConstant.ARTARGET_NULL));
                if (onComplete != null)
                    onComplete(false);
                yield break;
            }

            //Online request. Prepare request content
            var postContent = new ArTarget_UpdateName.PostData() {name = newName};
            var requestContent = new ArTarget_UpdateName.RequestContent() {arTargetId = arTarget.arTargetId};

            //Launch request
            var updateArTargetRequest =
                AppearitionRequest<ArTarget_UpdateName>.LaunchAPICall_POST<ArTargetHandler>(arTarget.productId, GetReusableApiRequest<ArTarget_UpdateName>(), postContent, requestContent);

            while (!updateArTargetRequest.IsDone)
                yield return null;

            //Handle success and failure
            if (updateArTargetRequest.RequestResponseObject.IsSuccess)
            {
                AppearitionLogger.LogInfo(ArTargetConstant.CHANGE_EXPERIENCE_NAME_ARTARGET_SUCCESS);

                //Soft update ArTarget
                arTarget.name = newName;
                if (onSuccess != null)
                    onSuccess(arTarget);
            }
            else
            {
                AppearitionLogger.LogError(string.Format(ArTargetConstant.CHANGE_EXPERIENCE_NAME_ARTARGET_FAILURE, newName));

                if (onFailure != null)
                    onFailure(updateArTargetRequest.Errors);
            }


            if (onComplete != null)
                onComplete(updateArTargetRequest.RequestResponseObject.IsSuccess);
        }

        #endregion

        #region Change Description and Copyright Info

        /// <summary>
        /// Changes the short description of the experience as displayed on the EMS.
        /// </summary>
        /// <param name="arTarget">The target ArTarget.</param>
        /// <param name="newShortDescription">New short description for the Experience.</param>
        /// <param name="onSuccess">Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void ChangeExperienceShortDescription(ArTarget arTarget, string newShortDescription, Action<ArTarget> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(ChangeExperienceShortDescriptionProcess(arTarget, newShortDescription, onSuccess, onFailure,
                onComplete));
        }

        /// <summary>
        /// Changes the short description of the experience as displayed on the EMS.
        /// </summary>
        /// <param name="arTarget">The target ArTarget.</param>
        /// <param name="newShortDescription">New short description for the Experience.</param>
        /// <param name="onSuccess">Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator ChangeExperienceShortDescriptionProcess(ArTarget arTarget, string newShortDescription, Action<ArTarget> onSuccess = null,
            Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            //Ref Check
            if (arTarget == null)
            {
                AppearitionLogger.LogError(ArTargetConstant.ARTARGET_NULL);
                if (onFailure != null)
                    onFailure(new EmsError(ArTargetConstant.ARTARGET_NULL));
                if (onComplete != null)
                    onComplete(false);
                yield break;
            }

            //Online request. Prepare request content
            var postContent = new ArTarget_UpdateShortDescription.PostData() {shortDescription = newShortDescription};
            var requestContent = new ArTarget_UpdateShortDescription.RequestContent() {arTargetId = arTarget.arTargetId};

            //Launch request
            var updateArTargetDesc =
                AppearitionRequest<ArTarget_UpdateShortDescription>.LaunchAPICall_POST<ArTargetHandler>(arTarget.productId, GetReusableApiRequest<ArTarget_UpdateShortDescription>(), postContent,
                    requestContent);

            while (!updateArTargetDesc.IsDone)
                yield return null;

            //Handle success and failure
            if (updateArTargetDesc.RequestResponseObject.IsSuccess)
            {
                AppearitionLogger.LogInfo(ArTargetConstant.CHANGE_EXPERIENCE_SHORT_DESCRIPTION_ARTARGET_SUCCESS);

                //Soft update ArTarget
                arTarget.shortDescription = newShortDescription;
                if (onSuccess != null)
                    onSuccess(arTarget);
            }
            else
            {
                AppearitionLogger.LogError(string.Format(ArTargetConstant.CHANGE_EXPERIENCE_SHORT_DESCRIPTION_ARTARGET_FAILURE, arTarget.arTargetId));

                if (onFailure != null)
                    onFailure(updateArTargetDesc.Errors);
            }


            if (onComplete != null)
                onComplete(updateArTargetDesc.RequestResponseObject.IsSuccess);
        }

        /// <summary>
        /// Changes the long description of the experience as displayed on the EMS.
        /// </summary>
        /// <param name="arTarget">The target ArTarget.</param>
        /// <param name="newLongDescription">New long description for the Experience.</param>
        /// <param name="onSuccess">Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void ChangeExperienceLongDescription(ArTarget arTarget, string newLongDescription, Action<ArTarget> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(ChangeExperienceLongDescriptionProcess(arTarget, newLongDescription, onSuccess, onFailure,
                onComplete));
        }

        /// <summary>
        /// Changes the long description of the experience as displayed on the EMS.
        /// </summary>
        /// <param name="arTarget">The target ArTarget.</param>
        /// <param name="newLongDescription">New long description for the Experience.</param>
        /// <param name="onSuccess">Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator ChangeExperienceLongDescriptionProcess(ArTarget arTarget, string newLongDescription, Action<ArTarget> onSuccess = null,
            Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            //Ref Check
            if (arTarget == null)
            {
                AppearitionLogger.LogError(ArTargetConstant.ARTARGET_NULL);
                if (onFailure != null)
                    onFailure(new EmsError(ArTargetConstant.ARTARGET_NULL));
                if (onComplete != null)
                    onComplete(false);
                yield break;
            }

            //Online request. Prepare request content
            var postContent = new ArTarget_UpdateLongDescription.PostData() {longDescription = newLongDescription};
            var requestContent = new ArTarget_UpdateLongDescription.RequestContent() {arTargetId = arTarget.arTargetId};

            //Launch request
            var updateArTargetDesc =
                AppearitionRequest<ArTarget_UpdateLongDescription>.LaunchAPICall_POST<ArTargetHandler>(arTarget.productId, GetReusableApiRequest<ArTarget_UpdateLongDescription>(), postContent,
                    requestContent);

            while (!updateArTargetDesc.IsDone)
                yield return null;

            //Handle success and failure
            if (updateArTargetDesc.RequestResponseObject.IsSuccess)
            {
                AppearitionLogger.LogInfo(ArTargetConstant.CHANGE_EXPERIENCE_LONG_DESCRIPTION_ARTARGET_SUCCESS);

                //Soft update ArTarget
                arTarget.longDescription = newLongDescription;
                if (onSuccess != null)
                    onSuccess(arTarget);
            }
            else
            {
                AppearitionLogger.LogError(string.Format(ArTargetConstant.CHANGE_EXPERIENCE_LONG_DESCRIPTION_ARTARGET_FAILURE, arTarget.arTargetId));

                if (onFailure != null)
                    onFailure(updateArTargetDesc.Errors);
            }


            if (onComplete != null)
                onComplete(updateArTargetDesc.RequestResponseObject.IsSuccess);
        }

        /// <summary>
        /// Changes the copyrightInfo of the experience as displayed on the EMS.
        /// </summary>
        /// <param name="arTarget">The target ArTarget.</param>
        /// <param name="newCopyrightInfo">New copyright info for the Experience.</param>
        /// <param name="onSuccess">Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void ChangeExperienceCopyrightInfo(ArTarget arTarget, string newCopyrightInfo, Action<ArTarget> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(ChangeExperienceCopyrightInfoProcess(arTarget, newCopyrightInfo, onSuccess, onFailure,
                onComplete));
        }

        /// <summary>
        /// Changes the copyrightInfo of the experience as displayed on the EMS.
        /// </summary>
        /// <param name="arTarget">The target ArTarget.</param>
        /// <param name="newCopyrightInfo">New copyright info for the Experience.</param>
        /// <param name="onSuccess">Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator ChangeExperienceCopyrightInfoProcess(ArTarget arTarget, string newCopyrightInfo, Action<ArTarget> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            //Ref Check
            if (arTarget == null)
            {
                AppearitionLogger.LogError(ArTargetConstant.ARTARGET_NULL);
                if (onFailure != null)
                    onFailure(new EmsError(ArTargetConstant.ARTARGET_NULL));
                if (onComplete != null)
                    onComplete(false);
                yield break;
            }

            //Online request. Prepare request content
            var postContent = new ArTarget_UpdateCopyrightInfo.PostData() {copyrightInfo = newCopyrightInfo};
            var requestContent = new ArTarget_UpdateCopyrightInfo.RequestContent() {arTargetId = arTarget.arTargetId};

            //Launch request
            var updateArTargetDesc =
                AppearitionRequest<ArTarget_UpdateCopyrightInfo>.LaunchAPICall_POST<ArTargetHandler>(arTarget.productId, GetReusableApiRequest<ArTarget_UpdateCopyrightInfo>(), postContent,
                    requestContent);

            while (!updateArTargetDesc.IsDone)
                yield return null;

            //Handle success and failure
            if (updateArTargetDesc.RequestResponseObject.IsSuccess)
            {
                AppearitionLogger.LogInfo(ArTargetConstant.CHANGE_EXPERIENCE_COPYRIGHT_INFO_ARTARGET_SUCCESS);

                //Soft update ArTarget
                arTarget.copyrightInfo = newCopyrightInfo;
                if (onSuccess != null)
                    onSuccess(arTarget);
            }
            else
            {
                AppearitionLogger.LogError(string.Format(ArTargetConstant.CHANGE_EXPERIENCE_COPYRIGHT_INFO_ARTARGET_FAILURE, arTarget.arTargetId));

                if (onFailure != null)
                    onFailure(updateArTargetDesc.Errors);
            }


            if (onComplete != null)
                onComplete(updateArTargetDesc.RequestResponseObject.IsSuccess);
        }

        #endregion

        #region Update / Modify Media

        /// <summary>
        /// Updates a Media inside an experience on the EMS using an arTargetId and the new media content.
        /// </summary>
        /// <param name="arTarget">The ArTarget which contains the MediaFile you wish to update or replace.</param>
        /// <param name="mediaData">The mediaFile of the media you wish to update. Make sure that the arMediaId is valid.</param>
        /// <param name="onSuccess">Contains the media pushed on the EMS. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void UpdateExperienceMedia(ArTarget arTarget, MediaFile mediaData,
            Action<ArTarget, MediaFile> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            UpdateExperienceMedia(arTarget, mediaData, null, false, onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Updates a Media inside an experience on the EMS using an arTargetId and the new media content.
        /// </summary>
        /// <param name="arTarget">The ArTarget which contains the MediaFile you wish to update or replace.</param>
        /// <param name="mediaData">The mediaFile of the media you wish to update. Make sure that the arMediaId is valid.</param>
        /// <param name="onSuccess">Contains the media pushed on the EMS. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator UpdateExperienceMediaProcess(ArTarget arTarget, MediaFile mediaData,
            Action<ArTarget, MediaFile> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            yield return UpdateExperienceMediaProcess(arTarget, mediaData, null, false, onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Updates a Media inside an experience on the EMS using an arTargetId and the new media content.
        /// Optionally, the file attached to this media can be replaced, and the media can either be kept or deleted from the library.
        /// </summary>
        /// <param name="arTarget">The ArTarget which contains the MediaFile you wish to update or replace.</param>
        /// <param name="mediaData">The mediaFile of the media you wish to update. Make sure that the arMediaId is valid.</param>
        /// <param name="mediaObjectReplacement">The object you wish to upload. The MediaFile needs to contain the updated information about this file (filename, mimetype, etc).</param>
        /// <param name="doDeleteMediaFromLibrary">Whether the media being replaced should be deleted from the library.</param>
        /// <param name="onSuccess">Contains the media pushed on the EMS. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        /// <param name="uploadTransferStatus"></param>
        public static void UpdateExperienceMedia(ArTarget arTarget, MediaFile mediaData, object mediaObjectReplacement, bool doDeleteMediaFromLibrary,
            Action<ArTarget, MediaFile> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null, DataTransferStatus uploadTransferStatus = null)
        {
            AppearitionGate.Instance.StartCoroutine
                (UpdateExperienceMediaProcess(arTarget, mediaData, mediaObjectReplacement, doDeleteMediaFromLibrary, onSuccess, onFailure, onComplete, uploadTransferStatus));
        }

        /// <summary>
        /// Updates a Media inside an experience on the EMS using an arTargetId and the new media content.
        /// Optionally, the file attached to this media can be replaced, and the media can either be kept or deleted from the library.
        /// </summary>
        /// <param name="arTarget">The ArTarget which contains the MediaFile you wish to update or replace.</param>
        /// <param name="mediaData">The mediaFile of the media you wish to update. Make sure that the arMediaId is valid.</param>
        /// <param name="mediaObjectReplacement">The object you wish to upload. The MediaFile needs to contain the updated information about this file (filename, mimetype, etc).</param>
        /// <param name="doDeleteMediaFromLibrary">Whether the media being replaced should be deleted from the library.</param>
        /// <param name="onSuccess">Contains the media pushed on the EMS. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        /// <param name="uploadTransferStatus"></param>
        public static IEnumerator UpdateExperienceMediaProcess(ArTarget arTarget, MediaFile mediaData, object mediaObjectReplacement, bool doDeleteMediaFromLibrary,
            Action<ArTarget, MediaFile> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null, DataTransferStatus uploadTransferStatus = null)
        {
            //Ref check
            if (mediaData == null)
            {
                AppearitionLogger.LogError(ArTargetConstant.UPDATE_EXPERIENCE_MEDIA_NULL);
                if (onFailure != null)
                    onFailure(new EmsError(ArTargetConstant.UPDATE_EXPERIENCE_MEDIA_NULL));
                if (onComplete != null)
                    onComplete(false);
                yield break;
            }

            //Handle the "update media data" differently from the "replace media content". Start with update media data.
            if (mediaObjectReplacement == null)
            {
                //Prepare ApiData
                var postData = new ArTarget_UpdateMediaSettings.PostData(mediaData);
                var requestContent = new ArTarget_UpdateMediaSettings.RequestContent() {
                    arTargetId = arTarget.arTargetId,
                    arMediaId = mediaData.arMediaId
                };

                //Launch request
                var updateMediaRequest =
                    AppearitionRequest<ArTarget_UpdateMediaSettings>.LaunchAPICall_POST<ArTargetHandler>(arTarget.productId, GetReusableApiRequest<ArTarget_UpdateMediaSettings>(), postData,
                        requestContent);

                while (!updateMediaRequest.IsDone)
                    yield return null;

                //Callback
                if (updateMediaRequest.RequestResponseObject.IsSuccess)
                {
                    AppearitionLogger.LogInfo(ArTargetConstant.UPDATE_EXPERIENCE_MEDIA_SUCCESS);
                }
                else
                {
                    AppearitionLogger.LogError(string.Format(ArTargetConstant.UPDATE_EXPERIENCE_MEDIA_FAILURE, arTarget.arTargetId));

                    if (onFailure != null)
                        onFailure(updateMediaRequest.Errors);
                }

                //Refresh ArTarget
                bool refreshSuccess = false;
                yield return GetSingleArTargetProcess(arTarget.productId, arTarget.arTargetId, success => arTarget = success, onFailure, complete => refreshSuccess = complete);

                if (refreshSuccess && onSuccess != null)
                    onSuccess(arTarget, mediaData);

                if (onComplete != null)
                    onComplete(refreshSuccess);
            }
            else //Handle the Media Replace absolutely differently.
            {
                //Firstly, handle delete or unlink.
                bool isSuccess = false;
                List<string> errors = new List<string>();

                //Remove and unlink
                yield return RemoveMediaFromArTargetProcess(arTarget, mediaData, doDeleteMediaFromLibrary,
                    success => arTarget = success, failure => errors.AddRange(failure.Errors), complete => isSuccess = complete);

                //Handle any failure
                if (!isSuccess)
                {
                    //AppearitionLogger.LogError(string.Format("An error has occurred when trying to {0} a media while trying to replace its content.",
                    //    (doDeleteMediaFromLibrary ? "delete" : "unlink")));

                    AppearitionLogger.LogError(string.Format(ArTargetConstant.UPDATE_EXPERIENCE_MEDIA_FAILURE, arTarget.arTargetId));

                    if (onFailure != null && errors != null && errors.Count > 0)
                        onFailure(new EmsError(errors));
                    if (onComplete != null)
                        onComplete(false);
                    yield break;
                }

                //Erase the irrelevant parts of the MediaFile, in case the EMS doesn't handle them..
                mediaData.arMediaId = 0;
                mediaData.url = "";
                mediaData.checksum = "";

                //Launch the upload and let the upload handle the rest !
                yield return AddMediaToExistingExperienceProcess(arTarget, mediaData, mediaObjectReplacement, false, onSuccess, onFailure, complete => isSuccess = complete, uploadTransferStatus);

                if (isSuccess)
                    AppearitionLogger.LogInfo(ArTargetConstant.UPDATE_EXPERIENCE_MEDIA_SUCCESS);
                onComplete?.Invoke(isSuccess);
            }
        }

        /// <summary>
        /// Removes a Media from a given ArTarget.
        /// </summary>
        /// <param name="arTarget">The target ArTarget which contains the MediaFile to remove.</param>
        /// <param name="media">The MediaFile to remove from the ArTarget.</param>
        /// <param name="onSuccess">Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void RemoveMediaFromArTarget(ArTarget arTarget, MediaFile media, Action<ArTarget> onSuccess, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            RemoveMediaFromArTarget(arTarget, media, false, onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Removes a Media from a given ArTarget.
        /// </summary>
        /// <param name="arTarget">The target ArTarget which contains the MediaFile to remove.</param>
        /// <param name="media">The MediaFile to remove from the ArTarget.</param>
        /// <param name="onSuccess">Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator RemoveMediaFromArTargetProcess(ArTarget arTarget, MediaFile media, Action<ArTarget> onSuccess, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            yield return RemoveMediaFromArTargetProcess(arTarget, media, false, onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Removes a Media from a given ArTarget.
        /// Variant allowing to delete the Media from the library.
        /// </summary>
        /// <param name="arTarget">The target ArTarget which contains the MediaFile to remove.</param>
        /// <param name="media">The MediaFile to remove from the ArTarget.</param>
        /// <param name="doDeleteMediaFromLibrary">Whether or not the media being replaced should be deleted from the library.</param>
        /// <param name="onSuccess">Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void RemoveMediaFromArTarget(ArTarget arTarget, MediaFile media, bool doDeleteMediaFromLibrary = false,
            Action<ArTarget> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(RemoveMediaFromArTargetProcess(arTarget, media, doDeleteMediaFromLibrary, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Removes a Media from a given ArTarget.
        /// Variant allowing to delete the Media from the library.
        /// </summary>
        /// <param name="arTarget">The target ArTarget which contains the MediaFile to remove.</param>
        /// <param name="media">The MediaFile to remove from the ArTarget.</param>
        /// <param name="doDeleteMediaFromLibrary">Whether or not the media being replaced should be deleted from the library.</param>
        /// <param name="onSuccess">Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator RemoveMediaFromArTargetProcess(ArTarget arTarget, MediaFile media, bool doDeleteMediaFromLibrary,
            Action<ArTarget> onSuccess, Action<EmsError> onFailure, Action<bool> onComplete)
        {
            //Ref Check
            if (arTarget == null)
            {
                AppearitionLogger.LogError(ArTargetConstant.PUBLISH_EXPERIENCE_NULL);
                if (onFailure != null)
                    onFailure(new EmsError(ArTargetConstant.PUBLISH_EXPERIENCE_NULL));
                if (onComplete != null)
                    onComplete(false);
                yield break;
            }

            //Online request. First, unlink.
            var unlinkRequestContent = new ArTarget_UnlinkMedia.RequestContent() {
                arTargetId = arTarget.arTargetId,
                arMediaId = media.arMediaId
            };

            var unlinkMediaRequest =
                AppearitionRequest<ArTarget_UnlinkMedia>.LaunchAPICall_POST<ArTargetHandler>(arTarget.productId, GetReusableApiRequest<ArTarget_UnlinkMedia>(), null, unlinkRequestContent);

            while (!unlinkMediaRequest.IsDone)
                yield return null;

            if (!unlinkMediaRequest.RequestResponseObject.IsSuccess)
            {
                AppearitionLogger.LogError(string.Format(ArTargetConstant.REMOVE_MEDIA_FROM_ARTARGET_UNLINK_FAILURE, media.arMediaId));

                if (onFailure != null && unlinkMediaRequest.RequestResponseObject.Errors != null && unlinkMediaRequest.RequestResponseObject.Errors.Length > 0)
                    onFailure(unlinkMediaRequest.Errors);
                if (onComplete != null)
                    onComplete(false);
                yield break;
            }


            //Then, if requested, delete the media.
            if (doDeleteMediaFromLibrary)
            {
                var deleteRequestContent = new Media_Delete.RequestContent() {arMediaId = media.arMediaId};
                var deleteMediaRequest = AppearitionRequest<Media_Delete>.LaunchAPICall_POST<ArTargetHandler>(arTarget.productId, GetReusableApiRequest<Media_Delete>(), null, deleteRequestContent);

                while (!deleteMediaRequest.IsDone)
                    yield return null;

                if (deleteMediaRequest.RequestResponseObject.IsSuccess)
                {
                    AppearitionLogger.LogInfo(string.Format(ArTargetConstant.REMOVE_MEDIA_FROM_ARTARGET_DELETE_SUCCESS, media.arMediaId));
                }
                else
                {
                    AppearitionLogger.LogError(string.Format(ArTargetConstant.REMOVE_MEDIA_FROM_ARTARGET_DELETE_FAILURE, media.arMediaId));

                    if (onFailure != null && deleteMediaRequest.RequestResponseObject.Errors != null && deleteMediaRequest.RequestResponseObject.Errors.Length > 0)
                        onFailure(deleteMediaRequest.Errors);
                    if (onComplete != null)
                        onComplete(false);
                    yield break;
                }
            }
            else if (unlinkMediaRequest.RequestResponseObject.IsSuccess)
                AppearitionLogger.LogInfo(ArTargetConstant.REMOVE_MEDIA_FROM_ARTARGET_UNLINK_SUCCESS);

            //Finally, refresh
            yield return GetSingleArTargetProcess(arTarget.productId, arTarget.arTargetId, onSuccess, onFailure, onComplete);
        }

        #endregion

        #region ArTarget Property

        /// <summary>
        /// Adds a new property to a given ArTarget.
        /// </summary>
        /// <param name="arTarget"></param>
        /// <param name="propertyKey"></param>
        /// <param name="propertyValue"></param>
        /// <param name="onSuccess">Contains the ArTarget as well as the list of all available properties. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void AddPropertyToArTarget(ArTarget arTarget, string propertyKey, object propertyValue, Action<ArTarget, List<Property>> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(AddPropertyToArTargetProcess(arTarget, propertyKey, propertyValue, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Adds a new property to a given ArTarget.
        /// </summary>
        /// <param name="arTarget"></param>
        /// <param name="propertyKey"></param>
        /// <param name="propertyValue"></param>
        /// <param name="onSuccess">Contains the ArTarget as well as the list of all available properties. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator AddPropertyToArTargetProcess(ArTarget arTarget, string propertyKey, object propertyValue, Action<ArTarget, List<Property>> onSuccess = null,
            Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            //Ref Check
            if (arTarget == null)
            {
                AppearitionLogger.LogError(ArTargetConstant.ARTARGET_NULL);
                if (onFailure != null)
                    onFailure(new EmsError(ArTargetConstant.ARTARGET_NULL));
                if (onComplete != null)
                    onComplete(false);
                yield break;
            }

            //Online request. Prepare request content
            var postContent = new ArTarget_AddProperty.PostData(propertyKey, propertyValue.ToString());
            var requestContent = new ArTarget_AddProperty.RequestContent() {arTargetId = arTarget.arTargetId};

            //Launch request
            var addPropertyRequest =
                AppearitionRequest<ArTarget_AddProperty>.LaunchAPICall_POST<ArTargetHandler>(arTarget.productId, GetReusableApiRequest<ArTarget_AddProperty>(), postContent, requestContent);

            while (!addPropertyRequest.IsDone)
                yield return null;

            //Handle success and failure
            if (addPropertyRequest.RequestResponseObject.IsSuccess)
            {
                AppearitionLogger.LogInfo(string.Format(ArTargetConstant.ADD_PROPERTY_SUCCESS, propertyKey, arTarget.arTargetId));
                yield return GetArTargetPropertiesProcess(arTarget, onSuccess);
            }
            else
            {
                AppearitionLogger.LogError(string.Format(ArTargetConstant.ADD_PROPERTY_FAILURE, propertyKey, arTarget.arTargetId, addPropertyRequest.Errors));

                if (onFailure != null)
                    onFailure(addPropertyRequest.Errors);
            }


            if (onComplete != null)
                onComplete(addPropertyRequest.RequestResponseObject.IsSuccess);
        }

        /// <summary>
        /// Fetches the properties of the given ArTarget
        /// </summary>
        /// <param name="arTarget"></param>
        /// <param name="onSuccess">Contains the ArTarget as well as the list of all available properties. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void GetArTargetProperties(ArTarget arTarget, Action<ArTarget, List<Property>> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(GetArTargetPropertiesProcess(arTarget, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Fetches the properties of the given ArTarget
        /// </summary>
        /// <param name="arTarget"></param>
        /// <param name="onSuccess">Contains the ArTarget as well as the list of all available properties. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator GetArTargetPropertiesProcess(ArTarget arTarget, Action<ArTarget, List<Property>> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            //Ref Check
            if (arTarget == null)
            {
                AppearitionLogger.LogError(ArTargetConstant.ARTARGET_NULL);
                if (onFailure != null)
                    onFailure(new EmsError(ArTargetConstant.ARTARGET_NULL));
                if (onComplete != null)
                    onComplete(false);
                yield break;
            }

            //Online request. Prepare request content
            var requestContent = new ArTarget_GetProperties.RequestContent() {arTargetId = arTarget.arTargetId};

            //Launch request
            var getPropertiesRequest =
                AppearitionRequest<ArTarget_GetProperties>.LaunchAPICall_GET<ArTargetHandler>(arTarget.productId, GetReusableApiRequest<ArTarget_GetProperties>(), requestContent);

            while (!getPropertiesRequest.IsDone)
                yield return null;

            //Handle success and failure
            if (getPropertiesRequest.RequestResponseObject.IsSuccess)
            {
                bool foundAny = getPropertiesRequest.RequestResponseObject.Data != null && getPropertiesRequest.RequestResponseObject.Data.Count > 0;
                AppearitionLogger.LogInfo(string.Format(ArTargetConstant.GET_PROPERTY_SUCCESS, foundAny ? getPropertiesRequest.RequestResponseObject.Data.Count.ToString() : "No",
                    arTarget.arTargetId));

                if (onSuccess != null)
                    onSuccess(arTarget, getPropertiesRequest.RequestResponseObject.Data);
            }
            else
            {
                AppearitionLogger.LogError(string.Format(ArTargetConstant.GET_PROPERTY_FAILURE, arTarget.arTargetId, getPropertiesRequest.Errors));

                if (onFailure != null)
                    onFailure(getPropertiesRequest.Errors);
            }


            if (onComplete != null)
                onComplete(getPropertiesRequest.RequestResponseObject.IsSuccess);
        }

        /// <summary>
        /// Updates a single property of a given ArTarget
        /// </summary>
        /// <param name="arTarget"></param>
        /// <param name="property"></param>
        /// <param name="newPropertyValue"></param>
        /// <param name="onSuccess">Contains the ArTarget as well as the list of all available properties. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        /// <param name="propertyKey"></param>
        public static void UpdateArTargetProperties(ArTarget arTarget, string propertyKey, object newPropertyValue, Action<ArTarget, List<Property>> onSuccess = null,
            Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(UpdateArTargetPropertiesProcess(arTarget, new Property(propertyKey, AppearitionConstants.SerializeJson(newPropertyValue)), onSuccess, onFailure,
                onComplete));
        }

        /// <summary>
        /// Updates a single property of a given ArTarget
        /// </summary>
        /// <param name="arTarget"></param>
        /// <param name="newPropertyValue"></param>
        /// <param name="onSuccess">Contains the ArTarget as well as the list of all available properties. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        /// <param name="propertyKey"></param>
        public static IEnumerator UpdateArTargetPropertiesProcess(ArTarget arTarget, string propertyKey, object newPropertyValue, Action<ArTarget, List<Property>> onSuccess = null,
            Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            yield return UpdateArTargetPropertiesProcess(arTarget, new Property(propertyKey, AppearitionConstants.SerializeJson(newPropertyValue)), onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Updates a single property of a given ArTarget
        /// </summary>
        /// <param name="arTarget"></param>
        /// <param name="property"></param>
        /// <param name="onSuccess">Contains the ArTarget as well as the list of all available properties. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void UpdateArTargetProperties(ArTarget arTarget, Property property, Action<ArTarget, List<Property>> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(UpdateArTargetPropertiesProcess(arTarget, property, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Updates a single property of a given ArTarget
        /// </summary>
        /// <param name="arTarget"></param>
        /// <param name="property"></param>
        /// <param name="onSuccess">Contains the ArTarget as well as the list of all available properties. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator UpdateArTargetPropertiesProcess(ArTarget arTarget, Property property, Action<ArTarget, List<Property>> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            //Ref Check
            if (arTarget == null)
            {
                AppearitionLogger.LogError(ArTargetConstant.ARTARGET_NULL);
                if (onFailure != null)
                    onFailure(new EmsError(ArTargetConstant.ARTARGET_NULL));
                if (onComplete != null)
                    onComplete(false);
                yield break;
            }

            //Online request. Prepare request content
            var postContent = new ArTarget_UpdateProperty.PostData(property);
            var requestContent = new ArTarget_UpdateProperty.RequestContent() {arTargetId = arTarget.arTargetId};

            //Launch request
            var updatePropertyRequest =
                AppearitionRequest<ArTarget_UpdateProperty>.LaunchAPICall_POST<ArTargetHandler>(arTarget.productId, GetReusableApiRequest<ArTarget_UpdateProperty>(), postContent, requestContent);

            while (!updatePropertyRequest.IsDone)
                yield return null;

            //Handle success and failure
            if (updatePropertyRequest.RequestResponseObject.IsSuccess)
            {
                AppearitionLogger.LogInfo(string.Format(ArTargetConstant.UPDATE_PROPERTY_SUCCESS, property.propertyKey, arTarget.arTargetId));
                yield return GetArTargetPropertiesProcess(arTarget, onSuccess);
            }
            else
            {
                AppearitionLogger.LogError(string.Format(ArTargetConstant.UPDATE_PROPERTY_FAILURE, property.propertyKey, arTarget.arTargetId, updatePropertyRequest.Errors));

                if (onFailure != null)
                    onFailure(updatePropertyRequest.Errors);
            }


            if (onComplete != null)
                onComplete(updatePropertyRequest.RequestResponseObject.IsSuccess);
        }

        /// <summary>
        /// Deletes a single property of a given ArTarget
        /// </summary>
        /// <param name="arTarget"></param>
        /// <param name="propertyKey"></param>
        /// <param name="onSuccess">Contains the ArTarget as well as the list of all available properties. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void DeleteArTargetProperties(ArTarget arTarget, string propertyKey, Action<ArTarget, List<Property>> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(DeleteArTargetPropertiesProcess(arTarget, propertyKey, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Deletes a single property of a given ArTarget
        /// </summary>
        /// <param name="arTarget"></param>
        /// <param name="propertyKey"></param>
        /// <param name="onSuccess">Contains the ArTarget as well as the list of all available properties. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator DeleteArTargetPropertiesProcess(ArTarget arTarget, string propertyKey, Action<ArTarget, List<Property>> onSuccess = null, Action<EmsError> onFailure = null,
        Action<bool> onComplete = null)
        {
            //Ref Check
            if (arTarget == null)
            {
                AppearitionLogger.LogError(ArTargetConstant.ARTARGET_NULL);
                if (onFailure != null)
                    onFailure(new EmsError(ArTargetConstant.ARTARGET_NULL));
                if (onComplete != null)
                    onComplete(false);
                yield break;
            }

            //Online request. Prepare request content
            var requestContent = new ArTarget_DeleteProperty.RequestContent() {arTargetId = arTarget.arTargetId, propertyKey = propertyKey};

            //Launch request
            var deletePropertyRequest =
                AppearitionRequest<ArTarget_DeleteProperty>.LaunchAPICall_POST<ArTargetHandler>(arTarget.productId, GetReusableApiRequest<ArTarget_DeleteProperty>(), null, requestContent);

            while (!deletePropertyRequest.IsDone)
                yield return null;

            //Handle success and failure
            if (deletePropertyRequest.RequestResponseObject.IsSuccess)
            {
                AppearitionLogger.LogInfo(string.Format(ArTargetConstant.DELETE_PROPERTY_SUCCESS, propertyKey, arTarget.arTargetId));
                yield return GetArTargetPropertiesProcess(arTarget, onSuccess);
            }
            else
            {
                AppearitionLogger.LogError(string.Format(ArTargetConstant.DELETE_PROPERTY_FAILURE, propertyKey, arTarget.arTargetId, deletePropertyRequest.Errors));

                if (onFailure != null)
                    onFailure(deletePropertyRequest.Errors);
            }


            if (onComplete != null)
                onComplete(deletePropertyRequest.RequestResponseObject.IsSuccess);
        }

        #endregion

        #region Tags

        #region Fetch All Tags

        /// <summary>
        /// Fetch all the experience tags registered on the channel selected in the current user profile.
        /// </summary>
        /// <param name="onSuccess">Contains all the tags from the channel of the given id. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void GetAllRegisteredTagsInChannel(Action<List<string>> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(GetAllRegisteredTagsInChannelProcess(AppearitionGate.Instance.CurrentUser.selectedChannel, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Fetch all the experience tags registered on the channel selected in the current user profile.
        /// </summary>
        /// <param name="onSuccess">Contains all the tags from the channel of the given id. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator GetAllRegisteredTagsInChannelProcess(Action<List<string>> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            yield return GetAllRegisteredTagsInChannelProcess(AppearitionGate.Instance.CurrentUser.selectedChannel, onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Fetch all the experience tags registered on the channel of a given id.
        /// </summary>
        /// <param name="channelId">Target Channel ID</param>
        /// <param name="onSuccess">Contains all the tags from the channel of the given id. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void GetAllRegisteredTagsInChannel(int channelId, Action<List<string>> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(GetAllRegisteredTagsInChannelProcess(channelId, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Fetch all the experience tags registered on the channel of a given id.
        /// </summary>
        /// <param name="channelId">Target Channel ID</param>
        /// <param name="onSuccess">Contains all the tags from the channel of the given id. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator GetAllRegisteredTagsInChannelProcess(int channelId, Action<List<string>> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            //Launch request
            var availableTagsRequest = AppearitionRequest<Asset_TagsByChannel>.LaunchAPICall_GET<ArTargetHandler>(channelId, GetReusableApiRequest<Asset_TagsByChannel>());

            //Wait for request..
            while (!availableTagsRequest.IsDone)
                yield return null;

            //All done!
            if (availableTagsRequest.RequestResponseObject.IsSuccess)
            {
                if (availableTagsRequest.CurrentState == AppearitionBaseRequest<Asset_TagsByChannel>.RequestState.SuccessOnline)
                    AppearitionLogger.LogInfo(ArTargetConstant.FETCH_AVAILABLE_TAGS_SUCCESS);
                else
                    AppearitionLogger.LogInfo(ArTargetConstant.FETCH_AVAILABLE_TAGS_SUCCESS_OFFLINE);


                //Callback it out ~
                if (onSuccess != null)
                    onSuccess(availableTagsRequest.RequestResponseObject.Data);
            }
            else
            {
                AppearitionLogger.LogError(string.Format(ArTargetConstant.FETCH_AVAILABLE_TAGS_FAILURE, channelId));

                //Request failed =(
                if (onFailure != null)
                    onFailure(availableTagsRequest.Errors);
            }

            if (onComplete != null)
                onComplete(availableTagsRequest.RequestResponseObject.IsSuccess);
        }

        /// <summary>
        /// Fetch all the experience tags the current user can set to an experience.
        /// </summary>
        /// <param name="onSuccess">Contains all the tags from the channel of the given id. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void GetAllAvailableTagsForCurrentUser(Action<List<string>> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(GetAllAvailableTagsForCurrentUserProcess(AppearitionGate.Instance.CurrentUser.selectedChannel, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Fetch all the experience tags the current user can set to an experience.
        /// </summary>
        /// <param name="onSuccess">Contains all the tags from the channel of the given id. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator GetAllAvailableTagsForCurrentUserProcess(Action<List<string>> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            yield return GetAllAvailableTagsForCurrentUserProcess(AppearitionGate.Instance.CurrentUser.selectedChannel, onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Fetch all the experience tags the current user can set to an experience.
        /// </summary>
        /// <param name="channelId">Target Channel ID</param>
        /// <param name="onSuccess">Contains all the tags from the channel of the given id. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void GetAllAvailableTagsForCurrentUser(int channelId, Action<List<string>> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(GetAllAvailableTagsForCurrentUserProcess(channelId, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Fetch all the experience tags the current user can set to an experience.
        /// </summary>
        /// <param name="channelId">Target Channel ID</param>
        /// <param name="onSuccess">Contains all the tags from the channel of the given id. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator GetAllAvailableTagsForCurrentUserProcess(int channelId, Action<List<string>> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            //Launch request
            var availableTagsRequest = AppearitionRequest<ArTarget_AvailableTags>.LaunchAPICall_GET<ArTargetHandler>(channelId, GetReusableApiRequest<ArTarget_AvailableTags>());

            //Wait for request..
            while (!availableTagsRequest.IsDone)
                yield return null;

            //All done!
            if (availableTagsRequest.RequestResponseObject.IsSuccess)
            {
                AppearitionLogger.LogInfo(ArTargetConstant.FETCH_AVAILABLE_TAGS_SUCCESS);

                //Callback it out ~
                if (onSuccess != null)
                    onSuccess(availableTagsRequest.RequestResponseObject.Data);
            }
            else
            {
                AppearitionLogger.LogError(string.Format(ArTargetConstant.FETCH_AVAILABLE_TAGS_FAILURE, channelId));

                //Request failed =(
                if (onFailure != null)
                    onFailure(availableTagsRequest.Errors);
            }

            if (onComplete != null)
                onComplete(availableTagsRequest.RequestResponseObject.IsSuccess);
        }

        #endregion

        #region Add Tag

        /// <summary>
        /// Adds a given experience tag to a given ArTarget.
        /// </summary>
        /// <param name="arTarget">Given ArTarget.</param>
        /// <param name="tag">The new tag to add to the ArTarget.</param>
        /// <param name="onSuccess">Contains the modified ArTarget. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void AddTagToExperience(ArTarget arTarget, string tag, Action<ArTarget> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(AddTagToExperienceProcess(arTarget, tag, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Adds a given experience tag to a given ArTarget.
        /// </summary>
        /// <param name="arTarget">Given ArTarget.</param>
        /// <param name="tag">The new tag to add to the ArTarget.</param>
        /// <param name="onSuccess">Contains the modified ArTarget. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator AddTagToExperienceProcess(ArTarget arTarget, string tag, Action<ArTarget> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            //Don't fire the request unless the tag is valid and isn't already present on the experience.
            if (string.IsNullOrEmpty(tag) || arTarget == null)
            {
                AppearitionLogger.LogError(ArTargetConstant.ADD_TAG_TO_ARTARGET_FAILURE_INVALID_PARAMS);
                onFailure?.Invoke(new EmsError(ArTargetConstant.ADD_TAG_TO_ARTARGET_FAILURE_INVALID_PARAMS));
                onComplete?.Invoke(false);
                yield break;
            }

            if (arTarget.ContainsTag(tag))
            {
                AppearitionLogger.LogWarning(string.Format(ArTargetConstant.ADD_TAG_TO_ARTARGET_FAILURE_EXIST, tag, arTarget.arTargetId));
                onFailure?.Invoke(new EmsError(string.Format(ArTargetConstant.ADD_TAG_TO_ARTARGET_FAILURE_EXIST, tag, arTarget.arTargetId)));
                onComplete?.Invoke(false);
                yield break;
            }


            ArTarget_AddTag.RequestContent requestContent = new ArTarget_AddTag.RequestContent() {arTargetId = arTarget.arTargetId, tag = tag};

            var addTagRequest =
                AppearitionRequest<ArTarget_AddTag>.LaunchAPICall_POST<ArTargetHandler>(arTarget.productId, GetReusableApiRequest<ArTarget_AddTag>(), null, requestContent);

            while (!addTagRequest.IsDone)
                yield return null;

            if (addTagRequest.RequestResponseObject.IsSuccess)
            {
                AppearitionLogger.LogInfo(string.Format(ArTargetConstant.ADD_TAG_TO_ARTARGET_SUCCESS, tag, arTarget.arTargetId));

                if (!arTarget.ContainsTag(tag))
                    arTarget.tags.Add(tag);

                if (onSuccess != null)
                    onSuccess(arTarget);
            }
            else
            {
                AppearitionLogger.LogError(string.Format(ArTargetConstant.ADD_TAG_TO_ARTARGET_FAILURE, tag, arTarget.arTargetId));

                if (onFailure != null)
                    onFailure(addTagRequest.Errors);
            }

            if (onComplete != null)
                onComplete(addTagRequest.RequestResponseObject.IsSuccess);
        }

        #endregion

        #region Remove Tag

        /// <summary>
        /// Remove a given experience tag from a given ArTarget.
        /// </summary>
        /// <param name="arTarget">Given ArTarget.</param>
        /// <param name="tag">The new tag to add to the ArTarget.</param>
        /// <param name="onSuccess">Contains the modified ArTarget. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void RemoveTagFromExperience(ArTarget arTarget, string tag, Action<ArTarget> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(RemoveTagFromExperienceProcess(arTarget, tag, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Remove a given experience tag from a given ArTarget.
        /// </summary>
        /// <param name="arTarget">Given ArTarget.</param>
        /// <param name="tag">The new tag to add to the ArTarget.</param>
        /// <param name="onSuccess">Contains the modified ArTarget. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator RemoveTagFromExperienceProcess(ArTarget arTarget, string tag, Action<ArTarget> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            //Don't fire the request unless the tag is valid and isn't already present on the experience.
            if (string.IsNullOrEmpty(tag) || arTarget == null)
            {
                AppearitionLogger.LogError(ArTargetConstant.REMOVE_TAG_FROM_ARTARGET_FAILURE_INVALID_PARAMS);
                onFailure?.Invoke(new EmsError(ArTargetConstant.REMOVE_TAG_FROM_ARTARGET_FAILURE_INVALID_PARAMS));
                onComplete?.Invoke(false);
                yield break;
            }

            if (!arTarget.ContainsTag(tag))
            {
                AppearitionLogger.LogWarning(string.Format(ArTargetConstant.REMOVE_TAG_FROM_ARTARGET_FAILURE_NOT_EXIST, tag, arTarget.arTargetId));
                onFailure?.Invoke(new EmsError(string.Format(ArTargetConstant.REMOVE_TAG_FROM_ARTARGET_FAILURE_NOT_EXIST, tag, arTarget.arTargetId)));
                onComplete?.Invoke(false);
                yield break;
            }

            ArTarget_RemoveTag.RequestContent requestContent = new ArTarget_RemoveTag.RequestContent() {arTargetId = arTarget.arTargetId, tag = tag};

            var removeTagRequest =
                AppearitionRequest<ArTarget_RemoveTag>.LaunchAPICall_POST<ArTargetHandler>(arTarget.productId, GetReusableApiRequest<ArTarget_RemoveTag>(), null, requestContent);

            while (!removeTagRequest.IsDone)
                yield return null;

            if (removeTagRequest.RequestResponseObject.IsSuccess)
            {
                AppearitionLogger.LogInfo(string.Format(ArTargetConstant.REMOVE_TAG_FROM_ARTARGET_SUCCESS, tag, arTarget.arTargetId));

                if (arTarget.ContainsTag(tag))
                {
                    for (int i = arTarget.tags.Count - 1; i >= 0; i--)
                    {
                        if (tag.Equals(arTarget.tags[i], StringComparison.InvariantCultureIgnoreCase))
                        {
                            arTarget.tags.RemoveAt(i);
                            break;
                        }
                    }
                }

                if (onSuccess != null)
                    onSuccess(arTarget);
            }
            else
            {
                AppearitionLogger.LogError(string.Format(ArTargetConstant.REMOVE_TAG_FROM_ARTARGET_FAILURE, tag, arTarget.arTargetId));

                if (onFailure != null)
                    onFailure(removeTagRequest.Errors);
            }

            if (onComplete != null)
                onComplete(removeTagRequest.RequestResponseObject.IsSuccess);
        }

        #endregion

        #region Find All Related Tags

        /// <summary>
        /// Fetch all the experience tags the current user can set to an experience.
        /// </summary>
        /// <param name="targetTag"></param>
        /// <param name="onSuccess">Contains all the tags from the channel of the given id. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void GetAllRelatedTags(string targetTag, Action<List<string>> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(GetAllRelatedTagsProcess(AppearitionGate.Instance.CurrentUser.selectedChannel, targetTag, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Fetch all the experience tags the current user can set to an experience.
        /// </summary>
        /// <param name="targetTag"></param>
        /// <param name="onSuccess">Contains all the tags from the channel of the given id. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator GetAllRelatedTagsProcess(string targetTag, Action<List<string>> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            yield return GetAllRelatedTagsProcess(AppearitionGate.Instance.CurrentUser.selectedChannel, targetTag, onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Fetch all the experience tags the current user can set to an experience.
        /// </summary>
        /// <param name="channelId">Target Channel ID</param>
        /// <param name="targetTag"></param>
        /// <param name="onSuccess">Contains all the tags from the channel of the given id. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void GetAllRelatedTags(int channelId, string targetTag, Action<List<string>> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(GetAllRelatedTagsProcess(channelId, targetTag, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Fetch all the experience tags the current user can set to an experience.
        /// </summary>
        /// <param name="channelId">Target Channel ID</param>
        /// <param name="targetTag"></param>
        /// <param name="onSuccess">Contains all the tags from the channel of the given id. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator GetAllRelatedTagsProcess(int channelId, string targetTag, Action<List<string>> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            var requestContent = new Asset_RelatedTagsByChannel.RequestContent() {tag = targetTag};

            //Launch request
            var availableTagsRequest =
                AppearitionRequest<Asset_RelatedTagsByChannel>.LaunchAPICall_GET<ArTargetHandler>(channelId, GetReusableApiRequest<Asset_RelatedTagsByChannel>(), requestContent);

            //Wait for request..
            while (!availableTagsRequest.IsDone)
                yield return null;

            //All done!
            if (availableTagsRequest.RequestResponseObject.IsSuccess)
            {
                if (availableTagsRequest.CurrentState == AppearitionBaseRequest<Asset_RelatedTagsByChannel>.RequestState.SuccessOnline)
                    AppearitionLogger.LogInfo(string.Format(ArTargetConstant.FETCH_RELATED_TAGS_SUCCESS, targetTag));
                else
                    AppearitionLogger.LogInfo(string.Format(ArTargetConstant.FETCH_RELATED_TAGS_SUCCESS_OFFLINE, targetTag));

                //Callback it out ~
                if (onSuccess != null)
                    onSuccess(availableTagsRequest.RequestResponseObject.Data);
            }
            else
            {
                AppearitionLogger.LogError(string.Format(ArTargetConstant.FETCH_RELATED_TAGS_FAILURE, targetTag));

                //Request failed =(
                if (onFailure != null)
                    onFailure(availableTagsRequest.Errors);
            }

            if (onComplete != null)
                onComplete(availableTagsRequest.RequestResponseObject.IsSuccess);
        }

        #endregion

        #endregion

        #region Lock and Unlock

        /// <summary>
        /// Locks an experience so that it cannot be edited until unlocked.
        /// </summary>
        /// <param name="arTarget">The target ArTarget.</param>
        /// <param name="onSuccess">Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void LockExperience(ArTarget arTarget, Action<ArTarget> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(LockExperienceProcess(arTarget, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Locks an experience so that it cannot be edited until unlocked.
        /// </summary>
        /// <param name="arTarget">The target ArTarget.</param>
        /// <param name="onSuccess">Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator LockExperienceProcess(ArTarget arTarget, Action<ArTarget> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            //Ref Check
            if (arTarget == null)
            {
                AppearitionLogger.LogError(ArTargetConstant.ARTARGET_NULL);
                if (onFailure != null)
                    onFailure(new EmsError(ArTargetConstant.ARTARGET_NULL));
                if (onComplete != null)
                    onComplete(false);
                yield break;
            }

            //Online request. Prepare request content
            var requestContent = new ArTarget_Lock.RequestContent() {arTargetId = arTarget.arTargetId};

            //Launch request
            var lockRequest =
                AppearitionRequest<ArTarget_Lock>.LaunchAPICall_POST<ArTargetHandler>(arTarget.productId, GetReusableApiRequest<ArTarget_Lock>(), null, requestContent);

            while (!lockRequest.IsDone)
                yield return null;

            //Handle success and failure
            if (lockRequest.RequestResponseObject.IsSuccess)
            {
                AppearitionLogger.LogInfo(string.Format(ArTargetConstant.LOCK_ARTARGET_SUCCESS, arTarget.arTargetId));

                //Soft update ArTarget
                arTarget.isLocked = true;
                if (onSuccess != null)
                    onSuccess(arTarget);
            }
            else
            {
                AppearitionLogger.LogError(string.Format(ArTargetConstant.LOCK_ARTARGET_FAILURE, arTarget.arTargetId));

                if (onFailure != null)
                    onFailure(lockRequest.Errors);
            }


            if (onComplete != null)
                onComplete(lockRequest.RequestResponseObject.IsSuccess);
        }

        /// <summary>
        /// Unlocks an experience so that it can be further edited.
        /// </summary>
        /// <param name="arTarget">The target ArTarget.</param>
        /// <param name="onSuccess">Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void UnlockExperience(ArTarget arTarget, Action<ArTarget> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(UnlockExperienceProcess(arTarget, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Unlocks an experience so that it can be further edited.
        /// </summary>
        /// <param name="arTarget">The target ArTarget.</param>
        /// <param name="onSuccess">Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator UnlockExperienceProcess(ArTarget arTarget, Action<ArTarget> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            //Ref Check
            if (arTarget == null)
            {
                AppearitionLogger.LogError(ArTargetConstant.ARTARGET_NULL);
                if (onFailure != null)
                    onFailure(new EmsError(ArTargetConstant.ARTARGET_NULL));
                if (onComplete != null)
                    onComplete(false);
                yield break;
            }

            //Online request. Prepare request content
            var requestContent = new ArTarget_Unlock.RequestContent() {arTargetId = arTarget.arTargetId};

            //Launch request
            var unlockRequest =
                AppearitionRequest<ArTarget_Unlock>.LaunchAPICall_POST<ArTargetHandler>(arTarget.productId, GetReusableApiRequest<ArTarget_Unlock>(), null, requestContent);

            while (!unlockRequest.IsDone)
                yield return null;

            //Handle success and failure
            if (unlockRequest.RequestResponseObject.IsSuccess)
            {
                AppearitionLogger.LogInfo(string.Format(ArTargetConstant.UNLOCK_ARTARGET_SUCCESS, arTarget.arTargetId));

                //Soft update ArTarget
                arTarget.isLocked = false;
                if (onSuccess != null)
                    onSuccess(arTarget);
            }
            else
            {
                AppearitionLogger.LogError(string.Format(ArTargetConstant.UNLOCK_ARTARGET_FAILURE, arTarget.arTargetId));

                if (onFailure != null)
                    onFailure(unlockRequest.Errors);
            }


            if (onComplete != null)
                onComplete(unlockRequest.RequestResponseObject.IsSuccess);
        }

        #endregion

        #endregion

        #region Copyright

        /// <summary>
        /// Fetch the copyright info for a single asset, and sets it to the appropriate field.
        /// </summary>
        /// <param name="asset">Target asset</param>
        /// <param name="onSuccess">Contains all the tags from the channel of the given id. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void GetExperienceCopyrightInfo(Asset asset, Action<Asset> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(GetExperienceCopyrightInfoProcess(asset, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Fetch the copyright info for a single asset, and sets it to the appropriate field.
        /// </summary>
        /// <param name="asset">Target asset</param>
        /// <param name="onSuccess">Contains all the tags from the channel of the given id. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator GetExperienceCopyrightInfoProcess(Asset asset, Action<Asset> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            //Ref Check
            if (asset == null)
            {
                AppearitionLogger.LogError(ArTargetConstant.ASSET_NULL);
                if (onFailure != null)
                    onFailure(new EmsError(ArTargetConstant.ASSET_NULL));
                if (onComplete != null)
                    onComplete(false);
                yield break;
            }

            var requestContent = new Asset_CopyrightInfo.RequestContent() {assetId = asset.assetId};

            //Launch request
            var assetCopyrightRequest = AppearitionRequest<Asset_CopyrightInfo>.LaunchAPICall_GET<ArTargetHandler>(asset.productId, GetReusableApiRequest<Asset_CopyrightInfo>(), requestContent);

            //Wait for request..
            while (!assetCopyrightRequest.IsDone)
                yield return null;

            //All done!
            if (assetCopyrightRequest.RequestResponseObject.IsSuccess)
            {
                asset.copyrightInfo = assetCopyrightRequest.RequestResponseObject.Data;

                if (assetCopyrightRequest.CurrentState == AppearitionBaseRequest<Asset_CopyrightInfo>.RequestState.SuccessOnline)
                    AppearitionLogger.LogInfo(ArTargetConstant.ASSET_COPYRIGHT_SUCCESS);
                else
                    AppearitionLogger.LogInfo(ArTargetConstant.ASSET_COPYRIGHT_SUCCESS_OFFLINE);

                //Callback it out ~
                if (onSuccess != null)
                    onSuccess(asset);
            }
            else
            {
                AppearitionLogger.LogError(string.Format(ArTargetConstant.ASSET_COPYRIGHT_FAILURE, asset.assetId));

                //Request failed =(
                if (onFailure != null)
                    onFailure(assetCopyrightRequest.Errors);
            }

            if (onComplete != null)
                onComplete(assetCopyrightRequest.RequestResponseObject.IsSuccess);
        }

        /// <summary>
        /// Fetch the copyright info for a single ArTarget, and sets it to the appropriate field. Also provides the full copyright information.
        ///
        /// Offline Capability.
        /// </summary>
        /// <param name="arTarget">Target arTarget</param>
        /// <param name="onSuccess">Contains all the tags from the channel of the given id. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void GetArTargetCopyrightInfo(ArTarget arTarget, Action<ArTarget, CopyrightInfo> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(GetArTargetCopyrightInfoProcess(arTarget, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Fetch the copyright info for a single ArTarget, and sets it to the appropriate field. Also provides the full copyright information.
        ///
        /// Offline Capability.
        /// </summary>
        /// <param name="arTarget">Target arTarget</param>
        /// <param name="onSuccess">Contains all the tags from the channel of the given id. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator GetArTargetCopyrightInfoProcess(ArTarget arTarget, Action<ArTarget, CopyrightInfo> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            //Ref Check
            if (arTarget == null)
            {
                AppearitionLogger.LogError(ArTargetConstant.ARTARGET_NULL);
                if (onFailure != null)
                    onFailure(new EmsError(ArTargetConstant.ARTARGET_NULL));
                if (onComplete != null)
                    onComplete(false);
                yield break;
            }

            var requestContent = new ArTarget_CopyrightInfo.RequestContent() {arTargetId = arTarget.arTargetId};

            //Launch request
            var arTargetCopyrightRequest =
                AppearitionRequest<ArTarget_CopyrightInfo>.LaunchAPICall_GET<ArTargetHandler>(arTarget.productId, GetReusableApiRequest<ArTarget_CopyrightInfo>(), requestContent);

            //Wait for request..
            while (!arTargetCopyrightRequest.IsDone)
                yield return null;

            //All done!
            if (arTargetCopyrightRequest.RequestResponseObject.IsSuccess)
            {
                if (arTargetCopyrightRequest.CurrentState == AppearitionBaseRequest<ArTarget_CopyrightInfo>.RequestState.SuccessOnline)
                    AppearitionLogger.LogInfo(ArTargetConstant.ARTARGET_COPYRIGHT_SUCCESS);
                else
                    AppearitionLogger.LogInfo(ArTargetConstant.ARTARGET_COPYRIGHT_SUCCESS_OFFLINE);

                arTarget.copyrightInfo = arTargetCopyrightRequest.RequestResponseObject.Data?.copyrightInfo;

                //Callback it out ~
                if (onSuccess != null)
                    onSuccess(arTarget, arTargetCopyrightRequest.RequestResponseObject.Data);
            }
            else
            {
                AppearitionLogger.LogError(string.Format(ArTargetConstant.ARTARGET_COPYRIGHT_FAILURE, arTarget.arTargetId));

                //Request failed =(
                if (onFailure != null)
                    onFailure(arTargetCopyrightRequest.Errors);
            }

            if (onComplete != null)
                onComplete(arTargetCopyrightRequest.RequestResponseObject.IsSuccess);
        }

        #endregion

        #region File Handling

        #region Asset

        /// <summary>
        /// Downloads the selected content of a given asset.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="contents"></param>
        /// <param name="downloadTargetImages"></param>
        /// <param name="downloadPreDownload"></param>
        /// <param name="downloadAllContent"></param>
        /// <param name="downloadTransferStatus"></param>
        /// <returns></returns>
        public static IEnumerator DownloadAssetsContent<T>(IEnumerable<T> contents,
            bool downloadTargetImages, bool downloadPreDownload = true, bool downloadAllContent = false,
            DataTransferStatus downloadTransferStatus = null) where T : Asset
        {
            int totalMediaCount = 0;
            int mediaDownloadedCount = 0;

            bool transferStatusCreated = false;
            if (downloadTransferStatus == null)
            {
                downloadTransferStatus = new DataTransferStatus();
                transferStatusCreated = true;
            }

            if (!ArTargetConstant.allowParallelDownloads)
            {
                int quantityOfFilesToDownload = 0;

                //Predict the amount of files required.
                foreach (T tmpAsset in contents)
                {
                    if (downloadTargetImages)
                        quantityOfFilesToDownload += tmpAsset.targetImages.Count;
                    if (downloadAllContent)
                    {
                        for (int i = 0; i < tmpAsset.mediaFiles.Count; i++)
                        {
                            if (!string.IsNullOrEmpty(tmpAsset.mediaFiles[i].url) &&
                                !string.IsNullOrEmpty(tmpAsset.mediaFiles[i].fileName))
                                quantityOfFilesToDownload++;
                        }
                    }
                    else if (downloadPreDownload)
                    {
                        for (int i = 0; i < tmpAsset.mediaFiles.Count; i++)
                        {
                            if (!string.IsNullOrEmpty(tmpAsset.mediaFiles[i].url) &&
                                !string.IsNullOrEmpty(tmpAsset.mediaFiles[i].fileName))
                                quantityOfFilesToDownload++;
                        }
                    }

                    downloadTransferStatus.SetTotalAmountOfItemsOverride(quantityOfFilesToDownload);
                }
            }

            foreach (T tmpAsset in contents)
            {
                if (tmpAsset == null || tmpAsset.mediaFiles == null)
                    continue;

                if (downloadTargetImages)
                {
                    for (int k = 0; k < tmpAsset.targetImages.Count; k++)
                    {
                        if (!string.IsNullOrEmpty(tmpAsset.targetImages[k].url))
                        {
                            TargetImage tmpTargetImage = tmpAsset.targetImages[k];
                            if (ArTargetConstant.allowParallelDownloads)
                                AppearitionGate.Instance.StartCoroutine(DownloadGenericFile(tmpTargetImage.url.Trim(), GetPathToTargetImage(tmpAsset, tmpTargetImage), tmpTargetImage.checksum, true,
                                    obj =>
                                    {
                                        tmpTargetImage.image = ImageUtility.LoadOrCreateSprite(obj, tmpAsset.targetImages[k].checksum);
                                        mediaDownloadedCount++;
                                    }, downloadTransferStatus));
                            else
                                yield return DownloadGenericFile(tmpTargetImage.url.Trim(), GetPathToTargetImage(tmpAsset, tmpTargetImage), tmpTargetImage.checksum, true,
                                    obj =>
                                    {
                                        tmpTargetImage.image = ImageUtility.LoadOrCreateSprite(obj, tmpAsset.targetImages[k].checksum);
                                        mediaDownloadedCount++;
                                    }, downloadTransferStatus);
                            totalMediaCount++;
                        }
                    }
                }

                if (downloadAllContent)
                {
                    for (int k = 0; k < tmpAsset.mediaFiles.Count; k++)
                    {
                        if (!string.IsNullOrEmpty(tmpAsset.mediaFiles[k].url) &&
                            !string.IsNullOrEmpty(tmpAsset.mediaFiles[k].fileName))
                        {
                            MediaFile tmpMediaFile = tmpAsset.mediaFiles[k];

                            //Manually download it.
                            if (ArTargetConstant.allowParallelDownloads)
                            {
                                if (tmpAsset.mediaFiles[k].IsContentLibraryItem)
                                    AppearitionGate.Instance.StartCoroutine(ContentLibraryHandler.GetContentItemProcess(
                                        tmpAsset.mediaFiles[k].contentItemProviderName, tmpAsset.mediaFiles[k].contentItemKey, true, true, GetKeyFromAssetAndMedia(tmpAsset, tmpAsset.mediaFiles[k]),
                                        obj => mediaDownloadedCount++, null, null, downloadTransferStatus));
                                else
                                    AppearitionGate.Instance.StartCoroutine(DownloadGenericFile(tmpMediaFile.url.Trim(), GetPathToMedia(tmpAsset, tmpMediaFile), tmpMediaFile.checksum, true,
                                        obj => mediaDownloadedCount++,
                                        downloadTransferStatus));
                            }
                            else
                            {
                                if (tmpAsset.mediaFiles[k].IsContentLibraryItem)
                                    yield return ContentLibraryHandler.GetContentItemProcess(
                                        tmpAsset.mediaFiles[k].contentItemProviderName, tmpAsset.mediaFiles[k].contentItemKey, true, true, GetKeyFromAssetAndMedia(tmpAsset, tmpAsset.mediaFiles[k]),
                                        obj => mediaDownloadedCount++, null, null, downloadTransferStatus);
                                else
                                    yield return DownloadGenericFile(tmpMediaFile.url.Trim(), GetPathToMedia(tmpAsset, tmpMediaFile), tmpMediaFile.checksum, true, obj => mediaDownloadedCount++,
                                        downloadTransferStatus);
                            }

                            totalMediaCount++;
                        }
                    }
                }
                else if (downloadPreDownload)
                {
                    for (int k = 0; k < tmpAsset.mediaFiles.Count; k++)
                    {
                        if (!string.IsNullOrEmpty(tmpAsset.mediaFiles[k].url) &&
                            !string.IsNullOrEmpty(tmpAsset.mediaFiles[k].fileName)
                            && tmpAsset.mediaFiles[k].isPreDownload)
                        {
                            MediaFile tmpMediaFile = tmpAsset.mediaFiles[k];

                            //Manually download it.
                            if (ArTargetConstant.allowParallelDownloads)
                            {
                                if (tmpAsset.mediaFiles[k].IsContentLibraryItem)
                                    AppearitionGate.Instance.StartCoroutine(ContentLibraryHandler.GetContentItemProcess(
                                        tmpAsset.mediaFiles[k].contentItemProviderName, tmpAsset.mediaFiles[k].contentItemKey, true, true, GetKeyFromAssetAndMedia(tmpAsset, tmpAsset.mediaFiles[k]),
                                        obj => mediaDownloadedCount++, null, null, downloadTransferStatus));
                                else
                                    AppearitionGate.Instance.StartCoroutine(DownloadGenericFile(tmpMediaFile.url.Trim(), GetPathToMedia(tmpAsset, tmpMediaFile), tmpMediaFile.checksum, true,
                                        obj => mediaDownloadedCount++,
                                        downloadTransferStatus));
                            }
                            else
                            {
                                if (tmpAsset.mediaFiles[k].IsContentLibraryItem)
                                    yield return ContentLibraryHandler.GetContentItemProcess(
                                        tmpAsset.mediaFiles[k].contentItemProviderName, tmpAsset.mediaFiles[k].contentItemKey, true, true, GetKeyFromAssetAndMedia(tmpAsset, tmpAsset.mediaFiles[k]),
                                        obj => mediaDownloadedCount++, null, null, downloadTransferStatus);
                                else
                                    yield return DownloadGenericFile(tmpMediaFile.url.Trim(), GetPathToMedia(tmpAsset, tmpMediaFile), tmpMediaFile.checksum, true, obj => mediaDownloadedCount++,
                                        downloadTransferStatus);
                            }

                            totalMediaCount++;
                        }
                    }
                }
            }

            //while (totalMediaCount != mediaDownloadedCount)
            //    yield return null;

            //Apply lock
            if (transferStatusCreated)
                downloadTransferStatus.ConfirmLastItemEntered();

            while (totalMediaCount != mediaDownloadedCount)
                yield return null;
        }

        /// <summary>
        /// Deletes the cached content of an asset based on given parameters.
        /// </summary>
        /// <typeparam name="T">The type of asset.</typeparam>
        /// <param name="asset">The given asset.</param>
        /// <param name="deleteTargetImages">Whether or not the target images are to be deleted.</param>
        /// <param name="deleteMediaFiles">Whether or not the MediaFile content are to be deleted.</param>
        /// <param name="onComplete">Occurs when the process is completed. Contains whether or not this process has been successfully completed.</param>
        public static void ClearLocalAssetContent<T>(T asset, bool deleteTargetImages, bool deleteMediaFiles, Action<bool> onComplete) where T : Asset
        {
            AppearitionGate.Instance.StartCoroutine(ClearLocalAssetContentProcess<T>(asset, deleteTargetImages, deleteMediaFiles, onComplete));
        }

        /// <summary>
        /// Clears the cached content of an asset based on given parameters.
        /// </summary>
        /// <typeparam name="T">The type of asset.</typeparam>
        /// <param name="asset">The given asset.</param>
        /// <param name="clearTargetImages">Whether or not the target images are to be removed from local storage.</param>
        /// <param name="clearMediaFiles">Whether or not the MediaFile content are to be removed from local storage.</param>
        /// <param name="onComplete">Occurs when the process is completed. Contains whether or not this process has been successfully completed.</param>
        public static IEnumerator ClearLocalAssetContentProcess<T>(T asset, bool clearTargetImages, bool clearMediaFiles, Action<bool> onComplete) where T : Asset
        {
            bool isSuccessful = true;
            if (clearTargetImages)
            {
                for (int i = 0; i < asset.targetImages.Count; i++)
                {
                    string path = GetPathToTargetImage(asset, asset.targetImages[i]);

                    if (File.Exists(path))
                    {
                        yield return DeleteFileProcess(path, success => isSuccessful = isSuccessful && success);

                        if (!isSuccessful)
                        {
                            AppearitionLogger.LogError("An error occured when trying to delete the file at path " + path);
                        }
                    }
                }
            }

            if (clearMediaFiles)
            {
                for (int i = 0; i < asset.mediaFiles.Count; i++)
                {
                    string path = GetPathToMedia(asset, asset.mediaFiles[i]);

                    if (File.Exists(path))
                    {
                        yield return DeleteFileProcess(path, success => isSuccessful = isSuccessful && success);

                        if (!isSuccessful)
                        {
                            AppearitionLogger.LogError("An error occured when trying to delete the file at path " + path);
                        }
                    }
                }
            }

            if (onComplete != null)
                onComplete(isSuccessful);
        }

        #endregion

        #region Media

        /// <summary>
        /// Loads the content of the MediaFile and returns it once loaded or downloaded in a callback.
        /// If this process failed, the callback will contain a null object.
        /// </summary>
        /// <param name="asset">The asset which contains the given Mediafile.</param>
        /// <param name="mediaFile">The MediaFile you wish to load.</param>
        /// <param name="onComplete">Occurs when the process is completed. Contains the data of the loaded MediaFile.</param>
        /// <param name="downloadTransferStatus"></param>
        public static void LoadMediaFileContent(Asset asset, MediaFile mediaFile, Action<Dictionary<string, byte[]>> onComplete = null, DataTransferStatus downloadTransferStatus = null)
        {
            AppearitionGate.Instance.StartCoroutine(LoadMediaFileContentProcess(asset, mediaFile, onComplete, downloadTransferStatus));
        }

        /// <summary>
        /// Loads the content of the MediaFile and returns it once loaded or downloaded in a callback.
        /// If this process failed, the callback will contain a null object.
        /// </summary>
        /// <param name="asset">The asset which contains the given Mediafile.</param>
        /// <param name="mediaFile">The MediaFile you wish to load.</param>
        /// <param name="onComplete">Occurs when the process is completed. Contains the data of the loaded MediaFile.</param>
        /// <param name="downloadTransferStatus"></param>
        public static IEnumerator LoadMediaFileContentProcess(Asset asset, MediaFile mediaFile, Action<Dictionary<string, byte[]>> onComplete = null, DataTransferStatus downloadTransferStatus = null)
        {
            if (mediaFile.IsContentLibraryItem)
                yield return ContentLibraryHandler.DownloadFirstLibraryFileFoundProcess(asset.productId, mediaFile.contentItemProviderName, mediaFile.contentItemKey,
                    GetKeyFromAssetAndMedia(asset, mediaFile),
                    onComplete, downloadTransferStatus);
            else
                yield return DownloadGenericFile(mediaFile.url.Trim(), GetPathToMedia(asset, mediaFile), mediaFile.checksum, true,
                    complete => onComplete?.Invoke(new Dictionary<string, byte[]> {{mediaFile.fileName, complete}}), downloadTransferStatus);
        }

        /// <summary>
        /// Deletes the cached files of a given MediaFile.
        /// </summary>
        /// <param name="asset">The asset which contains the given Mediafile.</param>
        /// <param name="mediaFile">The MediaFile you wish to delete.</param>
        /// <param name="onComplete">Occurs when the process is completed. Contains whether or not this process has been successfully completed.</param>
        /// <returns></returns>
        public static void ClearLocalMediaFileContent(Asset asset, MediaFile mediaFile, Action<bool> onComplete)
        {
            AppearitionGate.Instance.StartCoroutine(ClearLocalMediaFileContentProcess(asset, mediaFile, onComplete));
        }

        /// <summary>
        /// Deletes the cached files of a given MediaFile.
        /// </summary>
        /// <param name="asset">The asset which contains the given Mediafile.</param>
        /// <param name="mediaFile">The MediaFile you wish to delete.</param>
        /// <param name="onComplete">Occurs when the process is completed. Contains whether or not this process has been successfully completed.</param>
        /// <returns></returns>
        public static IEnumerator ClearLocalMediaFileContentProcess(Asset asset, MediaFile mediaFile, Action<bool> onComplete)
        {
            yield return DeleteFileProcess(GetPathToMedia(asset, mediaFile), onComplete);
        }

        #endregion

        #endregion
    }
}