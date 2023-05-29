// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: UserSiteJob.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace Appearition.Job
{
    [System.Serializable]
    public class UserSiteJob
    {
        public long UserProfileId;
        public List<SiteJob> SiteJobs;
    }
}