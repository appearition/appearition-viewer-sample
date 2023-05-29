using System.Collections.Generic;
using Appearition.API;

namespace Appearition.Assessments.API
{
    /// <summary>
    /// https://api.appearition.com/TenantName/api/Assessment/List/0 , where 0 is Channel ID 
    /// </summary>
    [System.Serializable]
    public class Assessment_List : BaseApiGet
    {
        public override int ApiVersion => 2;
        public List<Assessment> Data;
    }
}