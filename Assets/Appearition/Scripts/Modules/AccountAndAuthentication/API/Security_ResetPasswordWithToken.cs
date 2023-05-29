using Appearition.API;

namespace Appearition.AccountAndAuthentication.API
{
    /// <summary>
    /// https://api.appearition.com/TenantName/api/Security/RequestPasswordWithToken/0 , where 0 is Channel ID 
    /// </summary>
    public class Security_ResetPasswordWithToken : BaseApiPost
    {
        public override AuthenticationOverrideType AuthenticationOverride => AuthenticationOverrideType.SessionToken;
        public override int ApiVersion => 2;
        public override TypeOfApiStorage ResponseSaveType => TypeOfApiStorage.None;

        //Variables
        public string Data;

        [System.Serializable]
        public class PostApi
        {
            public string Token;
            public string Password;
            public string ConfirmPassword;
            public int ClientTimezoneOffset;
            public string ClientTimezoneName;
        }
    }
}