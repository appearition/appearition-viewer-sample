using System.Collections;
using System.Collections.Generic;
using Appearition.API;

namespace Appearition.Promotions.API
{
    /// <summary>
    /// https://api.appearition.com/TenantName/api/Promotions/ListByChannel/0 , where 0 is Channel ID 
    /// </summary>
    [System.Serializable]
    public class Promotions_ListByChannel : BaseApiGet
    {
        [System.Serializable]
        public class ApiData
        {
            public List<Promotion> promotions;
        }

        public ApiData Data;
    }
}