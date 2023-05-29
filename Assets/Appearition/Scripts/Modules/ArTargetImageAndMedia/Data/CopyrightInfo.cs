using System.Collections.Generic;

namespace Appearition.ArTargetImageAndMedia
{
    [System.Serializable]
    public class CopyrightInfo
    {
        public string createdByUsername;
        public string createdUtcDate;
        public string createdUtcDateStr;
        public System.DateTime CreatedUtcDate => AppearitionGate.ConvertStringToDateTime(createdUtcDateStr);
        public string copyrightInfo;
        /// <summary>
        /// Full copyright info, including rich text.
        /// </summary>
        public string displayCopyrightInfo;
        public List<CopyrightDetails> contentCopyright;
    }
}