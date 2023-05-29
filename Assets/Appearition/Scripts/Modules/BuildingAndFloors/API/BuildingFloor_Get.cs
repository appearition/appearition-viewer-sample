using Appearition.API;

namespace Appearition.BuildingsAndFloors.API
{
    /// <summary>
    /// https://api.appearition.com/TenantName/api/BuildingFloor/Get/0?buildingId=1&buildingFloorId=2 , where 0 is Channel ID, 1 is the buildingId and 2 is the buildingFloorId
    /// </summary>
    [System.Serializable]
    public class BuildingFloor_Get : BaseApiGet
    {
        public override int ApiVersion => 2;

        public Floor Data;

        //Request Params
        [System.Serializable]
        public class RequestContent : BaseRequestContent
        {
            public int buildingId;
            public int buildingFloorId;
        }
    }
}