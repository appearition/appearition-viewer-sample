using Appearition.API;

namespace Appearition.AccountAndAuthentication.API
{
    /// <summary>
    /// https://api.appearition.com/TenantName/api/OAuth/OAuthCallback/0?clientType=1 , where 0 is Channel ID and 1 is the clientType.
    /// </summary>
    [System.Serializable]
    public class OAuth_OAuthCallback : BaseApiGet
    {
        public override AuthenticationOverrideType AuthenticationOverride => AuthenticationOverrideType.ApplicationToken;
        public override int ApiVersion => 2;
        public override TypeOfApiStorage ResponseSaveType => TypeOfApiStorage.None;

        //Variables
        public string Data;

        //Request Params
        [System.Serializable]
        public class RequestContent : BaseRequestContent
        {
            public string clientType;
        }
    }
}