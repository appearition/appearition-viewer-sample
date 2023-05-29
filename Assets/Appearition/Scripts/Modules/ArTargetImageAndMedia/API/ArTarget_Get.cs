// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: ArTarget_Get.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Appearition.API;
using Appearition.Common;

namespace Appearition.ArTargetImageAndMedia.API
{
    /// <summary>
    /// https://api.appearition.com/TenantName/api/ArTarget/Get/0?arTargetId=1 , where 0 is Channel ID and 1 is Ar Target ID
    /// </summary>
    [System.Serializable]
    public class ArTarget_Get : BaseApiGet
    {
        public override TypeOfApiStorage ResponseSaveType => TypeOfApiStorage.Custom;

        //Request Params
        [System.Serializable]
        public class RequestContent : BaseRequestContent
        {
            public int arTargetId;
        }

        //Variables
        public ArTarget Data;

        #region Storage

        #region READ

        public override string GetCustomOfflineJsonContent(int channelId, object postBodyIfAny, BaseRequestContent requestContentIfAny)
        {
            RequestContent requestContent = null;
            if (requestContentIfAny is RequestContent newContent)
                requestContent = newContent;

            if (requestContent == null)
                return "";

            //Load the current database
            List<ArTarget> database = CustomApiStorageDatabase<ArTarget, ArTargetHandler>.GetStoredContent();

            ArTarget outcome = null;

            //Filter based on the body
            for (int i = 0; i < database.Count; i++)
            {
                if ((channelId == 0 || channelId == database[i].productId) && database[i].arTargetId == requestContent.arTargetId)
                {
                    outcome = database[i];
                    break;
                }
            }

            if (outcome == null)
                return "";

            //Return a look-alike
            ArTarget_Get offlineRequest = new ArTarget_Get() {
                Data = outcome
            };

            return AppearitionConstants.SerializeJson(offlineRequest);
        }

        #endregion

        #region WRITE

        public override void UpdateCustomOfflineJsonContent(BaseApi api, int channelId, object postBodyIfAny, BaseRequestContent requestContentIfAny)
        {
            if (api.GetType() != typeof(ArTarget_Get))
            {
                AppearitionLogger.LogWarning("Tried to update the custom response content of this API with a different type: " + api.GetType());
                return;
            }

            ArTarget_Get response = (ArTarget_Get) api;
            CustomApiStorageDatabase<ArTarget, ArTargetHandler>.UpdateStorageWithNewLiveContent(new List<ArTarget>() {response.Data}, AreTwoAssetsTheSame);
        }

        public static bool AreTwoAssetsTheSame(Asset oldAsset, Asset newAsset)
        {
            return oldAsset.assetId.Trim().Equals(newAsset.assetId.Trim(), StringComparison.InvariantCultureIgnoreCase);
        }

        #endregion

        #endregion
    }
}