using System;
using System.Collections;
using System.Collections.Generic;
using Appearition.Common;
using Appearition.Internal;
using Appearition.QAndA.API;
using UnityEngine;

namespace Appearition.QAndA
{
    public class QAndAHandler : BaseHandler
    {
        /// <summary>
        /// Fetches the Q&A content from the EMS.
        ///
        /// Offline Capability.
        /// </summary>
        /// <param name="onSuccess">Contains the Q&A Data as on the EMS. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void GetQAndAContent(Action<List<Question>> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(GetQAndAContentProcess(AppearitionGate.Instance.CurrentUser.selectedChannel, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Fetches the Q&A content from the EMS.
        ///
        /// Offline Capability.
        /// </summary>
        /// <param name="onSuccess">Contains the Q&A Data as on the EMS. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator GetQAndAContentProcess(Action<List<Question>> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            yield return GetQAndAContentProcess(AppearitionGate.Instance.CurrentUser.selectedChannel, onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Fetches the Q&A content from the EMS.
        ///
        /// Offline Capability.
        /// </summary>
        /// <param name="channelId">The id of the targeted channel.</param>
        /// <param name="onSuccess">Contains the Q&A Data as on the EMS. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void GetQAndAContent(int channelId, Action<List<Question>> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(GetQAndAContentProcess(channelId, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Fetches the Q&A content from the EMS.
        ///
        /// Offline Capability.
        /// </summary>
        /// <param name="channelId">The id of the targeted channel.</param>
        /// <param name="onSuccess">Contains the Q&A Data as on the EMS. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator GetQAndAContentProcess(int channelId, Action<List<Question>> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            //Launch request
            var qAndAListRequest = AppearitionRequest<QandA_List>.LaunchAPICall_GET<QAndAHandler>(channelId, GetReusableApiRequest<QandA_List>());

            //Wait for request..
            while (!qAndAListRequest.IsDone)
                yield return null;

            //All done!
            if (qAndAListRequest.RequestResponseObject.Data != null && qAndAListRequest.RequestResponseObject.IsSuccess)
            {
                List<Question> outcome = new List<Question>(qAndAListRequest.RequestResponseObject.Data);

                if(qAndAListRequest.CurrentState == AppearitionBaseRequest<QandA_List>.RequestState.SuccessOnline)
                    AppearitionLogger.LogInfo(string.Format(QAndAConstants.Q_AND_A_LIST_SUCCESS, channelId));
                else 
                    AppearitionLogger.LogInfo(string.Format(QAndAConstants.Q_AND_A_LIST_SUCCESS_OFFLINE, channelId));

                //Callback it out ~
                if (onSuccess != null)
                    onSuccess(outcome);
            }
            else
            {
                AppearitionLogger.LogError(string.Format(QAndAConstants.Q_AND_A_LIST_FAILURE, channelId));

                //Request failed =(
                if (onFailure != null)
                    onFailure(qAndAListRequest.Errors);
            }

            if (onComplete != null)
                onComplete(qAndAListRequest.RequestResponseObject.Data != null && qAndAListRequest.RequestResponseObject.IsSuccess);
        }
    }
}