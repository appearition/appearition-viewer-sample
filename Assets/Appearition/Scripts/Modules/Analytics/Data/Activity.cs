using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Appearition.Analytics
{
    /// <summary>
    /// An activity is a collection of several activity events.
    /// When recording event, it's a good habit to group them per activity. One activity can be a single UI panel, for instance.
    /// Within that activity, you can record multiple events, such as where the user clicks and when.
    /// </summary>
    [System.Serializable]
    public class Activity : IDisposable
    {
        #region Activity Event Container

        /// <summary>
        /// Container class for an activity event data.
        /// Each class which inherits from Activity can use it to facilitate transportation and demand of additional parameters.
        /// </summary>
        public abstract class ActivityEventContainer<T> where T : ActivityEventContainer<T>
        {
            /// <summary>
            /// The code of this specific event
            /// </summary>
            public virtual string EventCode => ConvertContainerTypeToEventCode<T>();

            /// <summary>
            /// Contains any analytics events to add as part of this container.
            /// </summary>
            protected List<AnalyticsData> analyticEvents;

            protected ActivityEventContainer()
            {
                analyticEvents = new List<AnalyticsData>();
            }

            /// <summary>
            /// Converts this container into a proper ActivityEvent.
            /// </summary>
            /// <returns></returns>
            public virtual ActivityEvent GenerateActivityEvent()
            {
                if (analyticEvents.Count > 0)
                    return new ActivityEvent(EventCode, analyticEvents);
                return new ActivityEvent(EventCode);
            }

            /// <summary>
            /// Converts the syntax of an activity event container to an event code.
            /// </summary>
            /// <returns></returns>
            public static string ConvertContainerTypeToEventCode<K>()
            {
                string valueString = typeof(K).Name;

                //Remove EventContainer; the event code doesn't need it.
                if (valueString.Contains("EventContainer"))
                {
                    int tmpLength = "EventContainer".Length;
                    valueString = valueString.Remove(valueString.Length - tmpLength);
                }

                return ConvertStringToCodeFormat(valueString);
            }
        }

        #endregion

        //Main activity variables
        public string SessionKey;
        public string ActivityKey;
        public string ActivityCode;
        public List<AnalyticsData> ActivityData;
        public List<ActivityEvent> Events;

        //Time variables
        public string StartUtcDateTime;
        public DateTime StartDateTime => AppearitionGate.ConvertStringToDateTime(StartUtcDateTime);
        public string EndUtcDateTime;
        public DateTime EndDateTime => AppearitionGate.ConvertStringToDateTime(EndUtcDateTime);

        //Premade Activities Variables
        protected virtual string DefaultActivityCode => ConvertStringToCodeFormat(GetType().Name);
        protected bool IsSubmitted { get; set; }

        protected Activity()
        {
            IsSubmitted = false;
            ActivityKey = AppearitionGate.GenerateNewGUID();
            StartUtcDateTime = AppearitionGate.GetCurrentDateAndTime();
            ActivityData = new List<AnalyticsData>();
            Events = new List<ActivityEvent>();

            if (string.IsNullOrEmpty(ActivityCode))
                ActivityCode = DefaultActivityCode;
        }

        public Activity(string sessionKey, string activityCode) : this()
        {
            SessionKey = sessionKey;
            ActivityCode = activityCode;
        }

        /// <summary>
        /// Finalizes the activity creation and stores this current activity in the ongoing session.
        /// If creating custom activities, make sure to call this method at the end of the creation process.
        /// </summary>
        protected void FinalizeActivityCreation()
        {
            AnalyticsHandler.AddActivityToCurrentSession(this);
        }

        /// <summary>
        /// Add a new activity event to this activity.
        /// </summary>
        /// <param name="newEvent"></param>
        public void AddEventToActivity(ActivityEvent newEvent)
        {
            if (newEvent == null)
                return;

            if (string.IsNullOrEmpty(newEvent.EventCode))
            {
                AppearitionLogger.LogError("An ActivityEvent with no EventCode was provided.");
                return;
            }

            Events.Add(newEvent);

            //Request for Session storage update.
            AnalyticsHandler.UpdateOngoingSessionLocalSave();
        }

        /// <summary>
        /// Add a new activity event to this activity.
        /// </summary>
        /// <param name="newEventContainer"></param>
        public void AddEventToActivity<T>(ActivityEventContainer<T> newEventContainer) where T : ActivityEventContainer<T>
        {
            AddEventToActivity(newEventContainer.GenerateActivityEvent());
        }

        #region Utilities

        /// <summary>
        /// Finalizes, closes and adds this activity to the current session to be submitted.
        /// </summary>
        public virtual void SubmitActivity()
        {
            IsSubmitted = true;
            AnalyticsHandler.SubmitActivityToCurrentSession(this);
        }

        /// <summary>
        /// Returns the last entered event which contains a timestamp, if any.
        /// </summary>
        /// <returns></returns>
        public ActivityEvent GetLastActivityWithTimestamp()
        {
            if (Events?.Count > 0)
                return Events.LastOrDefault(o => !string.IsNullOrEmpty(o.EventUtcDateTime));
            return null;
        }

        /// <summary>
        /// Converts a string to a ActivityCode or EventCode format.
        /// eg WhateverString -> WHATEVER_STRING
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected static string ConvertStringToCodeFormat(string input)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < input.Length; i++)
            {
                if (i > 0 && char.IsUpper(input[i]))
                    sb.Append('_');

                sb.Append(char.ToUpper(input[i]));
            }

            return sb.ToString();
        }

        #endregion

        #region IDisposable Implementation 

        public void Dispose()
        {
            if (!IsSubmitted)
                SubmitActivity();
        }

        #endregion
    }
}