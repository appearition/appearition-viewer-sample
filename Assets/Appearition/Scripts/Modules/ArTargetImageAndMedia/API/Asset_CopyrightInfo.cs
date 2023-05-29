// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: Asset_MediaByAsset.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using Appearition.API;

namespace Appearition.ArTargetImageAndMedia.API
{
    /// <summary>
    /// https://api.appearition.com/TenantName/api/Asset/CopyrightInfo/0?assetId={1} , where 0 is Channel ID and 1 is the given AssetId.
    /// </summary>
    [System.Serializable]
    public class Asset_CopyrightInfo : BaseApiGet
    {
        public override int ApiVersion => 2;

        public string Data;

        //Request Params
        [System.Serializable]
        public class RequestContent : BaseRequestContent
        {
            public string assetId;
        }

        public override string GetJsonFileExtraParams(object postBodyIfAny, BaseRequestContent requestContentIfAny)
        {
            RequestContent content = (RequestContent) requestContentIfAny;
            if (content == null)
                return "";

            return $"{content.assetId}";
        }
    }
}