// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: ManageLocation_DeleteProperty.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using Appearition.API;

namespace Appearition.Location.API
{
    /// <summary>
    /// https://api.appearition.com/TenantName/api/ManageLocation/DeleteProperty/0?arTargetId=1&propertyKey=key , where 0 is Channel ID, 1 is Location ID and key contains the property key.
    /// </summary>
    [System.Serializable]
    public class ManageLocation_DeleteProperty : BaseApiPost
    {
        public override TypeOfApiStorage ResponseSaveType => TypeOfApiStorage.None;
        public override int ApiVersion => 2;

        //Request Params
        [System.Serializable]
        public class RequestContent : BaseRequestContent
        {
            public int arLocationId;
            public string propertyKey;
        }
    }
}