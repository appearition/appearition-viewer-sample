namespace Appearition
{
    public static partial class UserRoleConstants
    {
        const string TENANT_SETTINGS_VIEWER = "TenantSettingsViewer";

        //Handy Properties
        public static bool HasPermissionToViewTenantSettings => IsAdmin || AppearitionGate.Instance.CurrentUser.ContainsRoleForSelectedTenant(TENANT_SETTINGS_VIEWER);
    }
}