// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: AppearitionGate.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Appearition.API;
using System.IO;
using Appearition.Common;
using Appearition.Common.ObjectExtensions;
using Appearition.Profile;
using UnityEngine.Networking;

namespace Appearition
{
    /// <inheritdoc />
    /// <summary>
    /// Handle of the Core SDK. Contains the current profile being loaded, handling of internet connectivity, common API and all common and shared utilities.
    /// </summary>
    public class AppearitionGate : MonoBehaviour
    {
        #region Singleton

        private static AppearitionGate _instance;

        public static AppearitionGate Instance
        {
            get
            {
                //Get
                if (_instance == null)
                {
                    _instance = FindObjectOfType<AppearitionGate>();
                    ////Create
                    //if (_instance == null)
                    //{
                    //    _instance = new GameObject("Appearition Gate").AddComponent<AppearitionGate>();

                    //    AppearitionLogger.Log("A new AppearitionGate object has been created and placed in the scene.", AppearitionLogger.LogLevel.Info);
                    //}
                }

                return _instance;
            }
        }

        #endregion

        #region Debug

        //EDITOR DEBUG UTILITIES
        #if UNITY_EDITOR
        /// <summary>
        /// Only in editor mode, fakes no internet connectivity.
        /// </summary>
        public bool debugSimulateNoInternetConnection = false;

        #endif

        //COMMON DEBUG UTILITIES

        [SerializeField] private AppearitionLogger.LogLevel _logLevel = AppearitionLogger.LogLevel.Warning;

        /// <summary>
        /// Defines whether or not the Appearition SDK should log any message in the console. Either way, _errors will be logged properly.
        /// </summary>
        public static AppearitionLogger.LogLevel LogLevel
        {
            get
            {
                try
                {
                    return Instance._logLevel;
                } catch (NullReferenceException)
                {
                } catch (MissingReferenceException)
                {
                    return AppearitionLogger.LogLevel.Warning;
                }

                return AppearitionLogger.LogLevel.Warning;
            }
            set { Instance._logLevel = value; }
        }

        #endregion

        #region profile

        [SerializeField] private UserProfile _currentUser;
        UserProfile _backupDefaultUser;

        /// <summary>
        /// Gets the current user ready to be used for Appearition API requests using values entered in the Appearition Engine Manager.
        /// </summary>
        /// <value>The current user.</value>
        public UserProfile CurrentUser
        {
            get
            {
                if (Application.isPlaying && _currentUser == null)
                {
                    _currentUser = UserProfile.CreateInstance<UserProfile>();
                }

                return _currentUser;
            }
            set { _currentUser = value; }
        }


        /// <summary>
        /// Reverts the current profile to a new instance based on the default profile, as set in the inspector during editor time.
        /// </summary>
        public void RevertCurrentUserToDefault()
        {
            if (CurrentUser != null && _backupDefaultUser != null)
            {
                _currentUser = null;
                CurrentUser = UserProfile.CreateCopy<UserProfile>(_backupDefaultUser);
            }

            AppearitionLogger.LogInfo("The Current Profile has been reset to default.");
        }

        #endregion

        /// <summary>
        /// Only used in WEBGL. Storage for the Bundle Identifier when not available in the player settings.
        /// </summary>
        public string appBundleIdentifier;

        public bool forceSingleInstance = false;

        #region Init

        /// <summary>
        /// Storage for the time of the last EMS connection test.
        /// </summary>
        private static DateTime _timeAtLastPing;

        /// <summary>
        /// Storage variable for HasInternetAccessToEMS.
        /// </summary>
        private static bool? _hasInternetAccessToEms;

        /// <summary>
        /// Checks for any internet access, including access to the EMS, on launch.
        /// A null value means that the check has not been made yet.
        /// </summary>
        /// <value>The has internet access to EM.</value>
        public static bool? HasInternetAccessToEms
        {
            get
            {
                //Handle debug no internet simulation
                #if UNITY_EDITOR
                if (Instance.debugSimulateNoInternetConnection)
                {
                    _hasInternetAccessToEms = false;
                    return _hasInternetAccessToEms;
                }
                #endif

                //Check if it needs to do a random EMS ping to ensure the program is connected to it. Do a test every few minutes or so when the connection is working.
                if (_hasInternetAccessToEms.HasValue)
                {
                    if ((_hasInternetAccessToEms.Value && DateTime.Now.Subtract(_timeAtLastPing).TotalSeconds >
                         AppearitionConstants.DELAY_BETWEEN_EACH_EMS_REACHABILITY_CHECK_IN_SECONDS) ||
                        //(!_hasInternetAccessToEms.Value && DateTime.Now.Subtract(_timeAtLastPing).TotalSeconds >
                        // AppearitionConstants.DELAY_BETWEEN_EACH_EMS_REACHABILITY_CHECK_IN_SECONDS / 10) || 
                        !_hasInternetAccessToEms.Value)
                    {
                        AppearitionLogger.Log("Verifying connection with EMS..", AppearitionLogger.LogLevel.Info);
                        _hasInternetAccessToEms = null;
                        PingEms(outcome => { _hasInternetAccessToEms = outcome; });
                    }
                }

                return _hasInternetAccessToEms;
            }
            protected set { _hasInternetAccessToEms = value; }
        }

        //Do an internet test on init. This script should rely only on itself.
        private void Awake()
        {
            if (forceSingleInstance && Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }

            //Create a working copy of the profile
            if (CurrentUser != null)
            {
                _backupDefaultUser = CurrentUser;
                CurrentUser = UserProfile.CreateCopy<UserProfile>(CurrentUser);
            }

            #if UNITY_EDITOR
            if (debugSimulateNoInternetConnection)
                return;
            #endif

            PingEms(outcome => { HasInternetAccessToEms = outcome; });
        }

        #endregion

        #region Ping

        /// <summary>
        /// Attempts to pings the EMS, and returns whether or not the ping was successful.
        ///
        /// API Requirement: Anonymous Token.
        /// </summary>
        /// <code>
        /// IEnumerator WaitForPing()
        /// {
        ///     bool? isPingSuccessful = default;
        ///
        ///     StartCoroutine(PingEms(obj => { isPingSuccessful = obj; }));
        ///
        ///     while(!isPingSuccessful.HasValue)
        ///         yield return null;
        ///
        ///     if(isPingSuccessful.Value) {
        ///         Debug.Log("Ping successfully completed!");
        ///     } else {
        ///         Debug.LogError("Unable to reach the EMS.");
        ///     }
        /// }
        /// </code>
        /// <param name="callback">Callback.</param>
        public static void PingEms(Action<bool> callback)
        {
            Instance.StartCoroutine(PingEmsProcess(callback));
        }

        private static IEnumerator PingEmsProcess(Action<bool> callback)
        {
            Health_Ping pingOutcome = null;

            //Launch ping !
            AppearitionRequest<Health_Ping>.LaunchAPICall_GET<BaseHandler>(Instance.CurrentUser.selectedChannel, 
                BaseHandler.GetReusableApiRequest<Health_Ping>(), null, obj => { pingOutcome = obj; });
            
            //Handle timeout and request
            float timer = AppearitionConstants.EMS_PING_TIMEOUT_SECONDS;
            while (pingOutcome == null)
            {
                timer -= Time.deltaTime;

                //Ping failed.
                if (timer <= 0)
                {
                    AppearitionLogger.LogError(string.Format("EMS Ping timeout. Unable to reach the EMS within {0} seconds.", AppearitionConstants.EMS_PING_TIMEOUT_SECONDS));
                    pingOutcome = new Health_Ping();
                    break;
                }

                yield return null;
            }

            AppearitionLogger.Log(pingOutcome != null && pingOutcome.IsSuccess ? "Ping to the EMS was successful." : "The EMS is unreachable.",
                AppearitionLogger.LogLevel.Info);

            //Store the last ping ApiData
            _timeAtLastPing = DateTime.Now;
            //callback ~
            if (callback != null)
                callback(pingOutcome != null && pingOutcome.IsSuccess);
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Using the format approved by the EMS, fetches the current date and time.
        /// This information will need to be used for the Learn Session, which you may want to submit if using this feature.
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentDateAndTime()
        {
            return ConvertDateTimeToString(DateTime.Now);
        }

        public static DateTime ConvertStringToDateTime(string dateTimeString)
        {
            return DateTime.Parse(dateTimeString);
        }

        public static string ConvertDateTimeToString(DateTime date)
        {
            return date.ToString("yyyy-MM-dd HH:mm:ss tt");
        }

        public static string GenerateNewGUID()
        {
            return Guid.NewGuid().ToString();
        }

        #endregion
    }
}