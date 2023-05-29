using System;
using System.Collections;
using System.Collections.Generic;
using Appearition.ArTargetImageAndMedia;
using UnityEngine;

namespace Appearition.ArDemo
{
    /// <summary>
    /// Template of an ArMedia class, handling the way a single MediaType should behave.
    /// All classes MUST implement the MediaFile constructor.
    /// </summary>
    public abstract class BaseMedia : MediaFile, IDisposable
    {
        #region Constants

        public enum MediaLoadingState
        {
            /// <summary>
            /// Loading has not started yet.
            /// </summary>
            NotLoaded,
            /// <summary>
            /// Media currently being downloaded or loaded.
            /// </summary>
            Loading,
            /// <summary>
            /// Media successfully downloaded and loaded in memory
            /// </summary>
            LoadingSuccessful,
            /// <summary>
            /// Media failed to load or was not found.
            /// </summary>
            LoadingFailed,
            /// <summary>
            /// Media not available for current platform or current application state.
            /// </summary>
            Unavailable
        }

        #endregion

        #region Events

        public event Action<BaseMedia, MediaLoadingState> OnMediaLoadingStateChanged;
        public event Action<BaseMedia, bool> OnMediaDisplayStateChanged;

        #endregion

        //References
        public virtual MediaHolder HolderRef { get; set; }
        public virtual Experience ExperienceRef { get; protected set; }
        
        //Internal Variables
        protected bool WasDisposed { get; private set; }


        protected BaseMedia(MediaFile cc) : base(cc)
        {
        }


        #region States

        [SerializeField, ReadOnly]
        MediaLoadingState _mediaLoadingState = MediaLoadingState.NotLoaded;

        public virtual MediaLoadingState LoadingState
        {
            get => _mediaLoadingState;
            set
            {
                _mediaLoadingState = value;
                OnMediaLoadingStateChanged?.Invoke(this, _mediaLoadingState);
            }
        }

        public virtual bool IsMediaReadyAndDownloaded => LoadingState == MediaLoadingState.LoadingSuccessful || LoadingState == MediaLoadingState.LoadingFailed;

        public virtual bool IsActive => HolderRef != null && HolderRef.gameObject.activeInHierarchy;

        protected float loadingProgress;

        public float LoadingProgress
        {
            get
            {
                if (LoadingState == MediaLoadingState.LoadingSuccessful || LoadingState == MediaLoadingState.LoadingFailed)
                    return 1.0f;
                return loadingProgress;
            }
        }
        #endregion

        #region Initialization

        /// <summary>
        /// Score which defines how appropriate the given media is to be of this type.
        /// Default types should be from 0 to 1, but absolute types can be of higher numbers to ensure they belong to this media.
        /// In here should happen all the selection logic.
        /// A common example would be to check if the mediaType value is matching to determine the score.
        /// Check existing ArMedia scripts to have a better idea of how this should be used.
        /// </summary>
        /// <returns></returns>
        public abstract float GenerateMediaAssociationScore();

        /// <summary>
        /// Path to a prefab to load for the ArMedia, which MUST contain MediaHolder at its root. If empty, will create a blank GameObject with a MediaHolder instead.
        /// </summary>
        public virtual string PathToPrefab { get; }

        /// <summary>
        /// Initializes the MediaType.
        /// </summary>
        /// <param name="holder"></param>
        /// <param name="associatedExperience"></param>
        /// <param name="media"></param>
        public virtual void Setup(MediaHolder holder, Experience associatedExperience, MediaFile media)
        {
            HolderRef = holder;
            ExperienceRef = associatedExperience;
        }

        /// <summary>
        /// Should handle and load this media in memory. If your MediaType has a file to download, or further set up, override this method.
        /// Do note that the base is empty and just calls the callback, so don't call it.
        /// </summary>
        /// <param name="onComplete"></param>
        /// <returns></returns>
        public abstract IEnumerator DownloadAndLoadMedia(Action<BaseMedia, MediaLoadingState> onComplete);

        #endregion

        #region Update

        public virtual void UpdateContent(MediaFile newMedia)
        {
            //Apply settings
            CopyValuesFrom(newMedia);
            //There is no need to check for media download change because the current state of the EMS makes it so that a media is deleted / re-created when changing its file.
        }
        
        /// <summary>
        /// Changes the display state of the given Media.
        /// </summary>
        /// <param name="state"></param>
        public virtual void ChangeDisplayState(AppearitionArHandler.TargetState state)
        {
            if (HolderRef != null)
                HolderRef.gameObject.SetActive(state.IsTracking());
            OnMediaDisplayStateChanged?.Invoke(this, state.IsTracking());
        }

        public virtual void OnExperienceTrackingStateChanged(bool newState)
        {
            ChangeDisplayState(AppearitionArHandler.TargetState.Tracking);
        }

        public virtual void OnUpdate()
        {
        }

        #endregion

        public virtual void Dispose()
        {
            WasDisposed = true;
        }
    }
}