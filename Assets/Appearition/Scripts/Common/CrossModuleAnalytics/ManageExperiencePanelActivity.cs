namespace Appearition.Analytics.Activities
{
    /// <summary>
    /// Main activity related to managing AR Experiences.
    /// Contains a variety for Events related to third party file loading, and functionality which cannot be tracked by the EMS such as leaving unexpectedly management menus.
    /// </summary>
    public class ManageExperiencePanelActivity : AppPanelActivity
    {
        //Shared constants
        const string FILENAME_EVENT_KEY = "FILENAME";
        const string ERROR_EVENT_KEY = "ERROR";
        const string MEDIATYPE_EVENT_KEY = "MEDIATYPE";

        #region Constructors 

        public ManageExperiencePanelActivity(string panelName) : base(panelName)
        {
        }

        public ManageExperiencePanelActivity(string previousPanelName, string panelName) : base(previousPanelName, panelName)
        {
        }

        #endregion

        #region Event Containers

        #region Abstract Event Containers

        public abstract class BaseTriggerEventContainer<T> : ActivityEventContainer<T> where T : BaseTriggerEventContainer<T>
        {
            public enum TriggerType
            {
                TargetImage,
                Location,
                Unknown
            }

            protected BaseTriggerEventContainer(TriggerType triggerType)
            {
                analyticEvents.Add(new AnalyticsData("TRIGGER_TYPE", triggerType.ToString()));
            }
        }

        #endregion

        /// <summary>
        /// Should occur when the user was prompted to load a trigger file using a third party tool.
        /// </summary>
        public class PromptUserToLoadTriggerFile : BaseTriggerEventContainer<PromptUserToLoadTriggerFile>
        {
            public PromptUserToLoadTriggerFile(TriggerType triggerType) : base(triggerType)
            {
            }
        }

        /// <summary>
        /// Should occur when the user was prompted to load a trigger file using a third party tool and succeeded.
        /// </summary>
        public class TriggerFileLoadingSuccess : BaseTriggerEventContainer<TriggerFileLoadingSuccess>
        {
            public TriggerFileLoadingSuccess(TriggerType triggerType, string fileName) : base(triggerType)
            {
                analyticEvents.Add(new AnalyticsData(FILENAME_EVENT_KEY, fileName));
            }
        }

        /// <summary>
        /// Should occur when the user was prompted to load a trigger file using a third party tool and failed.
        /// </summary>
        public class TriggerFileLoadingFailure : BaseTriggerEventContainer<TriggerFileLoadingFailure>
        {
            public TriggerFileLoadingFailure(TriggerType triggerType, string fileNameIfAny, string error) : base(triggerType)
            {
                analyticEvents.Add(new AnalyticsData(FILENAME_EVENT_KEY, fileNameIfAny));
                analyticEvents.Add(new AnalyticsData(ERROR_EVENT_KEY, error));
            }
        }

        /// <summary>
        /// Should occur when the user was prompted to load a media file.
        /// </summary>
        public class PromptUserToLoadMediaFile : ActivityEventContainer<PromptUserToLoadMediaFile>
        {
            public PromptUserToLoadMediaFile(string mediaType)
            {
                analyticEvents.Add(new AnalyticsData(MEDIATYPE_EVENT_KEY, mediaType));
            }
        }

        /// <summary>
        /// Should occur when the user was prompted to load a media file using a third party tool and succeed.
        /// </summary>
        public class MediaFileLoadingSuccess : ActivityEventContainer<MediaFileLoadingSuccess>
        {
            public MediaFileLoadingSuccess(string mediaType, string fileName)
            {
                analyticEvents.Add(new AnalyticsData(MEDIATYPE_EVENT_KEY, mediaType));
                analyticEvents.Add(new AnalyticsData(FILENAME_EVENT_KEY, fileName));
            }
        }

        /// <summary>
        /// Should occur when the user was prompted to load a media file using a third party tool and failed.
        /// </summary>
        public class MediaFileLoadingFailure : ActivityEventContainer<MediaFileLoadingFailure>
        {
            public MediaFileLoadingFailure(string mediaType, string fileName, string error)
            {
                analyticEvents.Add(new AnalyticsData(MEDIATYPE_EVENT_KEY, mediaType));
                analyticEvents.Add(new AnalyticsData(FILENAME_EVENT_KEY, fileName));
                analyticEvents.Add(new AnalyticsData(ERROR_EVENT_KEY, error));
            }
        }

        /// <summary>
        /// Should occur when the user cancels updating an experience half way through without submitting 
        /// </summary>
        public class ExperienceUpdateCancelledByUser : ActivityEventContainer<ExperienceUpdateCancelledByUser>
        {
        }

        /// <summary>
        /// Should occur when the user cancels creating an experience half way through without submitting.
        /// </summary>
        public class ExperienceCreationCancelledByUser : ActivityEventContainer<ExperienceCreationCancelledByUser>
        {
        }

        #endregion
    }
}