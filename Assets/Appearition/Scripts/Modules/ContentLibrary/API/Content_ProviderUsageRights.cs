using Appearition.API;

namespace Appearition.ContentLibrary.API
{
    /// <summary>
    /// https://api.appearition.com/TenantName/api/Content/Item/0?providerName=1 , where 0 is Channel ID, 1 is provider name
    /// </summary>
    [System.Serializable]
    public class Content_ProviderUsageRights : BaseApiGet
    {
        public override int ApiVersion => 2;

        //Request Params
        [System.Serializable]
        public class RequestContent : BaseRequestContent
        {
            public string providerName;
        }

        //Variables
        public string Data;
    }
}