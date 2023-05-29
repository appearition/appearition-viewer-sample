using System;
using System.Collections.Generic;
using System.Linq;
using Appearition.QAndA;

namespace Appearition.Assessments
{
    /// <summary>
    /// Data for a single question as shown and solved by the student.
    /// </summary>
    [System.Serializable]
    public class AssessmentSubmissionQuestion
    {
        public long questionId;
        public long questionTypeId;
        public string questionTypeText;
        public string questionText;
        public int ordinalPosition;

        public int questionScore;
        public int achievedScore;
        public int totalAttempts;

        public List<AssessmentSubmissionQuestionRelatedNode> studentAssessmentQuestionRelatedNodes;
        public List<AssessmentSubmissionQuestionAttempt> studentAssessmentQuestionAttempts;
        public List<AssessmentSubmissionQuestionOption> studentAssessmentQuestionOptions;
        
        /// <summary>
        /// Question properties passed on from the question template.
        /// </summary>
        public List<QuestionProperty> questionProperties = new List<QuestionProperty>();

        /// <summary>
        /// If the question contains a MediaKey attached in the Question Properties, will safely retrieve it.
        /// </summary>
        public string MediaKey => questionProperties?.FirstOrDefault(o => o.propertyKey.Equals("MediaKey", StringComparison.InvariantCultureIgnoreCase))?.propertyValue;

        /// <summary>
        /// Used to store attempts runtime
        /// </summary>
        [System.NonSerialized] public List<QuestionAttemptScore> questionAttemptScores;

        public AssessmentSubmissionQuestion()
        {
            studentAssessmentQuestionRelatedNodes = new List<AssessmentSubmissionQuestionRelatedNode>();
            studentAssessmentQuestionAttempts = new List<AssessmentSubmissionQuestionAttempt>();
            studentAssessmentQuestionOptions = new List<AssessmentSubmissionQuestionOption>();
        }
    }
}