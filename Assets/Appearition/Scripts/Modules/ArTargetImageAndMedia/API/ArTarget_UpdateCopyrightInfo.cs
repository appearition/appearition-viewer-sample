// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: Asset_MediaByAsset.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using Appearition.API;

namespace Appearition.ArTargetImageAndMedia.API
{
    /// <summary>
    /// https://api.appearition.com/TenantName/api/ArTarget/UpdateCopyrightInfo/0?arTargetId={1} , where 0 is Channel ID and 1 ArTarget id.
    /// </summary>
    [System.Serializable]
    public class ArTarget_UpdateCopyrightInfo : BaseApiPost
    {
        public override int ApiVersion => 2;
        //public override AuthenticationOverrideType AuthenticationOverride => AuthenticationOverrideType.SessionToken;
        public override TypeOfApiStorage ResponseSaveType => TypeOfApiStorage.None;

        /// <summary>
        /// Post ApiData
        /// </summary>
        [System.Serializable]
        public class PostData
        {
            public string copyrightInfo;
        }

        //Request Params
        [System.Serializable]
        public class RequestContent : BaseRequestContent
        {
            public int arTargetId;
        }
    }
}
