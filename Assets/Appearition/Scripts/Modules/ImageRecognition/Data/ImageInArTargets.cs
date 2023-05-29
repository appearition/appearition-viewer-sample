using System.Collections.Generic;

namespace Appearition.ImageRecognition
{
    [System.Serializable]
    public class ImageInArTargets
    {
        [System.Serializable]
        public class ImageData {
            public int arImageId;
            public string arKey;
            public string imageUrl;
            public bool isMissingFromIrDataStore;
        }
        
        public int arTargetId;
        public string arTargetKey;
        public string arTargetName;
        public int productId;
        public string productName;
        public List<ImageData> images;
    }
}