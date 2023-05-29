using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Appearition.Common;
using Appearition.ContentLibrary.API;
using Appearition.Internal;
using UnityEngine;

namespace Appearition.ContentLibrary
{
    /// <summary>
    /// Handler in charge of taking care of the content library features and utilities.
    /// </summary>
    public class ContentLibraryHandler : BaseHandler
    {
        #region Handler Settings

        /// <summary>
        /// Path to the directory holding Content library provider.
        /// </summary>
        /// <param name="providerName"></param>
        /// <returns></returns>
        public static string GetPathToContentProviderDirectory(string providerName)
        {
            return string.Format("{0}/{1}", GetHandlerStoragePath<ContentLibraryHandler>(), providerName);
        }

        /// <summary>
        /// Path to the directory holding Content library file.
        /// </summary>
        /// <param name="providerName"></param>
        /// <param name="fileKey"></param>
        /// <returns></returns>
        public static string GetPathToContentFileDirectory(string providerName, string fileKey)
        {
            if (string.IsNullOrEmpty(fileKey))
                return GetPathToContentProviderDirectory(providerName);
            return $"{GetPathToContentProviderDirectory(providerName)}/{fileKey}";
        }

        /// <summary>
        /// Path to the directory holding Content library thumbnail.
        /// </summary>
        /// <param name="providerName"></param>
        /// <param name="fileKey"></param>
        /// <returns></returns>
        public static string GetPathToContentThumbnailDirectory(string providerName, string fileKey)
        {
            if (string.IsNullOrEmpty(fileKey))
                return $"{GetPathToContentProviderDirectory(providerName)}/Thumbnail";
            return $"{GetPathToContentProviderDirectory(providerName)}/{fileKey}/Thumbnail";
        }

        /// <summary>
        /// Path to a Content library file.
        /// </summary>
        /// <param name="providerName"></param>
        /// <param name="file"></param>
        /// <param name="fileKey"></param>
        public static string GetPathToContentFile(string providerName, ContentFile file, string fileKey)
        {
            if (string.IsNullOrEmpty(file.SubFolder))
                return $"{GetPathToContentFileDirectory(providerName, fileKey)}/{file.FileName.Trim()}";
            return $"{GetPathToContentFileDirectory(providerName, fileKey)}/{file.SubFolder}/{file.FileName.Trim()}";
        }

        /// <summary>
        /// Tries to find the first file with stored under a given fileKey.
        /// </summary>
        /// <param name="providerName"></param>
        /// <param name="fileKey"></param>
        /// <returns></returns>
        public static string TryToFindFirstItemWithContentKey(string providerName, string fileKey)
        {
            string pathToDirectory = GetPathToContentFileDirectory(providerName, fileKey);

            //Return the first file path inside
            return Directory.GetFiles(pathToDirectory, "*").FirstOrDefault() ?? "";
        }

        /// <summary>
        /// Path to a Content library file.
        /// </summary>
        /// <param name="providerName"></param>
        /// <param name="thumbnail"></param>
        /// <param name="fileKey"></param>
        public static string GetPathToContentThumbnail(string providerName, ContentFile thumbnail, string fileKey)
        {
            return string.Format("{0}/{1}", GetPathToContentThumbnailDirectory(providerName, fileKey), thumbnail?.FileName.Trim() ?? "thumbnail.jpg");
        }

        #endregion

        #region Content

        #region Provider

        /// <summary>
        /// Fetches all the Content Library Providers at a given channel.
        /// </summary>
        /// <param name="onSuccess">Contains a list of the providers. </param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void GetContentProviders(Action<List<ContentProvider>> onSuccess, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(GetContentProvidersProcess(AppearitionGate.Instance.CurrentUser.selectedChannel, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Fetches all the Content Library Providers at a given channel.
        /// </summary>
        /// <param name="onSuccess">Contains a list of the providers. </param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator GetContentProvidersProcess(Action<List<ContentProvider>> onSuccess, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            yield return GetContentProvidersProcess(AppearitionGate.Instance.CurrentUser.selectedChannel, onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Fetches all the Content Library Providers at a given channel.
        /// </summary>
        /// <param name="channelId">Target Channel Id</param>
        /// <param name="onSuccess">Contains a list of the providers. </param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void GetContentProviders(int channelId, Action<List<ContentProvider>> onSuccess, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(GetContentProvidersProcess(channelId, onSuccess, onFailure, onComplete));
        }


        /// <summary>
        /// Fetches all the Content Library Providers at a given channel.
        /// </summary>
        /// <param name="channelId">Target Channel Id</param>
        /// <param name="onSuccess">Contains a list of the providers. </param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator GetContentProvidersProcess(int channelId, Action<List<ContentProvider>> onSuccess, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            //Online request
            var providersRequest = AppearitionRequest<Content_Providers>.LaunchAPICall_GET<ContentLibraryHandler>
                (channelId, GetReusableApiRequest<Content_Providers>());

            while (!providersRequest.IsDone)
                yield return null;

            //Debug result
            if (providersRequest.RequestResponseObject.IsSuccess)
            {
                AppearitionLogger.LogInfo(ContentLibraryConstants.GET_PROVIDERS_SUCCESS);
                onSuccess?.Invoke(providersRequest.RequestResponseObject.Data);
            }
            else
            {
                AppearitionLogger.LogError(string.Format(ContentLibraryConstants.GET_PROVIDERS_FAILURE, channelId));
                if (onFailure != null && providersRequest.RequestResponseObject.Errors.Length > 0)
                    onFailure(providersRequest.Errors);
            }

            if (onComplete != null)
                onComplete(providersRequest.RequestResponseObject.IsSuccess);
        }

        #endregion

        #region Search

        /// <summary>
        /// Queries the EMS to get content library items from a given provider.
        /// Optionally, their thumbnails and files can be downloaded at this point.
        /// </summary>
        /// <param name="providerName">Name of the provider to get Content Library Items from.</param>
        /// <param name="query">Search query.</param>
        /// <param name="onSuccess">Contains the result of the search query. All the Content Library Items are present inside Items Found.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void GetSpecificContentLibraryItems(string providerName, Content_Search.QueryContent query,
            Action<Content_Search.QueryOutcome> onSuccess, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(GetSpecificContentLibraryItemsProcess(AppearitionGate.Instance.CurrentUser.selectedChannel, providerName, query, false, false, onSuccess, onFailure,
                onComplete, null));
        }

        /// <summary>
        /// Queries the EMS to get content library items from a given provider.
        /// Optionally, their thumbnails and files can be downloaded at this point.
        /// </summary>
        /// <param name="providerName">Name of the provider to get Content Library Items from.</param>
        /// <param name="query">Search query.</param>
        /// <param name="onSuccess">Contains the result of the search query. All the Content Library Items are present inside Items Found.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator GetSpecificContentLibraryItemsProcess(string providerName, Content_Search.QueryContent query,
            Action<Content_Search.QueryOutcome> onSuccess, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            yield return GetSpecificContentLibraryItemsProcess(AppearitionGate.Instance.CurrentUser.selectedChannel, providerName, query, false, false, onSuccess, onFailure, onComplete, null);
        }

        /// <summary>
        /// Queries the EMS to get content library items from a given provider.
        /// Optionally, their thumbnails and files can be downloaded at this point.
        /// </summary>
        /// <param name="channelId">Target Channel Id</param>
        /// <param name="providerName">Name of the provider to get Content Library Items from.</param>
        /// <param name="query">Search query.</param>
        /// <param name="onSuccess">Contains the result of the search query. All the Content Library Items are present inside Items Found.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void GetSpecificContentLibraryItems(int channelId, string providerName, Content_Search.QueryContent query,
            Action<Content_Search.QueryOutcome> onSuccess, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(GetSpecificContentLibraryItemsProcess(channelId, providerName, query, false, false, onSuccess, onFailure, onComplete, null));
        }

        /// <summary>
        /// Queries the EMS to get content library items from a given provider.
        /// Optionally, their thumbnails and files can be downloaded at this point.
        /// </summary>
        /// <param name="channelId">Target Channel Id</param>
        /// <param name="providerName">Name of the provider to get Content Library Items from.</param>
        /// <param name="query">Search query.</param>
        /// <param name="onSuccess">Contains the result of the search query. All the Content Library Items are present inside Items Found.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator GetSpecificContentLibraryItemsProcess(int channelId, string providerName, Content_Search.QueryContent query,
            Action<Content_Search.QueryOutcome> onSuccess, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            yield return GetSpecificContentLibraryItemsProcess(channelId, providerName, query, false, false, onSuccess, onFailure, onComplete, null);
        }

        /// <summary>
        /// Queries the EMS to get content library items from a given provider.
        /// Optionally, their thumbnails and files can be downloaded at this point.
        /// </summary>
        /// <param name="providerName">Name of the provider to get Content Library Items from.</param>
        /// <param name="query">Search query.</param>
        /// <param name="downloadThumbnail">Whether to download Content Library Item thumbnails.</param>
        /// <param name="downloadFiles">Whether to download Content Library Item files.</param>
        /// <param name="onSuccess">Contains the result of the search query. All the Content Library Items are present inside Items Found.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        /// <param name="downloadTransferStatus">The transfer process tracker.</param>
        public static void GetSpecificContentLibraryItems(string providerName, Content_Search.QueryContent query,
            bool downloadThumbnail, bool downloadFiles, Action<Content_Search.QueryOutcome> onSuccess, Action<EmsError> onFailure = null, Action<bool> onComplete = null,
            DataTransferStatus downloadTransferStatus = null)
        {
            AppearitionGate.Instance.StartCoroutine(GetSpecificContentLibraryItemsProcess(AppearitionGate.Instance.CurrentUser.selectedChannel, providerName, query, downloadThumbnail, downloadFiles,
                onSuccess, onFailure,
                onComplete, downloadTransferStatus));
        }

        /// <summary>
        /// Queries the EMS to get content library items from a given provider.
        /// Optionally, their thumbnails and files can be downloaded at this point.
        /// </summary>
        /// <param name="providerName">Name of the provider to get Content Library Items from.</param>
        /// <param name="query">Search query.</param>
        /// <param name="downloadThumbnail">Whether to download Content Library Item thumbnails.</param>
        /// <param name="downloadFiles">Whether to download Content Library Item files.</param>
        /// <param name="onSuccess">Contains the result of the search query. All the Content Library Items are present inside Items Found.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        /// <param name="downloadTransferStatus">The transfer process tracker.</param>
        public static IEnumerator GetSpecificContentLibraryItemsProcess(string providerName, Content_Search.QueryContent query,
            bool downloadThumbnail, bool downloadFiles, Action<Content_Search.QueryOutcome> onSuccess, Action<EmsError> onFailure = null, Action<bool> onComplete = null,
            DataTransferStatus downloadTransferStatus = null)
        {
            yield return GetSpecificContentLibraryItemsProcess(AppearitionGate.Instance.CurrentUser.selectedChannel, providerName, query, downloadThumbnail, downloadFiles, onSuccess, onFailure,
                onComplete, downloadTransferStatus);
        }

        /// <summary>
        /// Queries the EMS to get content library items from a given provider.
        /// Optionally, their thumbnails and files can be downloaded at this point.
        /// </summary>
        /// <param name="channelId">Target Channel Id</param>
        /// <param name="providerName">Name of the provider to get Content Library Items from.</param>
        /// <param name="query">Search query.</param>
        /// <param name="downloadThumbnail">Whether to download Content Library Item thumbnails.</param>
        /// <param name="downloadFiles">Whether to download Content Library Item files.</param>
        /// <param name="onSuccess">Contains the result of the search query. All the Content Library Items are present inside Items Found.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        /// <param name="downloadTransferStatus">The transfer process tracker.</param>
        public static void GetSpecificContentLibraryItems(int channelId, string providerName, Content_Search.QueryContent query,
            bool downloadThumbnail, bool downloadFiles, Action<Content_Search.QueryOutcome> onSuccess, Action<EmsError> onFailure = null, Action<bool> onComplete = null,
            DataTransferStatus downloadTransferStatus = null)
        {
            AppearitionGate.Instance.StartCoroutine(GetSpecificContentLibraryItemsProcess(channelId, providerName, query, downloadThumbnail, downloadFiles, onSuccess, onFailure, onComplete,
                downloadTransferStatus));
        }

        /// <summary>
        /// Queries the EMS to get content library items from a given provider.
        /// Optionally, their thumbnails and files can be downloaded at this point.
        /// </summary>
        /// <param name="channelId">Target Channel Id</param>
        /// <param name="providerName">Name of the provider to get Content Library Items from.</param>
        /// <param name="query">Search query.</param>
        /// <param name="downloadThumbnail">Whether to download Content Library Item thumbnails.</param>
        /// <param name="downloadFiles">Whether to download Content Library Item files.</param>
        /// <param name="onSuccess">Contains the result of the search query. All the Content Library Items are present inside Items Found.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        /// <param name="downloadTransferStatus">The transfer process tracker.</param>
        public static IEnumerator GetSpecificContentLibraryItemsProcess(int channelId, string providerName, Content_Search.QueryContent query,
            bool downloadThumbnail, bool downloadFiles, Action<Content_Search.QueryOutcome> onSuccess, Action<EmsError> onFailure = null, Action<bool> onComplete = null,
            DataTransferStatus downloadTransferStatus = null)
        {
            if (string.IsNullOrEmpty(providerName))
            {
                AppearitionLogger.LogError(ContentLibraryConstants.NO_PROVIDER_NAME_PROVIDED);
                onFailure?.Invoke(new EmsError(ContentLibraryConstants.NO_PROVIDER_NAME_PROVIDED));
                onComplete?.Invoke(false);
                yield break;
            }

            if (query == null)
                query = ContentLibraryConstants.GetDefaultSearchQuery();

            Content_Item.RequestContent requestContent = new Content_Item.RequestContent() {providerName = providerName};

            //Launch request
            var searchRequest = AppearitionRequest<Content_Search>.LaunchAPICall_POST<ContentLibraryHandler>
                (channelId, GetReusableApiRequest<Content_Search>(), query, requestContent);

            //Wait for request..
            while (!searchRequest.IsDone)
                yield return null;

            //All done!
            if (searchRequest.RequestResponseObject.Data != null && searchRequest.RequestResponseObject.IsSuccess)
            {
                if (downloadThumbnail || downloadFiles)
                {
                    for (int i = 0; i < searchRequest.RequestResponseObject.Data.ItemsFound.Count; i++)
                    {
                        if (downloadThumbnail && searchRequest.RequestResponseObject.Data.ItemsFound[i].ThumbnailImage != null &&
                            !string.IsNullOrEmpty(searchRequest.RequestResponseObject.Data.ItemsFound[i].ThumbnailImage.FileName))
                            yield return DownloadLibraryFileThumbnail(providerName, searchRequest.RequestResponseObject.Data.ItemsFound[i].ThumbnailImage,
                                searchRequest.RequestResponseObject.Data.ItemsFound[i].Title.Trim(), true, null, downloadTransferStatus);

                        if (downloadFiles && searchRequest.RequestResponseObject.Data.ItemsFound[i].Files != null)
                        {
                            for (int k = 0; k < searchRequest.RequestResponseObject.Data.ItemsFound[i].Files.Count; k++)
                            {
                                yield return DownloadLibraryFileContent(providerName, searchRequest.RequestResponseObject.Data.ItemsFound[i].Files[k],
                                    searchRequest.RequestResponseObject.Data.ItemsFound[i].Title.Trim(), null, downloadTransferStatus);
                            }
                        }
                    }
                }

                if (searchRequest.RequestResponseObject.Data.ItemsFound.Count == 0)
                    AppearitionLogger.LogInfo(string.Format(ContentLibraryConstants.SEARCH_SUCCESS_EMPTY, providerName, AppearitionConstants.SerializeJson(query)));
                else
                    AppearitionLogger.LogInfo(string.Format(ContentLibraryConstants.SEARCH_SUCCESS, providerName, searchRequest.RequestResponseObject.Data.ItemsFound.Count,
                        AppearitionConstants.SerializeJson(query)));

                //Callback it out ~
                if (onSuccess != null)
                    onSuccess(searchRequest.RequestResponseObject.Data);
            }
            else
            {
                AppearitionLogger.LogError(string.Format(ContentLibraryConstants.SEARCH_FAILURE, providerName, channelId, AppearitionConstants.SerializeJson(query)));

                //Request failed =(
                if (onFailure != null)
                    onFailure(searchRequest.Errors);
            }

            if (onComplete != null)
                onComplete(searchRequest.RequestResponseObject.IsSuccess);
        }

        #endregion

        #region Get Content Item

        public static IEnumerator GetContentItemProcess(string providerName, string contentItemKey, bool downloadThumbnail, bool downloadContentFiles, string customFileKey = "",
            Action<ContentItem> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null, DataTransferStatus downloadTransferStatus = null)
        {
            yield return GetContentItemProcess(AppearitionGate.Instance.CurrentUser.selectedChannel, providerName, contentItemKey,
                downloadThumbnail, downloadContentFiles, customFileKey, onSuccess, onFailure, onComplete, downloadTransferStatus);
        }

        public static IEnumerator GetContentItemProcess(int channelId, string providerName, string contentItemKey, bool downloadThumbnail, bool downloadContentFiles, string customFileKey = "",
            Action<ContentItem> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null, DataTransferStatus downloadTransferStatus = null)
        {
            if (string.IsNullOrEmpty(providerName))
            {
                AppearitionLogger.LogError(ContentLibraryConstants.NO_PROVIDER_NAME_PROVIDED);
                onFailure?.Invoke(new EmsError(ContentLibraryConstants.NO_PROVIDER_NAME_PROVIDED));
                onComplete?.Invoke(false);
                yield break;
            }

            Content_Item reusableApiContainer = GetReusableApiRequest<Content_Item>();
            Content_Item.RequestContent requestContent = new Content_Item.RequestContent() {providerName = providerName, itemKey = contentItemKey};

            //Launch request
            var getContentItem =
                AppearitionRequest<Content_Item>.LaunchAPICall_GET<ContentLibraryHandler>(channelId, reusableApiContainer, requestContent, obj => { reusableApiContainer = obj; });

            //Wait for request..
            while (!getContentItem.IsDone)
                yield return null;

            //All done!
            if (reusableApiContainer != null && reusableApiContainer.IsSuccess)
            {
                //Success ! If required, download the sub-content.
                if (downloadThumbnail && reusableApiContainer.Data.ThumbnailImage != null && !string.IsNullOrEmpty(reusableApiContainer.Data.ThumbnailImage.FileName))
                    yield return DownloadLibraryFileThumbnail(providerName, reusableApiContainer.Data.ThumbnailImage,
                        string.IsNullOrEmpty(customFileKey) ? reusableApiContainer.Data.Title.Trim() : customFileKey, true, null, downloadTransferStatus);

                if (downloadContentFiles && reusableApiContainer.Data.Files != null)
                {
                    for (int i = 0; i < reusableApiContainer.Data.Files.Count; i++)
                        yield return DownloadLibraryFileContent(providerName, reusableApiContainer.Data.Files[i],
                            string.IsNullOrEmpty(customFileKey) ? reusableApiContainer.Data.Title.Trim() : customFileKey, null, downloadTransferStatus);
                }

                if (getContentItem.CurrentState == AppearitionBaseRequest<Content_Item>.RequestState.SuccessOnline)
                    AppearitionLogger.LogInfo(string.Format(ContentLibraryConstants.GET_CONTENT_ITEM_SUCCESS, channelId));
                else
                    AppearitionLogger.LogInfo(string.Format(ContentLibraryConstants.GET_CONTENT_ITEM_SUCCESS_OFFLINE, channelId));

                //Callback it out ~
                if (onSuccess != null)
                    onSuccess(reusableApiContainer.Data);
            }
            else
            {
                AppearitionLogger.LogError(string.Format(ContentLibraryConstants.GET_CONTENT_ITEM_FAILURE, channelId));

                //Request failed =(
                if (onFailure != null)
                    onFailure(getContentItem.Errors);
            }

            if (onComplete != null)
                onComplete(getContentItem.RequestResponseObject.IsSuccess);
        }

        #endregion

        #endregion

        #region Download

        #region Main Downloads

        /// <summary>
        /// Downloads a single content library file, stores it, and returns the byte content.
        /// </summary>
        /// <param name="providerName">The content provider name.</param>
        /// <param name="file">The given content library file.</param>
        /// <param name="itemKey">Item key, mainly used to pick the file storage location. Can be the item Title.</param>
        /// <param name="onComplete">Occurs when the process is completed. Contains whether or not this process has been successfully completed.</param>
        /// <param name="downloadTransferStatus">The transfer process tracker.</param>
        /// <returns></returns>
        public static IEnumerator DownloadLibraryFileContent<T>(string providerName, T file, string itemKey, Action<byte[]> onComplete = null,
            DataTransferStatus downloadTransferStatus = null) where T : ContentFile
        {
            if (file == null)
            {
                AppearitionLogger.LogWarning(ContentLibraryConstants.CONTENT_FILE_DOWNLOAD_NO_FILE_GIVEN);
                onComplete?.Invoke(null);
                yield break;
            }

            yield return DownloadGenericFile(file.Url, GetPathToContentFile(providerName, file, itemKey), file.Checksum, true, onComplete, downloadTransferStatus);
        }

        /// <summary>
        /// Downloads a single content library file, stores it, and returns the byte content.
        /// </summary>
        /// <param name="providerName">The content provider name.</param>
        /// <param name="itemKey">Item key, mainly used to pick the file storage location. Can be the item Title.</param>
        /// <param name="file">The given content library file.</param>
        /// <param name="loadThumbnail"></param>
        /// <param name="onComplete">Occurs when the process is completed. Contains whether or not this process has been successfully completed.</param>
        /// <param name="downloadTransferStatus">The transfer process tracker.</param>
        /// <returns></returns>
        public static IEnumerator DownloadLibraryFileThumbnail<T>(string providerName, T file, string itemKey, bool loadThumbnail = true, Action<Sprite> onComplete = null,
            DataTransferStatus downloadTransferStatus = null) where T : ContentThumbnail
        {
            if (file == null)
            {
                AppearitionLogger.LogWarning("The file to download is null.");
                onComplete?.Invoke(null);
                yield break;
            }

            yield return DownloadGenericFile(file.Url, GetPathToContentThumbnail(providerName, file, itemKey), file.Checksum, true, bytes =>
            {
                if (bytes != null && loadThumbnail)
                    file.thumbnail = ImageUtility.LoadOrCreateSprite(bytes, file.Checksum);
                onComplete?.Invoke(file.thumbnail);
            }, downloadTransferStatus);
        }

        #endregion

        #region Download First Item Found

        /// <summary>
        /// From a given provider name and item key, fetches the first file that can be downloaded.
        /// </summary>
        /// <param name="providerName">The provider name</param>
        /// <param name="contentItemKey">The content library item key</param>
        /// <param name="itemKey">Item key, mainly used to pick the file storage location. Can be the item Title.</param>
        /// <param name="onComplete">Occurs when the process is completed. Contains whether or not this process has been successfully completed.</param>
        /// <param name="downloadTransferStatus">The transfer process tracker.</param>
        public static IEnumerator DownloadFirstLibraryFileFoundProcess(string providerName, string contentItemKey, string itemKey, Action<Dictionary<string, byte[]>> onComplete = null,
            DataTransferStatus downloadTransferStatus = null)
        {
            yield return DownloadFirstLibraryFileFoundProcess(AppearitionGate.Instance.CurrentUser.selectedChannel, providerName,
                contentItemKey, itemKey, onComplete, downloadTransferStatus);
        }

        /// <summary>
        /// From a given provider name and item key, fetches the first file that can be downloaded.
        /// </summary>
        /// <param name="channelId">The target channel</param>
        /// <param name="providerName">The provider name</param>
        /// <param name="contentItemKey">The content library item key</param>
        /// <param name="itemKey">Item key, mainly used to pick the file storage location. Can be the item Title.</param>
        /// <param name="onComplete">Occurs when the process is completed. Contains whether or not this process has been successfully completed.</param>
        /// <param name="downloadTransferStatus">The transfer process tracker.</param>
        public static IEnumerator DownloadFirstLibraryFileFoundProcess(int channelId, string providerName, string contentItemKey, string itemKey, Action<Dictionary<string, byte[]>> onComplete = null,
            DataTransferStatus downloadTransferStatus = null)
        {
            ContentItem item = null;
            yield return GetContentItemProcess(channelId, providerName, contentItemKey, false, false, itemKey, newItem => item = newItem);

            Dictionary<string, byte[]> outcome = new Dictionary<string, byte[]>();

            if (item == null || item.Files == null || item.Files.Count == 0)
                onComplete?.Invoke(outcome);
            else
            {
                for (int i = item.Files.Count - 1; i >= 0; i--)
                    yield return DownloadLibraryFileContent(providerName, item.Files[i], itemKey,
                        complete => outcome.Add(item.Files[i].FileName, complete), downloadTransferStatus);

                onComplete?.Invoke(outcome);
            }
        }

        #endregion

        #endregion

        #region Clear Content

        /// <summary>
        /// Clears the cached content of an asset based on given parameters.
        /// </summary>
        /// <param name="providerName"></param>
        /// <param name="file"></param>
        /// <param name="itemKey">Item key, mainly used to pick the file storage location. Can be the item Title.</param>
        /// <param name="onComplete">Occurs when the process is completed. Contains whether or not this process has been successfully completed.</param>
        public static IEnumerator ClearLocalAssetContentProcess<T>(string providerName, T file, string itemKey = null, Action<bool> onComplete = null) where T : ContentFile
        {
            bool isSuccessful = true;

            string path = GetPathToContentFile(providerName, file, itemKey);

            if (File.Exists(path))
            {
                yield return DeleteFileProcess(path, success => isSuccessful = isSuccessful && success);

                if (!isSuccessful)
                {
                    AppearitionLogger.LogError("An error occured when trying to delete the file at path " + path);
                }
            }

            if (onComplete != null)
                onComplete(isSuccessful);
        }

        #endregion
    }
}