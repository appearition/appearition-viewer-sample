using System.Collections.Generic;
using Appearition.API;

namespace Appearition.QAndA.API
{
    /// <summary>
    /// https://api.appearition.com/TenantName/api/QandA/List/0 , where 0 is Channel ID 
    /// </summary>
    public class QandA_List : BaseApiGet
    {
        public override int ApiVersion => 2;
        public List<Question> Data;
    }
}