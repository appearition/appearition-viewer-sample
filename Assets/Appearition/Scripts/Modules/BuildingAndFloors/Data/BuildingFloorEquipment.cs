namespace Appearition.BuildingsAndFloors
{
    [System.Serializable]
    public class BuildingFloorEquipment : BuildingEquipment
    {
        public int buildingId;
        public int buildingFloorId;
        public int buildingFloorEquipmentId;

        public BuildingFloorEquipment()
        {
        }

        public BuildingFloorEquipment(BuildingFloorEquipment cc)
        { 
        }

        public void CopyFrom(BuildingFloorEquipment cc)
        { 
            base.CopyFrom(cc);
            buildingId = cc.buildingId;
            buildingFloorId = cc.buildingFloorId;
            buildingFloorEquipmentId = cc.buildingFloorEquipmentId;
        }
    }
}