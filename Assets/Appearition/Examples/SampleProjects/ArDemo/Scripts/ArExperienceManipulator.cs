using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Appearition.Common.ListExtensions;
using UnityEngine;

namespace Appearition.ArDemo
{
    public class ArExperienceManipulator : MonoBehaviour
    {
        #region Internal Singleton

        static ArExperienceManipulator _instance;

        public static ArExperienceManipulator Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<ArExperienceManipulator>();
                return _instance;
            }
        }

        #endregion

        #region Events

        public delegate void ManipulationStateChanged(bool isCurrentlyManipulating);

        /// <summary>
        /// Occurs whenever the state of manipulation of the model has been changed. If true, the model is currently being translated, rotated or scaled.
        /// </summary>
        public static event ManipulationStateChanged OnManipulationStateChanged;


        public delegate void ModelTransformReset();

        /// <summary>
        /// Occurs whenever the model transform values are reset. Most likely called by the user via UI.
        /// </summary>
        public static event ModelTransformReset OnModelTransformReset;

        public delegate void ManipulationActiveStateChanged(bool isActive);

        /// <summary>
        /// Occurs whenever IsModelManipulationActive value changes. Determines whether or not the model can currently be manipulated or not.
        /// </summary>
        public static event ManipulationActiveStateChanged OnManipulationActiveStateChanged;

        #endregion

        //References
        /// <summary>
        /// Reference to the current camera used for manipulation.
        /// </summary>
        public Camera CurrentCamera => AppearitionArHandler.ProviderHandler?.ProviderCamera;

        //Internal Variables
        Vector3 _backupModelPos;
        Quaternion _backupModelRot;
        Vector3 _backupModelScale;
        [Header("Manipulation Settings")] public float translationSpeed = 0.3f;
        public float rotationSpeed = 5f;
        [Tooltip("Relative to the Provider's manipulation multiplier.")]
        public float scalingSpeed = 0.5f;
        [Tooltip("Relative to the Provider's manipulation multiplier.")]
        public Vector2 scaleMinMax = new Vector2(0.1f, 10f);
        bool _wasManipulatedThisFrame = false;
        bool _wasManipulatedLastFrame = false;
        Coroutine autoRotate = null;
        public float autoRotateSpeed = 1f;

        /// <summary>
        /// Current object being manipulated
        /// </summary>
        public Transform CurrentInteractiveObject { get; set; } = null;


        //Properties 
        static bool _isModelManipulationActive;

        /// <summary>
        /// Whether or not the model can be manipulated. Toggles when a new model becomes selected.
        /// </summary>
        public static bool IsModelManipulationActive
        {
            get => _isModelManipulationActive;

            protected set
            {
                _isModelManipulationActive = value;
                OnManipulationActiveStateChanged?.Invoke(_isModelManipulationActive);
            }
        }

        public static bool AllowTranslation => AppearitionArHandler.ProviderHandler?.AllowTranslation ?? false;
        public static bool AllowRotation => AppearitionArHandler.ProviderHandler?.AllowRotation ?? false;
        public static bool AllowScaling => AppearitionArHandler.ProviderHandler?.AllowScaling ?? false;


        void Awake()
        {
            //Subscribes to events
            InputHandler.OnDoubleFingerOngoingHold += InputHandler_OnDoubleFingerOngoingHold;
            InputHandler.OnSingleFingerOngoingHold += InputHandler_OnSingleFingerOngoingHold;
            InputHandler.OnAutoRotateToggled += InputHandler_OnAutoRotateToggled;
            AppearitionArHandler.OnTargetStateChanged += AppearitionArHandler_OnTargetStateChanged;
        }

        private void InputHandler_OnAutoRotateToggled(bool toggleState)
        {
            if (toggleState)
            {
                autoRotate = StartCoroutine(InputHandler_AutoRotate());
            }
            else
            {
                if (autoRotate != null)
                {
                    StopCoroutine(autoRotate);
                    autoRotate = null;
                }
            }
        }

        void OnDestroy()
        {
            InputHandler.OnDoubleFingerOngoingHold -= InputHandler_OnDoubleFingerOngoingHold;
            InputHandler.OnSingleFingerOngoingHold -= InputHandler_OnSingleFingerOngoingHold;
            AppearitionArHandler.OnTargetStateChanged -= AppearitionArHandler_OnTargetStateChanged;
            InputHandler.OnAutoRotateToggled += InputHandler_OnAutoRotateToggled;
            StopCoroutine(InputHandler_AutoRotate());
        }

        #region Setup Model To Manipulate

        private void AppearitionArHandler_OnTargetStateChanged(ArExperience experience, AppearitionArHandler.TargetState newState)
        {
            var experiences = experience?.transform.parent?.GetComponentsInChildren<ArExperience>().ToList();
            if (experiences == null)
                experiences = FindObjectsOfType<ArExperience>().ToList();
            int interactiveExp = experiences?.FindIndex(o => o.IsInteractive && o.IsCurrentlyTracking) ?? -1;
            bool shouldBeInteractive = interactiveExp >= 0;

            if (shouldBeInteractive)
                CurrentInteractiveObject = experiences[interactiveExp].MediaContentHolder != null ? experiences[interactiveExp].MediaContentHolder : experiences[interactiveExp].transform;
            else
                CurrentInteractiveObject = null;

            IsModelManipulationActive = CurrentInteractiveObject != null;
        }

        #endregion


        /// <summary>
        /// On single finger hold, if there's any model, rotate the model.
        /// </summary>
        /// <param name="currentScreenPos"></param>
        /// <param name="scaledDeltaPos"></param>
        private void InputHandler_OnSingleFingerOngoingHold(Vector2 currentScreenPos, Vector2 scaledDeltaPos)
        {
            //No camera, no manipulation.
            if (CurrentCamera == null || CurrentInteractiveObject == null)
                return;

            if (IsModelManipulationActive && AllowRotation)
            {
                //Vector3 rotationDelta = (CurrentCamera.transform.right * scaledDeltaPos.y + CurrentInteractiveObject.transform.forward * -scaledDeltaPos.x) * rotationSpeed * Time.deltaTime;
                //CurrentInteractiveObject.RotateAround(CurrentInteractiveObject.position, rotationDelta.normalized, rotationDelta.magnitude);
                Vector3 centerPoint = CurrentInteractiveObject.transform.position;

                if (CurrentInteractiveObject.transform.childCount > 0)
                {
                    Vector3 centers = Vector3.zero;
                    for (int i = 0; i < CurrentInteractiveObject.childCount; i++)
                        centers += CurrentInteractiveObject.GetChild(i).position;
                    centerPoint = centers / CurrentInteractiveObject.childCount;
                }

                //CurrentInteractiveObject.RotateAround(CurrentInteractiveObject.position, CurrentInteractiveObject.up, -scaledDeltaPos.x * rotationSpeed * Time.deltaTime);
                CurrentInteractiveObject.RotateAround(centerPoint, CurrentInteractiveObject.up, -scaledDeltaPos.x * rotationSpeed * Time.deltaTime);

                _wasManipulatedThisFrame = true;
            }
        }


        /// <summary>
        /// AutoRotate
        /// </summary>
        /// <returns></returns>
        private IEnumerator InputHandler_AutoRotate()
        {
            while (true)
            {
                InputHandler_OnSingleFingerOngoingHold(Vector2.zero, new Vector2(autoRotateSpeed, 0f));
                //if (IsModelManipulationActive && AllowRotation)
                //{
                //    Vector3 centerPoint = CurrentInteractiveObject.transform.position;

                //    if (CurrentInteractiveObject.transform.childCount > 0)
                //    {
                //        Vector3 centers = Vector3.zero;
                //        for (int i = 0; i < CurrentInteractiveObject.childCount; i++)
                //            centers += CurrentInteractiveObject.GetChild(i).position;
                //        centerPoint = centers / CurrentInteractiveObject.childCount;
                //    }

                //    //CurrentInteractiveObject.RotateAround(CurrentInteractiveObject.position, CurrentInteractiveObject.up, -scaledDeltaPos.x * rotationSpeed * Time.deltaTime);
                //    CurrentInteractiveObject.RotateAround(centerPoint, CurrentInteractiveObject.up, rotationSpeed * Time.deltaTime);

                //    _wasManipulatedThisFrame = true;
                //}
                yield return null;
            }
        }

        /// <summary>
        /// On double fingers hold, if any model, translate and scale the mode.
        /// </summary>
        /// <param name="screenCenterPos"></param>
        /// <param name="scaledCenterDeltaPos"></param>
        /// <param name="scaledDistanceDelta"></param>
        private void InputHandler_OnDoubleFingerOngoingHold(Vector2 screenCenterPos, Vector2 scaledCenterDeltaPos, float scaledDistanceDelta)
        {
            //No camera, no manipulation.
            if (CurrentCamera == null || CurrentInteractiveObject == null)
                return;

            if (IsModelManipulationActive)
            {
                if (AllowTranslation)
                {
                    CurrentInteractiveObject.position += (CurrentCamera.transform.right * scaledCenterDeltaPos.x + CurrentCamera.transform.up * scaledCenterDeltaPos.y) *
                                                         AppearitionArHandler.ProviderHandler.ManipulationMultiplier * translationSpeed * Time.deltaTime;
                }

                if (AllowScaling)
                {
                    CurrentInteractiveObject.localScale = Mathf.Clamp(
                                                              CurrentInteractiveObject.localScale.x + scaledDistanceDelta * scalingSpeed *
                                                              AppearitionArHandler.ProviderHandler.ManipulationScaleMultiplier * Time.deltaTime,
                                                              scaleMinMax.x * AppearitionArHandler.ProviderHandler.ManipulationScaleMultiplier,
                                                              scaleMinMax.y * AppearitionArHandler.ProviderHandler.ManipulationScaleMultiplier) *
                                                          Vector3.one;
                }

                _wasManipulatedThisFrame = true;
            }
        }

        void LateUpdate()
        {
            //After update, check if the model was manipulated. If not, then tell the event about it!
            if (!_wasManipulatedThisFrame && _wasManipulatedLastFrame)
            {
                OnManipulationStateChanged?.Invoke(false);
            }
            else if (_wasManipulatedThisFrame)
            {
                OnManipulationStateChanged?.Invoke(true);
            }

            //Refresh the flag value
            _wasManipulatedLastFrame = _wasManipulatedThisFrame;
            _wasManipulatedThisFrame = false;
        }

        /// <summary>
        /// Resets the model being manipulated by its default values.
        /// Also triggers the event.
        /// </summary>
        public void ResetModelTransform()
        {
            if (IsModelManipulationActive)
            {
                CurrentInteractiveObject.localPosition = _backupModelPos;
                CurrentInteractiveObject.localRotation = _backupModelRot;
                CurrentInteractiveObject.localScale = _backupModelScale;

                OnModelTransformReset?.Invoke();
            }
        }

        /// <summary>
        /// Resets the model being manipulated by its default values.
        /// </summary>
        public void ResetModelTransform(Transform target)
        {
            if (target != null)
            {
                target.localPosition = _backupModelPos;
                target.localRotation = _backupModelRot;
                target.localScale = _backupModelScale;
            }
        }

        /// <summary>
        /// Float Remap
        /// </summary>
        /// <param name="value"></param>
        /// <param name="from1"></param>
        /// <param name="to1"></param>
        /// <param name="from2"></param>
        /// <param name="to2"></param>
        /// <returns></returns>
        float Remap(float value, float from1, float to1, float from2, float to2)
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }
    }
}