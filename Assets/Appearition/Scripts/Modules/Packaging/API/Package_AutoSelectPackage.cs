using Appearition.API;

namespace Appearition.Packaging.API
{
    /// <summary>
    /// https://api.appearition.com/TenantName/api/Package/AutoSelectPackage/0 , where 0 is Channel ID 
    /// </summary>
    [System.Serializable]
    public class Package_AutoSelectPackage : BaseApiPost
    {
        public override AuthenticationOverrideType AuthenticationOverride => AuthenticationOverrideType.SessionToken;
        public override int ApiVersion => 2;
        public override TypeOfApiStorage ResponseSaveType => TypeOfApiStorage.None;

        //Variables
        public ApiData Data;

        [System.Serializable]
        public class ApiData : PackageProcessingStatus
        {
        }
    }
}