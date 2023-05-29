// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: Asset_MediaByAsset.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Reflection;
using Appearition.API;
using Appearition.Common;

namespace Appearition.ArTargetImageAndMedia.API
{
    /// <summary>
    /// https://api.appearition.com/TenantName/api/Asset/MediaByAsset/0 , where 0 is Channel ID 
    /// </summary>
    [System.Serializable]
    public class Asset_MediaByAsset : BaseApiGet
    {
        public override int ApiVersion => 2;
        public override TypeOfApiStorage ResponseSaveType => TypeOfApiStorage.Custom;

        public List<MediaFile> Data;

        /// <summary>
        /// Post ApiData
        /// </summary>
        [System.Serializable]
        public class PostData
        {
            public string assetId;
        }

        #region Storage

        #region READ

        public override string GetCustomOfflineJsonContent(int channelId, object postBodyIfAny, BaseRequestContent requestContentIfAny)
        {
            //Get the post data first
            //Try to get the post body
            PostData query = null;
            if (postBodyIfAny is PostData newBody)
                query = newBody;

            if (query == null || string.IsNullOrEmpty(query.assetId))
                return "";

            //Get the database
            var database = CustomApiStorageDatabase<Asset, ArTargetHandler>.GetStoredContent();
            Asset assetFound = null;

            for (int i = 0; i < database.Count; i++)
            {
                if ((channelId == 0 || channelId == database[i].productId) && database[i].assetId.Equals(query.assetId, StringComparison.InvariantCultureIgnoreCase))
                {
                    assetFound = database[i];
                    break;
                }
            }

            if (assetFound == null)
                return "";

            //Finally, export request
            return AppearitionConstants.SerializeJson(new Asset_MediaByAsset() {Data = new List<MediaFile>(assetFound.mediaFiles), IsSuccess = true});
        }

        #endregion

        #region WRITE

        public override void UpdateCustomOfflineJsonContent(BaseApi api, int channelId, object postBodyIfAny, BaseRequestContent requestContentIfAny)
        {
            //This API is not of a write-type.
        }

        #endregion

        #endregion
    }
}