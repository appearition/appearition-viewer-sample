using System;
using System.Linq;
using Appearition.Learn;

namespace Appearition.Assessments
{
    /// <summary>
    /// Encapsulates business rules and logic for learn data nodes
    /// </summary>
    public static class LearnDataExtensions
    {
        /// <summary>
        /// By default all nodes are touchable unless that have a property named "Is Touchable" and its set to false.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static bool IsNodeTouchable(LearnNode node)
        {
            if (node == null) return false;

            if (node.nodeProperties != null
                && node.nodeProperties.Any(x => StripWhitespace(x.propertyKey).Equals("istouchable", StringComparison.OrdinalIgnoreCase)
                                                && IsFalse(x.propertyValue))) return false;

            return true;
        }

        public static bool IsFalse(string theValue)
        {
            if (String.IsNullOrEmpty(theValue)) return true;

            var cleanValue = StripWhitespace(theValue.Trim());
            return cleanValue.Equals("false", StringComparison.OrdinalIgnoreCase)
                   || cleanValue.Equals("no", StringComparison.OrdinalIgnoreCase)
                   || cleanValue.Equals("0", StringComparison.OrdinalIgnoreCase);
        }

        public static string StripWhitespace(string theValue)
        {
            if (String.IsNullOrEmpty(theValue)) return theValue;

            return theValue.Replace(" ", String.Empty);
        }
    }
}