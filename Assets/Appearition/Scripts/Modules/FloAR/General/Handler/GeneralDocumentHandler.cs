// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: GeneralDocumentHandler.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using Appearition.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using Appearition.Common.ObjectExtensions;
using Appearition.GeneralDocument.API;

namespace Appearition.GeneralDocument
{
    /// <summary>
    /// Handler in charge of providing functionality related to the General Documents module.
    /// </summary>
    public class GeneralDocumentHandler : BaseFloARHandler
    {
        /// <summary>
        /// Fetches all the general doc info content from the EMS.
        /// If you have existing content, pass in a GeneralSyncManifest and the difference will be returned instead.
        /// In case of no connection, will simply load the existing data.
        ///
        /// API Requirement: Session Token. Offline Capability.
        /// </summary>
        /// <param name="onSuccess">Contains all the general document data on the EMS. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void FetchAllGeneralDocumentInfo(Action<GeneralSyncManifest> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            FetchAllGeneralDocumentInfo(null, false, onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Fetches all the general doc info content from the EMS.
        /// If you have existing content, pass in a GeneralSyncManifest and the difference will be returned instead.
        /// In case of no connection, will simply load the existing data.
        ///
        /// API Requirement: Session Token. Offline Capability.
        /// </summary>
        /// <param name="onSuccess">Contains all the general document data on the EMS. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator FetchAllGeneralDocumentInfoProcess(Action<GeneralSyncManifest> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            yield return FetchAllGeneralDocumentInfoProcess(null, false, onSuccess, onFailure, onComplete);
        }

        ///  <summary>
        ///  Fetches all the general doc info content from the EMS.
        ///  If you have existing content, pass in a GeneralSyncManifest and the difference will be returned instead.
        /// In case of no connection, will simply load the existing data.
        /// 
        ///  API Requirement: Session Token. Offline Capability.
        ///  </summary>
        /// <param name="downloadGeneralDocuments">Whether or not the general documents files should also be downloaded.</param>
        /// <param name="onSuccess">Contains all the general document data on the EMS. Only called if the request is successful.</param>
        ///  <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        ///  <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void FetchAllGeneralDocumentInfo(bool downloadGeneralDocuments, Action<GeneralSyncManifest> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            FetchAllGeneralDocumentInfo(null, downloadGeneralDocuments, onSuccess, onFailure, onComplete);
        }

        ///  <summary>
        ///  Fetches all the general doc info content from the EMS.
        ///  If you have existing content, pass in a GeneralSyncManifest and the difference will be returned instead.
        /// In case of no connection, will simply load the existing data.
        /// 
        ///  API Requirement: Session Token. Offline Capability.
        ///  </summary>
        /// <param name="downloadGeneralDocuments">Whether or not the general documents files should also be downloaded.</param>
        /// <param name="onSuccess">Contains all the general document data on the EMS. Only called if the request is successful.</param>
        ///  <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        ///  <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator FetchAllGeneralDocumentInfoProcess(bool downloadGeneralDocuments, Action<GeneralSyncManifest> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            yield return FetchAllGeneralDocumentInfoProcess(null, downloadGeneralDocuments, onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Updates the given general doc info content. Only the difference in content will be downloaded for a lightweight transfer.
        /// Consider storing that content locally for making full use of this system.
        /// In case of no connection, will simply load the existing data.
        ///
        /// API Requirement: Session Token. Offline Capability.
        /// </summary>
        /// <param name="localDocumentInfoData"></param>
        /// <param name="onSuccess">Contains the difference between the provided data and the data on the EMS. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void FetchAllGeneralDocumentInfo(GeneralSyncManifest localDocumentInfoData, Action<GeneralSyncManifest> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            FetchAllGeneralDocumentInfo(localDocumentInfoData, false, onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Updates the given general doc info content. Only the difference in content will be downloaded for a lightweight transfer.
        /// Consider storing that content locally for making full use of this system.
        /// In case of no connection, will simply load the existing data.
        ///
        /// API Requirement: Session Token. Offline Capability.
        /// </summary>
        /// <param name="localDocumentInfoData"></param>
        /// <param name="onSuccess">Contains the difference between the provided data and the data on the EMS. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator FetchAllGeneralDocumentInfoProcess(GeneralSyncManifest localDocumentInfoData, Action<GeneralSyncManifest> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            yield return FetchAllGeneralDocumentInfoProcess(localDocumentInfoData, false, onSuccess, onFailure, onComplete);
        }

        ///  <summary>
        ///  Updates the given general doc info content. Only the difference in content will be downloaded for a lightweight transfer.
        ///  Consider storing that content locally for making full use of this system.
        /// In case of no connection, will simply load the existing data.
        /// 
        ///  API Requirement: Session Token. Offline Capability.
        ///  </summary>
        ///  <param name="localDocumentInfoData"></param>
        /// <param name="downloadGeneralDocuments">Whether or not the general documents files should also be downloaded.</param>
        /// <param name="onSuccess">Contains the difference between the provided data and the data on the EMS. Only called if the request is successful.</param>
        ///  <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        ///  <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void FetchAllGeneralDocumentInfo(GeneralSyncManifest localDocumentInfoData, bool downloadGeneralDocuments, Action<GeneralSyncManifest> onSuccess = null,
            Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(FetchAllGeneralDocumentInfoProcess(localDocumentInfoData, downloadGeneralDocuments, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Updates the given general doc info content. Only the difference in content will be downloaded for a lightweight transfer.
        /// Consider storing that content locally for making full use of this system.
        /// In case of no connection, will simply load the existing data.
        /// 
        /// API Requirement: Session Token. Offline Capability.
        /// </summary>
        /// <param name="localDocumentInfoData"></param>
        /// <param name="downloadGeneralDocuments">Whether or not the general documents files should also be downloaded.</param>
        /// <param name="onSuccess">Contains the difference between the provided data and the data on the EMS. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator FetchAllGeneralDocumentInfoProcess(GeneralSyncManifest localDocumentInfoData, bool downloadGeneralDocuments, Action<GeneralSyncManifest> onSuccess = null,
            Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            //Create data if the developer passed in null data.
            if (localDocumentInfoData == null)
                localDocumentInfoData = new GeneralSyncManifest();

            //Online request
            General_GetGeneralDocSyncManifest.PostApi postRequestData = new General_GetGeneralDocSyncManifest.PostApi(localDocumentInfoData);

            var getGeneralDocUpdatesRequest = AppearitionRequest<General_GetGeneralDocSyncManifest>.LaunchAPICall_POST<GeneralDocumentHandler>
                (AppearitionGate.Instance.CurrentUser.selectedChannel, GetReusableApiRequest<General_GetGeneralDocSyncManifest>(), postRequestData);

            while (!getGeneralDocUpdatesRequest.IsDone)
                yield return null;

            //Debug result
            if (getGeneralDocUpdatesRequest.RequestResponseObject.IsSuccess)
            {
                //Apply the difference on the manifest.
                GeneralSyncManifest newData = getGeneralDocUpdatesRequest.RequestResponseObject.Data;

                //If there was no data, simply add the new content.
                if (localDocumentInfoData.GeneralDocs == null)
                    localDocumentInfoData.GeneralDocs = new List<General>();

                localDocumentInfoData.GeneralDocs.AddRange(newData.GeneralDocs);

                //Handle the general document download if required
                if (downloadGeneralDocuments)
                {
                    AppearitionLogger.LogInfo("Downloading the new general document files..");

                    for (int i = 0; i < newData.GeneralDocs.Count; i++)
                    {
                        //Handle whether to create or delete the file
                        if (newData.GeneralDocs[i].IsActive)
                        {
                            var i1 = i;
                            yield return FetchGeneralDocumentFileProcess(newData.GeneralDocs[i1], false,
                                updatedDocument => newData.GeneralDocs[i1] = updatedDocument);
                        }
                    }
                }

                //Either way, delete the files which require to be deleted.
                for (int i = 0; i < newData.GeneralDocs.Count; i++)
                {
                    if (!newData.GeneralDocs[i].IsActive)
                    {
                        //Delete the content
                        yield return DeleteDocumentAndDocumentJsonData<General, GeneralDocumentHandler>(newData.GeneralDocs[i]);
                    }
                }

                //As a data to be saved, save the entire current sync state of the General docs data.
                getGeneralDocUpdatesRequest.RequestResponseObject.Data = localDocumentInfoData;

                AppearitionLogger.LogInfo("General document data was successfully updated!");
            }
            else
                AppearitionLogger.LogError("An error happened when trying to fetch the general document data update.");

            //Finally, callback
            if (onSuccess != null && getGeneralDocUpdatesRequest.RequestResponseObject.IsSuccess)
                onSuccess(localDocumentInfoData);

            if (onFailure != null && getGeneralDocUpdatesRequest.RequestResponseObject.Errors != null && getGeneralDocUpdatesRequest.RequestResponseObject.Errors.Length > 0)
                onFailure(getGeneralDocUpdatesRequest.Errors);

            if (onComplete != null)
                onComplete(getGeneralDocUpdatesRequest.RequestResponseObject.IsSuccess);
        }

        /// <summary>
        /// Given document data, downloads the file if required, and loads the document into memory if required.
        ///
        /// API Requirement: Session Token. Offline Capability.
        /// </summary>
        /// <param name="document">The original General Doc, which might not contain any runtime data.</param>
        /// <param name="loadInMemory">Whether or not the file content is to be loaded in memory.</param>
        /// <param name="onSuccess">Contains the updated General Doc. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void FetchGeneralDocumentFile(General document, bool loadInMemory, Action<General> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(FetchGeneralDocumentFileProcess(document, loadInMemory, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Given document data, downloads the file if required, and loads the document into memory if required.
        ///
        /// API Requirement: Session Token. Offline Capability.
        /// </summary>
        /// <param name="document">The original General Doc, which might not contain any runtime data.</param>
        /// <param name="loadInMemory">Whether or not the file content is to be loaded in memory.</param>
        /// <param name="onSuccess">Contains the updated General Doc. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator FetchGeneralDocumentFileProcess
            (General document, bool loadInMemory, Action<General> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            //Firstly, check if the local version if the same. If so, don't go through the request process.
            bool foundLocalVersion = DoesDocumentExistsAndMatchesLocalVersion<General, GeneralDocumentHandler>(document);

            if (foundLocalVersion)
            {
                //Load onto memory if necessary. If failed, try to go through the request process to download the document.
                if (loadInMemory)
                {
                    bool successfullyLoadedOntoMemory = true;

                    yield return LoadLocalDocumentOntoMemoryInDocumentContent<General, GeneralDocumentHandler>(document, updatedDoc => successfullyLoadedOntoMemory = updatedDoc != null);

                    if (successfullyLoadedOntoMemory)
                    {
                        AppearitionLogger.LogInfo(string.Format("Document named {0} was successfully loaded onto memory.", document.DocumentName));
                        if (onSuccess != null)
                            onSuccess(document);
                        if (onComplete != null)
                            onComplete(true);
                    }
                    else
                        AppearitionLogger.LogError(string.Format("Failed to load the document named {0} onto memory. Tries to download it again..", document.DocumentName));
                }
                else
                {
                    AppearitionLogger.LogInfo(string.Format("Document named {0} was already downloaded with a matching version.", document.DocumentName));

                    if (onSuccess != null)
                        onSuccess(document);
                    if (onComplete != null)
                        onComplete(true);
                    yield break;
                }
            }
            
            //Online request
            var generalDocumentRequestContent = new General_GetDocument.GetDocumentRequestContent
                {DocumentId = document.DocumentId};

            //Prepare the request
            var getGeneralDocumentRequest = AppearitionRequest<General_GetDocument>.LaunchAPICall_GET< GeneralDocumentHandler>
                (AppearitionGate.Instance.CurrentUser.selectedChannel, GetReusableApiRequest<General_GetDocument>(), generalDocumentRequestContent);

            while (!getGeneralDocumentRequest.IsDone)
                yield return null;

            //Save file locally
            if (getGeneralDocumentRequest.RequestResponseObject.IsSuccess && getGeneralDocumentRequest.DownloadedBytes != null)
            {
                //Store the stream data 
                if (loadInMemory)
                    document.DocumentContent = getGeneralDocumentRequest.DownloadedBytes.ToStream();

                yield return SaveContentToFileProcess(getGeneralDocumentRequest.DownloadedBytes, GetDocumentFullPath<General, GeneralDocumentHandler>(document));

                //Save the document data
                SaveDocumentDataLocally<General, GeneralDocumentHandler>(document);
                AppearitionLogger.LogInfo("General document was successfully fetched!");
            }
            else
                AppearitionLogger.LogError(string.Format("An error happened when trying to fetch the general document of id {0}.", document.DocumentId));

            //Finally, callback
            if (onSuccess != null && getGeneralDocumentRequest.RequestResponseObject.IsSuccess)
                onSuccess(document);

            if (onFailure != null && getGeneralDocumentRequest.RequestResponseObject.Errors != null && getGeneralDocumentRequest.RequestResponseObject.Errors.Length > 0)
                onFailure(getGeneralDocumentRequest.Errors);

            if (onComplete != null)
                onComplete(getGeneralDocumentRequest.RequestResponseObject.IsSuccess);
        }
    }
}