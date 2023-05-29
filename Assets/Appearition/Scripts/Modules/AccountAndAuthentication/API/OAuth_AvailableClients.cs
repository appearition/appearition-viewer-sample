using Appearition.API;

namespace Appearition.AccountAndAuthentication.API
{
    /// <summary>
    /// https://api.appearition.com/TenantName/api/OAuth/AvailableClients/0 , where 0 is Channel ID 
    /// </summary>
    [System.Serializable]
    public class OAuth_AvailableClients : BaseApiGet
    {
        public override AuthenticationOverrideType AuthenticationOverride => AuthenticationOverrideType.ApplicationToken;
        public override int ApiVersion => 2;

        //Variables
        public OAuthClient[] Data;
    }
}