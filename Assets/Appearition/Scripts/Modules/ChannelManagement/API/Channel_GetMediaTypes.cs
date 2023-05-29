// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: Channel_GetMediaTypes.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using System.Collections.Generic;
using Appearition.API;
using Appearition.Common;

namespace Appearition.ChannelManagement.API
{
    /// <summary>
    /// https://api.appearition.com/TenantName/api/Channel/GetMediaTypes/0 , where 0 is Channel ID 
    /// </summary>
    [System.Serializable]
    public class Channel_GetMediaTypes : BaseApiGet
    {
        public override AuthenticationOverrideType AuthenticationOverride => AuthenticationOverrideType.ApplicationToken;
        public override int ApiVersion => 2;

        //Variables
        public ApiData Data;

        //SubClasses
        public class ApiData
        {
            //Variables
            public List<MediaType> mediaTypes;
        }
    }
}