using Appearition.API;

namespace Appearition.AccountAndAuthentication.API
{
    /// <summary>
    /// https://api.appearition.com/TenantName/api/Registration/Register/0 , where 0 is Channel ID 
    /// </summary>
    [System.Serializable]
    public class Registration_Register : BaseApiPost
    {
        public override AuthenticationOverrideType AuthenticationOverride => AuthenticationOverrideType.ApplicationToken;
        public override int ApiVersion => 2;
        public override TypeOfApiStorage ResponseSaveType => TypeOfApiStorage.None;

        //Variables
        public ApiData Data;

        [System.Serializable]
        public class ApiData : AccountStatus
        {
        }

        [System.Serializable]
        public class PostApi : RegistrationForm
        {
        }
    }
}