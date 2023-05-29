namespace Appearition.ArTargetImageAndMedia
{
    [System.Serializable]
    public class DataTransform
    {
        public string ProviderName;
        public string TemplateText;

        public DataTransform()
        {
        }

        public DataTransform(DataTransform cc)
        {
            ProviderName = cc.ProviderName;
            TemplateText = cc.TemplateText;
        }
    }
}