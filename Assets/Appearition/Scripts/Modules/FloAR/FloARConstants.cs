// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: FloARConstants.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Appearition
{
    /// <summary>
    /// While the FloAR modules are not merged with the main EMS instance, this constant table will contain data defines.
    /// </summary>
    public static class FloARConstants
    {
        public const string PROFILE_FLOAR_TOKEN_NAME = "FloArToken";
        public const string FLOAR_INSTANCE_END_POINT_LOCATION = "http://www.aakhaar.com:7001";
        public const int FLOAR_INSTANCE_CHANNEL_ID = 1;
    }
}