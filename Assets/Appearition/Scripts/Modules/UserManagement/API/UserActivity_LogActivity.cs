using Appearition.API;

namespace Appearition.UserManagement
{
    /// <summary>
    /// https://api.appearition.com/TenantName/api/UserActivity/LogActivity/0 , where 0 is Channel ID 
    /// </summary>
    [System.Serializable]
    public class UserActivity_LogActivity : BaseApiPost
    {
        public override AuthenticationOverrideType AuthenticationOverride
        {
            get { return AuthenticationOverrideType.SessionToken; }
        }

        public override int ApiVersion
        {
            get { return 2; }
        }

        //Variables
        public ApiData Data;

        [System.Serializable]
        public class ApiData
        {
        }

        [System.Serializable]
        public class PostApi : UserActivity
        {
        }
    }
}