using System;
using System.Collections;
using System.Collections.Generic;
using Appearition.ArTargetImageAndMedia;
using Appearition.Common.ListExtensions;
using UnityEngine;
using Vuforia;

namespace Appearition.ArDemo.Vuforia
{
    public class VuforiaMarkerlessProviderHandler : BaseVuforiaProviderHandler, IOfflineMarkerlessArProviderHandler
    {
        //References
        [SerializeField] PlaneFinderBehaviour _planeFinder;
        [SerializeField] ContentPositioningBehaviour _contentPositioning;
        [SerializeField] AnchorBehaviour _anchorTemplate;
        PositionalDeviceTracker _positionalDeviceTracker;
        SmartTerrain _smartTerrain;

        public GameObject CurrentAnchor { get; private set; }
        List<Asset> _assetsCurrentlyLoading = new List<Asset>(); 
        List<VuforiaTargetEventHandler> _activeExperiences = new List<VuforiaTargetEventHandler>();
        List<ArExperience> _experiencesBeingLoaded = new List<ArExperience>();
        List<GameObject> _contentReadyToBePlaced = new List<GameObject>();

        //Internal Variables
        public override float ScaleMultiplier => 1f;
        public override float ManipulationMultiplier => 0.01f;
        public override float ManipulationScaleMultiplier => 300f;
        protected override Vector2 ClippingPlanes => new Vector2(0.1f, 100f);

        #region Init

        protected override void Awake()
        {
            base.Awake();

            if (_planeFinder != null)
            {
                var contentPlacer = _planeFinder.GetComponent<ContentPositioningBehaviour>();
                if (contentPlacer != null)
                    contentPlacer.OnContentPlaced.AddListener(OnMarkerlessAnchorPlaced);
            }

            VuforiaARController.Instance.RegisterVuforiaStartedCallback(OnVuforiaStarted);
            DeviceTrackerARController.Instance.RegisterTrackerStartedCallback(OnTrackerStarted);
        }

        void OnVuforiaStarted()
        {
            if (!_isActive)
                return;

            var stateManager = TrackerManager.Instance.GetStateManager();

            // Check trackers to see if started and start if necessary
            _positionalDeviceTracker = TrackerManager.Instance.GetTracker<PositionalDeviceTracker>();
            _smartTerrain = TrackerManager.Instance.GetTracker<SmartTerrain>();

            if (_positionalDeviceTracker != null && _smartTerrain != null)
            {
                if (!_positionalDeviceTracker.IsActive)
                {
                    Debug.LogError("The Ground Plane feature requires the Device Tracker to be started. " +
                                   "Please enable it in the Vuforia Configuration or start it at runtime through the scripting API.");
                    return;
                }

                if (_positionalDeviceTracker.IsActive && !_smartTerrain.IsActive)
                    _smartTerrain.Start();
            }
            else
            {
                if (_positionalDeviceTracker == null)
                    Debug.Log("PositionalDeviceTracker returned null. GroundPlane not supported on this device.");
                if (_smartTerrain == null)
                    Debug.Log("SmartTerrain returned null. GroundPlane not supported on this device.");
            }
        }

        void OnTrackerStarted()
        {
            _positionalDeviceTracker = TrackerManager.Instance.GetTracker<PositionalDeviceTracker>();
            _smartTerrain = TrackerManager.Instance.GetTracker<SmartTerrain>();

            if (_positionalDeviceTracker != null && _smartTerrain != null)
            {
                if (!_positionalDeviceTracker.IsActive)
                {
                    Debug.LogError("The Ground Plane feature requires the Device Tracker to be started. " +
                                   "Please enable it in the Vuforia Configuration or start it at runtime through the scripting API.");
                    return;
                }

                if (!_smartTerrain.IsActive)
                    _smartTerrain.Start();

                Debug.Log("PositionalDeviceTracker is Active?: " + _positionalDeviceTracker.IsActive +
                          "\nSmartTerrain Tracker is Active?: " + _smartTerrain.IsActive);
            }
        }

        #endregion

        public override void ChangeArProviderActiveState(bool shouldBeEnabled)
        {
            base.ChangeArProviderActiveState(shouldBeEnabled);

             //if (!shouldBeEnabled && _planeFinder != null)
             if(_planeFinder != null)
                _planeFinder.gameObject.SetActive(shouldBeEnabled);
        }

        public override void ChangeScanningState(bool shouldScan)
        {
            IsScanning = shouldScan;

            if (_planeFinder != null)
                _planeFinder.gameObject.SetActive(shouldScan);

            if (_contentPositioning != null && shouldScan)
            {
                if (_contentPositioning.AnchorStage != null)
                {
                    //Stop any sub-loading processes. 
                    StopAllCoroutines();
                    
                    //Store the current experiences, and delete everything that's loaded.
                    List<Asset> assetsToReload = new List<Asset>(_assetsCurrentlyLoading);
                    for (int i = 0; i < RegisteredExperiences.Count; i++)
                    { 
                        if(RegisteredExperiences[i] != null && RegisteredExperiences[i].ExperienceRef != null && RegisteredExperiences[i].ExperienceRef.Data != null 
                           && _assetsCurrentlyLoading.HiziAll(o=>o.assetId != RegisteredExperiences[i].ExperienceRef.Data.assetId))
                            assetsToReload.Add(RegisteredExperiences[i].ExperienceRef.Data);
                    }
                    
                    ClearExperiencesBeingTracked();

                    //Release anchor
                    if (_contentPositioning != null && !string.IsNullOrEmpty(_contentPositioning.AnchorStage.gameObject.scene.name))
                    {
                        Destroy(_contentPositioning.AnchorStage.gameObject);
                        AnchorBehaviour anchor = Instantiate(_anchorTemplate, transform);
                        _contentPositioning.AnchorStage = anchor;
                    }
                    CurrentAnchor = null;

                    //Restart the loading process.
                    for (int i = 0; i < assetsToReload.Count; i++)
                        StartCoroutine(LoadSelectedExperience(assetsToReload[i]));
                }
            }
        }

        /// <summary>
        /// This method stops and restarts the PositionalDeviceTracker.
        /// It is called by the UI Reset Button and when RELOCALIZATION status has
        /// not changed for 10 seconds.
        /// </summary>
        public override void ResetTracking()
        {
            if (!_isActive)
                return;

            AppearitionArHandler.ChangeArProviderScanningState(true);

            _smartTerrain = TrackerManager.Instance.GetTracker<SmartTerrain>();
            _positionalDeviceTracker = TrackerManager.Instance.GetTracker<PositionalDeviceTracker>();

            // Stop and restart trackers
            if (_smartTerrain != null && _positionalDeviceTracker != null)
            {
                _smartTerrain.Stop(); // stop SmartTerrain tracker before PositionalDeviceTracker
                _positionalDeviceTracker.Reset();
                _smartTerrain.Start(); // start SmartTerrain tracker after PositionalDeviceTracker
            }
        }

        #region Experience

        /// <summary>
        /// Occurs when opening the AR mode. Start loading the asset in advance, but.. hidden!
        /// </summary>
        /// <param name="asset"></param>
        /// <returns></returns>
        public IEnumerator LoadSelectedExperience(Asset asset)
        {
            bool? isLoaded = default;
            var experience = AppearitionArHandler.CreateExperienceFromAsset<ArExperience>(this, asset, true, onComplete => isLoaded = onComplete);
            experience.gameObject.SetActive(false);
            int index = _experiencesBeingLoaded.Count;
            _experiencesBeingLoaded.Add(experience);

            //Wait for the anchor to be ready 
            while (CurrentAnchor == null)
                yield return null;

            //Handle load failure if it happened at this point in time. 
            if (isLoaded.HasValue && !isLoaded.Value)
            {
                OnValidateAppearitionExperience(false);
                yield break;
            }

            //Make sure it still exists..
            if (experience == null)
                yield break;

            //Place the experience as a child of the anchor
            AttachExperienceToAnchor(experience);

            //Setup the vuforia event handler
            VuforiaTargetEventHandler eventHandler = CurrentAnchor.GetComponent<VuforiaTargetEventHandler>();
            if (eventHandler == null)
                eventHandler = CurrentAnchor.gameObject.AddComponent<VuforiaTargetEventHandler>();
            RegisteredExperiences.Add(eventHandler);
            eventHandler.Setup(this, experience, CurrentAnchor.GetComponent<AnchorBehaviour>());

            _contentReadyToBePlaced.RemoveAt(index);
            _experiencesBeingLoaded.RemoveAt(index);
            _activeExperiences.Add(eventHandler);


        }

        void AttachExperienceToAnchor(ArExperience experience)
        {
            if (CurrentAnchor != null)
            {
                experience.transform.SetParent(CurrentAnchor.transform);
                experience.gameObject.SetActive(true);
                experience.transform.localPosition = Vector3.zero;
                experience.transform.localRotation = Quaternion.identity;
                experience.transform.localScale = Vector3.one;
            }
        }

        /// <summary>
        /// Occurs when the user taps on a markerless plane and creates a new anchor.
        /// </summary>
        /// <param name="contentHolder"></param>
        private void OnMarkerlessAnchorPlaced(GameObject contentHolder)
        {
            //Let the loader know that it should be placing the goods
            if (CurrentAnchor == null)
            {
                CurrentAnchor = contentHolder;
                CurrentAnchor.transform.SetParent(transform);
                AppearitionArHandler.ChangeArProviderScanningState(false);
            }
        }

        /// <summary>
        /// Occurs after the experience is loaded; contains whether it was successful or not.
        /// </summary>
        /// <param name="isValid"></param>
        void OnValidateAppearitionExperience(bool isValid)
        {
            //If the experience picked up was not valid, clear content and restart scanning.
            if (!isValid)
            {
                ClearExperiencesBeingTracked();
                ResetTracking();
            }
        }

        #endregion

        #region Cleanup

        public override void ClearExperiencesBeingTracked()
        {
            for (int i = _activeExperiences.Count - 1; i >= 0; i--)
            {
                if (_activeExperiences[i] != null)
                    Destroy(_activeExperiences[i].gameObject);
            }

            _activeExperiences.Clear();

            for (int i = _contentReadyToBePlaced.Count - 1; i >= 0; i--)
            {
                if (_contentReadyToBePlaced[i] != null)
                    Destroy(_contentReadyToBePlaced[i].gameObject);
            }

            _contentReadyToBePlaced.Clear();

            for (int i = _experiencesBeingLoaded.Count - 1; i >= 0; i--)
            {
                if (_experiencesBeingLoaded[i] != null)
                    Destroy(_experiencesBeingLoaded[i].gameObject);
            }

            _experiencesBeingLoaded.Clear();

            //Let the ArHandler know about the new Object's state.
            if (IsArProviderActive)
                AppearitionArHandler.ArProviderTargetStateChanged(null, AppearitionArHandler.TargetState.None);
        }

        #endregion
    }
}