// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: Learn_Tracking.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using Appearition.API;

namespace Appearition.Learn.API
{
    /// <summary>
    /// https://api.appearition.com/TenantName/api/Learn/Tracking/0 , where 0 is Channel ID 
    /// </summary>
    [System.Serializable]
    public class Learn_Tracking : BaseApiPost
    {
        public override AuthenticationOverrideType AuthenticationOverride => AuthenticationOverrideType.SessionToken;
        public override int ApiVersion => 2;
        public override TypeOfApiStorage ResponseSaveType => TypeOfApiStorage.None;

        /// <summary>
        /// Post ApiData
        /// </summary>
        [System.Serializable]
        public class PostData : LearningSession
        {
        }
    }
}