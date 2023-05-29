using Appearition.API;

namespace Appearition.ImageRecognition
{
    /// <summary>
    /// https://api.appearition.com/TenantName/api/ImageRecognition/SyncArImagesWithIrDatastore/0 , where 0 is Channel ID 
    /// </summary>
    [System.Serializable]
    public class ImageRecognition_SyncArImagesWithIrDatastore : BaseApiPost
    {
        public override AuthenticationOverrideType AuthenticationOverride => AuthenticationOverrideType.SessionToken;
        public override int ApiVersion => 2;
        public override TypeOfApiStorage ResponseSaveType => TypeOfApiStorage.None;

        /// <summary>
        /// Post ApiData
        /// </summary>
        [System.Serializable]
        public class PostData
        {
            public int channelId;
            public string provider;
            
            public PostData(DataStore cc)
            {
                channelId = cc.channelId;
                provider = cc.provider;
            }
        }
    }
}