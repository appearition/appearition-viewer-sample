// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: ArImage_List.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using Appearition.API;

namespace Appearition.ArTargetImageAndMedia.API
{
    /// <summary>
    /// https://api.appearition.com/TenantName/api/ArImage/List/0 , where 0 is Channel ID 
    /// </summary>
    [System.Serializable]
    public class ArImage_List : BaseApiGet
    {
        //Variables
        public TargetImage[] Data;
    }
}