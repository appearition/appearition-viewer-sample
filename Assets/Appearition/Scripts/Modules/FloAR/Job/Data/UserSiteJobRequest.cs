// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: UserSiteJobRequest.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

namespace Appearition.Job
{
    [System.Serializable]
    public class UserSiteJobRequest
    {
        public long ProjectId;
        public long UserProfileId;
        public long JobId;
        public long SiteId;
    }
}