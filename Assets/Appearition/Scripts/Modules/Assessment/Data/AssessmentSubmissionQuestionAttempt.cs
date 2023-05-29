namespace Appearition.Assessments
{
    /// <summary>
    /// Single answer attempt made by the student.
    /// </summary>
    [System.Serializable]
    public class AssessmentSubmissionQuestionAttempt
    {
        /// <summary>
        /// Is this the first attempt? Second attempt? Starts at 0.
        /// </summary>
        public int attemptNo;
        /// <summary>
        /// Node associated to the option.
        /// </summary>
        public int nodeId;
        /// <summary>
        /// The option text provided to the student.
        /// </summary>
        public string answerText;
        /// <summary>
        /// Score achieved the student should achieve if they answered correctly.
        /// </summary>
        public int achievedScore;
        /// <summary>
        /// Whether this attempt was the correct response.
        /// </summary>
        public bool isCorrect;
        /// <summary>
        /// Message shown to the student upon submitting this attempt.
        /// </summary>
        public string messageShown;

        public AssessmentSubmissionQuestionAttempt()
        { 
        }

        public AssessmentSubmissionQuestionAttempt(AssessmentSubmissionQuestionOption option, int attempt, int score, string message)
        {
            nodeId = option.nodeId;
            answerText = option.optionText;
            isCorrect = option.isCorrect;
            attemptNo = attempt;
            achievedScore = score;
            messageShown = message;
        }
    }
}