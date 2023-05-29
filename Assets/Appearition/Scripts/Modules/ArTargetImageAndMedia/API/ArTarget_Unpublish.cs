// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: ArTarget_Unpublish.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using Appearition.API;

namespace Appearition.ArTargetImageAndMedia.API
{
    /// <summary>
    /// https://api.appearition.com/TenantName/api/ArTarget/Unpublish/0 , where 0 is Channel ID 
    /// </summary>
    [System.Serializable]
    public class ArTarget_Unpublish : BaseApiPost
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