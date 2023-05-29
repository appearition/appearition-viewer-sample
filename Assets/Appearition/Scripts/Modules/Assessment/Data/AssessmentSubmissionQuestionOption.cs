using Appearition.QAndA;

namespace Appearition.Assessments
{
    /// <summary>
    /// Single option as shown to the student.
    /// </summary>
    [System.Serializable]
    public class AssessmentSubmissionQuestionOption
    {
        /// <summary>
        /// The node related to this option.
        /// </summary>
        public int nodeId;
        /// <summary>
        /// The text shown to the user for this option.
        /// </summary>
        public string optionText;
        /// <summary>
        /// Whether or not this option is the correct answer.
        /// </summary>
        public bool isCorrect;

        //Runtime Variables
        /// <summary>
        /// The name of the node
        /// </summary>
        [System.NonSerialized]public string nodeName;
        /// <summary>
        /// The position of the question within a list
        /// </summary>
        [System.NonSerialized] public int ordinalPosition;
        /// <summary>
        /// The message to show to the user upon attempting
        /// </summary>
        [System.NonSerialized] public string messageToShow;

        /// <summary>
        /// Whether or not this question option has a node attached to it.
        /// </summary>
        public bool ContainsNodeInfo => nodeId != 0;

        public AssessmentSubmissionQuestionOption()
        {
        }
    }
}