using System;
using System.Collections;
using System.Collections.Generic;
using Appearition.Common.ListExtensions;
using UnityEngine;
using Vuforia;

namespace Appearition.ArDemo.Vuforia
{
    public abstract class BaseVuforiaProviderHandler : BaseArProviderHandler
    {
        Camera _providerCamera;

        public override Camera ProviderCamera
        {
            get
            {
                if (_providerCamera == null)
                    _providerCamera = VuforiaBehaviour.Instance.GetComponent<Camera>();
                return _providerCamera;
            }
        }

        private ObjectTracker _objectTracker;

        public ObjectTracker ObjectTracker
        {
            get
            {
                if (_objectTracker == null)
                    _objectTracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
                return _objectTracker;
            }
        }

        private ImageTargetFinder _currentTargetFinder;

        public ImageTargetFinder CurrentTargetFinder
        {
            get
            {
                if (_currentTargetFinder == null)
                    _currentTargetFinder = (ImageTargetFinder) ObjectTracker?.GetTargetFinder<ImageTargetFinder>();
                return _currentTargetFinder;
            }
            protected set => _currentTargetFinder = value;
        }

        /// <summary>
        /// Whether Vuforia's engine is active or not.
        /// </summary>
        public override bool IsArProviderActive => VuforiaBehaviour.Instance != null && VuforiaBehaviour.Instance.enabled && _isActive;

        protected bool _isActive;

        //Hack-Init Variables
        protected static float cameraDepthBackup = -99999f;
        static BaseVuforiaProviderHandler _providerInChargeOfHack;
        [Tooltip("Loads the credentials from the Image Recognition module in the EMS")]
        public bool useExternalVuforiaLicense = true;
        static BaseVuforiaProviderHandler _providerInChargeOfLicense;

        //Storage
        /// <summary>
        /// 
        /// </summary>
        protected List<VuforiaTargetEventHandler> RegisteredExperiences { get; private set; } = new List<VuforiaTargetEventHandler>();

        //Internal Variables
        protected virtual Vector2 ClippingPlanes => new Vector2(10, 5000);

        #region Init

        protected virtual void Awake()
        {
            //HACK, enable Vuforia Camera during initialization.
            //This needs to happen after awake but before the app runs.
            if (ProviderCamera.depth > -100)
            {
                cameraDepthBackup = ProviderCamera.depth;
                ProviderCamera.depth = -999f;
                ProviderCamera.enabled = true;
                _providerInChargeOfHack = this;
            }
        }

        protected virtual IEnumerator Start()
        {
            yield return new WaitForSeconds(1);

            //Restore camera
            if (_providerInChargeOfHack == this)
            {
                ProviderCamera.enabled = false;
                ProviderCamera.depth = cameraDepthBackup;
            }
        }

        public virtual void InitializeVuforia()
        {
            VuforiaBehaviour.Instance.enabled = true;
            VuforiaRuntime.Instance.InitVuforia();
        }

        /// <summary>
        /// Get the license set up for Vuforia.
        /// </summary>
        /// <returns></returns>
        public override IEnumerator SetupLicense()
        {
            if (useExternalVuforiaLicense && _providerInChargeOfLicense == null)
            {
                _providerInChargeOfLicense = this;
                yield return VuforiaLicenseFetcher.FetchVuforiaLicense();
            }
        }

        #endregion

        #region ArProvider State

        /// <summary>
        /// Initializes or de-initializes the ArProvider.
        /// </summary>
        /// <param name="shouldBeEnabled"></param>
        public override void ChangeArProviderActiveState(bool shouldBeEnabled)
        {
            _isActive = shouldBeEnabled;

            if (VuforiaManager.Instance != null)
            {
                ProviderCamera.enabled = shouldBeEnabled;

                if (!VuforiaManager.Instance.Initialized)
                    InitializeVuforia();
                else if (VuforiaBehaviour.Instance.enabled != shouldBeEnabled && VuforiaManager.Instance.Initialized)
                    VuforiaBehaviour.Instance.enabled = shouldBeEnabled;
            }

            if (shouldBeEnabled)
            {
                ProviderCamera.nearClipPlane = ClippingPlanes.x;
                ProviderCamera.farClipPlane = ClippingPlanes.y;
            }
        }

        public override void RefocusCamera()
        {
            CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
        }

        #endregion

        #region Experience State

        /// <summary>
        /// Called by the EventHandler.
        /// In most cases, we'll just be letting the experience about the new update.
        /// </summary>
        /// <param name="experience"></param>
        /// <param name="previousState"></param>
        /// <param name="currentState"></param>
        public virtual void ExperienceStateUpdated(ArExperience experience, AppearitionArHandler.TargetState previousState, AppearitionArHandler.TargetState currentState)
        {
            experience.ArProviderTrackingStateChanged(currentState);
            AppearitionArHandler.ArProviderTargetStateChanged(experience, currentState);
        }

        /// <inheritdoc />
        public override AppearitionArHandler.TargetState GetExperienceTargetState(Experience experience)
        {
            if (experience == null || experience.Data == null)
            {
                var first = RegisteredExperiences.HiziFirstOrDefault();
                if (first != null)
                    return first.CurrentTargetState;
                return AppearitionArHandler.TargetState.None;
            }

            for (int i = 0; i < RegisteredExperiences.Count; i++)
            {
                if(RegisteredExperiences[i] == null)
                    continue;
                if (RegisteredExperiences[i].ExperienceRef == null || RegisteredExperiences[i].ExperienceRef.Data == null)
                    continue;
                if (RegisteredExperiences[i].ExperienceRef.Data.assetId.Equals(experience.Data.assetId, StringComparison.InvariantCultureIgnoreCase))
                    return RegisteredExperiences[i].LastTargetState;
            }

            return AppearitionArHandler.TargetState.None;
        }

        #endregion
    }
}