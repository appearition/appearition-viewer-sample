// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: ArTarget_UnlinkMedia.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using Appearition.API;

namespace Appearition.ArTargetImageAndMedia.API
{
    /// <summary>
    /// https://api.appearition.com/TenantName/api/ArTarget/UnlinkMedia/0 , where 0 is Channel ID 
    /// </summary>
    [System.Serializable]
    public class ArTarget_UnlinkMedia : BaseApiPost
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
    }
}