// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: Account_Authenticate.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using Appearition.API;

namespace Appearition.AccountAndAuthentication.API
{
    /// <summary>
    /// https://api.appearition.com/TenantName/api/Account/Authenticate/0 , where 0 is Channel ID 
    /// </summary>
    [System.Serializable]
    public class Account_Authenticate : BaseApiPost
    {
        public override int ApiVersion => 2;
        public override TypeOfApiStorage ResponseSaveType => TypeOfApiStorage.None;

        //Variables
        public ApiData Data;

        [System.Serializable]
        public class ApiData
        {
            public string sessionToken;
        }

        /// <summary>
        /// Post ApiData
        /// </summary>
        [System.Serializable]
        public class PostData
        {
            //Variables
            public string username;
            public string password;
            public string appId;
        }
    }
}