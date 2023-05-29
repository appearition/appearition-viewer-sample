// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: SiteHandler.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using Appearition.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using Appearition.Common.ObjectExtensions;
using Appearition.Internal;
using Appearition.Site.API;

namespace Appearition.Site
{
    /// <summary>
    /// Handler in charge of providing functionality related to the Site module.
    /// </summary>
    public class SiteHandler : BaseFloARHandler
    {
        /// <summary>
        /// Fetches all the sites content from the EMS.
        /// If you have existing content, pass in a SiteSyncManifest and the difference will be returned instead.
        /// In case of no connection, will simply load the existing data.
        ///
        /// API Requirement: Session Token. Offline Capability.
        /// </summary>
        /// <param name="onSuccess">Contains all the Sites data on the EMS. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void FetchAllSites(Action<SiteSyncManifest> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            FetchAllSites(null, false, onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Fetches all the sites content from the EMS.
        /// If you have existing content, pass in a SiteSyncManifest and the difference will be returned instead.
        /// In case of no connection, will simply load the existing data.
        ///
        /// API Requirement: Session Token. Offline Capability.
        /// </summary>
        /// <param name="onSuccess">Contains all the Sites data on the EMS. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator FetchAllSitesProcess(Action<SiteSyncManifest> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            yield return FetchAllSitesProcess(null, false, onSuccess, onFailure, onComplete);
        }

        ///  <summary>
        ///  Fetches all the sites content from the EMS.
        ///  If you have existing content, pass in a SiteSyncManifest and the difference will be returned instead.
        /// In case of no connection, will simply load the existing data.
        /// 
        ///  API Requirement: Session Token. Offline Capability.
        ///  </summary>
        /// <param name="downloadSitesDocuments">Whether or not the sites documents should also be downloaded.</param>
        /// <param name="onSuccess">Contains all the Sites data on the EMS. Only called if the request is successful.</param>
        ///  <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        ///  <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void FetchAllSites(bool downloadSitesDocuments, Action<SiteSyncManifest> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            FetchAllSites(null, downloadSitesDocuments, onSuccess, onFailure, onComplete);
        }

        ///  <summary>
        ///  Fetches all the sites content from the EMS.
        ///  If you have existing content, pass in a SiteSyncManifest and the difference will be returned instead.
        /// In case of no connection, will simply load the existing data.
        /// 
        ///  API Requirement: Session Token. Offline Capability.
        ///  </summary>
        /// <param name="downloadSitesDocuments">Whether or not the sites documents should also be downloaded.</param>
        /// <param name="onSuccess">Contains all the Sites data on the EMS. Only called if the request is successful.</param>
        ///  <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        ///  <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator FetchAllSitesProcess(bool downloadSitesDocuments, Action<SiteSyncManifest> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            yield return FetchAllSitesProcess(null, downloadSitesDocuments, onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Updates the given sites content. Only the difference in content will be downloaded for a lightweight transfer.
        /// Consider storing that content locally for making full use of this system.
        /// In case of no connection, will simply load the existing data.
        ///
        /// API Requirement: Session Token. Offline Capability.
        /// </summary>
        /// <param name="localSitesData"></param>
        /// <param name="onSuccess">Contains the difference between the provided data and the data on the EMS. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void FetchAllSites(SiteSyncManifest localSitesData, Action<SiteSyncManifest> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            FetchAllSites(localSitesData, false, onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Updates the given sites content. Only the difference in content will be downloaded for a lightweight transfer.
        /// Consider storing that content locally for making full use of this system.
        /// In case of no connection, will simply load the existing data.
        ///
        /// API Requirement: Session Token. Offline Capability.
        /// </summary>
        /// <param name="localSitesData"></param>
        /// <param name="onSuccess">Contains the difference between the provided data and the data on the EMS. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator FetchAllSitesProcess(SiteSyncManifest localSitesData, Action<SiteSyncManifest> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            yield return FetchAllSitesProcess(localSitesData, false, onSuccess, onFailure, onComplete);
        }

        ///  <summary>
        ///  Updates the given sites content. Only the difference in content will be downloaded for a lightweight transfer.
        ///  Consider storing that content locally for making full use of this system.
        /// In case of no connection, will simply load the existing data.
        /// 
        ///  API Requirement: Session Token. Offline Capability.
        ///  </summary>
        ///  <param name="localSitesData"></param>
        /// <param name="downloadSitesDocuments">Whether or not the sites documents should also be downloaded.</param>
        /// <param name="onSuccess">Contains the difference between the provided data and the data on the EMS. Only called if the request is successful.</param>
        ///  <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        ///  <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void FetchAllSites(SiteSyncManifest localSitesData, bool downloadSitesDocuments, Action<SiteSyncManifest> onSuccess = null,
            Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(FetchAllSitesProcess(localSitesData, downloadSitesDocuments, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Updates the given sites content. Only the difference in content will be downloaded for a lightweight transfer.
        /// Consider storing that content locally for making full use of this system.
        /// In case of no connection, will simply load the existing data.
        /// 
        /// API Requirement: Session Token. Offline Capability.
        /// </summary>
        /// <param name="localSitesData"></param>
        /// <param name="downloadSitesDocuments">Whether or not the sites documents should also be downloaded.</param>
        /// <param name="onSuccess">Contains the difference between the provided data and the data on the EMS. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator FetchAllSitesProcess(SiteSyncManifest localSitesData, bool downloadSitesDocuments, Action<SiteSyncManifest> onSuccess = null,
            Action<EmsError> onFailure = null,
            Action<bool> onComplete =
                null)
        {
            //Create data if the developer passed in null data.
            if (localSitesData == null)
                localSitesData = new SiteSyncManifest();

            //Online request
            Site_GetSiteSyncManifest.PostApi postRequestData = new Site_GetSiteSyncManifest.PostApi(localSitesData);

            var getSitesUpdatesRequest = AppearitionRequest<Site_GetSiteSyncManifest>.LaunchAPICall_POST<SiteHandler>
                (AppearitionGate.Instance.CurrentUser.selectedChannel, GetReusableApiRequest<Site_GetSiteSyncManifest>(), postRequestData);

            while (!getSitesUpdatesRequest.IsDone)
                yield return null;

            //Debug result
            if (getSitesUpdatesRequest.RequestResponseObject.IsSuccess)
            {
                //Apply the difference on the manifest.
                SiteSyncManifest newData = getSitesUpdatesRequest.RequestResponseObject.Data;

                //If there was no data, simply add the new content.

                if (localSitesData.Sites == null)
                    localSitesData.Sites = new List<Site>();
                localSitesData.Sites.AddRange(newData.Sites);

                //Handle the sites document download if required
                if (downloadSitesDocuments)
                {
                    AppearitionLogger.LogInfo("Downloading the new Sites document..");

                    for (int i = 0; i < newData.Sites.Count; i++)
                    {
                        for (int k = 0; k < newData.Sites[i].Docs.Count; k++)
                        {
                            //Handle whether to create or delete the file
                            if (newData.Sites[i].Docs[k].IsActive)
                            {
                                var i1 = i;
                                var k1 = k;
                                yield return FetchSiteDocumentFileProcess(newData.Sites[i].Docs[k], false,
                                    updatedDocument => newData.Sites[i1].Docs[k1] = updatedDocument);
                            }
                        }
                    }
                }

                //Either way, delete the files which require to be deleted.
                for (int i = 0; i < newData.Sites.Count; i++)
                {
                    for (int k = 0; k < newData.Sites[i].Docs.Count; k++)
                    {
                        if (!newData.Sites[i].Docs[k].IsActive)
                        {
                            //Delete the content
                            yield return DeleteDocumentAndDocumentJsonData<SiteDocument, SiteHandler>(newData.Sites[i].Docs[k]);
                        }
                    }
                }

                //As a data to be saved, save the entire current sync state of the Site data.
                getSitesUpdatesRequest.RequestResponseObject.Data = localSitesData;

                if (getSitesUpdatesRequest.CurrentState == AppearitionBaseRequest<Site_GetSiteSyncManifest>.RequestState.SuccessOnline)

                    AppearitionLogger.LogInfo("Sites data was successfully updated!");
                else
                    AppearitionLogger.LogInfo("Sites data successfully loaded offline!");
            }
            else
                AppearitionLogger.LogError("An error happened when trying to fetch the sites data update.");

            //Finally, callback
            if (onSuccess != null && getSitesUpdatesRequest.RequestResponseObject.IsSuccess)
                onSuccess(localSitesData);

            if (onFailure != null && getSitesUpdatesRequest.RequestResponseObject.Errors != null && getSitesUpdatesRequest.RequestResponseObject.Errors.Length > 0)
                onFailure(getSitesUpdatesRequest.Errors);

            if (onComplete != null)
                onComplete(getSitesUpdatesRequest.RequestResponseObject.IsSuccess);
        }

        /// <summary>
        /// Given document data, downloads the file if required, and loads the document into memory if required.
        ///
        /// API Requirement: Session Token. Offline Capability.
        /// </summary>
        /// <param name="document">The original SiteDocument, which might not contain any runtime data.</param>
        /// <param name="loadInMemory">Whether or not the file content is to be loaded in memory.</param>
        /// <param name="onSuccess">Contains the updated SiteDocument. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void FetchSiteDocumentFile(SiteDocument document, bool loadInMemory, Action<SiteDocument> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(FetchSiteDocumentFileProcess(document, loadInMemory, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Given document data, downloads the file if required, and loads the document into memory if required.
        ///
        /// API Requirement: Session Token. Offline Capability.
        /// </summary>
        /// <param name="document">The original SiteDocument, which might not contain any runtime data.</param>
        /// <param name="loadInMemory">Whether or not the file content is to be loaded in memory.</param>
        /// <param name="onSuccess">Contains the updated SiteDocument. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator FetchSiteDocumentFileProcess
            (SiteDocument document, bool loadInMemory, Action<SiteDocument> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            //Firstly, check if the local version if the same. If so, don't go through the request process.
            bool foundLocalVersion = DoesDocumentExistsAndMatchesLocalVersion<SiteDocument, SiteHandler>(document);

            if (foundLocalVersion)
            {
                //Load onto memory if necessary. If failed, try to go through the request process to download the document.
                if (loadInMemory)
                {
                    bool successfullyLoadedOntoMemory = true;

                    yield return LoadLocalDocumentOntoMemoryInDocumentContent<SiteDocument, SiteHandler>(document, updatedDoc => successfullyLoadedOntoMemory = updatedDoc != null);

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
            var siteDocumentRequestContent = new Site_GetDocument.GetDocumentRequestContent
                {DocumentId = document.DocumentId};

            //Prepare the request
            var getSiteDocumentRequest = AppearitionRequest<Site_GetDocument>.LaunchAPICall_GET<SiteHandler>
                (AppearitionGate.Instance.CurrentUser.selectedChannel, GetReusableApiRequest<Site_GetDocument>(), siteDocumentRequestContent);

            while (!getSiteDocumentRequest.IsDone)
                yield return null;

            //Save file locally
            if (getSiteDocumentRequest.RequestResponseObject.IsSuccess && getSiteDocumentRequest.DownloadedBytes != null)
            {
                //Store the stream data 
                if (loadInMemory)
                    document.DocumentContent = getSiteDocumentRequest.DownloadedBytes.ToStream();

                if (getSiteDocumentRequest.CurrentState == AppearitionBaseRequest<Site_GetDocument>.RequestState.SuccessOnline)
                {
                    yield return SaveContentToFileProcess(getSiteDocumentRequest.DownloadedBytes, GetDocumentFullPath<SiteDocument, SiteHandler>(document));

                    //Save the document data
                    SaveDocumentDataLocally<SiteDocument, SiteHandler>(document);
                    AppearitionLogger.LogInfo("Site document was successfully fetched!");
                }
                else
                    AppearitionLogger.LogInfo("Site document successfully loaded offline!");
            }
            else
                AppearitionLogger.LogError(string.Format("An error happened when trying to fetch the site document of id {0}.", document.DocumentId));

            //Finally, callback
            if (onSuccess != null && getSiteDocumentRequest.RequestResponseObject.IsSuccess)
                onSuccess(document);

            if (onFailure != null && getSiteDocumentRequest.RequestResponseObject.Errors != null && getSiteDocumentRequest.RequestResponseObject.Errors.Length > 0)
                onFailure(getSiteDocumentRequest.Errors);

            if (onComplete != null)
                onComplete(getSiteDocumentRequest.RequestResponseObject.IsSuccess);
        }
    }
}