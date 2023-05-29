#pragma warning disable 0649

namespace Appearition.QAndA
{
    [System.Serializable]
    public class QuestionAnswer : QuestionOption
    {
        [UnityEngine.SerializeField] string answerText;
        public override string QuestionText => answerText;
    }
}