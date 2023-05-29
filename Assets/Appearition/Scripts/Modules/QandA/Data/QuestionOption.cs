namespace Appearition.QAndA
{
    [System.Serializable]
    public abstract class QuestionOption
    {
        public int nodeId;
        public string nodeName;
        public virtual string QuestionText => optionText;
        public string optionText;
        [UnityEngine.SerializeField] int parentNodeId;

        public int? ParentNodeId
        {
            get => parentNodeId == 0 ? default(int?) : parentNodeId;
            set => parentNodeId = value.GetValueOrDefault();
        }

        public int ordinalPosition;
    }
}