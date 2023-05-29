using System.Collections.Generic;
using Appearition.Common;

namespace Appearition.BuildingsAndFloors
{
    [System.Serializable]
    public class PostFilterQuery
    {
        public string name;
        public int page;
        public int recordsPerPage;
        public List<Property> properties;

        public PostFilterQuery()
        {
            page = 0;
            recordsPerPage = 10;
            properties = new List<Property>();
        }

        public PostFilterQuery(string nameToQuery, int page, int recordsPerPage, List<Property> properties)
        {
            this.name = nameToQuery;
            this.page = page;
            this.recordsPerPage = recordsPerPage;
            if (properties != null)
                this.properties = new List<Property>(properties);
        }

        public PostFilterQuery(string nameToQuery, int page, int recordsPerPage, Dictionary<string, string> properties)
        {
            this.name = nameToQuery;
            this.page = page;
            this.recordsPerPage = recordsPerPage;
            if (properties != null)
            {
                this.properties = new List<Property>();
                foreach (var kvp in properties)
                    this.properties.Add(new Property(kvp));
            }
        }

        public override string ToString()
        {
            string outcome = $"name:{name}, records per page: {recordsPerPage}, page: {page}.\nProperties:";

            if (properties == null || properties.Count == 0)
                outcome += " none.";
            else
            {
                foreach (var tmpProperty in properties)
                    outcome += $"\nKey:{tmpProperty.propertyKey} - Value:{tmpProperty.propertyValue}";
            }

            return outcome;
        }
    }
}