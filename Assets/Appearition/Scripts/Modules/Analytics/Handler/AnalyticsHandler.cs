using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Appearition.Analytics.API;
using Appearition.Common;
using UnityEngine;

namespace Appearition.Analytics
{
    /// <summary>
    /// Handles Analytics related API requests.
    /// When wishing to start a session or add content, refer to this handler.
    /// This handler will also manage analytics submission based on the application life cycle.
    /// </summary>
    public sealed class AnalyticsHandler : BaseHandler
    {
        /// <summary>
        /// Contains the current ongoing session, if any.
        /// In order to start a new session, call BeginSession. This variable will automatically be populated from the outcome.
        /// The session will be stored locally, and will by synced with the EMS when available.
        /// If a user logs out, you should end the session to sync the content back to the EMS.
        /// If the application closes, the content will automatically be synced next time the application is turned on using existing content.
        /// </summary>
        public static Session OngoingSession { get; private set; }

        static Coroutine _syncPendingAnalyticsCoroutine;

        #region Path Utilities

        /// <summary>
        /// Generates the path to a Session storage file using a session key.
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <returns></returns>
        public static string GetPathToSessionFile(string sessionKey)
        {
            return string.Format("{0}/{1}{2}", GetHandlerStoragePath<AnalyticsHandler>(), sessionKey, AnalyticsConstants.SESSION_FILE_EXTENSION);
        }

        /// <summary>
        /// Loads all the stored sessions.
        /// </summary>
        /// <returns></returns>
        static List<Session> LoadAllStoredSession()
        {
            List<Session> outcome = new List<Session>();
            string[] paths = Directory.GetFiles(GetHandlerStoragePath<AnalyticsHandler>(), string.Format("*{0}", AnalyticsConstants.SESSION_FILE_EXTENSION));

            for (int i = 0; i < paths.Length; i++)
            {
                try
                {
                    Session tmp = AppearitionConstants.DeserializeJson<Session>(File.ReadAllText(paths[i]));

                    Debug.Log(tmp.SessionKey + " was loaded");
                    if (tmp != null)
                        outcome.Add(tmp);
                } catch
                {
                }
            }

            return outcome;
        }

        /// <summary>
        /// Updates the local content of this current session using the content of the OnGoing session.
        /// This steps ensures that the content is not lost.
        /// </summary>
        public static void UpdateOngoingSessionLocalSave()
        {
            if (OngoingSession == null)
                return;

            string fullPath = GetPathToSessionFile(OngoingSession.SessionKey);
            if (!Directory.Exists(Path.GetDirectoryName(fullPath)))
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            File.WriteAllText(fullPath, AppearitionConstants.SerializeJson(OngoingSession));

            AppearitionLogger.LogDebug(string.Format(AnalyticsConstants.SESSION_CONTENT_SAVED_LOCALLY, OngoingSession.SessionKey));
        }

        #endregion

        #region Initialize Analytics

        /// <summary>
        /// Initializes the main analytics module.
        /// Ensures that any sessions leftovers are synced properly.
        /// By default, begins a new session.
        /// /// </summary>
        public static void InitializeAnalytics()
        {
            InitializeAnalytics(true);
        }

        /// <summary>
        /// Initializes the main analytics module.
        /// Ensures that any sessions leftovers are synced properly.
        /// Optionally, starts a new session as well.
        /// </summary>
        /// <param name="shouldStartSession"></param>
        /// <param name="onSessionStarted"></param>
        public static void InitializeAnalytics(bool shouldStartSession, Action<Session> onSessionStarted = null)
        {
            InitializeAnalytics(shouldStartSession ? AppearitionGate.Instance.CurrentUser.selectedTenant : "",
                shouldStartSession ? AppearitionGate.Instance.CurrentUser.selectedChannel : 0,
                onSessionStarted);
        }

        /// <summary>
        /// Initializes the main analytics module.
        /// Ensures that any sessions leftovers are synced properly.
        /// Optionally, starts a new session as well.
        /// /// </summary>
        /// <param name="tenantKey"></param>
        /// <param name="channelId"></param>
        /// <param name="onSessionStarted"></param>
        public static void InitializeAnalytics(string tenantKey, int channelId, Action<Session> onSessionStarted = null)
        {
            if (!string.IsNullOrEmpty(tenantKey))
            {
                BeginSession(tenantKey, channelId);

                if (onSessionStarted != null)
                    onSessionStarted(OngoingSession);
            }
        }

        #endregion

        #region Session management

        /// <summary>
        /// Begin a new Analytics Session. If any session was ongoing, it will be handled and closed.
        /// </summary>
        /// <returns></returns>
        public static Session BeginSession()
        {
            return BeginSession(AppearitionGate.Instance.CurrentUser.selectedTenant, AppearitionGate.Instance.CurrentUser.selectedChannel);
        }

        /// <summary>
        /// Begin a new Analytics Session. If any session was ongoing, it will be handled and closed.
        /// </summary>
        /// <param name="tenantKey">The target tenant to record this session against.</param>
        /// <param name="channelId">The target channel to record this session against.</param>
        /// <returns></returns>
        public static Session BeginSession(string tenantKey, int channelId)
        {
            //If there was a previous session, end it in parallel.
            if (OngoingSession != null)
                EndCurrentSession();

            OngoingSession = new Session(tenantKey, channelId);

            //Store it
            UpdateOngoingSessionLocalSave();

            //Sync
            SyncPendingAnalytics();

            AppearitionLogger.LogInfo(string.Format(AnalyticsConstants.SESSION_CREATED_INFO, OngoingSession.SessionKey));

            return OngoingSession;
        }

        /// <summary>
        /// Add a given activity to the current session. Ensures that it is properly set up and part of the session, but does not sync it with the EMS.
        /// Once the activity is considered complete, submit it using SubmitActivityToCurrentSession.
        /// </summary>
        /// <param name="activity"></param>
        /// <returns></returns>
        public static Session AddActivityToCurrentSession(Activity activity)
        {
            return AddActivityToSession(OngoingSession, activity);
        }

        static Session AddActivityToSession(Session session, Activity activity)
        {
            if (activity == null)
            {
                AppearitionLogger.LogError(AnalyticsConstants.ACTIVITY_NULL);
                return null;
            }

            if (string.IsNullOrEmpty(activity.ActivityCode))
            {
                AppearitionLogger.LogError(string.Format(AnalyticsConstants.ACTIVITY_NO_CODE, activity.ActivityKey));
                return null;
            }

            //If there is no ongoing session, start one.
            if (OngoingSession == null && session == null)
            {
                BeginSession();
                session = OngoingSession;
            }

            //Check the details, then sync it.
            if (string.IsNullOrEmpty(activity.SessionKey))
                activity.SessionKey = session.SessionKey;

            int indexIfAny = session.Activities.FindIndex(o => o.ActivityKey == activity.ActivityKey);

            if (indexIfAny > 0)
            {
                session.Activities[indexIfAny] = activity;
                AppearitionLogger.LogInfo(string.Format(AnalyticsConstants.ACTIVITY_SUCCESSFULLY_UPDATED_IN_SESSION, activity.ActivityKey, activity.SessionKey));
            }
            else
            {
                session.Activities.Add(activity);
                AppearitionLogger.LogInfo(string.Format(AnalyticsConstants.ACTIVITY_SUCCESSFULLY_ADDED_TO_SESSION, activity.ActivityKey, activity.SessionKey));
            }

            //Finally, save
            UpdateOngoingSessionLocalSave();

            return session;
        }

        /// <summary>
        /// Finalizes an activity as part of the current session, and syncs it with the EMS.
        /// </summary>
        /// <param name="activity"></param>
        /// <returns></returns>
        public static Session SubmitActivityToCurrentSession(Activity activity)
        {
            if (activity == null)
            {
                AppearitionLogger.LogError(AnalyticsConstants.ACTIVITY_NULL);
                return null;
            }

            if (string.IsNullOrEmpty(activity.ActivityCode))
            {
                AppearitionLogger.LogError(string.Format(AnalyticsConstants.ACTIVITY_NO_CODE, activity.ActivityKey));
                return null;
            }

            //Ensure the activity is bound to a session
            if (OngoingSession == null || string.IsNullOrEmpty(activity.SessionKey))
                AddActivityToCurrentSession(activity);


            //Submit it
            AppearitionGate.Instance.StartCoroutine(SubmitActivityToSessionProcess(OngoingSession, activity));

            return OngoingSession;
        }

        /// <summary>
        /// Finalizes an Analytics Session.
        /// While this process is optional, it ensures that the content on the EMS matches the local content,
        /// and that the end time of the session is recorded properly.
        /// </summary>
        public static void EndCurrentSession()
        {
            if (OngoingSession != null)
                AppearitionGate.Instance.StartCoroutine(EndSessionProcess(OngoingSession, isSuccess => { OngoingSession = null; }));
        }

        #endregion

        #region Content Sync with EMS

        /// <summary>
        /// Syncs the current local data with the EMS.
        /// If a sync process is currently ongoing, waits for it to be complete before starting it.
        /// </summary>
        public static void SyncPendingAnalytics()
        {
            //Prevent double-sync.
            AppearitionGate.Instance.StartCoroutine(SyncPendingAnalyticsProcessWhenAvailable());
        }

        /// <summary>
        /// Syncs the current local data with the EMS.
        /// If a sync process is currently ongoing, waits for it to be complete before starting it.
        /// </summary>
        /// <returns></returns>
        public static IEnumerator SyncPendingAnalyticsProcessWhenAvailable()
        {
            AppearitionLogger.LogInfo(AnalyticsConstants.SYNC_WITH_EMS_REQUESTED);

            while (_syncPendingAnalyticsCoroutine != null)
                yield return null;

            _syncPendingAnalyticsCoroutine = AppearitionGate.Instance.StartCoroutine(SyncPendingAnalyticsProcess());
        }

        /// <summary>
        /// Syncs the local data with the EMS.
        /// </summary>
        /// <returns></returns>
        static IEnumerator SyncPendingAnalyticsProcess()
        {
            //Load all local session data.
            List<Session> allStoredSessions = LoadAllStoredSession();

            int amountOfProcesses = 0;

            for (int i = 0; i < allStoredSessions.Count; i++)
            {
                if (allStoredSessions[i] == null)
                    continue;

                //Handle the start sync for the ongoing session if the sync hasn't been successful or done yet.
                if (OngoingSession != null && allStoredSessions[i].SessionKey == OngoingSession.SessionKey && !allStoredSessions[i].IsSyncedWithEms)
                {
                    amountOfProcesses++;
                    AppearitionGate.Instance.StartCoroutine(StartNewSessionProcess(allStoredSessions[i], b =>
                    {
                        if(OngoingSession != null)
                            OngoingSession.IsSyncedWithEms = b;
                        amountOfProcesses--;
                    }));
                }
                //Handle sessions which have begun but not ended or weren't synced. Those include past sessions.
                else if (OngoingSession == null || allStoredSessions[i].SessionKey != OngoingSession.SessionKey)
                {
                    amountOfProcesses++;
                    AppearitionGate.Instance.StartCoroutine(EndSessionProcess(allStoredSessions[i], b => amountOfProcesses--));
                }
            }

            //Handle main process with timeout. If a session successfully got submitted, reset timeout.
            //If the timeout goes off or all pending sessions are submitted, the process is complete.
            int previousAmount = amountOfProcesses;
            float timeoutTimer = AnalyticsConstants.SESSION_SYNC_TIMEOUT;

            while (amountOfProcesses > 0 && timeoutTimer > 0)
            {
                if (previousAmount != amountOfProcesses)
                    timeoutTimer = AnalyticsConstants.SESSION_SYNC_TIMEOUT;
                else
                    timeoutTimer -= Time.deltaTime;

                yield return null;
            }

            if (amountOfProcesses > 0 && timeoutTimer < 0)
                AppearitionLogger.LogInfo(AnalyticsConstants.SYNC_WITH_EMS_TIMEOUT);
            else
                AppearitionLogger.LogInfo(AnalyticsConstants.SYNC_WITH_EMS_SUCCESSFULLY_COMPLETED);


            //Empty variables
            _syncPendingAnalyticsCoroutine = null;
        }

        #endregion

        #region API Processes

        static IEnumerator StartNewSessionProcess(Session sessionData, Action<bool> onComplete = null)
        {
            if (sessionData == null)
                yield break;

            var startSessionRequest =
                AppearitionRequest<Analytics_StartSessionTracking>.LaunchAPICall_POST<AnalyticsHandler>(sessionData.ChannelId, GetReusableApiRequest<Analytics_StartSessionTracking>(), sessionData);

            while (!startSessionRequest.IsDone)
                yield return null;

            if (startSessionRequest.RequestResponseObject.IsSuccess)
            {
                AppearitionLogger.LogInfo(string.Format(AnalyticsConstants.SESSION_BEGAN_SUCCESS, sessionData.SessionKey));
                sessionData.IsSyncedWithEms = true;
            }
            else
                AppearitionLogger.LogError(string.Format(AnalyticsConstants.SESSION_BEGAN_FAILURE, sessionData.SessionKey));

            //Either way, store the session locally if it doesn't exist.
            UpdateOngoingSessionLocalSave();

            if (onComplete != null)
                onComplete(startSessionRequest.RequestResponseObject.IsSuccess);
        }

        static IEnumerator SubmitActivityToSessionProcess(Session sessionData, Activity newActivity, Action<bool> onComplete = null)
        {
            //Make sure that the activity has a Session Key.
            if (string.IsNullOrEmpty(newActivity.SessionKey) || sessionData.Activities.All(o => o.ActivityKey != newActivity.ActivityKey))
                AddActivityToSession(sessionData, newActivity);

            //If the Session hasn't been synced with the EMS yet, handle the syncing.
            if (!sessionData.IsSyncedWithEms)
            {
                yield return SyncPendingAnalyticsProcessWhenAvailable();

                while (_syncPendingAnalyticsCoroutine != null)
                    yield return null;

                //If the sync still failed, don't bother submitting the activity just now.
                //The content is stored, everything will be submitted as part of next sync.
                if (!sessionData.IsSyncedWithEms)
                {
                    Debug.LogError("=(");
                    yield break;
                }
            }

            //Find the end time as well.
            if (string.IsNullOrEmpty(newActivity.EndUtcDateTime))
            {
                if (string.IsNullOrWhiteSpace(newActivity.EndUtcDateTime))
                {
                    ActivityEvent lastEventWithTime = newActivity.GetLastActivityWithTimestamp();
                    newActivity.EndUtcDateTime = lastEventWithTime != null ? lastEventWithTime.EventUtcDateTime : newActivity.StartUtcDateTime;
                }
            }

            var addActivityRequest =
                AppearitionRequest<Analytics_AddSessionActivity>.LaunchAPICall_POST<AnalyticsHandler>(sessionData.ChannelId, GetReusableApiRequest<Analytics_AddSessionActivity>(), newActivity);

            while (!addActivityRequest.IsDone)
                yield return null;

            if (addActivityRequest.RequestResponseObject.IsSuccess)
                AppearitionLogger.LogInfo(string.Format(AnalyticsConstants.ACTIVITY_SYNCED_SUCCESS, newActivity.ActivityKey, newActivity.ActivityCode, sessionData.SessionKey));
            else
                AppearitionLogger.LogError(string.Format(AnalyticsConstants.ACTIVITY_SYNCED_FAILURE, newActivity.ActivityKey, newActivity.ActivityCode));

            if (onComplete != null)
                onComplete(addActivityRequest.RequestResponseObject.IsSuccess);
        }

        static IEnumerator EndSessionProcess(Session sessionData, Action<bool> onComplete = null)
        {
            if (sessionData == null)
                yield break;

            //As part of the process, check if there is any missing information, like session end time.
            if (string.IsNullOrEmpty(sessionData.EndUtcDateTime))
            {
                //If the last activity had an end time, use that one.
                if (sessionData.Activities?.Count > 0)
                {
                    Activity tmpActivity = sessionData.Activities.Last();

                    if (!string.IsNullOrEmpty(tmpActivity.EndUtcDateTime))
                        sessionData.EndUtcDateTime = tmpActivity.EndUtcDateTime;
                    else
                    {
                        //Find the last event entered for that event, and send it to both the activity end time and the session end time.
                        //If all fail, then the last activity's start time becomes the end time of the last activity and session.
                        ActivityEvent lastEventWithTime = tmpActivity.GetLastActivityWithTimestamp();
                        sessionData.EndUtcDateTime = lastEventWithTime != null ? lastEventWithTime.EventUtcDateTime : tmpActivity.StartUtcDateTime;
                    }
                }
                else
                    //No activities? Just.. enter the end time of the session as its start time, I  guess.
                    sessionData.EndUtcDateTime = sessionData.StartUtcDateTime;
            }

            //Now, for each activity, same logic. Make sure all activities have an end time.
            if (sessionData.Activities?.Count > 0)
            {
                for (int i = 0; i < sessionData.Activities.Count; i++)
                {
                    if (string.IsNullOrWhiteSpace(sessionData.Activities[i].EndUtcDateTime))
                    {
                        ActivityEvent lastEventWithTime = sessionData.Activities[i].GetLastActivityWithTimestamp();
                        sessionData.Activities[i].EndUtcDateTime =
                            lastEventWithTime != null ? lastEventWithTime.EventUtcDateTime : sessionData.Activities[i].StartUtcDateTime;
                    }
                }
            }

            var endSessionRequest = AppearitionRequest<Analytics_EndSessionTracking>.LaunchAPICall_POST<AnalyticsHandler>(sessionData.ChannelId, GetReusableApiRequest<Analytics_EndSessionTracking>(), sessionData);

            while (!endSessionRequest.IsDone)
                yield return null;

            if (endSessionRequest.RequestResponseObject.IsSuccess)
            {
                AppearitionLogger.LogInfo(string.Format(AnalyticsConstants.SESSION_END_SUCCESS, sessionData.SessionKey));

                //Ended successfully, remove local file.
                string filePath = GetPathToSessionFile(sessionData.SessionKey);

                if (File.Exists(filePath))
                    File.Delete(filePath);
            }
            else
                AppearitionLogger.LogError(string.Format(AnalyticsConstants.SESSION_END_FAILURE, sessionData.SessionKey));

            if (onComplete != null)
                onComplete(endSessionRequest.RequestResponseObject.IsSuccess);
        }

        #endregion
    }
}