namespace Appearition
{
    public static partial class UserRoleConstants
    {
        const string ACCOUNT_PASSWORD_RESET = "AccountPasswordReset";
        const string ACCOUNT_REGISTRATION = "AccountRegistration";

        public static bool HasPermissionToResetPassword => IsAdmin || AppearitionGate.Instance.CurrentUser.ContainsRoleForSelectedTenant(ACCOUNT_PASSWORD_RESET);
        public static bool HasPermissionToRegister => IsAdmin || AppearitionGate.Instance.CurrentUser.ContainsRoleForSelectedTenant(ACCOUNT_REGISTRATION);
    }
}