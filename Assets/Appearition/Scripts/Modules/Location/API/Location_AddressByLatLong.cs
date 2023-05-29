using Appearition.API;

namespace Appearition.Location.API
{
    /// <summary>
    /// https://api.appearition.com/TenantName/api/Location/AddressByLatLong/0 , where 0 is Channel ID
    /// The post data is a PostFilterQuery object.
    /// </summary>
    [System.Serializable]
    public class Location_AddressByLatLong : BaseApiPost
    {
        public override int ApiVersion => 2;

        //Variables
        public LocationAddress Data;

        //Request Params
        [System.Serializable]
        public class RequestContent : BaseRequestContent
        {
            public double latitude;
            public double longitude;
        }
    }
}