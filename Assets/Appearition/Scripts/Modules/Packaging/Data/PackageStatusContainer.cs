namespace Appearition.Packaging
{
    [System.Serializable]
    public class PackageStatusContainer
    {
        public PackageStatus[] Packages;
        public float StatusCheckIntervalInSeconds;
        public float TooLongThresholdInSeconds;
        public string Message;
    }
}