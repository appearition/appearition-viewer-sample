// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: ArTarget_CreateMedia.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using Appearition.API;
using Appearition.Common;

namespace Appearition.ArTargetImageAndMedia.API
{
    /// <summary>
    /// https://api.appearition.com/TenantName/api/ArTarget/CreateMedia/0 , where 0 is Channel ID 
    /// </summary>
    [System.Serializable]
    public class ArTarget_CreateMedia : BaseApiPost
    {
        public override int ApiVersion => 2;
        //public override AuthenticationOverrideType AuthenticationOverride => AuthenticationOverrideType.SessionToken;
        public override TypeOfApiStorage ResponseSaveType => TypeOfApiStorage.None;
        
        //Request Params
        [System.Serializable]
        public class RequestContent : BaseRequestContent
        {
            public int arTargetId;
        }

        //Variables
        public MediaFile Data;

        /// <summary>
        /// Post ApiData
        /// </summary>
        [System.Serializable]
        public class PostData : MediaFile
        {
            public PostData(MediaFile cc) : base(cc)
            {
            }
        }
    }
}