// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: ArTarget_Create.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using Appearition.API;

namespace Appearition.ArTargetImageAndMedia.API
{
    /// <summary>
    /// https://api.appearition.com/TenantName/api/ArTarget/Create/0 , where 0 is Channel ID 
    /// </summary>
    [System.Serializable]
    public class ArTarget_Create : BaseApiPost
    {
        //public override AuthenticationOverrideType AuthenticationOverride => AuthenticationOverrideType.SessionToken;
        public override TypeOfApiStorage ResponseSaveType => TypeOfApiStorage.None;
        
        //Variables
        public ArTarget Data;

        //Post ApiData
        [System.Serializable]
        public class PostData
        {
            //Variables
            public int productId;
            public string name;
        }
    }
}