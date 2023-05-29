using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Appearition.ArTargetImageAndMedia;
using Appearition.Common.ListExtensions;
using UnityEngine;
using Vuforia;

namespace Appearition.ArDemo.Vuforia
{
    /// <summary>
    /// Implementation of Vuforia's offline marker-based AR.
    /// It uses the same system as the target database but loading runtime using EMS data.
    /// </summary>
    public class VuforiaOfflineProviderHandler : BaseVuforiaProviderHandler, IOfflineMarkerArProviderHandler
    {
        //References
        //List<VuforiaTargetEventHandler> _existingTargets = new List<VuforiaTargetEventHandler>();

        //Data
        /// <summary>
        /// Contains the library of assets that will be loaded / is loaded by Vuforia's offline marker database.
        /// In order to update it, call UpdateTrackableAssetCollection(List<Asset>);
        /// </summary>
        public List<Asset> TrackableAssets { get; } = new List<Asset>();

        public override float ScaleMultiplier => 0.1f;
        public override float ManipulationScaleMultiplier => 0.1f;
        protected override Vector2 ClippingPlanes => new Vector2(0.01f, 200f);
        bool _createOnceInitialized = false;

        protected override void Awake()
        {
            base.Awake();
            VuforiaARController.Instance.RegisterBeforeVuforiaTrackersInitializedCallback(BeforeVuforiaTrackersInitialized);
            VuforiaARController.Instance.RegisterVuforiaInitializedCallback(OnInitialized);
        }

        private void BeforeVuforiaTrackersInitialized()
        {
            try
            {
                var tracker = TrackerManager.Instance.GetTracker<PositionalDeviceTracker>();
                if (tracker == null)
                    tracker = TrackerManager.Instance.InitTracker<PositionalDeviceTracker>();

                tracker.Start();
            } catch (Exception e)
            {
                Debug.LogWarning("Failed to pre-start the positional tracker! " + e);
            }
        }

        public void OnInitialized()
        {
            if (_createOnceInitialized && IsArProviderActive)
            {
                _createOnceInitialized = false;
                if (_process == null)
                    _process = StartCoroutine(CreateAllOfflineTrackables());
            }
        }

        #region Asset Updating

        public void UpdateTrackableAssetCollection(List<Asset> allTrackableAssets)
        {
            TrackableAssets.Clear();
            for (int i = 0; i < allTrackableAssets.Count; i++)
            {
                if (allTrackableAssets[i] != null && allTrackableAssets[i].targetImages != null && allTrackableAssets[i].targetImages.Count > 0
                    && !string.IsNullOrEmpty(allTrackableAssets[i].targetImages[0].url))
                    TrackableAssets.Add(allTrackableAssets[i]);
            }
        }

        #endregion

        #region Lifecycle

        Coroutine _process;

        public override void ChangeArProviderActiveState(bool shouldBeEnabled)
        {
            _process = null;
            base.ChangeArProviderActiveState(shouldBeEnabled);

            if (shouldBeEnabled)
            {
                ResetTrackers();
                if (_process == null)
                    _process = StartCoroutine(CreateAllOfflineTrackables());
            }
        }

        /// <summary>
        /// Whether Vuforia should be doing cloud scan or not.
        /// </summary>
        /// <param name="shouldScan"></param>
        public override void ChangeScanningState(bool shouldScan)
        {
            if (CurrentTargetFinder == null)
                return;

            if (shouldScan)
                CurrentTargetFinder.StartRecognition();
            else
                CurrentTargetFinder.Stop();

            IsScanning = shouldScan;
        }

        IEnumerator CreateAllOfflineTrackables()
        {
            PositionalDeviceTracker tracker = null;
            while (tracker == null || !tracker.IsActive)
            {
                tracker = TrackerManager.Instance.GetTracker<PositionalDeviceTracker>();
                yield return null;
            }

            for (int i = 0; i < TrackableAssets.Count; i++)
            {
                //_existingTargets.Add(CreateImageRecoObject(TrackableAssets[i]));
                CreateImageRecoObject(TrackableAssets[i]);
                Debug.Log("Created trackable for asset of id " + TrackableAssets[i].assetId);
            }

            _process = null;
        }


        public override void InitializeVuforia()
        {
            base.InitializeVuforia();
            StartCoroutine(VuforiaInitializationStateObserverProcess());
        }

        IEnumerator VuforiaInitializationStateObserverProcess()
        {
            while (VuforiaRuntime.Instance.InitializationState != VuforiaRuntime.InitState.INITIALIZED)
                yield return null;

            ObjectTracker tracker = TrackerManager.Instance.GetTracker<ObjectTracker>();

            if (tracker == null)
                tracker = TrackerManager.Instance.InitTracker<ObjectTracker>();

            OnInitialized();
        }

        /// <summary>
        /// This method stops and restarts the PositionalDeviceTracker.
        /// It is called by the UI Reset Button and when RELOCALIZATION status has
        /// not changed for 10 seconds.
        /// </summary>
        public void ResetTrackers()
        {
            if (!_isActive)
                return;

            var positionalDeviceTracker = TrackerManager.Instance.GetTracker<PositionalDeviceTracker>();
            // Stop and restart trackers
            if (positionalDeviceTracker != null)
            {
                if (positionalDeviceTracker.IsActive)
                    positionalDeviceTracker.Reset();
                else
                    positionalDeviceTracker.Start();
            }
        }

        #endregion

        #region Object Creation

        /// <summary>
        /// Creates a single ImageTargetBehavior equipped with all it needs.
        /// </summary>
        /// <param name="asset"></param>
        /// <returns></returns>
        void CreateImageRecoObject(Asset asset)
        {
            var imageSource = TrackerManager.Instance.GetTracker<ObjectTracker>().RuntimeImageSource;
            imageSource.SetImage(asset.targetImages[0].image.texture, 0.1f, asset.assetId);
            //imageSource.SetFile(VuforiaUnity.StorageType.STORAGE_ABSOLUTE, ArTargetHandler.GetPathToTargetImage(asset, asset.targetImages[0]), 0.1f, asset.assetId);
            var dataset = ObjectTracker.CreateDataSet();
            var trackable = dataset.CreateTrackable(imageSource, asset.assetId);
            ObjectTracker.ActivateDataSet(dataset);
            var targetBehavior = (ImageTargetBehaviour) trackable;

            //Add non-setup experience and event
            var eventHandler = trackable.gameObject.AddComponent<VuforiaTargetEventHandler>();

            ArExperience experience = AppearitionArHandler.CreateExperienceFromAsset<ArExperience>(this, asset, false, trackable.gameObject, null);

            RegisteredExperiences.Add(eventHandler);
            eventHandler.Setup(this, experience, targetBehavior);
            eventHandler.OnTrackableStatusChangedEvent += EventHandler_OnTrackableStatusChangedEvent;
        }

        /// <summary>
        /// Occurs when a target has been found and picked up. Loads the content.
        /// </summary>
        /// <param name="eventHandler"></param>
        /// <param name="status"></param>
        private void EventHandler_OnTrackableStatusChangedEvent(VuforiaTargetEventHandler eventHandler, TrackableBehaviour.StatusChangeResult status)
        {
            if (status.NewStatus == TrackableBehaviour.Status.TRACKED && IsScanning)
            {
                if (eventHandler.ExperienceRef != null)
                    StartCoroutine(eventHandler.ExperienceRef.LoadContent());
            }
        }

        #endregion

        #region Cleanup

        public override void ClearExperiencesBeingTracked()
        {
            for (int i = 0; i < RegisteredExperiences.Count; i++)
            {
                if (RegisteredExperiences[i] != null && RegisteredExperiences[i].ExperienceRef != null) // && _existingTargets[i].ExperienceRef.AreMediaLoaded)
                    RegisteredExperiences[i].ExperienceRef.UnloadContent();
            }

            ResetTrackers();

            //Let the ArHandler know about the new Object's state.
            if (IsArProviderActive)
                AppearitionArHandler.ArProviderTargetStateChanged(null, AppearitionArHandler.TargetState.None);
        }

        public void ResetTrackableAssetContainer()
        {
            for (int i = 0; i < RegisteredExperiences.Count; i++)
                RegisteredExperiences[i].OnTrackableStatusChangedEvent -= EventHandler_OnTrackableStatusChangedEvent;

            ObjectTracker.DestroyAllDataSets(true);
            RegisteredExperiences.Clear();
        }

        /// <summary>
        /// In the case of Image recognition, clear the currently tracked experiences and continue scanning
        /// </summary>
        public override void ResetTracking()
        {
            ClearExperiencesBeingTracked();

            for (int i = 0; i < RegisteredExperiences.Count; i++)
            {
                if (RegisteredExperiences[i] != null && RegisteredExperiences[i].ExperienceRef != null)
                {
                    RegisteredExperiences[i].ExperienceRef.SetupExperience(RegisteredExperiences[i].ExperienceRef.Data, this, RegisteredExperiences[i].ExperienceRef.BackupSetupPosition);
                    if(RegisteredExperiences[i].CurrentTargetState.IsTracking())
                        RegisteredExperiences[i].ExperienceRef.ArProviderTrackingStateChanged(RegisteredExperiences[i].CurrentTargetState);
                }
            }
        }

        #endregion
    }
}