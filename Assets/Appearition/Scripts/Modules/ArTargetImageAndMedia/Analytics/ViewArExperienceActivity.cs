using Appearition;

namespace Appearition.Analytics.Activities
{
    /// <summary>
    /// Main activity related to viewing AR Experiences.
    /// Contains a variety for Events related to ArProviders, which include OnTargetFound, OnTargetLost, OnExperienceLoaded, etc.
    /// </summary>
    public class ViewArExperienceActivity : Activity
    {

        //Shared constants
        protected const string AR_MODE_DATA_KEY = "AR_MODE";
        protected const string ASSET_ID_DATA_KEY = "ASSET_ID";
        protected const string MEDIA_ID_DATA_KEY = "AR_MEDIA_ID";

        public ViewArExperienceActivity(string assetId, ArMode arMode = ArMode.None)
        {
            ActivityData.Add(new AnalyticsData(ASSET_ID_DATA_KEY, assetId));
            ActivityData.Add(new AnalyticsData(AR_MODE_DATA_KEY, arMode.ToString()));

            FinalizeActivityCreation();
        }

        #region Event Containers 

        #region Abstract Containers 

        /// <summary>
        /// Base event container for any Media-specific Activity Events.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public abstract class BaseArMediaEventContainer<T> : ActivityEventContainer<T> where T : BaseArMediaEventContainer<T>
        {
            protected int arMediaId;

            protected BaseArMediaEventContainer(int newArMediaId)
            {
                arMediaId = newArMediaId;
                analyticEvents.Add(new AnalyticsData(MEDIA_ID_DATA_KEY, arMediaId.ToString()));
            }
        }

        #endregion

        /// <summary>
        /// Activity event to trigger when a target image has been picked up by the ArProvider.
        /// </summary>
        public class TrackingBegin : ActivityEventContainer<TrackingBegin>
        {
        }

        /// <summary>
        /// Activity event to trigger when a target image has been lost from the ArProvider.
        /// </summary>
        public class TrackingEnded : ActivityEventContainer<TrackingEnded>
        {
            public enum Reason
            {
                EndedByUser,
                TrackingLost,
                IssueFromPlugin
            }

            public TrackingEnded(Reason reasonWhyTrackingEnded, string additionalInfo = "")
            {
                analyticEvents.Add(new AnalyticsData("REASON_WHY_ENDED", reasonWhyTrackingEnded.ToString()));
                analyticEvents.Add(new AnalyticsData("ADDITIONAL_INFO", additionalInfo));
            }
        }

        /// <summary>
        /// Activity event to trigger when a target image has been lost from the ArProvider.
        /// Should include the AssetId of the experience.
        /// </summary>
        public class TargetFoundExperienceNotAccessible : ActivityEventContainer<TargetFoundExperienceNotAccessible>
        {
        }

        /// <summary>
        /// Activity event to trigger when a media has been requested to be played.
        /// Following this event, the media should start downloading.
        /// Should include the ArMediaId.
        /// </summary>
        public class MediaRequested : BaseArMediaEventContainer<MediaRequested>
        {
            public MediaRequested(int newArMediaId) : base(newArMediaId)
            {
            }
        }

        /// <summary>
        /// Activity event to trigger when a media has been downloaded and loaded successfully and is presented to the user.
        /// Should include the ArMediaId.
        /// </summary>
        public class MediaPlayed : BaseArMediaEventContainer<MediaPlayed>
        {
            public MediaPlayed(int newArMediaId) : base(newArMediaId)
            {
            }
        }

        /// <summary>
        /// Activity event to trigger when a media has been requested for downloaded but failed loading or failed being presented to the user.
        /// Should include the ArMediaId.
        /// </summary>
        public class MediaFailedToLoad : BaseArMediaEventContainer<MediaFailedToLoad>
        {
            public enum ReasonOfFailure
            {
                FileNotFound,
                ErrorWhileLoading,
                FileTypeNotImplemented,
                MediaNotSetupProperly,
                Unknown
            }

            public MediaFailedToLoad(int newArMediaId, ReasonOfFailure reasonOfFailure) : base(newArMediaId)
            {
                analyticEvents.Add(new AnalyticsData("REASON_OF_FAILURE", reasonOfFailure.ToString()));
            }
        }

        /// <summary>
        /// Should occur when a media is being interacted with by the user.
        /// This include click (eg open weblink?), manipulation (translation, rotation, scale), expansion/retraction, change of display state, etc.
        /// </summary>
        public class MediaInteracted : BaseArMediaEventContainer<MediaInteracted>
        {
            public MediaInteracted(int newArMediaId, string interactionType) : base(newArMediaId)
            {
                analyticEvents.Add(new AnalyticsData("INTERACTION_TYPE", interactionType));
            }
        }

        #endregion
    }
}