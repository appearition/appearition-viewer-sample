namespace Appearition
{
    public static partial class UserRoleConstants
    {
        //Ar Management
        const string AR_CREATOR = "ArCreator";
        const string AR_CREATOR_MINE_ONLY = "ArCreatorMineOnly";
        const string AR_LOCK = "ArLock";
        const string AR_TAG_EDITOR = "ArTagEditor";
        const string AR_PRIVATE_TAG_EDITOR = "ArPrivateTagEditor";
        const string AR_PUBLISHER = "ArPublisher";

        //Market
        const string AR_MARKET_PUBLISHER = "ArMarketPublisher";
        const string AR_MARKET_VIEWER = "ArMarketViewer";
        
        //Handy Properties
        public static bool HasPermissionToPublishExperiences => IsAdmin || AppearitionGate.Instance.CurrentUser.ContainsRoleForSelectedTenant(AR_PUBLISHER);
        public static bool HasPermissionToCreateAndEditAnyExperiences => IsAdmin || AppearitionGate.Instance.CurrentUser.ContainsRoleForSelectedTenant(AR_CREATOR);
        public static bool HasPermissionToCreateAndEditOwnedExperiences => IsAdmin || AppearitionGate.Instance.CurrentUser.ContainsRoleForSelectedTenant(AR_CREATOR_MINE_ONLY);
        public static bool HasPermissionToLockExperiences => IsAdmin || AppearitionGate.Instance.CurrentUser.ContainsRoleForSelectedTenant(AR_LOCK);
        public static bool HasPermissionToTagAnyExperience => IsAdmin || AppearitionGate.Instance.CurrentUser.ContainsRoleForSelectedTenant(AR_TAG_EDITOR);
        public static bool HasPermissionToTagOwnedExperience => IsAdmin || AppearitionGate.Instance.CurrentUser.ContainsRoleForSelectedTenant(AR_PRIVATE_TAG_EDITOR);

        public static bool HasPermissionToViewMarketplace => IsAdmin || AppearitionGate.Instance.CurrentUser.ContainsRoleForSelectedTenant(AR_MARKET_VIEWER);
        public static bool HasPermissionToEditMarketplace => IsAdmin || AppearitionGate.Instance.CurrentUser.ContainsRoleForSelectedTenant(AR_MARKET_PUBLISHER);
    }
}