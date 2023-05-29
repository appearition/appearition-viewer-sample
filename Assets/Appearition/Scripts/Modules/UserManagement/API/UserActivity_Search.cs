using Appearition.API;

namespace Appearition.UserManagement
{
    /// <summary>
    /// https://api.appearition.com/TenantName/api/UserActivity/Search/0 , where 0 is Channel ID 
    /// </summary>
    [System.Serializable]
    public class UserActivity_Search : BaseApiPost
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
        public class ApiData : ExtendedUserActivity
        {
        }

        [System.Serializable]
        public class PostApi : ExtendedUserActivity
        {
        }
    }
}