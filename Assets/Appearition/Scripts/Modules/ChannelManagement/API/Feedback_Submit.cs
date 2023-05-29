using Appearition.API;

namespace Appearition.ChannelManagement.API
{
    /// <summary>
    /// https://api.appearition.com/TenantName/api/Feedback/Submit/0 , where 0 is Channel ID 
    /// </summary>
    [System.Serializable]
    public class Feedback_Submit : BaseApiPost
    {
        public override AuthenticationOverrideType AuthenticationOverride => AuthenticationOverrideType.SessionToken;
        public override TypeOfApiStorage ResponseSaveType => TypeOfApiStorage.None;

        /// <summary>
        /// Post ApiData
        /// </summary>
        public class PostData : FeedbackContent
        {
            //Variables
            public int productId;
            public string os;
            public string device;

            public PostData()
            {
            }

            public PostData(FeedbackContent cc)
            {
                rating = cc.rating;
                comments = cc.comments;
            }
        }
    }
}