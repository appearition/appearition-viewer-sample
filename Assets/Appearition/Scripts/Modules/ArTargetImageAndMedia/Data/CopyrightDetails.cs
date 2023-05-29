namespace Appearition.ArTargetImageAndMedia
{
    [System.Serializable]
    public class CopyrightDetails
    {
        public string title;
        public string owner;
        public string ownerContactDetails;
        public string legalInfoUrl;
        public string usageRights;
        public string createdUtcDate;
        public string createdUtcDateStr;
        public System.DateTime CreatedUtcDate => AppearitionGate.ConvertStringToDateTime(createdUtcDateStr);
        public string formattedOutput;
    }
}