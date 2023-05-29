namespace Appearition.Packaging
{
    [System.Serializable]
    public class BasePackage
    {
        public int Id;
        public string Name;
        public string PackageKey;
        public string Description;
        public bool NeedsBilling;
        public int Ordinal;
    }
}