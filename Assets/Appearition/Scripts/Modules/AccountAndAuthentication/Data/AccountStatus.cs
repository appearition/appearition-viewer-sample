namespace Appearition.AccountAndAuthentication
{
    [System.Serializable]
    public class AccountStatus
    {
        public bool IsAccountVerified;
        public bool IsAccountLocked;
        public string SessionToken;
    }
}