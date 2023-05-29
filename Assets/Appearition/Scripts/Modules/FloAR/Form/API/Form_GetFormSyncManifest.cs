// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: Form_GetFormSyncManifest.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using System.Collections.Generic;
using Appearition.API;

namespace Appearition.Form.API
{
    /// <summary>
    /// https://api.appearition.com/TenantName/api/Form/GetFormSyncManifest/0 , where 0 is Channel ID 
    /// </summary>
    [System.Serializable]
    public class Form_GetFormSyncManifest : BaseApiPost
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
        public FormSyncManifest Data;

        [System.Serializable]
        public class PostApi : FormSyncManifest
        {
            public PostApi()
            {
            }

            public PostApi(FormSyncManifest cc)
            {
                ProjectId = cc.ProjectId;
                FormGroups = cc.FormGroups == null ? new List<FormGroup>() : new List<FormGroup>(cc.FormGroups);
            }
        }
    }
}