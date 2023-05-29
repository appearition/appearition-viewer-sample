// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: ArTarget_Publish.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using Appearition.API;

namespace Appearition.ArTargetImageAndMedia.API
{
    /// <summary>
    /// https://api.appearition.com/TenantName/api/ArTarget/Publish/0?arTargetId=1 , where 0 is Channel ID and 1 is the arTargetId
    /// </summary>
    [System.Serializable]
    public class ArTarget_Publish : BaseApiPost
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
    }
}