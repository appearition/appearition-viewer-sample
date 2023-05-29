// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: ArTarget_GetProperties.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using System.Collections.Generic;
using Appearition.API;
using Appearition.Common;

namespace Appearition.ArTargetImageAndMedia.API
{
    /// <summary>
    /// https://api.appearition.com/TenantName/api/ArTarget/GetProperties/0?arTargetId=1 , where 0 is Channel ID, 1 is Ar Target ID .
    /// </summary>
    [System.Serializable]
    public class ArTarget_GetProperties : BaseApiGet
    {
        public override int ApiVersion => 2;

        //Data
        public List<Property> Data;
        
        //Request Params
        [System.Serializable]
        public class RequestContent : BaseRequestContent
        {
            public int arTargetId;
        }
    }
}