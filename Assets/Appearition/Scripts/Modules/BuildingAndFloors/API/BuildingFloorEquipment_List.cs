using System.Collections.Generic;
using Appearition.API;

namespace Appearition.BuildingsAndFloors.API
{
    /// <summary>
    /// https://api.appearition.com/TenantName/api/BuildingFloorEquipment/List/0?buildingId=1&buildingFloorId=2 , where 0 is Channel ID, 1 is the buildingId and 2 is the buildingFloorId
    ///
    /// The post data is a PostFilterQuery object.
    /// </summary>
    [System.Serializable]
    public class BuildingFloorEquipment_List : BaseApiPost
    {
        public override int ApiVersion => 2;

        //Variables
        public List<BuildingFloorEquipment> Data;

        //Request Params
        [System.Serializable]
        public class RequestContent : BaseRequestContent
        {
            public int buildingId;
            public int buildingFloorId;
        }
    }
}