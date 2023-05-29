using System;
using Appearition.API;

namespace Appearition.AccountAndAuthentication.API
{
    /// <summary>
    /// https://api.appearition.com/TenantName/api/Account/AccountStatus/0 , where 0 is Channel ID 
    /// </summary>
    [System.Serializable, Obsolete]
    public class Account_AccountStatus : BaseApiGet
    {
        public override AuthenticationOverrideType AuthenticationOverride => AuthenticationOverrideType.SessionToken;
        public override int ApiVersion => 2;

        //Variables
        public ApiData Data;

        [System.Serializable]
        public class ApiData : AccountStatus
        {
        }
    }
}