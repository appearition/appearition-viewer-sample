using System.Collections.Generic;

namespace Appearition.Packaging
{
    [System.Serializable]
    public class ExtendedModule : BaseModule
    {
        public int PackageId;
        public int ModuleId;
        public bool IsToBeDeleted;
        public bool IsNew;
        public List<ExtendedModuleSetting> Settings;
    }
}