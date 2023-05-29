// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: Channel_GetSettings.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using Appearition.API;
using Appearition.Common;

namespace Appearition.ChannelManagement.API
{
    /// <summary>
    /// https://api.appearition.com/TenantName/api/Channel/GetSettings/0 , where 0 is Channel ID 
    /// </summary>
    [System.Serializable]
    public class Channel_GetSettings : BaseApiGet
    {
        public override int ApiVersion => 2;

        //Variables
        public ApiData Data;

        //SubClasses
        public class ApiData
        {
            //Variables
            public int channelId;
            public Setting[] settings;
        }
    }
}