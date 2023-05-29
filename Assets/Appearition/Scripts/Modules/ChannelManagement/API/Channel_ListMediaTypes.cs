// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: Channel_ListMediaTypes.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using Appearition.API;
using Appearition.Common;

namespace Appearition.ChannelManagement.API
{
    /// <summary>
    /// https://api.appearition.com/TenantName/api/Channel/ListMediaTypes/0 , where 0 is Channel ID 
    /// </summary>
    [System.Serializable]
    public class Channel_ListMediaTypes : BaseApiGet
    {
        public override AuthenticationOverrideType AuthenticationOverride => AuthenticationOverrideType.ApplicationToken;
        public override int ApiVersion => 2;

        //Variables
        public ApiData Data;

        //SubClasses
        [System.Serializable]
        public class ApiData
        {
            //Variables
            public MediaType[] mediaTypes;
        }
    }
}