namespace Appearition
{
    public static partial class UserRoleConstants
    {
        const string CONTENT_LIBRARY_MANAGER = "ContentLibraryManager";

        //Handy Properties
        public static bool HasPermissionToManageContentLibrary => IsAdmin || AppearitionGate.Instance.CurrentUser.ContainsRoleForSelectedTenant(CONTENT_LIBRARY_MANAGER);
    }
}