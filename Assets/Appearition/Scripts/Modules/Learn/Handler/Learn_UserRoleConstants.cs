namespace Appearition
{
    public static partial class UserRoleConstants
    {
        const string LEARN_VIEWER = "LearnViewer";
        const string LEARN_EDITOR = "LearnEditor";
        const string LEARN_TRACKING_SUBMITTER = "LearnTrackingSubmitter";
        const string LEARN_TRACKING_VIEWER_EVERYONE = "LearnTrackingViewerEveryone";
        const string LEARN_TRACKING_VIEWER_MINE_ONLY = "LearnTrackingViewerMineOnly";

        //Handy Properties
        public static bool HasPermissionToViewLearnContent => IsAdmin || AppearitionGate.Instance.CurrentUser.ContainsRoleForSelectedTenant(LEARN_VIEWER);
        public static bool HasPermissionToEditLearnContent => IsAdmin || AppearitionGate.Instance.CurrentUser.ContainsRoleForSelectedTenant(LEARN_EDITOR);
        public static bool HasPermissionToSubmitLearnTracking => IsAdmin || AppearitionGate.Instance.CurrentUser.ContainsRoleForSelectedTenant(LEARN_TRACKING_SUBMITTER);
        public static bool HasPermissionToViewAllLearnTracking => IsAdmin || AppearitionGate.Instance.CurrentUser.ContainsRoleForSelectedTenant(LEARN_TRACKING_VIEWER_EVERYONE);
        public static bool HasPermissionToViewMyLearnTracking => IsAdmin || AppearitionGate.Instance.CurrentUser.ContainsRoleForSelectedTenant(LEARN_TRACKING_VIEWER_MINE_ONLY);
    }
}