// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: FormHandler.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using Appearition.Common;
using Appearition.Form.API;
using Appearition.Internal;
using Appearition.Site;
using Appearition.Site.API;

namespace Appearition.Form
{
    /// <summary>
    /// Handler in charge of providing functionality related to the Form module.
    /// </summary>
    public sealed class FormHandler : BaseHandler
    {
        /// <summary>
        /// Fetches all the form content from the EMS.
        /// If you have existing content, pass in a FormSyncManifest and the difference will be returned instead.
        /// In case of no connection, will simply load the existing data.
        /// 
        ///  API Requirement: Session Token. Offline Capability.
        /// </summary>
        /// <param name="onSuccess">Contains the difference between the provided data and the data on the EMS. Only called if the request is successful.</param>
        ///  <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        ///  <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void FetchAllForms(Action<FormSyncManifest> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            FetchAllForms(null, onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Fetches all the form content from the EMS.
        /// If you have existing content, pass in a FormSyncManifest and the difference will be returned instead.
        /// In case of no connection, will simply load the existing data.
        /// 
        ///  API Requirement: Session Token. Offline Capability.
        /// </summary>
        /// <param name="onSuccess">Contains the difference between the provided data and the data on the EMS. Only called if the request is successful.</param>
        ///  <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        ///  <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator FetchAllFormsProcess(Action<FormSyncManifest> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            yield return FetchAllFormsProcess(null, onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Updates the given form content. Only the difference in content will be downloaded for a lightweight transfer.
        /// Consider storing that content locally for making full use of this system.
        /// In case of no connection, will simply load the existing data.
        /// 
        ///  API Requirement: Session Token. Offline Capability.
        /// </summary>
        /// <param name="localFormData"></param>
        /// <param name="onSuccess">Contains the difference between the provided data and the data on the EMS. Only called if the request is successful.</param>
        ///  <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        ///  <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void FetchAllForms(FormSyncManifest localFormData, Action<FormSyncManifest> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(FetchAllFormsProcess(localFormData, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Updates the given form content. Only the difference in content will be downloaded for a lightweight transfer.
        /// Consider storing that content locally for making full use of this system.
        /// In case of no connection, will simply load the existing data.
        /// 
        ///  API Requirement: Session Token. Offline Capability.
        /// </summary>
        /// <param name="localFormData"></param>
        /// <param name="onSuccess">Contains the difference between the provided data and the data on the EMS. Only called if the request is successful.</param>
        ///  <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        ///  <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator FetchAllFormsProcess(FormSyncManifest localFormData, Action<FormSyncManifest> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete =
            null)
        {
            //Create data if the developer passed in null data.
            if (localFormData == null)
                localFormData = new FormSyncManifest();

            //Online request
            Form_GetFormSyncManifest.PostApi postRequestData = new Form_GetFormSyncManifest.PostApi(localFormData);

            var getFormsUpdatesRequest = AppearitionRequest<Form_GetFormSyncManifest>.LaunchAPICall_POST<FormHandler>
                (AppearitionGate.Instance.CurrentUser.selectedChannel, GetReusableApiRequest<Form_GetFormSyncManifest>(), postRequestData);

            while (!getFormsUpdatesRequest.IsDone)
                yield return null;

            //Debug result
            if (getFormsUpdatesRequest.RequestResponseObject.IsSuccess)
            {
                //Apply the difference on the manifest.
                FormSyncManifest newData = getFormsUpdatesRequest.RequestResponseObject.Data;

                //If there was no data, simply add the new content.
                if (localFormData.FormGroups == null)
                    localFormData.FormGroups = new List<FormGroup>();
                localFormData.FormGroups.AddRange(newData.FormGroups);

                //As a data to be saved, save the entire current sync state of the Site data.
                getFormsUpdatesRequest.RequestResponseObject.Data = localFormData;

                if (getFormsUpdatesRequest.CurrentState == AppearitionBaseRequest<Form_GetFormSyncManifest>.RequestState.SuccessOnline)
                    AppearitionLogger.LogInfo("Form data was successfully updated!");
                else
                    AppearitionLogger.LogInfo("Form data successfully loaded offline!");
            }
            else
                AppearitionLogger.LogError("An error happened when trying to fetch the form data update.");

            //Finally, callback
            if (onSuccess != null && getFormsUpdatesRequest.RequestResponseObject.IsSuccess)
                onSuccess(localFormData);

            if (onFailure != null && getFormsUpdatesRequest.RequestResponseObject.Errors != null && getFormsUpdatesRequest.RequestResponseObject.Errors.Length > 0)
                onFailure(getFormsUpdatesRequest.Errors);

            if (onComplete != null)
                onComplete(getFormsUpdatesRequest.RequestResponseObject.IsSuccess);
        }
    }
}