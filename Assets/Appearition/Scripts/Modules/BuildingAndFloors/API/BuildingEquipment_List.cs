using System.Collections.Generic;
using Appearition.API;

namespace Appearition.BuildingsAndFloors.API
{
    /// <summary>
    /// https://api.appearition.com/TenantName/api/BuildingEquipment/List/0 , where 0 is Channel ID
    ///
    /// The post data is a PostFilterQuery object.
    /// </summary>
    [System.Serializable]
    public class BuildingEquipment_List : BaseApiPost
    {
        public override int ApiVersion => 2;

        //Variables
        public QueryOutcome Data;

        [System.Serializable]
        public class QueryOutcome
        {
            public List<BuildingEquipment> Equipments;
            public int Page;
            public int RecordsPerPage;
            public int TotalRecords;
        }
    }
}