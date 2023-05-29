// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: Part_GetPartSyncManifest.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using Appearition.API;
using System.Collections.Generic;

namespace Appearition.Part.API
{
    /// <summary>
    /// https://api.appearition.com/TenantName/api/Part/GetPartSyncManifest/0 , where 0 is Channel ID 
    /// </summary>
    [System.Serializable]
    public class Part_GetPartSyncManifest : BaseApiPost
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
        public PartSyncManifest Data;

        [System.Serializable]
        public class PostApi : PartSyncManifest
        {
            public PostApi()
            {
            }

            public PostApi(PartSyncManifest cc)
            {
                ProjectId = cc.ProjectId;
                Parts = cc.Parts == null ? new List<Part>() : new List<Part>(cc.Parts);
            }
        }
    }
}