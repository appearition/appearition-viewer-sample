// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: JobFormPost.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace Appearition.Job
{
    [System.Serializable]
    public class JobFormPost
    {
        public long FormStatus;
        public string FormKey;
        public long FormGroupId;
        public string EnteredUtcDate;
        public List<JobFormFieldPost> Fields;
        public string ReviewerComments;

        static Dictionary<long, string> _formStatusTypeNameDictionary = new Dictionary<long, string>
            {{801, "Ready for review"}, {802, "Review in progress"}, {803, "Approved"}, {804, "Rejected"}};

        /// <summary>
        /// Fetches the name of a form status from a given id.
        /// </summary>
        /// <param name="formStatus"></param>
        /// <returns></returns>
        public static string GetFormStatusTypeName(int formStatus)
        {
            if (_formStatusTypeNameDictionary.ContainsKey(formStatus))
                return _formStatusTypeNameDictionary[formStatus];
            return "";
        }
    }
}