using System.Collections;
using System.Collections.Generic;
using Appearition.Common;

namespace Appearition.BuildingsAndFloors
{
    [System.Serializable]
    public class BuildingEquipment
    {
        public int buildingEquipmentId;
        public string name;
        public List<Property> properties;

        public BuildingEquipment()
        {
        }

        public BuildingEquipment(BuildingEquipment cc)
        {
            CopyFrom(cc);
        }

        public virtual void CopyFrom(BuildingEquipment cc)
        {
            buildingEquipmentId = cc.buildingEquipmentId;
            name = cc.name;
            if (cc.properties != null)
                properties = new List<Property>(cc.properties);
        }
    }
}