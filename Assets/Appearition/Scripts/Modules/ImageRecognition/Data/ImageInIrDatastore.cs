namespace Appearition.ImageRecognition
{
    [System.Serializable]
    public class ImageInIrDataStore
    {
        public string key;
        public int channelId;
        public string imageThumbnailUrl;
        public string imageUrl;
        public bool isOrphan;
    }
}