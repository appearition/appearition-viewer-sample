// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: Job.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace Appearition.Job
{
    [System.Serializable]
    public class Job
    {
        public long JobId;
        public string JobNo;
        public string PTW;
        public string PurchaseOrderNo;
        public string TroubleTicket;
        public long JobStatus;
        public long SiteId;

        static Dictionary<long, string> _jobStatusTypeNameDictionary = new Dictionary<long, string> {
            {501, "Assigned"},
            {502, "Ready for review"},
            {503, "Review in progress"},
            {504, "Approved"},
            {505, "Rejected"},
            {506, "Cancelled"}
        };

        /// <summary>
        /// Fetches the name of a job status from a given id.
        /// </summary>
        /// <param name="jobStatusId"></param>
        /// <returns></returns>
        public static string GetJobStatusTypeName(long jobStatusId)
        {
            if (_jobStatusTypeNameDictionary.ContainsKey(jobStatusId))
                return _jobStatusTypeNameDictionary[jobStatusId];
            return "";
        }
    }
}