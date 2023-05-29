using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Appearition.ArTargetImageAndMedia;
using Appearition.Common;
using Appearition.Common.ListExtensions;


namespace Appearition.ArDemo
{
    /// <summary>
    /// Class containing a single Appearition Experience.
    /// </summary>
    public class ArExperience : Experience
    {
        #region Events

        public delegate void TrackingStateChanged(AppearitionArHandler.TargetState trackingState);

        /// <summary>
        /// Occurs whenever the tracking state of the Ar Experience has changed. Communicates the new tracking state.
        /// </summary>
        public event TrackingStateChanged OnTrackingStateChanged;

        #endregion


        public AppearitionArHandler.TargetState CurrentTargetState => ProviderRef != null ? ProviderRef.GetExperienceTargetState(this) : AppearitionArHandler.TargetState.Unknown;

        /// <summary>
        /// Whether or not the current Experience is being tracked.
        /// </summary>
        public bool IsCurrentlyTracking => CurrentTargetState.IsTracking();

        GameObject _loadingIndicator;

        //Settings
        protected override Quaternion RotationOffset => Quaternion.Euler(0,0, 0);

        #region Tags

        public bool IsMarker => Data?.IsMarker() ?? false;
        public bool IsMarkerless => Data?.IsMarkerless() ?? false;
        public bool IsInteractive => Data?.IsInteractive() ?? false;

        #endregion

        #region Update

        /// <summary>
        /// Called by the ArProvider handler, informing the experience that the tracking state has changed.
        /// </summary>
        /// <param name="trackingState"></param>
        /// <param name="invokeEvent"></param>
        public virtual void ArProviderTrackingStateChanged(AppearitionArHandler.TargetState trackingState, bool invokeEvent = true)
        {
            ChangeMediaDisplayState(trackingState);

            if(invokeEvent)
                AppearitionArHandler.ArProviderTargetStateChanged(this, trackingState);

            //If it's tracking and medias haven't been loaded yet; load them.
            if (trackingState.IsTracking() && Medias != null)
            {
                //Create a rotating indicator 
                if (_loadingIndicator == null && Medias.HiziAny(o => o.LoadingState == BaseMedia.MediaLoadingState.NotLoaded))
                {
                    _loadingIndicator = Instantiate(Resources.Load<GameObject>("LoadingRotatingHolder"), MediaContentHolder);
                    _loadingIndicator.transform.localScale = Vector3.one * 0.1f;
                    _loadingIndicator.transform.localPosition = new Vector3(0.0254f, 0.001f, -0.0254f);
                }

                for (int i = 0; i < Medias.Count; i++)
                {
                    if (Medias[i].LoadingState == BaseMedia.MediaLoadingState.NotLoaded)
                    {
                        //Medias[i].HolderRef.StartCoroutine(Medias[i].DownloadAndLoadMedia(OnMediaLoaded));
                        StartCoroutine(Medias[i].DownloadAndLoadMedia(OnMediaLoaded));
                    }
                }
            }

            if (invokeEvent && OnTrackingStateChanged != null)
                OnTrackingStateChanged(trackingState);
        }

        protected override void OnMediaLoaded(BaseMedia media, BaseMedia.MediaLoadingState state)
        {
            base.OnMediaLoaded(media, state);

            if (state == BaseMedia.MediaLoadingState.LoadingSuccessful)
            {
                media.ChangeDisplayState(CurrentTargetState);
                if (_loadingIndicator != null)
                {
                    Destroy(_loadingIndicator);
                    _loadingIndicator = null;
                }
            }
        }
        
        #endregion
    }
}