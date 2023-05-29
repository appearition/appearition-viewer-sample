// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: Account_RegisterDevice.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using System.Collections.Generic;
using Appearition.API;

namespace Appearition.AccountAndAuthentication.API
{
    /// <summary>
    /// https://api.appearition.com/TenantName/api/Account/RegisterDevice/0 , where 0 is Channel ID 
    /// </summary>
    [System.Serializable]
    public class Account_RegisterDevice : BaseApiPost
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

        public override bool OverrideAuthenticationHeader
        {
            get { return true; }
        }

        public override int? ChannelIdOverride
        {
            get { return FloARConstants.FLOAR_INSTANCE_CHANNEL_ID; }
        }


        #endregion

        public override TypeOfApiStorage ResponseSaveType => TypeOfApiStorage.None;


        //Variables
        public ApiData Data;

        [System.Serializable]
        public class ApiData
        {
            public string Token;
        }

        /// <summary>
        /// Post ApiData
        /// </summary>
        [System.Serializable]
        public class PostData
        {
            //Variables
            public string DeviceInformation;
        }

        //Request extra headers
        [System.NonSerialized] public string username;
        [System.NonSerialized] public string password;

        protected override Dictionary<string, string> ExtraRequestUrlHeaders
        {
            get
            {
                return new Dictionary<string, string>
                    {{"authentication-username", username}, {"authentication-password", password}};
            }
        }
    }
}