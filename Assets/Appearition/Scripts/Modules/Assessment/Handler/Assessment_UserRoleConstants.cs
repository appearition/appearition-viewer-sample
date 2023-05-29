namespace Appearition
{
    public static partial class UserRoleConstants
    {
        const string ASSESSMENT_EDITOR = "AssessmentEditor";
        const string ASSESSMENT_RESULTS_VIEWER_EVERYONE = "AssessmentResultsViewerEveryone";
        const string ASSESSMENT_RESULTS_VIEWER_MINE_ONLY = "AssessmentResultsViewerMineOnly";
        const string ASSESSMENT_SUBMITTER = "AssessmentSubmitter";
        const string ASSESSMENT_VIEWER = "AssessmentViewer";
        
        public static bool HasPermissionToEditAssessment => IsAdmin || AppearitionGate.Instance.CurrentUser.ContainsRoleForSelectedTenant(ASSESSMENT_EDITOR);
        public static bool HasPermissionToViewEveryonesAssessmentResults => IsAdmin || AppearitionGate.Instance.CurrentUser.ContainsRoleForSelectedTenant(ASSESSMENT_RESULTS_VIEWER_EVERYONE);
        public static bool HasPermissionToViewTheirOwnAssessmentResults => IsAdmin || AppearitionGate.Instance.CurrentUser.ContainsRoleForSelectedTenant(ASSESSMENT_RESULTS_VIEWER_MINE_ONLY);
        public static bool HasPermissionSubmitAssessments => IsAdmin || AppearitionGate.Instance.CurrentUser.ContainsRoleForSelectedTenant(ASSESSMENT_SUBMITTER);
        public static bool HasPermissionToViewAssessmentContent => IsAdmin || AppearitionGate.Instance.CurrentUser.ContainsRoleForSelectedTenant(ASSESSMENT_VIEWER);
    }
}