namespace Appearition.ContentLibrary
{
    [System.Serializable]
    public class ContentProvider
    {
        public string ProviderName;
        public string ProviderDisplayName;
        public int Ordinal;
        public bool SupportsUploadingNewContentItem;
        public bool SupportsModifyingContentItem;
        public bool SupportsDeletingContentItem;
    }
}