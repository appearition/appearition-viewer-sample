using System.Collections.Generic;
using Appearition.API;

namespace Appearition.BuildingsAndFloors.API
{
    /// <summary>
    /// https://api.appearition.com/TenantName/api/BuildingMedia/Remove/0?buildingId=1&buildingMediaId=2, where 0 is Channel ID, 1 is the buildingId and 2 is the buildingMediaId
    /// </summary>
    [System.Serializable]
    public class BuildingMedia_Remove : BaseApiPost
    {
        public override int ApiVersion => 2;

        public string Data;

        //Request Params
        [System.Serializable]
        public class RequestContent : BaseRequestContent
        {
            public int buildingId;
            public int buildingMediaId;
        }
    }
}