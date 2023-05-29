// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: Channel_Create.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using Appearition.API;

namespace Appearition.ChannelManagement.API
{
    /// <summary>
    /// https://api.appearition.com/TenantName/api/Channel/Create/0 , where 0 is Channel ID 
    /// </summary>
    [System.Serializable]
    public class Channel_Create : BaseApiPost
    {
        public override AuthenticationOverrideType AuthenticationOverride => AuthenticationOverrideType.SessionToken;
        public override int ApiVersion => 2;
        public override TypeOfApiStorage ResponseSaveType => TypeOfApiStorage.None;
        
        //Variables
        public Channel Data;

        /// <summary>
        /// Post ApiData
        /// </summary>
        [System.Serializable]
        public class PostData
        {
            //Variables
            public string name;
        }
    }
}