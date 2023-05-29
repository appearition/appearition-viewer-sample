
using System.Collections.Generic;
using Appearition.API;
using Appearition.Common;

namespace Appearition.Tenant.API
{
    /// <summary>
    /// https://api.appearition.com/TenantName/api/TenantSettings/List/0 , where 0 is Channel ID 
    /// </summary>
    [System.Serializable]
    public class TenantSettings_List : BaseApiGet
    {
        public override int ApiVersion
        {
            get { return 2; }
        }

        //Variables
        public List<Setting> Data;
    }
}