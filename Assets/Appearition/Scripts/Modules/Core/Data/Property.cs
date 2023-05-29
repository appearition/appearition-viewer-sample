using System;
using System.Collections.Generic;

namespace Appearition.Common
{
    [System.Serializable]
    public class Property
    {
        public string propertyKey;
        public string propertyValue;

        public Property()
        {
        }

        public Property(string newKey, string newValue)
        {
            propertyKey = newKey;
            propertyValue = newValue;
        }

        public Property(Property cc)
        {
            propertyKey = cc.propertyKey;
            propertyValue = cc.propertyValue;
        }

        public Property(KeyValuePair<string, string> kvp)
        {
            propertyKey = kvp.Key;
            propertyValue = kvp.Value;
        }
    }

    public static class PropertyExtensions
    {
        #region Quick Read

        public static T GetPropertyWithKey<T>(this List<Property> properties, string key, Func<string, T> parser)
        {
            T outcome = default;

            if (properties == null || properties.Count == 0 || parser == null || string.IsNullOrEmpty(key))
                return outcome;

            int index = properties.FindIndex(o => o.propertyKey.Equals(key, StringComparison.InvariantCultureIgnoreCase));

            if (index >= 0)
            {
                try
                {
                    outcome = parser(properties[index].propertyValue);
                } catch
                {
                    outcome = default;
                }
            }

            return outcome;
        }

        #region Main Primitive Types

        static int? TryParseInt(string value)
        {
            if (int.TryParse(value, out int outcome))
                return outcome;
            return default;
        }

        public static int? GetIntPropertyWithKey(this List<Property> properties, string key)
        {
            return GetPropertyWithKey(properties, key, TryParseInt);
        }

        public static int GetIntPropertyWithKey(this List<Property> properties, string key, int defaultValue)
        {
            return GetPropertyWithKey(properties, key, TryParseInt).GetValueOrDefault(defaultValue);
        }


        static long? TryParseLong(string value)
        {
            if (long.TryParse(value, out long outcome))
                return outcome;
            return default;
        }

        public static long? GetLongPropertyWithKey(this List<Property> properties, string key)
        {
            return GetPropertyWithKey(properties, key, TryParseLong);
        }

        public static long GetLongPropertyWithKey(this List<Property> properties, string key, long defaultValue)
        {
            return GetPropertyWithKey(properties, key, TryParseLong).GetValueOrDefault(defaultValue);
        }


        static float? TryParseFloat(string value)
        {
            if (float.TryParse(value, out float outcome))
                return outcome;
            return default;
        }

        public static float? GetFloatPropertyWithKey(this List<Property> properties, string key)
        {
            return GetPropertyWithKey(properties, key, TryParseFloat);
        }

        public static float GetFloatPropertyWithKey(this List<Property> properties, string key, float defaultValue)
        {
            return GetPropertyWithKey(properties, key, TryParseFloat).GetValueOrDefault(defaultValue);
        }

        static double? TryParseDouble(string value)
        {
            if (double.TryParse(value, out double outcome))
                return outcome;
            return default;
        }

        public static double? GetDoublePropertyWithKey(this List<Property> properties, string key)
        {
            return GetPropertyWithKey(properties, key, TryParseDouble);
        }

        public static double GetDoublePropertyWithKey(this List<Property> properties, string key, double defaultValue)
        {
            return GetPropertyWithKey(properties, key, TryParseDouble).GetValueOrDefault(defaultValue);
        }


        static bool? TryParseBool(string value)
        {
            if (bool.TryParse(value, out bool outcome))
                return outcome;
            return default;
        }

        public static bool? GetBoolPropertyWithKey(this List<Property> properties, string key)
        {
            return GetPropertyWithKey(properties, key, TryParseBool);
        }

        public static bool GetBoolPropertyWithKey(this List<Property> properties, string key, bool defaultValue)
        {
            return GetPropertyWithKey(properties, key, TryParseBool).GetValueOrDefault(defaultValue);
        }

        #endregion

        #endregion

        #region Quick Write

        public static void SetPropertyWithKey(this List<Property> properties, string key, string value)
        {
            int index = properties.FindIndex(o => o.propertyKey.Equals(key));

            if (index >= 0)
            {
                if (value == null)
                    properties.RemoveAt(index);
                else
                    properties[index] = new Property(key, value);
            }
            else
                properties.Add(new Property(key, value));
        }
        
        #endregion

        #region Dictionary

        public static Dictionary<string, string> ToDictionary(this List<Property> properties)
        {
            Dictionary<string, string> outcome = new Dictionary<string, string>();

            if (properties == null)
                return outcome;

            for (int i = 0; i < properties.Count; i++)
            {
                try
                {
                    outcome.Add(properties[i].propertyKey, properties[i].propertyValue);
                } catch (Exception e)
                {
                    UnityEngine.Debug.LogWarning($"Error when trying to convert properties to dictionary: {e}");
                }
            }

            return outcome;
        }

        public static List<Property> ToPropertyList(this Dictionary<string, string> dictionary)
        {
            List<Property> outcome = new List<Property>();

            if (dictionary == null)
                return outcome;

            foreach (var kvp in dictionary)
                outcome.Add(new Property(kvp.Key, kvp.Value));

            return outcome;
        }

        #endregion
    }
}