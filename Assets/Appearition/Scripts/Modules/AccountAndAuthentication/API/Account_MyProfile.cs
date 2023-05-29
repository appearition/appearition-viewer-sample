// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: Account_MyProfile.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using Appearition.API;

namespace Appearition.AccountAndAuthentication.API
{
    /// <summary>
    /// https://api.appearition.com/TenantName/api/Account/MyProfile/0 , where 0 is Channel ID 
    /// </summary>
    [System.Serializable]
    public class Account_MyProfile : BaseApiPost
    {
        public override AuthenticationOverrideType AuthenticationOverride => AuthenticationOverrideType.SessionToken;
        public override int ApiVersion => 2;
        public override bool RequiresTenant => false;

        //Variables
        public ExtendedProfile Data;
    }
}