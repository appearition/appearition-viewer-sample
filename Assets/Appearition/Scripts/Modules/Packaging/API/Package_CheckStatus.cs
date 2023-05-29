using Appearition.API;

namespace Appearition.Packaging.API
{
    /// <summary>
    /// https://api.appearition.com/TenantName/api/Package/CheckStatus/0 , where 0 is Channel ID 
    /// </summary>
    [System.Serializable]
    public class Package_CheckStatus : BaseApiGet
    {
        public override AuthenticationOverrideType AuthenticationOverride => AuthenticationOverrideType.SessionToken;
        public override int ApiVersion => 2;
        public override TypeOfApiStorage ResponseSaveType => TypeOfApiStorage.None;

        //Variables
        public ApiData Data;

        [System.Serializable]
        public class ApiData : PackageStatusContainer
        {
        }

        //Request Params
        [System.Serializable]
        public class RequestContent : BaseRequestContent
        {
            public int packageId;
        }
    }
}