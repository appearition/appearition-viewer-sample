using System.Collections;
using System.Collections.Generic;
using Appearition.Common;
using UnityEngine;
using System;
using Appearition.ArTargetImageAndMedia;
using Appearition.Internal;
using Appearition.Promotions.API;

namespace Appearition.Promotions
{
    public class PromotionsHandler : BaseHandler
    {
        #region Handler Settings

        /// <summary>
        /// Path to the Media directory (or any data extending MediaFile), usually contained inside Asset directory.
        /// </summary>
        /// <param name="promotion"></param>
        /// <param name="media"></param>
        /// <returns></returns>
        public static string GetPathToMediaDirectory(Promotion promotion, MediaFile media)
        {
            return string.Format("{0}/{1}", GetHandlerStoragePath<PromotionsHandler>(), promotion.id);
        }

        /// <summary>
        /// Path to a Media file.
        /// </summary>
        /// <param name="promotion"></param>
        /// <param name="media"></param>
        public static string GetPathToMedia(Promotion promotion, MediaFile media)
        {
            return string.Format("{0}/{1}", GetPathToMediaDirectory(promotion, media), media.fileName.Trim());
        }

        #endregion

        #region List By Channel

        /// <summary>
        /// Fetches the Promotions of the selected channel.
        /// Do note that expired promotions do not appear.
        ///
        /// Offline Capability.
        /// </summary>
        /// <param name="downloadMedia">Whether to download the media now, or manually later using LoadMediaFileContentProcess.</param>
        /// <param name="onSuccess">Contains the Promotions as on the EMS. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void GetChannelPromotions(bool downloadMedia, Action<List<Promotion>> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(GetChannelPromotionsProcess( AppearitionGate.Instance.CurrentUser.selectedChannel, downloadMedia, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Fetches the Promotions of the selected channel.
        /// Do note that expired promotions do not appear.
        ///
        /// Offline Capability.
        /// </summary>
        /// <param name="downloadMedia">Whether to download the media now, or manually later using LoadMediaFileContentProcess.</param>
        /// <param name="onSuccess">Contains the Promotions as on the EMS. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator GetChannelPromotionsProcess(bool downloadMedia, Action<List<Promotion>> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            yield return GetChannelPromotionsProcess( AppearitionGate.Instance.CurrentUser.selectedChannel, downloadMedia, onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Fetches the Promotions of a given channel.
        /// Do note that expired promotions do not appear.
        ///
        /// Offline Capability.
        /// </summary>
        /// <param name="channelId">The id of the targeted channel.</param>
        /// <param name="downloadMedia">Whether to download the media now, or manually later using LoadMediaFileContentProcess.</param>
        /// <param name="onSuccess">Contains the Promotions as on the EMS. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void GetChannelPromotions(int channelId, bool downloadMedia, Action<List<Promotion>> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(GetChannelPromotionsProcess(channelId, downloadMedia, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Fetches the Promotions of a given channel.
        /// Do note that expired promotions do not appear.
        /// 
        /// Offline Capability.
        /// </summary>
        /// <param name="channelId">The id of the targeted channel.</param>
        /// <param name="downloadMedia">Whether to download the media now, or manually later using LoadMediaFileContentProcess.</param>
        /// <param name="onSuccess">Contains the Promotions as on the EMS. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator GetChannelPromotionsProcess(int channelId, bool downloadMedia, Action<List<Promotion>> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            //Launch request
            var promotionsListRequest = AppearitionRequest<Promotions_ListByChannel>.LaunchAPICall_GET<PromotionsHandler>(channelId, GetReusableApiRequest<Promotions_ListByChannel>());

            //Wait for request..
            while (!promotionsListRequest.IsDone)
                yield return null;

            //All done!
            if (promotionsListRequest.RequestResponseObject.Data != null && promotionsListRequest.RequestResponseObject.IsSuccess)
            {
                var outcome = new List<Promotion>(promotionsListRequest.RequestResponseObject.Data.promotions);

                if (promotionsListRequest.CurrentState == AppearitionBaseRequest<Promotions_ListByChannel>.RequestState.SuccessOnline)
                    AppearitionLogger.LogInfo(string.Format(PromotionsConstants.LIST_BY_CHANNEL_SUCCESS, channelId));
                else
                {
                    AppearitionLogger.LogInfo(string.Format(PromotionsConstants.LIST_BY_CHANNEL_SUCCESS_OFFLINE, channelId));
                    //Remove promotions which are gone
                    for (int i = outcome.Count - 1; i >= 0; i--)
                    {
                        if (outcome[i].DateEnd > DateTime.Now)
                            outcome.RemoveAt(i);
                    }
                }

                if (downloadMedia)
                {
                    for (int i = 0; i < outcome.Count; i++)
                    {
                        if (outcome[i].mediaFiles != null)
                        {
                            for (int k = 0; k < outcome[i].mediaFiles.Count; k++)
                                yield return LoadMediaFileContentProcess(outcome[i], outcome[i].mediaFiles[k]);
                        }
                    }
                }

                //Callback it out ~
                onSuccess?.Invoke(outcome);
            }
            else
            {
                AppearitionLogger.LogError(string.Format(PromotionsConstants.LIST_BY_CHANNEL_SUCCESS, channelId));

                //Request failed =(
                if (onFailure != null)
                    onFailure(promotionsListRequest.Errors);
            }

            if (onComplete != null)
                onComplete(promotionsListRequest.RequestResponseObject.Data != null && promotionsListRequest.RequestResponseObject.IsSuccess);
        }

        #endregion

        #region FileHandling

        #region Media

        /// <summary>
        /// Loads the content of the MediaFile and returns it once loaded or downloaded in a callback.
        /// If this process failed, the callback will contain a null object.
        /// </summary>
        /// <param name="promotion">The promotion which contains the given Mediafile.</param>
        /// <param name="mediaFile">The MediaFile you wish to load.</param>
        /// <param name="onComplete">Occurs when the process is completed. Contains the data of the loaded MediaFile.</param>
        /// <param name="downloadTransferStatus"></param>
        public static void LoadMediaFileContent(Promotion promotion, MediaFile mediaFile, Action<Dictionary<string, byte[]>> onComplete = null, DataTransferStatus downloadTransferStatus = null)
        {
            AppearitionGate.Instance.StartCoroutine(LoadMediaFileContentProcess(promotion, mediaFile, onComplete, downloadTransferStatus));
        }

        /// <summary>
        /// Loads the content of the MediaFile and returns it once loaded or downloaded in a callback.
        /// If this process failed, the callback will contain a null object.
        /// </summary>
        /// <param name="promotion">The promotion which contains the given Mediafile.</param>
        /// <param name="mediaFile">The MediaFile you wish to load.</param>
        /// <param name="onComplete">Occurs when the process is completed. Contains the data of the loaded MediaFile.</param>
        /// <param name="downloadTransferStatus"></param>
        public static IEnumerator LoadMediaFileContentProcess(Promotion promotion, MediaFile mediaFile, Action<Dictionary<string, byte[]>> onComplete = null,
            DataTransferStatus downloadTransferStatus = null)
        {
            yield return DownloadGenericFile(mediaFile.url.Trim(), GetPathToMedia(promotion, mediaFile), mediaFile.checksum, true,
                complete => onComplete?.Invoke(new Dictionary<string, byte[]> {{mediaFile.fileName, complete}}), downloadTransferStatus);
        }

        /// <summary>
        /// Deletes the cached files of a given MediaFile.
        /// </summary>
        /// <param name="promotion">The promotion which contains the given Mediafile.</param>
        /// <param name="mediaFile">The MediaFile you wish to delete.</param>
        /// <param name="onComplete">Occurs when the process is completed. Contains whether or not this process has been successfully completed.</param>
        /// <returns></returns>
        public static void ClearLocalMediaFileContent(Promotion promotion, MediaFile mediaFile, Action<bool> onComplete)
        {
            AppearitionGate.Instance.StartCoroutine(ClearLocalMediaFileContentProcess(promotion, mediaFile, onComplete));
        }

        /// <summary>
        /// Deletes the cached files of a given MediaFile.
        /// </summary>
        /// <param name="promotion">The promotion which contains the given Mediafile.</param>
        /// <param name="mediaFile">The MediaFile you wish to delete.</param>
        /// <param name="onComplete">Occurs when the process is completed. Contains whether or not this process has been successfully completed.</param>
        /// <returns></returns>
        public static IEnumerator ClearLocalMediaFileContentProcess(Promotion promotion, MediaFile mediaFile, Action<bool> onComplete)
        {
            yield return DeleteFileProcess(GetPathToMedia(promotion, mediaFile), onComplete);
        }

        #endregion

        #endregion
    }
}