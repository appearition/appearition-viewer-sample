using System.Collections.Generic;
using System.Linq;
using Appearition.Common;
using Appearition.Common.ListExtensions;

namespace Appearition.BuildingsAndFloors
{
    [System.Serializable]
    public class Floor
    {
        //Consts
        const string JSON_FLOOR_PLAN_PROPERTY_KEY = "JsonFloorPlan";

        //JSON Variables
        public int buildingId;
        public int buildingFloorId;
        public string name;
        public List<Property> properties;

        public Floor()
        {
        }

        public Floor(Floor cc)
        {
            CopyFrom(cc);
        }

        public void CopyFrom(Floor cc)
        {
            buildingId = cc.buildingId;
            buildingFloorId = cc.buildingFloorId;
            name = cc.name;
            if (cc.properties != null)
                properties = new List<Property>(cc.properties);
        }

        /// <summary>
        /// Fetches a working copy of the current Floor Plan data (containing walls and other info) used by the Coverage handler.
        /// </summary>
        /// <returns></returns>
        public FloorPlan GetFloorPlanData()
        {
            if (properties == null)
                return new FloorPlan();

            return properties.GetPropertyWithKey(JSON_FLOOR_PLAN_PROPERTY_KEY, AppearitionConstants.DeserializeJson<FloorPlan>);
        }

        /// <summary>
        /// Update the the Floor Plan data (containing walls and other info) for the Coverage to analyze at later date.
        /// </summary>
        /// <param name="data"></param>
        public void SetFloorPlanData(FloorPlan data)
        {
            if (properties == null)
                properties = new List<Property>();
            
            properties.SetPropertyWithKey(JSON_FLOOR_PLAN_PROPERTY_KEY, AppearitionConstants.SerializeJson(data));
        }
    }
}