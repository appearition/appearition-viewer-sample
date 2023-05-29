// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: Mop_GetMopSyncManifest.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using Appearition.API;
using System.Collections.Generic;

namespace Appearition.Mop.API
{
    /// <summary>
    /// https://api.appearition.com/TenantName/api/Mop/GetMopSyncManifest/0 , where 0 is Channel ID 
    /// </summary>
    [System.Serializable]
    public class Mop_GetMopSyncManifest : BaseApiPost
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
        public MopSyncManifest Data;

        [System.Serializable]
        public class PostApi : MopSyncManifest
        {
            public PostApi()
            {
            }

            public PostApi(MopSyncManifest cc)
            {
                ProjectId = cc.ProjectId;
                Mops = cc.Mops == null ? new List<Mop>() : new List<Mop>(cc.Mops);
            }
        }
    }
}