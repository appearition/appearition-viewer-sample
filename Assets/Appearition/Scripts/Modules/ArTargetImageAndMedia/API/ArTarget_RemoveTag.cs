// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: ArTarget_Get.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using Appearition.API;

namespace Appearition.ArTargetImageAndMedia.API
{
    /// <summary>
    /// https://api.appearition.com/TenantName/api/ArTarget/RemoveTag/0?arTargetId=1&tag=tmp , where 0 is Channel ID, 1 is Ar Target ID and tmp is the tag to be removed.
    /// </summary>
    [System.Serializable]
    public class ArTarget_RemoveTag : BaseApiPost
    {
        //public override AuthenticationOverrideType AuthenticationOverride => AuthenticationOverrideType.SessionToken;
        public override TypeOfApiStorage ResponseSaveType => TypeOfApiStorage.None;
        
        //Request Params
        [System.Serializable]
        public class RequestContent : BaseRequestContent
        {
            public int arTargetId;
            public string tag;
        }
    }
}