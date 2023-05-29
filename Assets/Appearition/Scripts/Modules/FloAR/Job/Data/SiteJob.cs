// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: SiteJob.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace Appearition.Job
{
    [System.Serializable]
    public class SiteJob
    {
        public long SiteId;
        public List<Job> Jobs;
    }
}