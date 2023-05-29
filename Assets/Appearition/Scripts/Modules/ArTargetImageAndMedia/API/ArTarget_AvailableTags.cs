// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: ArTarget_Get.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using Appearition.API;
using System.Collections.Generic;

namespace Appearition.ArTargetImageAndMedia.API
{
    /// <summary>
    /// https://api.appearition.com/TenantName/api/ArTarget/AvailableTags/0 , where 0 is Channel ID 
    /// </summary>
    [System.Serializable]
    public class ArTarget_AvailableTags : BaseApiGet
    {
        public List<string> Data;
    }
}