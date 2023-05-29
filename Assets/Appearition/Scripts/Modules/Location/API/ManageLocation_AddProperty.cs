// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: ManageLocation_AddProperty.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using Appearition.API;
using Appearition.Common;

namespace Appearition.Location.API
{
    /// <summary>
    /// https://api.appearition.com/TenantName/api/ManageLocation/AddProperty/0?arTargetId=1 , where 0 is Channel ID, 1 is Location ID.
    /// </summary>
    [System.Serializable]
    public class ManageLocation_AddProperty : BaseApiPost
    {
        public override TypeOfApiStorage ResponseSaveType => TypeOfApiStorage.None;
        public override int ApiVersion => 2;

        //Request Params
        [System.Serializable]
        public class RequestContent : BaseRequestContent
        {
            public int arLocationId;
        }

        /// <summary>
        /// Post ApiData
        /// </summary>
        [System.Serializable]
        public class PostData : Property
        {
            public PostData(string key, string value) : base(key, value)
            {
            }

            public PostData(Property cc) : base(cc)
            {
            }
        }
    }
}