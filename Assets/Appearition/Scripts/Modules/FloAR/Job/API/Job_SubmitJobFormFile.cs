// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: Job_SubmitJobFormFile.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using System.Collections.Generic;
using Appearition.API;

namespace Appearition.Job.API
{
    /// <summary>
    /// https://api.appearition.com/TenantName/api/Job/SubmitFormFile/0 , where 0 is Channel ID 
    /// </summary>
    [System.Serializable]
    public class Job_SubmitJobFormFile : BaseApiPost
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

        public override TypeOfPost FormType
        {
            get { return TypeOfPost.MultiForms; }
        }

        //Response Variables
        public string Data;

        //Request extra headers
        [System.NonSerialized] public string ProjectId;
        [System.NonSerialized] public string JobId;
        [System.NonSerialized] public string SiteId;
        [System.NonSerialized] public string FieldKey;
        [System.NonSerialized] public string FormKey;

        protected override Dictionary<string, string> ExtraRequestUrlHeaders
        {
            get
            {
                return new Dictionary<string, string> {
                    {"ProjectId", ProjectId},
                    {"JobId", JobId},
                    {"SiteId", SiteId},
                    {"FieldKey", FieldKey},
                    {"FormKey", FormKey},
                };
            }
        }
    }
}