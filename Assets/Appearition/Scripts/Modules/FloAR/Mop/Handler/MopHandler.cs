// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: MopHandler.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using Appearition.Common;
using Appearition.Mop.API;
using Appearition.Common.ObjectExtensions;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Appearition.Mop
{
    /// <summary>
    /// Handler in charge of providing functionality related to the Mop (Method of Operation) module.
    /// </summary>
    public class MopHandler : BaseFloARHandler
    {
        /// <summary>
        /// Fetches all the mop document content from the EMS.
        /// If you have existing content, pass in a MopSyncManifest and the difference will be returned instead.
        /// In case of no connection, will simply load the existing data.
        ///
        /// API Requirement: Session Token. Offline Capability.
        /// </summary>
        /// <param name="onSuccess">Contains all the Mop data on the EMS. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void FetchAllMopDocumentInfo(Action<MopSyncManifest> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            FetchAllMopDocumentInfo(null, false, onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Fetches all the mop document content from the EMS.
        /// If you have existing content, pass in a MopSyncManifest and the difference will be returned instead.
        /// In case of no connection, will simply load the existing data.
        ///
        /// API Requirement: Session Token. Offline Capability.
        /// </summary>
        /// <param name="onSuccess">Contains all the Mop data on the EMS. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator FetchAllMopDocumentInfoProcess(Action<MopSyncManifest> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            yield return FetchAllMopDocumentInfoProcess(null, false, onSuccess, onFailure, onComplete);
        }

        ///  <summary>
        ///  Fetches all the mop document content from the EMS.
        ///  If you have existing content, pass in a MopSyncManifest and the difference will be returned instead.
        /// In case of no connection, will simply load the existing data.
        /// 
        ///  API Requirement: Session Token. Offline Capability.
        ///  </summary>
        /// <param name="downloadMopDocuments">Whether or not the Mop documents files should also be downloaded.</param>
        /// <param name="onSuccess">Contains all the Mop data on the EMS. Only called if the request is successful.</param>
        ///  <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        ///  <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void FetchAllMopDocumentInfo(bool downloadMopDocuments, Action<MopSyncManifest> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            FetchAllMopDocumentInfo(null, downloadMopDocuments, onSuccess, onFailure, onComplete);
        }

        ///  <summary>
        ///  Fetches all the mop document content from the EMS.
        ///  If you have existing content, pass in a MopSyncManifest and the difference will be returned instead.
        /// In case of no connection, will simply load the existing data.
        /// 
        ///  API Requirement: Session Token. Offline Capability.
        ///  </summary>
        /// <param name="downloadMopDocuments">Whether or not the Mop documents files should also be downloaded.</param>
        /// <param name="onSuccess">Contains all the Mop data on the EMS. Only called if the request is successful.</param>
        ///  <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        ///  <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator FetchAllMopDocumentInfoProcess(bool downloadMopDocuments, Action<MopSyncManifest> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            yield return FetchAllMopDocumentInfoProcess(null, downloadMopDocuments, onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Updates the given mop document content. Only the difference in content will be downloaded for a lightweight transfer.
        /// Consider storing that content locally for making full use of this system.
        /// In case of no connection, will simply load the existing data.
        ///
        /// API Requirement: Session Token. Offline Capability.
        /// </summary>
        /// <param name="localMopData"></param>
        /// <param name="onSuccess">Contains the difference between the provided data and the data on the EMS. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void FetchAllMopDocumentInfo(MopSyncManifest localMopData, Action<MopSyncManifest> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            FetchAllMopDocumentInfo(localMopData, false, onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Updates the given mop document content. Only the difference in content will be downloaded for a lightweight transfer.
        /// Consider storing that content locally for making full use of this system.
        /// In case of no connection, will simply load the existing data.
        ///
        /// API Requirement: Session Token. Offline Capability.
        /// </summary>
        /// <param name="localMopsData"></param>
        /// <param name="onSuccess">Contains the difference between the provided data and the data on the EMS. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator FetchAllMopDocumentInfoProcess(MopSyncManifest localMopsData, Action<MopSyncManifest> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            yield return FetchAllMopDocumentInfoProcess(localMopsData, false, onSuccess, onFailure, onComplete);
        }

        ///  <summary>
        ///  Updates the given mop document content. Only the difference in content will be downloaded for a lightweight transfer.
        ///  Consider storing that content locally for making full use of this system.
        /// In case of no connection, will simply load the existing data.
        /// 
        ///  API Requirement: Session Token. Offline Capability.
        ///  </summary>
        ///  <param name="localMopsData"></param>
        /// <param name="downloadMopDocuments">Whether or not the Mop documents files should also be downloaded.</param>
        /// <param name="onSuccess">Contains the difference between the provided data and the data on the EMS. Only called if the request is successful.</param>
        ///  <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        ///  <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void FetchAllMopDocumentInfo(MopSyncManifest localMopsData, bool downloadMopDocuments, Action<MopSyncManifest> onSuccess = null,
            Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(FetchAllMopDocumentInfoProcess(localMopsData, downloadMopDocuments, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Updates the given mop document content. Only the difference in content will be downloaded for a lightweight transfer.
        /// Consider storing that content locally for making full use of this system.
        /// In case of no connection, will simply load the existing data.
        /// 
        /// API Requirement: Session Token. Offline Capability.
        /// </summary>
        /// <param name="localMopsData"></param>
        /// <param name="downloadMopDocuments">Whether or not the Mop documents files should also be downloaded.</param>
        /// <param name="onSuccess">Contains the difference between the provided data and the data on the EMS. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator FetchAllMopDocumentInfoProcess(MopSyncManifest localMopsData, bool downloadMopDocuments, Action<MopSyncManifest> onSuccess = null,
            Action<EmsError> onFailure = null,
            Action<bool> onComplete =
                null)
        {
            //Create data if the developer passed in null data.
            if (localMopsData == null)
                localMopsData = new MopSyncManifest();

            //Online request
            Mop_GetMopSyncManifest.PostApi postRequestData = new Mop_GetMopSyncManifest.PostApi(localMopsData);

            var getMopDocumentInfoUpdatesRequest = AppearitionRequest<Mop_GetMopSyncManifest>.LaunchAPICall_POST<MopHandler>
                (AppearitionGate.Instance.CurrentUser.selectedChannel, GetReusableApiRequest<Mop_GetMopSyncManifest>(), postRequestData);

            while (!getMopDocumentInfoUpdatesRequest.IsDone)
                yield return null;

            //Debug result
            if (getMopDocumentInfoUpdatesRequest.RequestResponseObject.IsSuccess)
            {
                //Apply the difference on the manifest.
                MopSyncManifest newData = getMopDocumentInfoUpdatesRequest.RequestResponseObject.Data;

                //If there was no data, simply add the new content.

                if (localMopsData.Mops == null)
                    localMopsData.Mops = new List<Mop>();
                localMopsData.Mops.AddRange(newData.Mops);

                //Handle the mop document download if required
                if (downloadMopDocuments)
                {
                    AppearitionLogger.LogInfo("Downloading the new Mop documents..");

                    for (int i = 0; i < newData.Mops.Count; i++)
                    {
                        //Handle whether to create or delete the file
                        if (newData.Mops[i].IsActive)
                        {
                            var i1 = i;
                            yield return FetchMopDocumentFileProcess(newData.Mops[i1], false,
                                updatedDocument => newData.Mops[i1] = updatedDocument);
                        }
                    }
                }

                //Either way, delete the files which require to be deleted.
                for (int i = 0; i < newData.Mops.Count; i++)
                {
                    if (!newData.Mops[i].IsActive)
                    {
                        //Delete the content
                        yield return DeleteDocumentAndDocumentJsonData<Mop, MopHandler>(newData.Mops[i]);
                    }
                }

                //As a data to be saved, save the entire current sync state of the mop data.
                getMopDocumentInfoUpdatesRequest.RequestResponseObject.Data = localMopsData;

                AppearitionLogger.LogInfo("Mop data was successfully updated!");
            }
            else
                AppearitionLogger.LogError("An error happened when trying to fetch the mop data update.");

            //Finally, callback
            if (onSuccess != null && getMopDocumentInfoUpdatesRequest.RequestResponseObject.IsSuccess)
                onSuccess(localMopsData);

            if (onFailure != null && getMopDocumentInfoUpdatesRequest.RequestResponseObject.Errors != null && getMopDocumentInfoUpdatesRequest.RequestResponseObject.Errors.Length > 0)
                onFailure(getMopDocumentInfoUpdatesRequest.Errors);

            if (onComplete != null)
                onComplete(getMopDocumentInfoUpdatesRequest.RequestResponseObject.IsSuccess);
        }

        /// <summary>
        /// Given document data, downloads the file if required, and loads the document into memory if required.
        ///
        /// API Requirement: Session Token. Offline Capability.
        /// </summary>
        /// <param name="document">The original MopDocument, which might not contain any runtime data.</param>
        /// <param name="loadInMemory">Whether or not the file content is to be loaded in memory.</param>
        /// <param name="onSuccess">Contains the updated MopDocument. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void FetchMopDocumentFile(Mop document, bool loadInMemory, Action<Mop> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(FetchMopDocumentFileProcess(document, loadInMemory, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Given document data, downloads the file if required, and loads the document into memory if required.
        ///
        /// API Requirement: Session Token. Offline Capability.
        /// </summary>
        /// <param name="document">The original MopDocument, which might not contain any runtime data.</param>
        /// <param name="loadInMemory">Whether or not the file content is to be loaded in memory.</param>
        /// <param name="onSuccess">Contains the updated MopDocument. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator FetchMopDocumentFileProcess
            (Mop document, bool loadInMemory, Action<Mop> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            //Firstly, check if the local version if the same. If so, don't go through the request process.
            bool foundLocalVersion = DoesDocumentExistsAndMatchesLocalVersion<Mop, MopHandler>(document);

            if (foundLocalVersion)
            {
                //Load onto memory if necessary. If failed, try to go through the request process to download the document.
                if (loadInMemory)
                {
                    bool successfullyLoadedOntoMemory = true;

                    yield return LoadLocalDocumentOntoMemoryInDocumentContent<Mop, MopHandler>(document, updatedDoc => successfullyLoadedOntoMemory = updatedDoc != null);
                    
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
            var mopDocumentRequestContent = new Mop_GetDocument.GetDocumentRequestContent
                {DocumentId = document.DocumentId};

            //Prepare the request
            var getMopDocumentRequest = AppearitionRequest<Mop_GetDocument>.LaunchAPICall_GET<MopHandler>
                (AppearitionGate.Instance.CurrentUser.selectedChannel, GetReusableApiRequest<Mop_GetDocument>(), mopDocumentRequestContent);

            while (!getMopDocumentRequest.IsDone)
                yield return null;

            //Save file locally
            if (getMopDocumentRequest.RequestResponseObject.IsSuccess && getMopDocumentRequest.DownloadedBytes != null)
            {
                //Store the stream data 
                if (loadInMemory)
                    document.DocumentContent = getMopDocumentRequest.DownloadedBytes.ToStream();

                yield return SaveContentToFileProcess(getMopDocumentRequest.DownloadedBytes, GetDocumentFullPath<Mop, MopHandler>(document));

                //Save the document data
                SaveDocumentDataLocally<Mop, MopHandler>(document);
                AppearitionLogger.LogInfo("Mop document was successfully fetched!");
            }
            else
                AppearitionLogger.LogError(string.Format("An error happened when trying to fetch the Mop document of id {0}.", document.DocumentId));

            //Finally, callback
            if (onSuccess != null && getMopDocumentRequest.RequestResponseObject.IsSuccess)
                onSuccess(document);

            if (onFailure != null && getMopDocumentRequest.RequestResponseObject.Errors != null && getMopDocumentRequest.RequestResponseObject.Errors.Length > 0)
                onFailure(getMopDocumentRequest.Errors);

            if (onComplete != null)
                onComplete(getMopDocumentRequest.RequestResponseObject.IsSuccess);
        }
    }
}