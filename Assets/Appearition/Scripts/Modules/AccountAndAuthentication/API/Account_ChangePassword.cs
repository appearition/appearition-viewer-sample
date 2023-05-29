using Appearition.API;

namespace Appearition.AccountAndAuthentication.API
{
    /// <summary>
    /// https://api.appearition.com/TenantName/api/Account/ChangePassword/0 , where 0 is Channel ID 
    /// </summary>
    [System.Serializable]
    public class Account_ChangePassword : BaseApiPost
    {
        public override AuthenticationOverrideType AuthenticationOverride => AuthenticationOverrideType.SessionToken;
        public override int ApiVersion => 2;
        public override TypeOfApiStorage ResponseSaveType => TypeOfApiStorage.None;

        [System.Serializable]
        public class PostApi
        {
            public string OldPassword;
            public string NewPassword;
            public string ConfirmNewPassword;
        }
    }
}