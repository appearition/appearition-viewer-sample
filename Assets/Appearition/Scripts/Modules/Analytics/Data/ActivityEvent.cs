using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Appearition.Analytics
{
    /// <summary>
    /// Contains the data for a single event as part of an activity.
    /// An event should contain an EventCode (referring to what kind of event this specific event is).
    /// The EventData should contain additional key-value-pair data you may want to attach to this event.
    /// If this event is to be shown on the EMS Dashboard, you can check the list of available event codes and required content on the EMS for each module.
    /// </summary>
    [System.Serializable]
    public class ActivityEvent
    {
        /// <summary>
        /// If this activity event is custom, this event code can be what you want it to be.
        /// If this activity event is to be shown in the EMS Dashboard, make sure it matches one of the available EventCodes for each module.
        /// </summary>
        public string EventCode;
        public List<AnalyticsData> EventData;

        public string EventUtcDateTime;
        public DateTime EventDateTime => AppearitionGate.ConvertStringToDateTime(EventUtcDateTime);

        public ActivityEvent()
        {
            EventUtcDateTime = AppearitionGate.GetCurrentDateAndTime();
            EventData = new List<AnalyticsData>();
        }

        public ActivityEvent(string eventCode) : this()
        {
            EventCode = eventCode;
        }

        public ActivityEvent(string eventCode, AnalyticsData eventData) : this(eventCode)
        {
            if (eventData != null)
                EventData.Add(eventData);
        }

        public ActivityEvent(string eventCode, List<AnalyticsData> eventData) : this(eventCode)
        {
            if (eventData != null && eventData.Count > 0)
                EventData.AddRange(eventData);
        }

        public ActivityEvent(string eventCode, Dictionary<string, string> eventData) : this(eventCode)
        {
            if (eventData != null)
            {
                foreach (var kvp in eventData)
                    EventData.Add(new AnalyticsData(kvp));
            }
        }
    }
}