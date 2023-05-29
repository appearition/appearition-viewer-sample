// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: Media_Delete.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using Appearition.API;

namespace Appearition.ArTargetImageAndMedia.API
{
    /// <inheritdoc />
    /// <summary>
    /// https://api.appearition.com/TenantName/api/Media/Delete/0 , where 0 is Channel ID 
    /// </summary>
    [System.Serializable]
    public class Media_Delete : BaseApiPost
    {
        //public override AuthenticationOverrideType AuthenticationOverride => AuthenticationOverrideType.SessionToken;
        public override TypeOfApiStorage ResponseSaveType => TypeOfApiStorage.None;

        //Request Params
        [System.Serializable]
        public class RequestContent : BaseRequestContent
        {
            public int arMediaId;
        }
    }
}