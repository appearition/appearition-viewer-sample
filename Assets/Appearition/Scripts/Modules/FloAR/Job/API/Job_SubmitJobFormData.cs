// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: Job_SubmitJobFormData.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using System.Collections.Generic;
using Appearition.API;

namespace Appearition.Job.API
{
    /// <summary>
    /// https://api.appearition.com/TenantName/api/Job/SubmitJobFormData/0 , where 0 is Channel ID 
    /// </summary>
    [System.Serializable]
    public class Job_SubmitJobFormData : BaseApiPost
    {
        #region FloAR module override 

        public override bool RequiresTenant
        {
            get { return false; }
        }

        public override string ForcedEndPoint
        {
            get { return FloARConstants.FLOAR_INSTANCE_END_POINT_LOCATION; }
        }

        public override AuthenticationOverrideType AuthenticationOverride
        {
            get { return AuthenticationOverrideType.Custom; }
        }

        public override string AuthenticationTokenCustomKey
        {
            get { return FloARConstants.PROFILE_FLOAR_TOKEN_NAME; }
        }

        public override int? ChannelIdOverride
        {
            get { return FloARConstants.FLOAR_INSTANCE_CHANNEL_ID; }
        }

        #endregion

        //Response Variables
        public Job Data;

        [System.Serializable]
        public class PostApi : JobPost
        {
            public PostApi()
            {
            }

            public PostApi(JobPost cc)
            {
                ProjectId = cc.ProjectId;
                UserProfileId = cc.UserProfileId;
                Forms = cc.Forms == null ? new List<JobFormPost>() : new List<JobFormPost>(cc.Forms);
                ReviewerComments = cc.ReviewerComments;

                JobId = cc.JobId;
                JobNo = cc.JobNo;
                PTW = cc.PTW;
                PurchaseOrderNo = cc.PurchaseOrderNo;
                TroubleTicket = cc.TroubleTicket;
                JobStatus = cc.JobStatus;
                SiteId = cc.SiteId;
            }
        }
    }
}