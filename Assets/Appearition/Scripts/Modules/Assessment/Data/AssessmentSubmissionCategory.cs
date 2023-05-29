using System.Collections.Generic;

namespace Appearition.Assessments
{
    /// <summary>
    /// Data for a single assessment category.
    /// </summary>
    [System.Serializable]
    public class AssessmentSubmissionCategory
    {
        public int categoryId;
        [System.NonSerialized] public string categoryName;
        public int proficiencyLevelId;
        [System.NonSerialized] public string proficiencyLevelName;

        public int maxAchievableScore;
        public int achievedScore;

        /// <summary>
        /// Contains all the data for each question answered by the user within this category.
        /// </summary>
        public List<AssessmentSubmissionQuestion> studentAssessmentQuestions;

        public AssessmentSubmissionCategory()
        {
        }
    }
}