// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: JobPost.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace Appearition.Job
{
    [System.Serializable]
    public class JobPost : Job
    {
        public long ProjectId;
        public long UserProfileId;
        public List<JobFormPost> Forms;
        public string ReviewerComments;
    }
}