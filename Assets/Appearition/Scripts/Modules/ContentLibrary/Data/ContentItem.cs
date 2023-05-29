using System.Collections.Generic;

namespace Appearition.ContentLibrary
{
    [System.Serializable]
    public class ContentItem
    {
        public string Title;
        public string ShortDescription;
        public string LongDescription;
        
        public string ProviderName;
        public string ContentFormatType;
        public string Key;
        public string Tag;
        public string Text;
        public bool IsPrivate;
        public bool CanBeDeleted;
        public bool CanEditTag;
        public string Metadata;
        
        public string Owner;
        public string OwnerContactDetails;
        public string LegalInfoUrl;
        public string UsageRights;
        
        public string CreatedUtcDate;
        public string CreatedUtcDateStr;
        public System.DateTime CreatedUtcDateTime => AppearitionGate.ConvertStringToDateTime(CreatedUtcDateStr);
        public string ModifiedUtcDate;
        public string ModifiedUtcDateStr;
        public System.DateTime ModifiedUtcDateTime => AppearitionGate.ConvertStringToDateTime(ModifiedUtcDateStr);

        public ContentThumbnail ThumbnailImage;
        public List<ContentFile> Files;
    }
}