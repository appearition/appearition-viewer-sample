using Appearition.API;

namespace Appearition.AccountAndAuthentication.API
{
    /// <summary>
    /// https://api.appearition.com/TenantName/api/Registration/Register/0 , where 0 is Channel ID 
    /// </summary>
    [System.Serializable]
    public class Registration_Status : BaseApiGet
    {
        public override AuthenticationOverrideType AuthenticationOverride => AuthenticationOverrideType.SessionToken;
        public override int ApiVersion => 2;
        public override TypeOfApiStorage ResponseSaveType => TypeOfApiStorage.None;

        //Variables
        public ApiData Data;

        [System.Serializable]
        public class ApiData : AccountStatus
        {
        }

        //Request Params
        [System.Serializable]
        public class RequestContent : BaseRequestContent
        {
            public string username;
        }
    }
}