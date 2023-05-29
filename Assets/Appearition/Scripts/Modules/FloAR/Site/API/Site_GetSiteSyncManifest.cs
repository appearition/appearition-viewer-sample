// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: Site_GetSiteSyncManifest.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using Appearition.API;
using System.Collections.Generic;

namespace Appearition.Site.API
{
    /// <summary>
    /// https://api.appearition.com/TenantName/api/Site/GetSiteSyncManifest/0 , where 0 is Channel ID 
    /// </summary>
    [System.Serializable]
    public class Site_GetSiteSyncManifest : BaseApiPost
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
        public SiteSyncManifest Data;

        [System.Serializable]
        public class PostApi : SiteSyncManifest
        {
            public PostApi()
            {
            }

            public PostApi(SiteSyncManifest cc)
            {
                ProjectId = cc.ProjectId;
                Sites = cc.Sites == null ? new List<Site>() : new List<Site>(cc.Sites);
            }
        }
    }
}