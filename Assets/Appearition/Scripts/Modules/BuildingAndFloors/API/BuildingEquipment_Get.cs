using Appearition.API;

namespace Appearition.BuildingsAndFloors.API
{
    /// <summary>
    /// https://api.appearition.com/TenantName/api/BuildingEquipment/Get/0?buildingEquipmentId=1 , where 0 is Channel ID and 1 is the buildingId
    /// </summary>
    [System.Serializable]
    public class BuildingEquipment_Get : BaseApiGet
    {
        public override int ApiVersion => 2;

        public BuildingEquipment Data;

        //Request Params
        [System.Serializable]
        public class RequestContent : BaseRequestContent
        {
            public int buildingEquipmentId;
        }
    }
}
