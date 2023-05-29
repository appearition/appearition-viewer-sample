using System.Collections;
using System.Collections.Generic;
using Appearition.API;
using UnityEngine;

namespace Appearition.ContentLibrary.API
{
    /// <summary>
    /// https://api.appearition.com/TenantName/api/Content/Search/0?providerName=1 , where 0 is Channel ID, 1 is provider name
    /// </summary>
    public class Content_Search : BaseApiPost
    {
        [System.Serializable]
        public class QueryContent
        {
            public bool FirstPageIsOne;
            public int Page;
            public int RecordsPerPage;
            public string SearchText;
            public string ContentFormatType;
            public List<string> FilterByTags;
            public bool FilterPublicUrlOnly;
            public string FilterUsageRights;
            public string SearchResponseMetadata;

            public QueryContent()
            {
            }

            public QueryContent(QueryContent cc)
            {
                FirstPageIsOne = cc.FirstPageIsOne;
                Page = cc.Page;
                RecordsPerPage = cc.RecordsPerPage;

                SearchText = cc.SearchText;
                ContentFormatType = cc.ContentFormatType;

                if (cc.FilterByTags == null)
                    FilterByTags = new List<string>();
                else
                    FilterByTags = new List<string>(cc.FilterByTags);

                FilterPublicUrlOnly = cc.FilterPublicUrlOnly;
                FilterUsageRights = cc.FilterUsageRights;
                SearchResponseMetadata = cc.SearchResponseMetadata;
            }
        }

        [System.Serializable]
        public class QueryOutcome
        {
            public List<ContentItem> ItemsFound = new List<ContentItem>();
            public int TotalResults;
            public int RecordsPerPage;
            public string SearchResponseMetadata;
            public string ContentFormatType;
            public string UsageRights;
        }

        public override int ApiVersion => 2;
        public QueryOutcome Data;

        //Request Params
        [System.Serializable]
        public class RequestContent : BaseRequestContent
        {
            public string providerName;
        }
    }
}