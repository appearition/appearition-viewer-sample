namespace Appearition.Assessments
{
    [System.Serializable]
    public class AssessmentScore
    {
        public string name;
        public string message;
        public int minScore;
        public int maxScore;
        public bool isAPass;

        public AssessmentScore()
        {
        }

        public AssessmentScore(AssessmentScore cc)
        {
            name = cc.name;
            message = cc.message;
            minScore = cc.minScore;
            maxScore = cc.maxScore;
            isAPass = cc.isAPass;
        }
    }
}