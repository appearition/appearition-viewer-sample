// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: ImageRecognition_SaveDataStore.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using Appearition.API;

namespace Appearition.ImageRecognition.API
{
    /// <summary>
    /// https://api.appearition.com/TenantName/api/ImageRecognition/SaveDataStore/0 , where 0 is Channel ID 
    /// </summary>
    [System.Serializable]
    public class ImageRecognition_SaveDataStore : BaseApiPost
    {
        public override AuthenticationOverrideType AuthenticationOverride => AuthenticationOverrideType.SessionToken;
        public override int ApiVersion => 2;
        public override TypeOfApiStorage ResponseSaveType => TypeOfApiStorage.None;

        /// <summary>
        /// Post ApiData
        /// </summary>
        [System.Serializable]
        public class PostData : DataStore
        {
            public PostData(DataStore cc) : base(cc)
            {
            }
        }
    }
}