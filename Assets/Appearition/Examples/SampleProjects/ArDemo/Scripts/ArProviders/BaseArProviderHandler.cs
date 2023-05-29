using System.Collections;
using UnityEngine;

namespace Appearition.ArDemo
{
    /// <summary>
    /// Template for an Ar Handler, in charge of interfacing the basic utilities for an Ar Provider's SDK.
    /// </summary>
    public abstract class BaseArProviderHandler : MonoBehaviour
    {
        /// <summary>
        /// Setup the license of the ArProvider, if need be.
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerator SetupLicense()
        {
            yield break;
        }

        /// <summary>
        /// Initialize or De-Initialize the ArProvider.
        /// In most cases, the camera should be DISABLED when the ArProvider is not active (otherwise, multiple providers will fight for camera ownership).
        /// </summary>
        /// <param name="shouldBeEnabled"></param>
        public abstract void ChangeArProviderActiveState(bool shouldBeEnabled);

        /// <summary>
        /// Whether the ArProvider should be looking for a new target. This only applies to single-target ArProviders.
        /// </summary>
        /// <param name="shouldScan"></param>
        public virtual void ChangeScanningState(bool shouldScan)
        {
        }

        /// <summary>
        /// Get the current state of a given Experience.
        /// If the experience is null, this method will return the state of the first active experience.
        /// </summary>
        /// <param name="experience"></param>
        /// <returns></returns>
        public abstract AppearitionArHandler.TargetState GetExperienceTargetState(Experience experience);

        /// <summary>
        /// Resets the experience. In the case of Markerless providers, this might mean "reset the placement of the experience";
        /// while for Marker-based provides, this might mean "reset and look for a new target".
        /// </summary>
        public abstract void ResetTracking();
        
        /// <summary>
        /// If any target is currently being tracked, destroys them and clears them.
        /// </summary>
        public abstract void ClearExperiencesBeingTracked();

        #region Functionality

        /// <summary>
        /// Some providers might require a different scale to represent objects normally. If not, set it to 1 by default.
        /// </summary>
        public virtual float ScaleMultiplier => 1;

        /// <summary>
        /// Multiplier applied to the manipulation of interactive experience handled by ArExperienceManipulator.
        /// </summary>
        public virtual float ManipulationMultiplier => 1f;

        public virtual float ManipulationScaleMultiplier => 1f;

        /// <summary>
        /// Used for manipulation. Sliding the finger horizontally will rotate on the vertical axis. Target-based experiences might use the Z axis, for instance.
        /// Other axis will reset on release.
        /// </summary>
        public virtual Vector3 VerticalAxisVector => Vector3.up;

        public virtual bool AllowTranslation { get; protected set; } = true;
        public virtual bool AllowRotation { get; protected set; } = true;
        public virtual bool AllowScaling { get; protected set; } = true;

        /// <summary>
        /// Refocus the camera manually. Do note that most ArProviders achieve this automatically.
        /// </summary>
        public virtual void RefocusCamera()
        {
        }

        #endregion

        #region Accessibility

        /// <summary>
        /// Used for inputs.
        /// </summary>
        public abstract Camera ProviderCamera { get; }

        #endregion

        #region Flags

        /// <summary>
        /// Defines whether or not the ArProvider is currently scanning.
        /// </summary>
        public virtual bool IsScanning { get; protected set; }

        /// <summary>
        /// Defines whether the ArProvider is currently initialized or not.
        /// </summary>
        public virtual bool IsArProviderInitialized { get; set; }

        /// <summary>
        /// Whether or not the current ArProvider is enabled.
        /// </summary>
        public virtual bool IsArProviderActive => AppearitionArHandler.ProviderHandler == this;

        #endregion
    }
}