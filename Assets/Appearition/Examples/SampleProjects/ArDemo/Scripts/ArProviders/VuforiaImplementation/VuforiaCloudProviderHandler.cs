#pragma warning disable 0649

using System.Collections;
using System.Linq;
using Vuforia;
using UnityEngine;
using Appearition.ArTargetImageAndMedia;

namespace Appearition.ArDemo.Vuforia
{
    public class VuforiaCloudProviderHandler : BaseVuforiaProviderHandler
    {
        //Handy Properties
        private CloudRecoBehaviour _cloudRecoB;

        public CloudRecoBehaviour CloudRecoB
        {
            get
            {
                if (_cloudRecoB == null)
                    _cloudRecoB = transform.parent.GetComponentInChildren<CloudRecoBehaviour>(true);
                return _cloudRecoB;
            }
        }

        /// <summary>
        /// Vuforia needs to scale everything by 640. Don't ask.
        /// </summary>
        public override float ScaleMultiplier => 480f; //640;

        public override float ManipulationScaleMultiplier => 480f; //640;

        public override Vector3 VerticalAxisVector => Vector3.forward;

        public override bool AllowTranslation => false;
        public override bool AllowRotation => false;

        //Internal Variables
        Coroutine _targetFoundProcess;

        protected override void Awake()
        {
            base.Awake();

            CloudRecoB.RegisterOnInitializedEventHandler(OnInitialized);
            CloudRecoB.RegisterOnInitErrorEventHandler(OnInitError);
            CloudRecoB.RegisterOnNewSearchResultEventHandler(OnNewSearchResult);
        }


        #region Init

        public void OnInitError(TargetFinder.InitState initError)
        {
            if (_isActive)
                Debug.LogError("Init error: " + initError);
            IsArProviderInitialized = false;
        }

        /// <summary>
        /// Occurs when the tracker is initialized.
        /// </summary>
        /// <param name="targetFinder">Target finder.</param>
        public virtual void OnInitialized(TargetFinder targetFinder)
        {
            IsArProviderInitialized = true;

            if (_isActive)
                Debug.Log("Cloud Handler initialized !");
            CurrentTargetFinder = targetFinder as ImageTargetFinder;
        }

        /// <summary>
        /// Step only required if the cloud reco was initialized, then de-initialized using DeInitTargetFinder to swap the credentials.
        /// </summary>
        public void InitTargetFinder()
        {
            if (CurrentTargetFinder != null &&
                CurrentTargetFinder.GetInitState() != TargetFinder.InitState.INIT_SUCCESS &&
                CurrentTargetFinder.GetInitState() != TargetFinder.InitState.INIT_RUNNING)
                CurrentTargetFinder.StartInit(CloudRecoB.AccessKey, CloudRecoB.SecretKey);
        }

        /// <summary>
        /// Step required before changing the Cloud reco credentials.
        /// </summary>
        /// <returns></returns>
        public bool DeInitTargetFinder()
        {
            if (CurrentTargetFinder != null && CurrentTargetFinder.GetInitState() == TargetFinder.InitState.INIT_SUCCESS)
            {
                CurrentTargetFinder.Deinit();
                return true;
            }

            return false;
        }

        #endregion

        #region ArProvider State

        /// <summary>
        /// Whether Vuforia should be doing cloud scan or not.
        /// </summary>
        /// <param name="shouldScan"></param>
        public override void ChangeScanningState(bool shouldScan)
        {
            IsScanning = shouldScan;

            if (CurrentTargetFinder == null)
                return;

            if (shouldScan)
                CurrentTargetFinder.StartRecognition();
            else
                CurrentTargetFinder.Stop();
        }

        /// <summary>
        /// If any target is currently being tracked, removes it and clears it. 
        /// </summary>
        public override void ClearExperiencesBeingTracked()
        {
            if (CurrentTargetFinder != null)
                CurrentTargetFinder.ClearTrackables();

            //Let the ArHandler know about the new Object's state.
            if (IsArProviderActive)
                AppearitionArHandler.ArProviderTargetStateChanged(null, AppearitionArHandler.TargetState.None);
        }

        /// <summary>
        /// In the case of Cloud Reco, the current target is to be cleaned and the provider to return to scanning mode.
        /// </summary>
        public override void ResetTracking()
        {
            ClearExperiencesBeingTracked();
            AppearitionArHandler.ChangeArProviderScanningState(true);
        }

        #endregion

        #region On Target Pickup

        public virtual void OnNewSearchResult(TargetFinder.TargetSearchResult targetSearchResult)
        {
            if (!IsArProviderActive)
                return;

            if (_targetFoundProcess != null)
            {
                AppearitionLogger.LogWarning("New target found but one target is already getting processed.");
                return;
            }

            TargetFinder.CloudRecoSearchResult cloudResult = (TargetFinder.CloudRecoSearchResult) targetSearchResult;

            AppearitionLogger.LogInfo(string.Format("Target found, AssetID:{0}", cloudResult.MetaData));

            _targetFoundProcess = StartCoroutine(TargetFoundProcess(cloudResult));
        }

        IEnumerator TargetFoundProcess(TargetFinder.CloudRecoSearchResult cloudResult)
        {
            //Stop scanning for now
            AppearitionArHandler.ChangeArProviderScanningState(false);
            bool? successfullyLoaded = default;

            //Create the experience
            Asset outcome = new Asset();
            outcome.assetId = cloudResult.MetaData;
            ArExperience experience = AppearitionArHandler.CreateExperienceFromAsset<ArExperience>(this, outcome, true, success => successfullyLoaded = success);

            //Vuforia requires the tracking to start on the same frame. Cannot wait until the result is ready..
            ImageTargetBehaviour imageTarget = (ImageTargetBehaviour) CurrentTargetFinder.EnableTracking(cloudResult, experience.gameObject);
            VuforiaTargetEventHandler eventHandler = experience.GetComponent<VuforiaTargetEventHandler>();

            if (CurrentTargetFinder != null)
            {
                if (eventHandler == null)
                    eventHandler = experience.gameObject.AddComponent<VuforiaTargetEventHandler>();

                //Register experience
                RegisteredExperiences.Add(eventHandler);
                eventHandler.Setup(this, experience, imageTarget);
                
            }

            while (!successfullyLoaded.HasValue)
                yield return null;

            if (!successfullyLoaded.GetValueOrDefault())
            {
                Debug.Log($"Unable to find the experience linked to the target of id {cloudResult.MetaData}");
                ClearExperiencesBeingTracked();
                AppearitionArHandler.ChangeArProviderScanningState(true);
            }

            _targetFoundProcess = null;
        }

        #endregion
    }
}