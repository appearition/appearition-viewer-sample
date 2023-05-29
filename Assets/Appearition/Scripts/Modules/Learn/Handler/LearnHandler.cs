// // -----------------------------------------------------------------------
// // Company:"Appearition Pty Ltd"
// // File: LearnHandler.cs
// // Copyright (c) 2019. All rights reserved.
// // -----------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using Appearition.Common;
using System;
using Appearition.Internal;
using Appearition.Learn.API;

namespace Appearition.Learn
{
    /// <summary>
    /// Handler in charge of taking care of the Learn module as it is on the EMS.
    /// Provides the node tree as well as tracking functionality.
    /// </summary>
    public class LearnHandler : BaseHandler
    {
        /// <summary>
        /// Fetches the learn module's content.
        /// Optionally, can provide a reusable list.
        ///
        /// Offline Capability.
        /// </summary>
        /// <param name="onSuccess">Contains the Learn Data as on the EMS, as a hierarchy node tree. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void GetLearnContent(Action<List<LearnNode>> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            GetLearnContent(AppearitionGate.Instance.CurrentUser.selectedChannel, onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Fetches the learn module's content.
        /// Optionally, can provide a reusable list.
        ///
        /// Offline Capability.
        /// </summary>
        /// <param name="onSuccess">Contains the Learn Data as on the EMS, as a hierarchy node tree. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator GetLearnContentProcess(Action<List<LearnNode>> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            yield return GetLearnContentProcess(AppearitionGate.Instance.CurrentUser.selectedChannel, onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Fetches the learn module's content.
        /// Optionally, can provide a reusable list.
        ///
        /// Offline Capability.
        /// </summary>
        /// <param name="channelId">The id of the targeted channel.</param>
        /// <param name="onSuccess">Contains the Learn Data as on the EMS, as a hierarchy node tree. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void GetLearnContent(int channelId, Action<List<LearnNode>> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(GetLearnContentProcess(channelId, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Fetches the learn module's content.
        /// Optionally, can provide a reusable list.
        ///
        /// Offline Capability.
        /// </summary>
        /// <param name="channelId">The id of the targeted channel.</param>
        /// <param name="onSuccess">Contains the Learn Data as on the EMS, as a hierarchy node tree. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator GetLearnContentProcess(int channelId, Action<List<LearnNode>> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            //Launch request
            var learnListRequest = AppearitionRequest<Learn_List>.LaunchAPICall_GET<LearnHandler>(channelId, GetReusableApiRequest<Learn_List>());

            //Wait for request..
            while (!learnListRequest.IsDone)
                yield return null;

            //All done!
            if (learnListRequest.RequestResponseObject.Data != null && learnListRequest.RequestResponseObject.IsSuccess)
            {
                var outcome = new List<LearnNode>(learnListRequest.RequestResponseObject.Data);

                if (learnListRequest.CurrentState == AppearitionBaseRequest<Learn_List>.RequestState.SuccessOnline)
                    AppearitionLogger.LogInfo(string.Format(LearnConstants.LEARN_LIST_SUCCESS, channelId));
                else
                    AppearitionLogger.LogInfo(string.Format(LearnConstants.LEARN_LIST_SUCCESS_OFFLINE, channelId));

                //Callback it out ~
                if (onSuccess != null)
                    onSuccess(outcome);
            }
            else
            {
                AppearitionLogger.LogError(string.Format(LearnConstants.LEARN_LIST_FAILURE, channelId));

                //Request failed =(
                if (onFailure != null)
                    onFailure(learnListRequest.Errors);
            }

            if (onComplete != null)
                onComplete(learnListRequest.RequestResponseObject.Data != null && learnListRequest.RequestResponseObject.IsSuccess);
        }

        /// <summary>
        /// Submits a full learn session.
        /// </summary>
        /// <param name="session">The learn session container executed by the user. </param>
        /// <param name="onSuccess">Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void SubmitLearnTracking(LearningSession session, Action onSuccess, Action<EmsError> onFailure, Action<bool> onComplete)
        {
            SubmitLearnTracking(AppearitionGate.Instance.CurrentUser.selectedChannel, session, onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Submits a full learn session.
        /// </summary>
        /// <param name="session">The learn session container executed by the user. </param>
        /// <param name="onSuccess">Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator SubmitLearnTrackingProcess(LearningSession session, Action onSuccess, Action<EmsError> onFailure, Action<bool> onComplete)
        {
            yield return SubmitLearnTrackingProcess(AppearitionGate.Instance.CurrentUser.selectedChannel, session, onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Submits a full learn session.
        /// Optionally, can provide a channel Id.
        /// </summary>
        /// <param name="channelId">The id of the targeted channel.</param>
        /// <param name="session">The learn session container executed by the user. </param>
        /// <param name="onSuccess">Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void SubmitLearnTracking(int channelId, LearningSession session, Action onSuccess, Action<EmsError> onFailure, Action<bool> onComplete)
        {
            AppearitionGate.Instance.StartCoroutine(SubmitLearnTrackingProcess(channelId, session, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Submits a full learn session.
        /// Optionally, can provide a channel Id.
        /// </summary>
        /// <param name="channelId">The id of the targeted channel.</param>
        /// <param name="session">The learn session container executed by the user. </param>
        /// <param name="onSuccess">Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator SubmitLearnTrackingProcess(int channelId, LearningSession session, Action onSuccess, Action<EmsError> onFailure, Action<bool> onComplete)
        {
            //Online request
            var submitTrackingRequest = AppearitionRequest<Learn_Tracking>.LaunchAPICall_POST<LearnHandler>
                (channelId, GetReusableApiRequest<Learn_Tracking>(), session);

            while (!submitTrackingRequest.IsDone)
                yield return null;

            if (submitTrackingRequest.RequestResponseObject.IsSuccess)
            {
                AppearitionLogger.LogInfo("Learn tracking submitted successfully!");
                if (onSuccess != null)
                    onSuccess();
            }
            else
            {
                AppearitionLogger.LogError("An error happened when trying to upload the learning session.");
                if (onFailure != null)
                    onFailure(submitTrackingRequest.Errors);
            }

            if (onComplete != null)
                onComplete(submitTrackingRequest.RequestResponseObject.IsSuccess);
        }
    }
}