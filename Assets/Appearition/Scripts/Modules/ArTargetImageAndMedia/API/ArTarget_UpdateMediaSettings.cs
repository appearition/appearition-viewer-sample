// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: ArTarget_UpdateMediaSettings.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using Appearition.API;
using Appearition.Common;

namespace Appearition.ArTargetImageAndMedia.API
{
    /// <summary>
    /// https://api.appearition.com/TenantName/api/ArTarget/UpdateMediaSettings/0 , where 0 is Channel ID 
    /// </summary>
    [System.Serializable]
    public class ArTarget_UpdateMediaSettings : BaseApiPost
    {
        //public override AuthenticationOverrideType AuthenticationOverride => AuthenticationOverrideType.SessionToken;
        public override TypeOfApiStorage ResponseSaveType => TypeOfApiStorage.None;

        //Request Params
        [System.Serializable]
        public class RequestContent : BaseRequestContent
        {
            public int arTargetId;
            public int arMediaId;
        }

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