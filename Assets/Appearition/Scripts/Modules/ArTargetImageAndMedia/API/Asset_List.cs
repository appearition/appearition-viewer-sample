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
    /// https://api.appearition.com/TenantName/api/Asset/List/0 , where 0 is Channel ID 
    /// </summary>
    [System.Serializable]
    public class Asset_List : BaseApiPost
    {
        public override int ApiVersion => 2;
        public override TypeOfApiStorage ResponseSaveType => TypeOfApiStorage.Custom;

        [System.Serializable]
        public class QueryContent
        {
            /// <summary>
            /// Will gather all the experiences whose name contain this string.
            /// </summary>
            public string Name;
            public string AssetId;
            /// <summary>
            /// Will gather all experiences which were created by this given username.
            /// </summary>
            public string CreatedByUsername;
            /// <summary>
            /// Starts at 0.
            /// </summary>
            public int Page;
            public int RecordsPerPage = 10;
            public List<string> Tags = new List<string>();
            /// <summary>
            /// If true, only experiences which contain all the given tags will be fetched.
            /// Otherwise, will fetch all experiences which contains at least one of the given tags.
            /// </summary>
            public bool FilterTagsUsingAnd = true;
            public bool IncludeTargetImages = true;
            public bool IncludeMedia = true;

            public QueryContent()
            {
            }

            public QueryContent(QueryContent cc)
            {
                Name = cc.Name;
                AssetId = cc.AssetId;
                CreatedByUsername = cc.CreatedByUsername;
                Page = cc.Page;
                RecordsPerPage = cc.RecordsPerPage;

                if (cc.Tags == null)
                    Tags = new List<string>();
                else
                    Tags = new List<string>(cc.Tags);

                FilterTagsUsingAnd = cc.FilterTagsUsingAnd;
                IncludeTargetImages = cc.IncludeTargetImages;
                IncludeMedia = cc.IncludeMedia;
            }
        }

        [System.Serializable]
        public class QueryOutcome
        {
            public List<Asset> assets = new List<Asset>();
            public int totalRecords;
        }

        public QueryOutcome Data;

        /// <summary>
        /// Post ApiData
        /// </summary>
        [System.Serializable]
        public class PostData : QueryContent
        {
            public PostData(QueryContent cc) : base(cc)
            {
            }
        }

        #region Storage

        #region READ

        public override string GetCustomOfflineJsonContent(int channelId, object postBodyIfAny, BaseRequestContent requestContentIfAny)
        {
            //Try to get the post body
            PostData query = null;
            if (postBodyIfAny is PostData newBody)
                query = newBody;

            if (query == null)
                return "";

            //Load the current database
            List<Asset> database = CustomApiStorageDatabase<Asset, ArTargetHandler>.GetStoredContent();
            QueryOutcome queryOutcome = new QueryOutcome() {
                assets = new List<Asset>()
            };

            //Filter based on the body
            for (int i = query.RecordsPerPage * query.Page; i < database.Count; i++)
            {
                if (i > query.RecordsPerPage * (query.Page + 1))
                    break;
                //If an item is found add it. Remove data as told by the query.
                if (IsAssetMatchingQuery(channelId, query, database[i]))
                {
                    if (!query.IncludeMedia)
                        database[i].mediaFiles = null;
                    if (!query.IncludeTargetImages)
                        database[i].targetImages = null;

                    queryOutcome.assets.Add(database[i]);
                }
            }

            //Return a look-alike
            Asset_List offlineRequest = new Asset_List() {
                Data = queryOutcome,
                Errors = null,
                IsSuccess = true,
            };

            return AppearitionConstants.SerializeJson(offlineRequest);
        }

        bool IsAssetMatchingQuery(int channelId, QueryContent query, Asset asset)
        {
            if (channelId != 0 && channelId != asset.productId)
                return false;

            //Check main info
            if (!string.IsNullOrEmpty(query.Name) && !query.Name.Equals(asset.name, StringComparison.InvariantCultureIgnoreCase))
                return false;
            if (!string.IsNullOrEmpty(query.AssetId) && !query.AssetId.Equals(asset.assetId, StringComparison.InvariantCultureIgnoreCase))
                return false;
            if (!string.IsNullOrEmpty(query.CreatedByUsername) && !query.CreatedByUsername.Equals(asset.createdByUsername, StringComparison.InvariantCultureIgnoreCase))
                return false;

            //Check tags
            if (query.Tags != null && query.Tags.Count > 0)
            {
                if (asset.tags == null || asset.tags.Count == 0)
                    return false;

                if (query.FilterTagsUsingAnd)
                {
                    int containedTags = 0;
                    for (int i = 0; i < query.Tags.Count; i++)
                    {
                        if (asset.ContainsTag(query.Tags[i]))
                            containedTags++;
                    }

                    if (containedTags != query.Tags.Count)
                        return false;
                }
                else
                {
                    bool foundAnyTags = false;
                    for (int i = 0; i < query.Tags.Count; i++)
                    {
                        if (asset.ContainsTag(query.Tags[i]))
                            foundAnyTags = true;
                    }

                    if (!foundAnyTags)
                        return false;
                }
            }

            return true;
        }

        #endregion

        #region WRITE

        public override void UpdateCustomOfflineJsonContent(BaseApi api, int channelId, object postBodyIfAny, BaseRequestContent requestContentIfAny)
        {
            if (api.GetType() != typeof(Asset_List))
            {
                AppearitionLogger.LogWarning("Tried to update the custom response content of this API with a different type: " + api.GetType());
                return;
            }

            Asset_List response = (Asset_List) api;
            CustomApiStorageDatabase<Asset, ArTargetHandler>.UpdateStorageWithNewLiveContent(response.Data.assets, AreTwoAssetsTheSame);
        }

        public static bool AreTwoAssetsTheSame(Asset oldAsset, Asset newAsset)
        {
            return oldAsset.assetId.Trim().Equals(newAsset.assetId.Trim(), StringComparison.InvariantCultureIgnoreCase);
        }

        #endregion

        #endregion
    }
}