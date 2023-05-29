namespace Appearition.UserManagement
{
    [System.Serializable]
    public class ExtendedUserActivity : UserActivity
    {
        public string TenantKey;
        public string Username;
        public string FromUtcDate;
        public string ToUtcDate;
        public int Page;
        public int RecordsPerPage;
    }
}