// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: General_GetGeneralDocSyncManifest.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using Appearition.API;
using System.Collections.Generic;

namespace Appearition.GeneralDocument.API
{
    /// <summary>
    /// https://api.appearition.com/TenantName/api/Site/GetGeneralDocSyncManifest/0 , where 0 is Channel ID 
    /// </summary>
    [System.Serializable]
    public class General_GetGeneralDocSyncManifest : BaseApiPost
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
        public GeneralSyncManifest Data;

        [System.Serializable]
        public class PostApi : GeneralSyncManifest
        {
            public PostApi()
            {
            }

            public PostApi(GeneralSyncManifest cc)
            {
                ProjectId = cc.ProjectId;
                GeneralDocs = cc.GeneralDocs == null ? new List<General>() : new List<General>(cc.GeneralDocs);
            }
        }
    }
}