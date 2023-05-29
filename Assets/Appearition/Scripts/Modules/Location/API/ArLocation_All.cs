// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: ArLocation_All.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using System.Collections.Generic;
using Appearition.API;

namespace Appearition.Location.API
{
    /// <summary>
    /// https://api.appearition.com/TenantName/api/ArLocation/All/0 , where 0 is Channel ID 
    /// </summary>
    [System.Serializable]
    public class ArLocation_All : BaseApiGet
    {
        public ApiData Data;

        [System.Serializable]
        public class ApiData
        {
            public List<PointOfInterest> locations;
        }
    }
}