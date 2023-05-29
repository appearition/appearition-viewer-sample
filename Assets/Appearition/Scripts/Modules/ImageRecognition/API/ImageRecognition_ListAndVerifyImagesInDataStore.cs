using System.Collections;
using System.Collections.Generic;
using Appearition.API;

namespace Appearition.ImageRecognition.API
{
    /// <summary>
    /// https://api.appearition.com/TenantName/api/ImageRecognition/ListAndVerifyImagesInDataStore/0 , where 0 is Channel ID 
    /// </summary>
    [System.Serializable]
    public class ImageRecognition_ListAndVerifyImagesInDataStore : BaseApiPost
    {
        public override AuthenticationOverrideType AuthenticationOverride => AuthenticationOverrideType.ApplicationToken;
        public override int ApiVersion => 2;

        //Variables
        public ApiData Data;

        [System.Serializable]
        public class ApiData
        {
            public List<ImageInIrDataStore> imagesInIrDataStores;
            public List<ImageInArTargets> imagesInArTargets;
        }

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