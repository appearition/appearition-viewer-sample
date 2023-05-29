using System.Collections.Generic;

namespace Appearition.Analytics
{
    /// <summary>
    /// Contains a data entry for a single event.
    /// </summary>
    [System.Serializable]
    public class AnalyticsData
    {
        public string Key;
        public string Value;

        public AnalyticsData()
        {
        }

        public AnalyticsData(string key, string value)
        {
            Key = key;
            Value = value;
        }

        public AnalyticsData(KeyValuePair<string, string> kvp)
        {
            Key = kvp.Key;
            Value = kvp.Value;
        }

        public override string ToString()
        {
            return string.Format("EventData Key: {0} ; Value: {1}", Key, Value);
        }
    }
}