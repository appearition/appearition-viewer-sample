// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: Channel_Get.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using Appearition.API;

namespace Appearition.ChannelManagement.API
{
    /// <summary>
    /// https://api.appearition.com/TenantName/api/Channel/Get/0 , where 0 is Channel ID 
    /// </summary>
    [System.Serializable]
    public class Channel_Get : BaseApiGet
    {
        public override int ApiVersion => 2;

        //Variables
        public Channel Data;
    }
}