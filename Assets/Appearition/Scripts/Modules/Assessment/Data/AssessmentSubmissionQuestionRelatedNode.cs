using System.Collections.Generic;
using Appearition.Learn;

namespace Appearition.Assessments
{
    [System.Serializable]
    public class AssessmentSubmissionQuestionRelatedNode
    {
        /// <summary>
        /// The id of a Learn Node related to this question.
        /// </summary>
        public int nodeId;

        //Runtime parameters to help with displaying
        [System.NonSerialized] public string nodeName;
        [System.NonSerialized] public bool isHighlighted;
        [System.NonSerialized] public bool isTransparent;

        public AssessmentSubmissionQuestionRelatedNode()
        {
        }
    }
}