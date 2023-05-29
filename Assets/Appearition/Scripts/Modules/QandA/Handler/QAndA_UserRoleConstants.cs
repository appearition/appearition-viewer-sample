using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Appearition
{
    public static partial class UserRoleConstants
    {
        const string Q_AND_A_VIEWER = "QandAViewer";
        const string Q_AND_A_EDITOR = "QandAEditor";
        
        public static bool HasPermissionToViewQAndAData => IsAdmin || AppearitionGate.Instance.CurrentUser.ContainsRoleForSelectedTenant(Q_AND_A_VIEWER);
        public static bool HasPermissionToEditQAndAData => IsAdmin || AppearitionGate.Instance.CurrentUser.ContainsRoleForSelectedTenant(Q_AND_A_EDITOR);
    }
}