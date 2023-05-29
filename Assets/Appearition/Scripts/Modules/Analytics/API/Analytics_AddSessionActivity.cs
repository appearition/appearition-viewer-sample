using Appearition.API;

namespace Appearition.Analytics.API
{
    /// <summary>
    /// https://api.appearition.com/TenantName/api/Analytics/AddSessionActivity/0 , where 0 is Channel ID 
    /// </summary>
    [System.Serializable]
    public class Analytics_AddSessionActivity : BaseApiPost
    {
        public override int ApiVersion => 2;
        public override TypeOfApiStorage ResponseSaveType => TypeOfApiStorage.None;

        //Variables
        public string Data;

        /// <summary>
        /// Post ApiData
        /// </summary>
        [System.Serializable]
        public class PostData : Activity
        {
        }
    }
}
