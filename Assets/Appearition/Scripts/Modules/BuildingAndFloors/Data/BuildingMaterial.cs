using System.Collections.Generic;
using Appearition.Common;

namespace Appearition.BuildingsAndFloors
{
    [System.Serializable]
    public class BuildingMaterial
    {
        public int buildingMaterialId;
        public string name;
        public List<Property> properties;

        public BuildingMaterial()
        {
        }

        public BuildingMaterial(BuildingMaterial cc)
        {
            CopyFrom(cc);
        }

        public void CopyFrom(BuildingMaterial cc)
        {
            buildingMaterialId = cc.buildingMaterialId;
            name = cc.name;
            if (cc.properties != null)
                properties = new List<Property>(cc.properties);
        }
    }
}