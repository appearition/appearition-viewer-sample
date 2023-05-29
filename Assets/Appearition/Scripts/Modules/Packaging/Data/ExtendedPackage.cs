using System.Collections.Generic;

namespace Appearition.Packaging
{
    [System.Serializable]
    public class ExtendedPackage : BasePackage
    {
        public int ProductId;
        public string RoleToAllocate;
        public string AllowedDomainNames;
        public bool IsNew;
        public bool IsPublished;
        public bool IsPublishedOriginal;
        public int ExpiryPeriodLength;
        public bool ExpireAtEndOfDay;
        public string ExpiryPeriodType;
        public bool CreateNewChannel;
        public string DefaultChannelName;
        public int FallbackPackageId;
        public List<ExtendedModule> Modules;
    }
}