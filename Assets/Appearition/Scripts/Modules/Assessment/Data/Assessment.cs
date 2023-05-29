using System;
using System.Collections;
using System.Collections.Generic;
using Appearition.Common.ListExtensions;

namespace Appearition.Assessments
{
    /// <summary>
    /// Unprocessed assessment template.
    /// </summary>
    [System.Serializable]
    public class Assessment
    {
        public int productId;
        public int assessmentId;
        public string assessmentName;

        public int categoryRandomisationTypeId;
        public string categoryRandomisationTypeText;
        public int targetProficiencyLevelId;
        public string targetProficiencyLevel;

        public List<AssessmentCategory> assessmentCategories = new List<AssessmentCategory>();
        public List<AssessmentProperty> assessmentProperties = new List<AssessmentProperty>();
        public List<AssessmentScore> assessmentScores = new List<AssessmentScore>();

        public int totalQuestions;

        public Assessment()
        {
        }

        public Assessment(Assessment cc)
        {
            productId = cc.productId;
            assessmentId = cc.assessmentId;
            assessmentName = cc.assessmentName;

            categoryRandomisationTypeId = cc.categoryRandomisationTypeId;
            categoryRandomisationTypeText = cc.categoryRandomisationTypeText;
            targetProficiencyLevelId = cc.targetProficiencyLevelId;
            targetProficiencyLevel = cc.targetProficiencyLevel;

            assessmentCategories = new List<AssessmentCategory>();
            for (int i = 0; i < cc.assessmentCategories.Count; i++)
                assessmentCategories.Add(new AssessmentCategory(cc.assessmentCategories[i]));

            assessmentProperties = new List<AssessmentProperty>();
            for (int i = 0; i < cc.assessmentProperties.Count; i++)
                assessmentProperties.Add(new AssessmentProperty(cc.assessmentProperties[i]));

            assessmentScores = new List<AssessmentScore>();
            for (int i = 0; i < cc.assessmentScores.Count; i++)
                assessmentScores.Add(new AssessmentScore(cc.assessmentScores[i]));
        }
    }
}