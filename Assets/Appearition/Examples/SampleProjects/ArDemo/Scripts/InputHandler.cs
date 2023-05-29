// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: "InputHandler.cs" 
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using System.Collections.Generic;
using Lean;
using Lean.Common;
using Lean.Touch;
using UnityEngine;

namespace Appearition.ArDemo
{
    /// <summary>
    /// Class in charge of interfacing with the selected input system, and contains events for every type of inputs which should be implemented by any class needing them.
    /// </summary>
    public class InputHandler : MonoBehaviour
    {
        #region Internal Singleton

        static InputHandler _instance;

        /// <summary>
        /// Internal instance of the input handler. Used to connect unity events to the static variables.
        /// </summary>
        protected static InputHandler Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<InputHandler>();
                    if (_instance == null)
                        _instance = new GameObject("Input Handler").AddComponent<InputHandler>();
                }

                return _instance;
            }
        }

        #endregion

        #region Events

        public delegate void InputStateChanged(bool areInputsEnabled);

        /// <summary>
        /// Occurs whenever the value of AreInputsEnabled have been changed.
        /// </summary>
        public static event InputStateChanged OnInputStateChanged;

        public delegate void SingleFingerTouchDown(Vector2 pos);

        /// <summary>
        /// Occurs whenever a touch has been recognized as a single finger touch. Provides the position of the touch in screen space.
        /// </summary>
        public static event SingleFingerTouchDown OnSingleFingerTouchDown;
        
        public delegate void SingleFingerTouch(Vector2 pos);

        /// <summary>
        /// Occurs whenever a touch has been recognized as a single finger touch. Provides the position of the touch in screen space.
        /// </summary>
        public static event SingleFingerTouch OnSingleFingerTouch;


        public delegate void SingleFingerOngoingHold(Vector2 currentScreenPos, Vector2 scaledDeltaPos);

        /// <summary>
        /// Occurs every frame when a single finger hold is happening. Updates with the actual screen position and scaled delta screen position.
        /// </summary>
        public static event SingleFingerOngoingHold OnSingleFingerOngoingHold;


        public delegate void AutoRotateToggled(bool toggleState);

        public static event AutoRotateToggled OnAutoRotateToggled;


        public delegate void DoubleFingerOngoingHold(Vector2 screenCenterPos, Vector2 scaledCenterDeltaPos, float scaledDistanceDelta);

        /// <summary>
        /// Occurs every frame when a two-fingers gesture is ongoing. Updates gesture's center position in screen space, the scaled delta position, and the pinch scale.
        /// </summary>
        public static event DoubleFingerOngoingHold OnDoubleFingerOngoingHold;

        #endregion

        //Internal Variables
        static bool _areInputsEnabled = false;


        /// <summary>
        /// Whether or not the inputs are currently enabled. If false, no events will be called.
        /// </summary>
        public static bool AreInputsEnabled
        {
            get => _areInputsEnabled;
            set
            {
                _areInputsEnabled = value;
                OnInputStateChanged?.Invoke(_areInputsEnabled);
            }
        }


        //Internal Variable
        [Header("Input Settings")] public bool areInputEnabledOnStart = false;
        [Tooltip("The distance in screen space before a touch is recognized as a hold, used for rotation, etc.")]
        public float screenDistanceThresholdForHold = 5f;
        public float timeBeforeATouchIsConsideredHold = 0.3f;
        List<LeanFinger> _reusableFingerList;
        bool _isOneFingerHoldOngoing = false;

        void Start()
        {
            if (FindObjectOfType<LeanTouch>() == null)
                gameObject.AddComponent<LeanTouch>();
            if (areInputEnabledOnStart)
                AreInputsEnabled = true;
        }

        public void AutoRotoate(bool state)
        {
            OnAutoRotateToggled(state);
        }

        void Update()
        {
            if (!AreInputsEnabled)
                return;

            //Get the fingers
            _reusableFingerList = LeanTouch.GetFingers(true, false);
            
            if(Input.GetMouseButtonDown(0) && Input.touchCount <= 1)
                OnSingleFingerTouchDown?.Invoke(Input.touchCount == 0 ? (Vector2)Input.mousePosition : Input.GetTouch(0).position);

            //Check how many fingers there are, and treat the outcome accordingly
            if (_reusableFingerList?.Count > 0)
            {
                switch (_reusableFingerList.Count)
                {
                    case 1:
                        //1 Finger? Check if it's a touch or a hold. But first, check if the current 1 finger press has become a touch.
                        if (!_isOneFingerHoldOngoing)
                        {
                            if (_reusableFingerList[0].ScaledDelta.magnitude > screenDistanceThresholdForHold || _reusableFingerList[0].Age > timeBeforeATouchIsConsideredHold)
                                _isOneFingerHoldOngoing = true;
                        }

                        //Now that the flags might have changed, handle them
                        if (_isOneFingerHoldOngoing)
                        {
                            //Call the one finger hold event.
                            OnSingleFingerOngoingHold?.Invoke(_reusableFingerList[0].ScreenPosition, _reusableFingerList[0].ScaledDelta);
                        }
                        else
                        {
                            //Check if the finger goes up. If so, call the single tap event.
                            if (_reusableFingerList[0].Up)
                            {
                                OnSingleFingerTouch?.Invoke(_reusableFingerList[0].ScreenPosition);
                            }
                        }

                        break;
                    case 2:
                        //Just send the finger pinch data.
                        Vector2 pinchCenterPosition = LeanGesture.GetScreenCenter(_reusableFingerList);
                        Vector2 pinchCenterDeltaPosition = LeanGesture.GetScaledDelta(_reusableFingerList);
                        float scaledDistanceDelta = LeanGesture.GetScaledDistance(_reusableFingerList) - LeanGesture.GetLastScaledDistance(_reusableFingerList);

                        OnDoubleFingerOngoingHold?.Invoke(pinchCenterPosition, pinchCenterDeltaPosition, scaledDistanceDelta);
                        break;
                    default:
                        //Zero or more than 2? Reset current input flags.
                        ResetInputFlags();
                        break;
                }
            }
            else
            {
                //Reset flags if no inputs are currently active
                ResetInputFlags();
            }
        }

        void ResetInputFlags()
        {
            _isOneFingerHoldOngoing = false;
        }
    }
}