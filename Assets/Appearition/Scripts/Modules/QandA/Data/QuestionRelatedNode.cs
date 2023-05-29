using System.Collections.Generic;

namespace Appearition.QAndA
{
    [System.Serializable]
    public class QuestionRelatedNode
    {
        public int nodeId;
        public string nodeName;
        public int parentNodeId;
        public int? ParentNodeId => parentNodeId == 0 ? default(int?) : parentNodeId;
        public List<QuestionProperty> questionRelatedNodeProperties;
    }
}