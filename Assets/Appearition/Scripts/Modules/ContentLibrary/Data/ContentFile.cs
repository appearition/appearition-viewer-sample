namespace Appearition.ContentLibrary
{
    [System.Serializable]
    public class ContentFile
    {
        public string Url;
        public string Metadata;
        public string MimeType;
        public string SubFolder;
        public string FileName;
        public string Checksum;
        public long FileSizeBytes;
        public long FileSizeKBytes;
        public long FileSizeMBytes;
    }
}