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
    /// https://api.appearition.com/TenantName/api/Asset/RelatedTagsByChannel/0 , where 0 is Channel ID
    /// </summary>
    [System.Serializable]
    public class Asset_TagsByChannel : BaseApiGet
    {
        public override int ApiVersion => 2;

        public List<string> Data;
    }
}