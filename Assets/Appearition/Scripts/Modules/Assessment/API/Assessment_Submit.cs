using Appearition.API;

namespace Appearition.Assessments.API
{
    /// <summary>
    /// https://api.appearition.com/TenantName/api/Assessment/Submit/0 , where 0 is Channel ID 
    /// </summary>
    [System.Serializable]
    public class Assessment_Submit : BaseApiPost
    {
        public override AuthenticationOverrideType AuthenticationOverride => AuthenticationOverrideType.SessionToken;
        public override int ApiVersion => 2;
        public override TypeOfApiStorage ResponseSaveType => TypeOfApiStorage.None;
    }
}