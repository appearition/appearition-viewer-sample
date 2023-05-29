using Appearition.API;

namespace Appearition.Analytics.API
{
    /// <summary>
    /// https://api.appearition.com/TenantName/api/Analytics/EndSessionTracking/0 , where 0 is Channel ID 
    /// </summary>
    [System.Serializable]
    public class Analytics_EndSessionTracking : BaseApiPost
    {
        public override int ApiVersion => 2;
        public override TypeOfApiStorage ResponseSaveType => TypeOfApiStorage.None;

        //Variables
        public string Data;

        /// <summary>
        /// Post ApiData
        /// </summary>
        [System.Serializable]
        public class PostData : Session
        {
        }
    }
}