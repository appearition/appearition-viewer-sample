// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: ArLocation_ListByChannel.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using Appearition.API;
using System.Collections.Generic;

namespace Appearition.Location.API
{
    [System.Serializable]
    public class ArLocation_ListByChannel : BaseApiGet
    {
        public ApiData Data;

        [System.Serializable]
        public class ApiData
        {
            public List<Location> locations;
        }
    }
}