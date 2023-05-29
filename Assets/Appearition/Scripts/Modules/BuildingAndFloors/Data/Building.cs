using System.Collections.Generic;
using Appearition.Common;

namespace Appearition.BuildingsAndFloors
{
    [System.Serializable]
    public class Building
    {
        public int buildingId;
        public string name;
        public List<Property> properties;

        public Building()
        {
        }

        public Building(Building cc)
        {
           CopyFrom(cc);
        }

        public void CopyFrom(Building cc)
        {
            buildingId = cc.buildingId;
            name = cc.name;
            if (cc.properties != null)
                properties = new List<Property>(cc.properties);
        }
    }
}