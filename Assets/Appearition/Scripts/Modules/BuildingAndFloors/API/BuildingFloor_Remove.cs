using System.Collections;
using System.Collections.Generic;
using Appearition.API;

namespace Appearition.BuildingsAndFloors.API
{
    /// <summary>
    /// https://api.appearition.com/TenantName/api/BuildingFloor/Remove/0?buildingId=1&buildingFloorId=2 , where 0 is Channel ID, 1 is the buildingId and 2 is the buildingFloorId
    /// </summary>
    [System.Serializable]
    public class BuildingFloor_Remove : BaseApiPost
    {
        public override int ApiVersion => 2;
        public override TypeOfApiStorage ResponseSaveType => TypeOfApiStorage.None;

        //Request Params
        [System.Serializable]
        public class RequestContent : BaseRequestContent
        {
            public int buildingId;
            public int buildingFloorId;
        }
    }
}