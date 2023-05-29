namespace Appearition
{
    public static partial class UserRoleConstants
    {
        const string ADMIN = "Admin";
        const string USER = "User";
        const string USER_ACTIVITY_LOGGER = "UserActivityLogger";

        public static bool IsAdmin => AppearitionGate.Instance.CurrentUser.ContainsRoleForSelectedTenant(ADMIN);
        public static bool IsUser => AppearitionGate.Instance.CurrentUser.ContainsRoleForSelectedTenant(USER);
    }
}