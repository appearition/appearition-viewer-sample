using System.Collections.Generic;
    
namespace Appearition.Packaging
{
    [System.Serializable]
    public class ExpiryPeriodType : PackageParameter
    {
        [System.Serializable]
        public class ExpiryPeriodGroup
        {
            public string Name;
            public bool Disabled;
        }
        
        public bool Disabled;
        public bool Selected;
        public ExpiryPeriodGroup Group;
    }
}