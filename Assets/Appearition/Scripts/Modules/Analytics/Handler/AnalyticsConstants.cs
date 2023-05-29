namespace Appearition.Analytics
{
    /// <summary>
    /// Contains message constants for the Analytics module.
    /// </summary>
    public static class AnalyticsConstants
    {
        public const string SESSION_FILE_EXTENSION = ".session";
        public const float SESSION_SYNC_TIMEOUT = 10f;

        #region Log Messages

        #region Main Methods 

        public const string SESSION_CREATED_INFO = "New ongoing session of key {0} has started.";
        public const string SESSION_CONTENT_SAVED_LOCALLY = "The content of the session of key {0} was successfully saved locally.";

        public const string ACTIVITY_SUCCESSFULLY_ADDED_TO_SESSION = "An activity with key {0} was successfully added to the session of key {1}";
        public const string ACTIVITY_SUCCESSFULLY_UPDATED_IN_SESSION = "An activity with key {0} was successfully updated in the session of key {1}";

        public const string SYNC_WITH_EMS_SUCCESSFULLY_COMPLETED = "The analytics data sync with the EMS was successfully completed. {1} sessions were synced.";
        public const string SYNC_WITH_EMS_TIMEOUT = "The analytics sync with the EMS resulted with a timeout.";

        public const string SYNC_WITH_EMS_REQUESTED = "Analytics sync with EMS has been requested.";
        
        #endregion

        #region API 

        public const string SESSION_BEGAN_SUCCESS = "The start of a new session of key {0} has successfully been synced with the EMS.";
        public const string SESSION_BEGAN_FAILURE =
            "An issue occured when trying to sync the start of the session of key {0} with the EMS. The data was successfully saved and the sync will be attempted again later.";

        public const string SESSION_END_SUCCESS = "The closure of the session of key {0} was successfully synced with the EMS.";
        public const string SESSION_END_FAILURE =
            "An issue occured when trying to sync the closure of the session of key {0}. The data was successfully saved and the sync will be attempted again later.";

        public const string ACTIVITY_SYNCED_SUCCESS = "The activity of key {0} and code {1} was successfully synced as part of the session of key {2}.";
        public const string ACTIVITY_SYNCED_FAILURE =
            "An issue occured when trying to sync the activity of key {0} and code {1}. The data was successfully saved and the sync will be attempted again later.";
        public const string ACTIVITY_NULL = "A null activity was provided as part of the current session.";
        public const string ACTIVITY_NO_CODE = "The activity of key {0} did not contain any Activity Code. Make sure to provide one before adding it to the current session.";

        #endregion

        #endregion
    }
}