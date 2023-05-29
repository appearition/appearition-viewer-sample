﻿using Appearition.API;

namespace Appearition.ArTargetImageAndMedia.API
{
    public class ArTarget_UnlinkArImage : BaseApiPost
    {
        //public override AuthenticationOverrideType AuthenticationOverride => AuthenticationOverrideType.SessionToken;
        public override TypeOfApiStorage ResponseSaveType => TypeOfApiStorage.None;

        //Request Params
        [System.Serializable]
        public class RequestContent : BaseRequestContent
        {
            public int arTargetId;
            public int arImageId;
        }
    }
}