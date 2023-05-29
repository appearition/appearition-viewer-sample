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
    /// https://api.appearition.com/TenantName/api/Asset/RelatedTagsByChannel/0?tag={1} , where 0 is Channel ID and 1 is the given tag.
    /// </summary>
    [System.Serializable]
    public class Asset_RelatedTagsByChannel : BaseApiGet
    {
        public override int ApiVersion => 2;

        public List<string> Data;

        //Request Params
        [System.Serializable]
        public class RequestContent : BaseRequestContent
        {
            public string tag;
        }

        public override string GetJsonFileExtraParams(object postBodyIfAny, BaseRequestContent requestContentIfAny)
        {
            if (requestContentIfAny is RequestContent requestContent)
                return requestContent.tag;
            return "";
        }
    }
}