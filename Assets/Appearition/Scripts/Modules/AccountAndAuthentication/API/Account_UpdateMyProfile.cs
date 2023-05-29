// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: Account_UpdateMyProfile.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using Appearition.API;

namespace Appearition.AccountAndAuthentication.API
{
    /// <summary>
    /// https://api.appearition.com/TenantName/api/Account/UpdateMyProfile/0 , where 0 is Channel ID 
    /// </summary>
    [System.Serializable]
    public class Account_UpdateMyProfile : BaseApiPost
    {
        public override AuthenticationOverrideType AuthenticationOverride => AuthenticationOverrideType.SessionToken;
        public override int ApiVersion => 2;
        public override TypeOfApiStorage ResponseSaveType => TypeOfApiStorage.None;

        [System.Serializable]
        public class PostApi : Profile
        {
        }
    }
}