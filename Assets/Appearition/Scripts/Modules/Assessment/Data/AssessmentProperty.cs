namespace Appearition.Assessments
{
    [System.Serializable]
    public class AssessmentProperty
    {
        public string propertyKey;
        public string propertyValue;
        public int fieldType;
        public string fieldTypeText;
        public int isReadOnly;
        public int ordinalPosition;

        public AssessmentProperty()
        { 
        }

        public AssessmentProperty(AssessmentProperty cc)
        {
            propertyKey = cc.propertyKey;
            propertyValue = cc.propertyValue;
            fieldType = cc.fieldType;
            fieldTypeText = cc.fieldTypeText;
            isReadOnly = cc.isReadOnly;
            ordinalPosition = cc.ordinalPosition;
        }
    }
}