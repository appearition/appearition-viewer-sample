using Appearition.API;

namespace Appearition.BuildingsAndFloors.API
{
    /// <summary>
    /// https://api.appearition.com/TenantName/api/Building/Get/0?buildingId=1 , where 0 is Channel ID and 1 is the buildingId
    /// </summary>
    [System.Serializable]
    public class Building_Get : BaseApiGet
    {
        public override int ApiVersion => 2;

        public Building Data;

        //Request Params
        [System.Serializable]
        public class RequestContent : BaseRequestContent
        {
            public int buildingId;
        }
    }
}
