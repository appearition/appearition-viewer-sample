using System.Collections.Generic;
using Appearition.API;

namespace Appearition.ContentLibrary.API
{
    /// <summary>
    /// https://api.appearition.com/TenantName/api/Content/Item/0 , where 0 is Channel ID
    /// </summary>
    [System.Serializable]
    public class Content_Providers : BaseApiGet
    {
        public override int ApiVersion => 2;

        //Variables
        public List<ContentProvider> Data;
    }
}