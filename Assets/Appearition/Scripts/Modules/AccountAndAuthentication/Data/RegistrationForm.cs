using System.Collections.Generic;
using Appearition.Common;

namespace Appearition.AccountAndAuthentication
{
    /// <summary>
    /// Form used when trying to register a new account to the EMS, either with OAuth or email.
    /// </summary>
    [System.Serializable]
    public class RegistrationForm 
    {
        public bool IsOAuth;
        public string OAuthProvider;
        public string EmailAddress;
        public string FirstName;
        public string LastName;
        public string Mobile;
        public string BusinessName;
        public string Country;
        public string Password;
        public string ConfirmPassword;
        public bool AcceptTandC;
        public int ClientTimezoneOffset;
        public string ClientTimezoneName;
        public string SelectedPlan;
        public List<Parameter> OtherProperties;
    }
}