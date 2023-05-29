// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: PartHandler.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using Appearition.Common;
using Appearition.Part.API;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Appearition.Part
{
    /// <summary>
    /// Handler in charge of providing functionality related to the Part module.
    /// </summary>
    public class PartHandler : BaseFloARHandler
    {
        /// <summary>
        /// Fetches all the part content from the EMS.
        /// If you have existing content, pass in a PartSyncManifest and the difference will be returned instead.
        /// In case of no connection, will simply load the existing data.
        ///
        /// API Requirement: Session Token. Offline Capability.
        /// </summary>
        /// <param name="onSuccess">Contains all the Parts data on the EMS. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void FetchAllParts(Action<PartSyncManifest> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            FetchAllParts(null, onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Fetches all the parts content from the EMS.
        /// If you have existing content, pass in a PartSyncManifest and the difference will be returned instead.
        /// In case of no connection, will simply load the existing data.
        ///
        /// API Requirement: Session Token. Offline Capability.
        /// </summary>
        /// <param name="onSuccess">Contains all the Parts data on the EMS. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator FetchAllPartsProcess(Action<PartSyncManifest> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            yield return FetchAllPartsProcess(null, onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Updates the given parts content. Only the difference in content will be downloaded for a lightweight transfer.
        /// Consider storing that content locally for making full use of this system.
        /// In case of no connection, will simply load the existing data.
        ///
        /// API Requirement: Session Token. Offline Capability.
        /// </summary>
        /// <param name="localPartsData"></param>
        /// <param name="onSuccess">Contains the difference between the provided data and the data on the EMS. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void FetchAllParts(PartSyncManifest localPartsData, Action<PartSyncManifest> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            FetchAllParts(localPartsData, onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Updates the given parts content. Only the difference in content will be downloaded for a lightweight transfer.
        /// Consider storing that content locally for making full use of this system.
        /// In case of no connection, will simply load the existing data.
        /// 
        /// API Requirement: Session Token. Offline Capability.
        /// </summary>
        /// <param name="localPartsData"></param>
        /// <param name="onSuccess">Contains the difference between the provided data and the data on the EMS. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator FetchAllPartsProcess(PartSyncManifest localPartsData, Action<PartSyncManifest> onSuccess = null,
            Action<EmsError> onFailure = null,
            Action<bool> onComplete =
                null)
        {
            //Create data if the developer passed in null data.
            if (localPartsData == null)
                localPartsData = new PartSyncManifest();

            //Online request
            Part_GetPartSyncManifest.PostApi postRequestData = new Part_GetPartSyncManifest.PostApi(localPartsData);

            var getPartsUpdatesRequest = AppearitionRequest<Part_GetPartSyncManifest>.LaunchAPICall_POST<PartHandler>
                (AppearitionGate.Instance.CurrentUser.selectedChannel, GetReusableApiRequest<Part_GetPartSyncManifest>(), postRequestData);

            while (!getPartsUpdatesRequest.IsDone)
                yield return null;

            //Debug result
            if (getPartsUpdatesRequest.RequestResponseObject.IsSuccess)
            {
                //Apply the difference on the manifest.
                PartSyncManifest newData = getPartsUpdatesRequest.RequestResponseObject.Data;

                //If there was no data, simply add the new content.

                if (localPartsData.Parts == null)
                    localPartsData.Parts = new List<Part>();
                localPartsData.Parts.AddRange(newData.Parts);

                //As a data to be saved, save the entire current sync state of the Part data.
                getPartsUpdatesRequest.RequestResponseObject.Data = localPartsData;

                AppearitionLogger.LogInfo("Parts data was successfully updated!");
            }
            else
                AppearitionLogger.LogError("An error happened when trying to fetch the parts data update.");

            //Finally, callback
            if (onSuccess != null && getPartsUpdatesRequest.RequestResponseObject.IsSuccess)
                onSuccess(localPartsData);

            if (onFailure != null && getPartsUpdatesRequest.RequestResponseObject.Errors != null && getPartsUpdatesRequest.RequestResponseObject.Errors.Length > 0)
                onFailure(getPartsUpdatesRequest.Errors);

            if (onComplete != null)
                onComplete(getPartsUpdatesRequest.RequestResponseObject.IsSuccess);
        }
    }
}