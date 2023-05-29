// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: ArTarget_UploadMedia.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using Appearition.API;
using Appearition.Common;

namespace Appearition.ArTargetImageAndMedia.API
{
    /// <summary>
    /// https://api.appearition.com/TenantName/api/ArTarget/UploadMedia/0 , where 0 is Channel ID and arTargetId is the target ID
    /// </summary>
    [System.Serializable]
    public class ArTarget_UploadMedia : BaseApiPost
    {
        //public override AuthenticationOverrideType AuthenticationOverride => AuthenticationOverrideType.SessionToken;
        public override TypeOfApiStorage ResponseSaveType => TypeOfApiStorage.None;

        //Request Params
        [System.Serializable]
        public class RequestContent : BaseRequestContent
        {
            public int arTargetId;
        }

        public override TypeOfPost FormType
        {
            get { return TypeOfPost.MultiForms; }
        }

        //Variables
        public MediaFile Data;
    }
}