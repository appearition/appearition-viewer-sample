namespace Appearition.Packaging
{
    [System.Serializable]
    public class ExtendedModuleSetting
    {
        public int Id;
        public int PackageModuleId;
        public string KeyName;
        public string KeyNameOriginal;
        public string Value;
        public string ValueOriginal;
        public bool IsNew;
        public bool IsEdit;
        public bool IsToBeDeleted;
    }
}