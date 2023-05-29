﻿using System.Collections;
using System.Collections.Generic;
using Appearition.API;

namespace Appearition.BuildingsAndFloors.API
{
    /// <summary>
    /// https://api.appearition.com/TenantName/api/BuildingFloor/Create/0 , where 0 is Channel ID
    ///
    /// The post data is a Floor object.
    /// </summary>
    [System.Serializable]
    public class BuildingFloor_Create : BaseApiPost
    {
        public override int ApiVersion => 2;
        public override TypeOfApiStorage ResponseSaveType => TypeOfApiStorage.None;

        public Floor Data;
    }
}