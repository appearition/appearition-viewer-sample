namespace Appearition.Assessments
{
    [System.Serializable]
    public class AssessmentCategoryQuestionType 
    {
        public long questionType;
        public string questionTypeText;
        public int ordinalPosition;
        public int totalQuestions;

        public AssessmentCategoryQuestionType()
        { 
        }
        
        public AssessmentCategoryQuestionType(AssessmentCategoryQuestionType cc)
        {
            questionType = cc.questionType;
            questionTypeText = cc.questionTypeText;
            ordinalPosition = cc.ordinalPosition;
            totalQuestions = cc.totalQuestions;
        }
    }
}