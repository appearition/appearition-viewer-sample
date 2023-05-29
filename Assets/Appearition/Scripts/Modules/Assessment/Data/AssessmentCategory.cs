using System.Collections.Generic;

namespace Appearition.Assessments
{
    [System.Serializable]
    public class AssessmentCategory
    {
        public int categoryId;
        public string category;
        public int ordinalPosition;
        
        public int proficiencyLevelId;
        public string proficiencyLevelName;
        public int questionTypeRandomisationTypeId;
        public string questionTypeRandomisationTypeText;
        public List<AssessmentCategoryQuestionType> assessmentCategoryQuestionTypes = new List<AssessmentCategoryQuestionType>();

        public int totalQuestions;

        public AssessmentCategory()
        { 
        }

        public AssessmentCategory(AssessmentCategory cc)
        {
            categoryId = cc.categoryId;
            category = cc.category;
            ordinalPosition = cc.ordinalPosition;
            
            proficiencyLevelId = cc.proficiencyLevelId;
            proficiencyLevelName = cc.proficiencyLevelName;
            questionTypeRandomisationTypeId = cc.questionTypeRandomisationTypeId;
            questionTypeRandomisationTypeText = cc.questionTypeRandomisationTypeText;

            assessmentCategoryQuestionTypes = new List<AssessmentCategoryQuestionType>();
            for(int i = 0; i < cc.assessmentCategoryQuestionTypes.Count; i++)
                assessmentCategoryQuestionTypes.Add(new AssessmentCategoryQuestionType(cc.assessmentCategoryQuestionTypes[i]));

            totalQuestions = cc.totalQuestions;
        }
    }
}