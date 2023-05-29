using System.Collections.Generic;
using Appearition.API;

namespace Appearition.BuildingsAndFloors.API
{
    /// <summary>
    /// https://api.appearition.com/TenantName/api/BuildingMedia/List/0?buildingId=1, where 0 is Channel ID, 1 is the buildingId
    /// </summary>
    [System.Serializable]
    public class BuildingMedia_List : BaseApiGet
    {
        public override int ApiVersion => 2;

        public List<BuildingMedia> Data;

        //Request Params
        [System.Serializable]
        public class RequestContent : BaseRequestContent
        {
            public int buildingId;
        }
    }
}