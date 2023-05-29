using Appearition.API;
using System.Collections.Generic;

namespace Appearition.Packaging.API
{
    /// <summary>
    /// https://api.appearition.com/TenantName/api/ManagePackage/UpdateModuleSettings/0 , where 0 is Channel ID 
    /// </summary>
    [System.Serializable]
    public class ManagePackage_UpdateModuleSettings : BaseApiPost
    {
        public override AuthenticationOverrideType AuthenticationOverride => AuthenticationOverrideType.SessionToken;
        public override int ApiVersion => 2;
        public override TypeOfApiStorage ResponseSaveType => TypeOfApiStorage.None;

        //Variables
        public ApiData Data;

        [System.Serializable]
        public class ApiData : ExtendedModule
        {
        }

        [System.Serializable]
        public class PostApi : ExtendedModule
        {
        }
    }
}