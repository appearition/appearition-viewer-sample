using System.Collections.Generic;
using Appearition.API;

namespace Appearition.Packaging.API
{
    /// <summary>
    /// https://api.appearition.com/TenantName/api/ManagePackage/Packages/0 , where 0 is Channel ID 
    /// </summary>
    [System.Serializable]
    public class ManagePackage_Packages : BaseApiGet
    {
        public override AuthenticationOverrideType AuthenticationOverride => AuthenticationOverrideType.ApplicationToken;
        public override int ApiVersion => 2;
        public override TypeOfApiStorage ResponseSaveType => TypeOfApiStorage.None;


        //Variables
        public ApiData Data;

        [System.Serializable]
        public class ApiData 
        {
            public List<PackageParameter> Roles;
            public List<PackageParameter> AllPackages;
            public List<ExtendedPackage> Packages;
            public List<BasicModule> AvailableModules;
            public List<ExpiryPeriodType> ExpiryPeriodTypeList;
            public ExtendedPackage Package;
        }
    }
}