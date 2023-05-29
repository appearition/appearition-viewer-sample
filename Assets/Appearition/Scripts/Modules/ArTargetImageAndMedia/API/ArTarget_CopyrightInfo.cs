// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: Asset_MediaByAsset.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using System.Collections.Generic;
using Appearition.API;

namespace Appearition.ArTargetImageAndMedia.API
{
    /// <summary>
    /// https://api.appearition.com/TenantName/api/ArTarget/CopyrightInfo/0?arTargetId={1} , where 0 is Channel ID and 1 is the given ArTargetId.
    /// </summary>
    [System.Serializable]
    public class ArTarget_CopyrightInfo : BaseApiGet
    {
        public override int ApiVersion => 2;

        public CopyrightInfo Data;

        //Request Params
        [System.Serializable]
        public class RequestContent : BaseRequestContent
        {
            public int arTargetId;
        }

        public override string GetJsonFileExtraParams(object postBodyIfAny, BaseRequestContent requestContentIfAny)
        {
            RequestContent content = (RequestContent) requestContentIfAny;
            if (content == null)
                return "";

            return $"{content.arTargetId}";
        }
    }
}