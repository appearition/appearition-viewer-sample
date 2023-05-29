// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: Job_GetJobFormData.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using Appearition.API;

namespace Appearition.Job.API
{
    /// <summary>
    /// https://api.appearition.com/TenantName/api/Job/GetJobFormData/0 , where 0 is Channel ID 
    /// </summary>
    [System.Serializable]
    public class Job_GetJobFormData : BaseApiPost
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
        public JobPost Data;

        [System.Serializable]
        public class PostApi : UserSiteJobRequest
        {
            public PostApi()
            {
            }

            public PostApi(UserSiteJobRequest cc)
            {
                ProjectId = cc.ProjectId;
                UserProfileId = cc.UserProfileId;
                JobId = cc.JobId;
                SiteId = cc.SiteId;
            }
        }
    }
}