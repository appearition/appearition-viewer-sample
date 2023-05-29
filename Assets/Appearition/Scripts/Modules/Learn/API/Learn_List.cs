// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: Learn_List.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using Appearition.API;
using System.Collections.Generic;

namespace Appearition.Learn.API
{
    /// <summary>
    /// https://api.appearition.com/TenantName/api/Learn/List/0 , where 0 is Channel ID 
    /// </summary>
    [System.Serializable]
    public class Learn_List : BaseApiGet
    {
        public override int ApiVersion => 2;

        //Variables
        public List<LearnNode> Data;
    }
}