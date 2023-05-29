using Appearition.API;

namespace Appearition.AccountAndAuthentication.API
{
    /// <summary>
    /// https://api.appearition.com/TenantName/api/Security/VerifyAccount/0?token=1 , where 0 is Channel ID and 1 is the Token.
    /// </summary>
    [System.Serializable]
    public class Security_VerifyAccount : BaseApiGet
    {
        public override AuthenticationOverrideType AuthenticationOverride => AuthenticationOverrideType.SessionToken;
        public override int ApiVersion => 2;
        public override TypeOfApiStorage ResponseSaveType => TypeOfApiStorage.None;

        //Variables
        public string Data;

        //Request Params
        [System.Serializable]
        public class RequestContent : BaseRequestContent
        {
            public string token;
        }
    }
}