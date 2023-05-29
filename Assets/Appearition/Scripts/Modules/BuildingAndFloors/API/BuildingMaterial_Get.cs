using Appearition.API;

namespace Appearition.BuildingsAndFloors.API
{
    /// <summary>
    /// https://api.appearition.com/TenantName/api/BuildingMaterial/Get/0?buildingMaterialId=1 , where 0 is Channel ID and 1 is the buildingMaterialId
    /// </summary>
    [System.Serializable]
    public class BuildingMaterial_Get : BaseApiGet
    {
        public override int ApiVersion => 2;

        public BuildingMaterial Data;

        //Request Params
        [System.Serializable]
        public class RequestContent : BaseRequestContent
        {
            public int buildingMaterialId;
        }
    }
}
