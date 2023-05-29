using System.Collections;
using System.Collections.Generic;
using Appearition.Common;
using System;
using System.Linq;
using System.Reflection;
using Appearition.Assessments.API;
using Appearition.Common.ListExtensions;
using Appearition.Internal;
using Appearition.Learn;
using Appearition.QAndA;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Appearition.Assessments
{
    public class AssessmentHandler : BaseHandler
    {
        #region List

        /// <summary>
        /// Fetches all available assessments.
        ///
        /// Offline Capability.
        /// </summary>
        /// <param name="onSuccess">Contains the Assessment Data as on the EMS. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void GetAssessments(Action<List<Assessment>> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(GetAssessmentsProcess(AppearitionGate.Instance.CurrentUser.selectedChannel, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Fetches all available assessments.
        ///
        /// Offline Capability.
        /// </summary>
        /// <param name="onSuccess">Contains the Assessment Data as on the EMS. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator GetAssessmentsProcess(Action<List<Assessment>> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            yield return GetAssessmentsProcess(AppearitionGate.Instance.CurrentUser.selectedChannel, onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Fetches all available assessments.
        ///
        /// Offline Capability.
        /// </summary>
        /// <param name="channelId">The id of the targeted channel.</param>
        /// <param name="onSuccess">Contains the Assessment Data as on the EMS. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void GetAssessments(int channelId, Action<List<Assessment>> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(GetAssessmentsProcess(channelId, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Fetches all available assessments.
        ///
        /// Offline Capability.
        /// </summary>
        /// <param name="channelId">The id of the targeted channel.</param>
        /// <param name="onSuccess">Contains the Assessment Data as on the EMS. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator GetAssessmentsProcess(int channelId, Action<List<Assessment>> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            //Launch request
            var assessmentListRequest = AppearitionRequest<Assessment_List>.LaunchAPICall_GET<AssessmentHandler>(channelId, GetReusableApiRequest<Assessment_List>());

            //Wait for request..
            while (!assessmentListRequest.IsDone)
                yield return null;

            //All done!
            if (assessmentListRequest.RequestResponseObject.Data != null && assessmentListRequest.RequestResponseObject.IsSuccess)
            {
                List<Assessment> outcome = new List<Assessment>(assessmentListRequest.RequestResponseObject.Data);

                if (assessmentListRequest.CurrentState == AppearitionBaseRequest<Assessment_List>.RequestState.SuccessOnline)
                    AppearitionLogger.LogInfo(string.Format(AssessmentConstants.ASSESSMENT_LIST_SUCCESS, channelId));
                else
                    AppearitionLogger.LogInfo(string.Format(AssessmentConstants.ASSESSMENT_LIST_SUCCESS_OFFLINE, channelId));

                //Callback it out ~
                if (onSuccess != null)
                    onSuccess(outcome);
            }
            else
            {
                AppearitionLogger.LogError(string.Format(AssessmentConstants.ASSESSMENT_LIST_FAILURE, channelId));

                //Request failed =(
                if (onFailure != null)
                    onFailure(assessmentListRequest.Errors);
            }

            if (onComplete != null)
                onComplete(assessmentListRequest.RequestResponseObject.Data != null && assessmentListRequest.RequestResponseObject.IsSuccess);
        }

        #endregion

        #region Submit

        /// <summary>
        /// Submits a completed assessment.
        /// </summary>
        /// <param name="assessment"></param>
        /// <param name="onSuccess">Contains the Assessment Data as on the EMS. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void SubmitAssessment(AssessmentSubmissionData assessment, Action onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(SubmitAssessmentProcess(AppearitionGate.Instance.CurrentUser.selectedChannel, assessment, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Submits a completed assessment.
        /// </summary>
        /// <param name="assessment"></param>
        /// <param name="onSuccess">Contains the Assessment Data as on the EMS. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator SubmitAssessmentProcess(AssessmentSubmissionData assessment, Action onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            yield return SubmitAssessmentProcess(AppearitionGate.Instance.CurrentUser.selectedChannel, assessment, onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Submits a completed assessment.
        /// </summary>
        /// <param name="channelId">The id of the targeted channel.</param>
        /// <param name="assessment"></param>
        /// <param name="onSuccess">Contains the Assessment Data as on the EMS. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void SubmitAssessment(int channelId, AssessmentSubmissionData assessment, Action onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(SubmitAssessmentProcess(channelId, assessment, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Submits a completed assessment.
        /// </summary>
        /// <param name="channelId">The id of the targeted channel.</param>
        /// <param name="assessment"></param>
        /// <param name="onSuccess">Contains the Assessment Data as on the EMS. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator SubmitAssessmentProcess(int channelId, AssessmentSubmissionData assessment, Action onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            //Don't fire the request unless the assessment is valid.
            if (assessment == null || string.IsNullOrEmpty(assessment.studentUsername) || string.IsNullOrEmpty(assessment.endDateTime))
            {
                AppearitionLogger.LogError(AssessmentConstants.ASSESSMENT_SUBMIT_INVALID);
                onFailure?.Invoke(new EmsError(AssessmentConstants.ASSESSMENT_SUBMIT_INVALID));
                onComplete?.Invoke(false);
                yield break;
            }

            var addTagRequest =
                AppearitionRequest<Assessment_Submit>.LaunchAPICall_POST<AssessmentHandler>(channelId, GetReusableApiRequest<Assessment_Submit>(), assessment);

            while (!addTagRequest.IsDone)
                yield return null;

            if (addTagRequest.RequestResponseObject.IsSuccess)
            {
                AppearitionLogger.LogInfo(string.Format(AssessmentConstants.ASSESSMENT_SUBMIT_SUCCESS, assessment.assessmentId, assessment.assessmentName, assessment.studentUsername, channelId));

                if (onSuccess != null)
                    onSuccess();
            }
            else
            {
                AppearitionLogger.LogError(string.Format(AssessmentConstants.ASSESSMENT_SUBMIT_FAILURE, assessment.assessmentId, assessment.assessmentName, channelId));

                if (onFailure != null)
                    onFailure(addTagRequest.Errors);
            }

            if (onComplete != null)
                onComplete(addTagRequest.RequestResponseObject.IsSuccess);
        }

        #endregion
    }
}