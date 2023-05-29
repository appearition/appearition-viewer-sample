// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: JobHandler.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using Appearition.AccountAndAuthentication;
using Appearition.Common;
using Appearition.Internal;
using Appearition.Job.API;

namespace Appearition.Job
{
    /// <summary>
    /// Handler in charge of providing functionality related to the Job module.
    /// </summary>
    public class JobHandler : BaseFloARHandler
    {
        #region Get Job Form

        /// <summary>
        /// Fetches all the jobs available for the logged in user.
        ///
        /// API Requirement: Session Token. Offline Capability.
        /// </summary>
        /// <param name="onSuccess">Contains all the jobs available for this logged in user. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void FetchAllJobs(Action<UserSiteJob> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(FetchAllJobsProcess(onSuccess, onFailure, onComplete));
        }

        public static IEnumerator FetchAllJobsProcess(Action<UserSiteJob> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            //Online request
            var getSiteJobsUpdatesRequest = AppearitionRequest<Job_GetSiteJobs>.LaunchAPICall_GET<JobHandler>
                (AppearitionGate.Instance.CurrentUser.selectedChannel, GetReusableApiRequest<Job_GetSiteJobs>());

            while (!getSiteJobsUpdatesRequest.IsDone)
                yield return null;

            //Debug result
            if (getSiteJobsUpdatesRequest.RequestResponseObject.IsSuccess)
            {
                //As a data to be saved, save the entire current sync state of the SiteJobs data. - UPDATE, now fully handled by the Request.
                //SaveJsonData<Job_GetSiteJobs, JobHandler>(getSiteJobsUpdatesRequest);
                if (getSiteJobsUpdatesRequest.CurrentState == AppearitionBaseRequest<Job_GetSiteJobs>.RequestState.SuccessOnline)
                    AppearitionLogger.LogInfo("Site Jobs data was successfully updated!");
                else
                    AppearitionLogger.LogInfo("Site Jobs data successfully loaded offline!");
            }
            else
                AppearitionLogger.LogError("An error happened when trying to fetch the site jobs data update.");

            //Finally, callback
            if (onSuccess != null && getSiteJobsUpdatesRequest.RequestResponseObject.IsSuccess)
                onSuccess(getSiteJobsUpdatesRequest.RequestResponseObject.Data);

            if (onFailure != null && getSiteJobsUpdatesRequest.RequestResponseObject.Errors != null && getSiteJobsUpdatesRequest.RequestResponseObject.Errors.Length > 0)
                onFailure(getSiteJobsUpdatesRequest.Errors);

            if (onComplete != null)
                onComplete(getSiteJobsUpdatesRequest.RequestResponseObject.IsSuccess);
        }

        /// <summary>
        /// Fetches the details of a previously posted job. Do note that only the user that posted it can access the job.
        ///
        /// API Requirement: Session Token.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="job"></param>
        /// <param name="onSuccess">Contains the job previously posted. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void FetchExistingJobFormData(UserProfile user, Job job, Action<JobPost> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            FetchExistingJobFormData(user.UserProfileId, job.JobId, job.SiteId, onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Fetches the details of a previously posted job. Do note that only the user that posted it can access the job.
        ///
        /// API Requirement: Session Token.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="job"></param>
        /// <param name="onSuccess">Contains the job previously posted. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator FetchExistingJobFormDataProcess(UserProfile user, Job job, Action<JobPost> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            yield return FetchExistingJobFormDataProcess(user.UserProfileId, job.JobId, job.SiteId, onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Fetches the details of a previously posted job. Do note that only the user that posted it can access the job.
        ///
        /// API Requirement: Session Token.
        /// </summary>
        /// <param name="userProfileId"></param>
        /// <param name="jobId"></param>
        /// <param name="siteId"></param>
        /// <param name="onSuccess">Contains the job previously posted. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void FetchExistingJobFormData(long userProfileId, long jobId, long siteId, Action<JobPost> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(FetchExistingJobFormDataProcess(userProfileId, jobId, siteId, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Fetches the details of a previously posted job. Do note that only the user that posted it can access the job.
        ///
        /// API Requirement: Session Token.
        /// </summary>
        /// <param name="userProfileId"></param>
        /// <param name="jobId"></param>
        /// <param name="siteId"></param>
        /// <param name="onSuccess">Contains the job previously posted. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator FetchExistingJobFormDataProcess(long userProfileId, long jobId, long siteId, Action<JobPost> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            //Online request
            Job_GetJobFormData.PostApi postRequestData = new Job_GetJobFormData.PostApi {
                UserProfileId = userProfileId,
                JobId = jobId,
                SiteId = siteId,
                ProjectId = FloARConstants.FLOAR_INSTANCE_CHANNEL_ID
            };

            //Launch request
            var getPastJobFormRequest = AppearitionRequest<Job_GetJobFormData>.LaunchAPICall_POST<JobHandler>
                (AppearitionGate.Instance.CurrentUser.selectedChannel, GetReusableApiRequest<Job_GetJobFormData>(), postRequestData);

            while (!getPastJobFormRequest.IsDone)
                yield return null;

            if (getPastJobFormRequest.RequestResponseObject.IsSuccess)
            {
                if (getPastJobFormRequest.CurrentState == AppearitionBaseRequest<Job_GetJobFormData>.RequestState.SuccessOnline)
                    AppearitionLogger.LogInfo("Previous job form data was successfully fetched!");
                else
                    AppearitionLogger.LogInfo("Previous job form successfully loaded offline!");
            }
            else
                AppearitionLogger.LogError("An error happened when trying to fetch a previous job form data. " + getPastJobFormRequest.Errors);

            //Finally, callback
            if (onSuccess != null && getPastJobFormRequest.RequestResponseObject.IsSuccess)
                onSuccess(getPastJobFormRequest.RequestResponseObject.Data);

            if (onFailure != null && getPastJobFormRequest.RequestResponseObject.Errors != null && getPastJobFormRequest.RequestResponseObject.Errors.Length > 0)
                onFailure(getPastJobFormRequest.Errors);

            if (onComplete != null)
                onComplete(getPastJobFormRequest.RequestResponseObject.IsSuccess);
        }

        #endregion

        #region Submit Job Forms

        /// <summary>
        /// Submits a job form completely. If any files are to be uploaded, it will also upload them while submitting.
        ///
        /// API Requirement: Session Token.
        /// </summary>
        /// <param name="mainJobPost"></param>
        /// <param name="onSuccess">Contains the updated state of the job that was just submitted. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void SubmitJobForm(JobPost mainJobPost, Action<Job> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            SubmitJobForm(mainJobPost, null, onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Submits a job form completely. If any files are to be uploaded, it will also upload them while submitting.
        ///
        /// API Requirement: Session Token.
        /// </summary>
        /// <param name="mainJobPost"></param>
        /// <param name="onSuccess">Contains the updated state of the job that was just submitted. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator SubmitJobFormProcess(JobPost mainJobPost, Action<Job> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            yield return SubmitJobFormProcess(mainJobPost, null, onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Submits a job form completely. If any files are to be uploaded, it will also upload them while submitting.
        ///
        /// API Requirement: Session Token.
        /// </summary>
        /// <param name="mainJobPost"></param>
        /// <param name="filesToUpload"></param>
        /// <param name="onSuccess">Contains the updated state of the job that was just submitted. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void SubmitJobForm(JobPost mainJobPost, List<JobFormFilePost> filesToUpload, Action<Job> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(SubmitJobFormProcess(mainJobPost, filesToUpload, onSuccess, onFailure, onComplete));
        }


        /// <summary>
        /// Submits a job form completely. If any files are to be uploaded, it will also upload them while submitting.
        ///
        /// API Requirement: Session Token.
        /// </summary>
        /// <param name="mainJobPost"></param>
        /// <param name="filesToUpload"></param>
        /// <param name="onSuccess">Contains the updated state of the job that was just submitted. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator SubmitJobFormProcess(JobPost mainJobPost, List<JobFormFilePost> filesToUpload, Action<Job> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            //Online request. First, send the FormData.
            Job_SubmitJobFormData.PostApi postApi = new Job_SubmitJobFormData.PostApi(mainJobPost);

            var getPostFormData = AppearitionRequest<Job_SubmitJobFormData>.LaunchAPICall_POST<JobHandler>
                (AppearitionGate.Instance.CurrentUser.selectedChannel, GetReusableApiRequest<Job_SubmitJobFormData>(), postApi);

            while (!getPostFormData.IsDone)
                yield return null;

            if (getPostFormData.RequestResponseObject.IsSuccess)
            {
                AppearitionLogger.LogInfo("Submitted job form data was successfully!");

                if (filesToUpload != null && filesToUpload.Count > 0)
                {
                    //If successfully, upload all the files too.
                    for (int i = 0; i < filesToUpload.Count; i++)
                    {
                        int index = i;

                        var reusableRequest = GetReusableApiRequest<Job_SubmitJobFormFile>();
                        reusableRequest.ProjectId = FloARConstants.FLOAR_INSTANCE_CHANNEL_ID.ToString();
                        reusableRequest.JobId = getPostFormData.RequestResponseObject.Data.JobId.ToString();
                        reusableRequest.SiteId = mainJobPost.SiteId.ToString();
                        reusableRequest.FormKey = filesToUpload[index].FormKey.ToString();
                        reusableRequest.FieldKey = filesToUpload[index].FieldKey.ToString();

                        var submitJobFileRequest = AppearitionRequest<Job_SubmitJobFormFile>.LaunchAPICall_MultiPartPOST<JobHandler>
                            (AppearitionGate.Instance.CurrentUser.selectedChannel, reusableRequest, null, new List<MultiPartFormParam> {filesToUpload[index].multiPartForm});

                        while (!submitJobFileRequest.IsDone)
                            yield return null;

                        //Debug outcomes
                        if (submitJobFileRequest.RequestResponseObject.IsSuccess)
                            AppearitionLogger.LogInfo(string.Format("File of name {0} was uploaded successfully as part of the job of id {1}", filesToUpload[index], mainJobPost.JobId));
                        else
                            AppearitionLogger.LogError(string.Format("An error occured when trying to upload the file of name {0} as part of the job of id {1}", filesToUpload[index],
                                mainJobPost.JobId));

                        //Empty fields for safety purpose
                        reusableRequest.ProjectId = reusableRequest.JobId = reusableRequest.SiteId = reusableRequest.FormKey = reusableRequest.FieldKey = null;
                    }
                }
            }
            else
                AppearitionLogger.LogError("An error happened when trying to submit the job form data." + getPostFormData.Errors);

            //Finally, callback
            if (onSuccess != null && getPostFormData.RequestResponseObject.IsSuccess)
                onSuccess(getPostFormData.RequestResponseObject.Data);

            if (onFailure != null && getPostFormData.RequestResponseObject.Errors != null && getPostFormData.RequestResponseObject.Errors.Length > 0)
                onFailure(getPostFormData.Errors);

            if (onComplete != null)
                onComplete(getPostFormData.RequestResponseObject.IsSuccess);
        }

        #endregion
    }
}