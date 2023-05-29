using System.Collections;
using System.Collections.Generic;
using Appearition.API;

namespace Appearition.ContentLibrary.API
{
    /// <summary>
    /// https://api.appearition.com/TenantName/api/Content/Item/0?providerName=1&itemKey=2 , where 0 is Channel ID, 1 is provider name, and 2 is the item key
    /// </summary>
    [System.Serializable]
    public class Content_Item : BaseApiGet
    {
        public override int ApiVersion => 2;

        //Request Params
        [System.Serializable]
        public class RequestContent : BaseRequestContent
        {
            public string providerName;
            public string itemKey;
        }

        //Variables
        public ContentItem Data;

        public override string GetJsonFileExtraParams(object postBodyIfAny, BaseRequestContent requestContentIfAny)
        {
            RequestContent content = (RequestContent) requestContentIfAny;
            if (content == null)
                return "";
            
            return $"{content.providerName}_{content.itemKey}";
        }
    }
}