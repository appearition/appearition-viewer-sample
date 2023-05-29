using System.Collections;
using System.Collections.Generic;
using Appearition.ArTargetImageAndMedia;
using Appearition.Common.ListExtensions;
using UnityEngine;

namespace Appearition.ArDemo.Standard
{
    /// <summary>
    /// Sample non-AR provider.
    /// The purpose of this "provider" is to simply display experience on the scene without any form of tracking.
    /// As such, it's considered to be an Offline Markerless Provider. 
    /// </summary>
    public class StandardArProvider : BaseArProviderHandler, IOfflineMarkerlessArProviderHandler
    {
        //References
        [SerializeField] Camera _camera;
        List<ArExperience> _activeExperiences = new List<ArExperience>();

        public override bool IsScanning => false;

        //Settings
        /// <summary>
        /// Whether to enable/disable the camera when the ArProvider becomes active, or just let the camera be.
        /// </summary>
        [Tooltip("Whether to enable/disable the camera when the ArProvider becomes active, or just let the camera be.")]
        public bool toggleCameraWithActiveState = true;

        public override Camera ProviderCamera => _camera;

        public override void ChangeArProviderActiveState(bool shouldBeEnabled)
        {
            if (toggleCameraWithActiveState)
                _camera.enabled = shouldBeEnabled;
            
            //Clear all experiences just in case
            ClearExperiencesBeingTracked();
        }

        public override AppearitionArHandler.TargetState GetExperienceTargetState(Experience experience)
        {
            if (_activeExperiences == null || _activeExperiences.Count == 0)
                return AppearitionArHandler.TargetState.None;

            var tmp = _activeExperiences.HiziFirstOrDefault(o => o.Data != null && o.Data.assetId.Equals(experience.Data.assetId));

            //In this case, if the experience exists, it's found and active
            return tmp != null ? AppearitionArHandler.TargetState.TargetFound : AppearitionArHandler.TargetState.None;
        }

        public IEnumerator LoadSelectedExperience(Asset asset)
        {
            bool? isSuccess = default;
            var experience = AppearitionArHandler.CreateExperienceFromAsset<ArExperience>(this, asset, true, success => isSuccess = success);
            _activeExperiences.Add(experience);
            
            experience.ArProviderTrackingStateChanged(AppearitionArHandler.TargetState.TargetFound);

            while (!isSuccess.HasValue)
                yield return null;
            
            //No placing needs to be done either.
            if (isSuccess.GetValueOrDefault())
                AppearitionArHandler.ArProviderTargetStateChanged(experience, GetExperienceTargetState(experience));
            else
            {
                AppearitionLogger.LogWarning($"The experience of name {asset.name} and id {asset.assetId} has failed to load and will be cleared.");
                ClearSingleExperience(experience);
            }
        }

        //This functionality doesn't make much sense in this mode, but we can reload the experience as a whole.
        public override void ResetTracking()
        {
            var asset = _activeExperiences.HiziFirstOrDefault()?.Data;

            ClearExperiencesBeingTracked();

            if (asset != null)
                StartCoroutine(LoadSelectedExperience(asset));
        }

        public override void ClearExperiencesBeingTracked()
        {
            for (int i = _activeExperiences.Count - 1; i >= 0; i--)
            {
                ClearSingleExperience(_activeExperiences[i]);
            }
            _activeExperiences.Clear();
        }

        void ClearSingleExperience(ArExperience experience)
        {
            if (experience != null)
            {
                experience.UnloadContent();
                Destroy(experience.gameObject);
            }
        }
    }
}