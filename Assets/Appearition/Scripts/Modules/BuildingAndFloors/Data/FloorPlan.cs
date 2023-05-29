using System.Collections.Generic;

namespace Appearition.BuildingsAndFloors
{
    [System.Serializable]
    public class FloorPlan
    {
        public string name;
        public List<FloorPlanMaterial> materials;
        public List<FloorPlanStructure> structures;
    }
}