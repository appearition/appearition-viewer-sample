using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Appearition.Assessments
{
    /// <summary>
    /// Runtime data for an assessment, which doubles as the data for assessment submission.
    /// </summary>
    [System.Serializable]
    public class AssessmentSubmissionData
    {
        public int productId;
        public string studentUsername;
        public int assessmentId;
        public string assessmentName;
        public int targetProficiencyLevelId;
        public string targetProficiencyLevelName;

        public string startDateTime;
        public string endDateTime;

        public int maxAchievableScoreValue;
        public int achievedScoreValue;
        public float achievedScorePercentage;
        public bool hasPassed;
        public string messageShown;

        public List<AssessmentSubmissionCategory> studentAssessmentCategories;

        /// <summary>
        /// Scores achieved by the user for quick access.
        /// </summary>
        public List<AssessmentScore> assessmentScores;


        #region Runtime Utilities

        Assessment _assessmentData;

        /// <summary>
        /// Fetches the minimum score required in order to get a passing grade for this assessment.
        /// </summary>
        public float GetMinPassingScore
        {
            get
            {
                float outcome = 100;

                if (assessmentScores == null || assessmentScores.Count == 0)
                    outcome = 50;
                else
                {
                    for (int i = 0; i < assessmentScores.Count; i++)
                    {
                        if (assessmentScores[i].minScore < outcome && assessmentScores[i].isAPass)
                            outcome = assessmentScores[i].minScore;
                    }
                }

                return outcome;
            }
        }

        #region Initialization

        public AssessmentSubmissionData(Assessment assessment)
        {
            _assessmentData = assessment;
        }

        public AssessmentSubmissionData(Assessment assessment, List<AssessmentSubmissionCategory> processedCategories)
        {
            //Main assessment info
            _assessmentData = assessment;
            productId = assessment.productId;
            assessmentId = assessment.assessmentId;
            assessmentName = assessment.assessmentName;

            studentAssessmentCategories = new List<AssessmentSubmissionCategory>(processedCategories);
        }

        #endregion

        /// <summary>
        /// Once initialized, call this method to register the start of this assessment. 
        /// </summary>
        /// <param name="username"></param>
        public void BeginAssessment(string username)
        {
            studentUsername = username;
            startDateTime = AppearitionGate.GetCurrentDateAndTime();
        }

        /// <summary>
        /// Once all categories are complete, call this method to get the final score and complete the data required for submission.
        /// </summary>
        /// <returns></returns>
        public AssessmentScore CompleteAssessment()
        {
            //Write end time
            endDateTime = AppearitionGate.GetCurrentDateAndTime();

            int totalScore = 0;

            //Apply the scores to all categories
            for (int i = 0; i < studentAssessmentCategories.Count; i++)
            {
                int currentScore = 0;

                for (int k = 0; k < studentAssessmentCategories[i].studentAssessmentQuestions.Count; k++)
                {
                    currentScore += studentAssessmentCategories[i].studentAssessmentQuestions[k].achievedScore;
                }

                studentAssessmentCategories[i].achievedScore = currentScore;
                totalScore += currentScore;
            }

            achievedScoreValue = totalScore;
            achievedScorePercentage = ((float) achievedScoreValue) / ((float) maxAchievableScoreValue) * 100f;

            //EDIT: Use percentages.
            //AssessmentScore score = _assessmentData.assessmentScores.FirstOrDefault(o => o.minScore >= totalScore && o.maxScore <= totalScore);
            AssessmentScore score = _assessmentData.assessmentScores.FirstOrDefault(o => achievedScorePercentage >= (float)o.minScore && achievedScorePercentage <= (float)o.maxScore);
            hasPassed = score == null || score.isAPass;
            messageShown = score?.message;

            if (score == null)
                AppearitionLogger.LogError($"No score was found for the assessment of id {assessmentId} and name {assessmentName} with a total score of {totalScore}.");
            return score;
        }

        #endregion
    }
}