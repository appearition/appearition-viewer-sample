using System;
using UnityEngine;
using Vuforia;
using System.Collections.Generic;
using System.Linq;
using Appearition.ArDemo.Vuforia;

namespace Appearition.ArDemo
{
    /// <summary>
    /// Event handler present on every experience tracked by Vuforia.
    /// It communicates the tracking state from Vuforia to the current provider, which will forward it to the experience and anything binding to the events in AppearitionArHandler.
    /// </summary>
    [RequireComponent(typeof(TrackableBehaviour))]
    public class VuforiaTargetEventHandler : MonoBehaviour
    {
        public event Action<VuforiaTargetEventHandler, TrackableBehaviour.StatusChangeResult> OnTrackableStatusChangedEvent;

        //References
        BaseVuforiaProviderHandler ProviderRef { get; set; }
        public ArExperience ExperienceRef { get; private set; }
        public TrackableBehaviour TrackableRef { get; private set; }

        //State
        public AppearitionArHandler.TargetState CurrentTargetState { get; private set; }
        public AppearitionArHandler.TargetState LastTargetState { get; private set; }

        //Internal Variables
        bool _fireEventWhenAssetHasLoaded;
        
        /// <summary>
        /// Setup the TargetImage Tracker event handler.
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="attachedExperience"></param>
        /// <param name="newImageTarget"></param>
        public void Setup(BaseVuforiaProviderHandler provider, ArExperience attachedExperience, TrackableBehaviour newImageTarget)
        {
            ProviderRef = provider;
            ExperienceRef = attachedExperience;
            TrackableRef = newImageTarget;

            TrackableRef.RegisterOnTrackableStatusChanged(OnTrackableStatusChangedAction);
        }

        private void OnTrackableStatusChangedAction(TrackableBehaviour.StatusChangeResult obj)
        {
            AppearitionArHandler.TargetState state = AppearitionArHandler.TargetState.None;

            //Convert state
            if (obj.NewStatus == TrackableBehaviour.Status.NO_POSE && TrackableRef.CurrentStatusInfo == TrackableBehaviour.StatusInfo.UNKNOWN)
                state = AppearitionArHandler.TargetState.Unknown;
            else
            {
                switch (obj.NewStatus)
                {
                    case TrackableBehaviour.Status.TRACKED:
                        state = AppearitionArHandler.TargetState.TargetFound;
                        break;
                    case TrackableBehaviour.Status.EXTENDED_TRACKED:
                        state = AppearitionArHandler.TargetState.TargetExtendedTracking;
                        break;
                    case TrackableBehaviour.Status.LIMITED:
                    case TrackableBehaviour.Status.NO_POSE:
                        state = AppearitionArHandler.TargetState.TargetLost;
                        break;
                }
            }

            CurrentTargetState = state;
            LastTargetState = CurrentTargetState;

            if (ExperienceRef == null || ExperienceRef.Data == null)
            {
                //Wait for the experience to be ready, and then fire the event.
                _fireEventWhenAssetHasLoaded = true;
            }
            else
                ProviderRef?.ExperienceStateUpdated(ExperienceRef, LastTargetState, state);

            OnTrackableStatusChangedEvent?.Invoke(this, obj);
        }

        void Update()
        {
            if (_fireEventWhenAssetHasLoaded && ExperienceRef != null && ExperienceRef.Data != null)
            {
                _fireEventWhenAssetHasLoaded = false;
                ProviderRef?.ExperienceStateUpdated(ExperienceRef, LastTargetState, CurrentTargetState);
                Debug.LogError("OwO !!!!!!!!!!!");
            }
        }

        //public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
        //{
        //    if (ExperienceRef != null)
        //    {
        //        if (newStatus == TrackableBehaviour.Status.NO_POSE && TrackableRef.CurrentStatusInfo == TrackableBehaviour.StatusInfo.UNKNOWN)
        //            ExperienceRef.ArProviderTrackingStateChanged(null);
        //        else
        //            ExperienceRef.ArProviderTrackingStateChanged(newStatus == TrackableBehaviour.Status.TRACKED ||
        //                                                         (newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED && AppearitionArDemoConstants.ENABLE_EXTENDED_TRACKING));
        //    }
        //}
    }
}