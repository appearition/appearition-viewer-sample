// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: Channel_UpdateSettings.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using Appearition.API;
using Appearition.Common;

namespace Appearition.ChannelManagement.API
{
    /// <summary>
    /// https://api.appearition.com/TenantName/api/Channel/UpdateSettings/0 , where 0 is Channel ID 
    /// </summary>
    [System.Serializable]
    public class Channel_UpdateSettings : BaseApiPost
    {
        public override AuthenticationOverrideType AuthenticationOverride => AuthenticationOverrideType.SessionToken;
        public override int ApiVersion => 2;
        public override TypeOfApiStorage ResponseSaveType => TypeOfApiStorage.None;

        /// <summary>
        /// Post ApiData
        /// </summary>
        public class PostData
        {
            //Variables
            public Setting[] settings;
        }
    }
}