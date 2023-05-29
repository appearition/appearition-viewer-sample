// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: ImageRecognition_ListAvailable.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using Appearition.API;
using System.Collections.Generic;

namespace Appearition.ImageRecognition.API
{
    /// <summary>
    /// https://api.appearition.com/TenantName/api/ImageRecognition/ListAvailable/0 , where 0 is Channel ID 
    /// </summary>
    [System.Serializable]
    public class ImageRecognition_ListAvailable : BaseApiGet
    {
        public override AuthenticationOverrideType AuthenticationOverride => AuthenticationOverrideType.ApplicationToken;
        public override int ApiVersion => 2;

        //Variables
        public ApiData Data;

        //SubClasses
        [System.Serializable]
        public class ApiData
        {
            //Variables
            public List<DataStore> dataStores;
        }
    }
}