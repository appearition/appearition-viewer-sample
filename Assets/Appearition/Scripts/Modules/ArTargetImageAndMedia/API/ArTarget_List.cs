// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: ArTarget_List.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Appearition.API;
using Appearition.Common;
using UnityEngine;

namespace Appearition.ArTargetImageAndMedia.API
{
    /// <summary>
    /// https://api.appearition.com/TenantName/api/ArTarget/List/0 , where 0 is Channel ID 
    /// </summary>
    [System.Serializable]
    public class ArTarget_List : BaseApiPost
    {
        public override int ApiVersion => 2;
        public override TypeOfApiStorage ResponseSaveType => TypeOfApiStorage.Custom;

        [System.Serializable]
        public class QueryContent : Asset_List.QueryContent
        {
            /// <summary>
            /// AssetId, basically.
            /// </summary>
            [SerializeField] string ArTargetKey;
            /// <summary>
            /// Published, Unpublished, or blank.
            /// </summary>
            public string Status;
            public bool MarketOnly;
            public bool ExcludeMarket;
            
            public QueryContent()
            { 
            }

            public QueryContent(QueryContent cc) : base(cc)
            {
                ArTargetKey = string.IsNullOrEmpty(cc.AssetId) ? cc.ArTargetKey : cc.AssetId;
                Status = cc.Status;
                MarketOnly = cc.MarketOnly;
                ExcludeMarket = cc.ExcludeMarket;
            }
        }

        [System.Serializable]
        public class QueryOutcome
        {
            public List<ArTarget> ArTargets = new List<ArTarget>();
            public int Page;
            public int TotalRecords;
            public int RecordsPerPage;
            public List<string> StatusOptions;
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
            List<ArTarget> database = CustomApiStorageDatabase<ArTarget, ArTargetHandler>.GetStoredContent();
            QueryOutcome queryOutcome = new QueryOutcome()
            {
                ArTargets = new List<ArTarget>()
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

                    queryOutcome.ArTargets.Add(database[i]);
                }
            }

            //Return a look-alike
            ArTarget_List offlineRequest = new ArTarget_List()
            {
                Data = queryOutcome,
                Errors = null,
                IsSuccess = true,
            };

            return AppearitionConstants.SerializeJson(offlineRequest);
        }

        bool IsAssetMatchingQuery(int channelId, QueryContent query, ArTarget arTarget)
        {
            if (channelId != 0 && channelId != arTarget.productId)
                return false;

            //Check main info
            if (!string.IsNullOrEmpty(query.Name) && !query.Name.Equals(arTarget.name, StringComparison.InvariantCultureIgnoreCase))
                return false;
            if (!string.IsNullOrEmpty(query.AssetId) && !query.AssetId.Equals(arTarget.assetId, StringComparison.InvariantCultureIgnoreCase))
                return false;
            if (!string.IsNullOrEmpty(query.CreatedByUsername) && !query.CreatedByUsername.Equals(arTarget.createdByUsername, StringComparison.InvariantCultureIgnoreCase))
                return false;
            if (!string.IsNullOrEmpty(query.Status) && query.Status.Equals("published", StringComparison.InvariantCultureIgnoreCase) != arTarget.isPublished)
                return false;
            if (query.MarketOnly && !arTarget.isInMarket)
                return false;
            if (query.ExcludeMarket && arTarget.isInMarket)
                return false;

            //Check tags
            if (query.Tags != null && query.Tags.Count > 0)
            {
                if (arTarget.tags == null || arTarget.tags.Count == 0)
                    return false;

                if (query.FilterTagsUsingAnd)
                {
                    int containedTags = 0;
                    for (int i = 0; i < query.Tags.Count; i++)
                    {
                        if (arTarget.ContainsTag(query.Tags[i]))
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
                        if (arTarget.ContainsTag(query.Tags[i]))
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
            if (api.GetType() != typeof(ArTarget_List))
            {
                AppearitionLogger.LogWarning("Tried to update the custom response content of this API with a different type: " + api.GetType());
                return;
            }

            ArTarget_List response = (ArTarget_List)api;
            CustomApiStorageDatabase<ArTarget, ArTargetHandler>.UpdateStorageWithNewLiveContent(response.Data.ArTargets, AreTwoAssetsTheSame);
        }

        public static bool AreTwoAssetsTheSame(ArTarget oldAsset, ArTarget newAsset)
        {
            return oldAsset.assetId.Trim().Equals(newAsset.assetId.Trim(), StringComparison.InvariantCultureIgnoreCase);
        }

        #endregion

        #endregion
    }
}