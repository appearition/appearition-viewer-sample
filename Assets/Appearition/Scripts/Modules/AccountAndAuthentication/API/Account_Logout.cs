// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: Account_Logout.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using Appearition.API;

namespace Appearition.AccountAndAuthentication.API
{
    /// <summary>
    /// https://api.appearition.com/TenantName/api/Account/Logout/0 , where 0 is Channel ID 
    /// </summary>
    [System.Serializable]
    public class Account_Logout : BaseApiGet
    {
        public override AuthenticationOverrideType AuthenticationOverride => AuthenticationOverrideType.SessionToken;
        public override int ApiVersion => 2;
        public override TypeOfApiStorage ResponseSaveType => TypeOfApiStorage.None;
    }
}