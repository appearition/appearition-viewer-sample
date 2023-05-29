namespace Appearition.Packaging
{
    [System.Serializable]
    public class PackageStatus
    {
        public string Message;
        public int PackageId;
        public bool IsWaitingForBilling;
        public bool DoPoolForStatus;
        public bool IsActive;
        public bool HasExistingActivePackage;
        public bool HasFallbackPackageOption;
        public int FallbackPackageId;
    }
}