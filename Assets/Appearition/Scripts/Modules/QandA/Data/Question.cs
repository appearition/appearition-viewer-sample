using System.Collections.Generic;

namespace Appearition.QAndA
{
    [System.Serializable]
    public class Question
    {
        public long questionId;
        public int productId;
        public int questionTypeId;
        public string questionText;

        public int randomAnswerTypeId;
        public string randomAnswerTypeText;
        public int randomIncorrectAnswerTypeId;
        public string randomIncorrectAnswerTypeText;
        public int maximumNumberOfIncorrectOptions;
        public int relatedPartTypeId;
        public string relatedPartTypeName;

        public List<QuestionAnswer> questionAnswers;
        public List<QuestionIncorrectOption> questionIncorrectOptions;
        
        public List<QuestionProperty> questionProperties;
        public List<QuestionCategory> questionCategories;
        public List<QuestionProficiencyLevel> questionProficiencyLevels;
        public List<QuestionRelatedNode> questionRelatedNodes;
        
        public List<QuestionAttemptScore> questionAttemptScores;
    }
}