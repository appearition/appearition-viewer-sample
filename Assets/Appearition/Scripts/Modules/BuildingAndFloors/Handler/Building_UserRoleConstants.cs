using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Appearition
{
    public static partial class UserRoleConstants
    {
        const string BUILDING_MANAGER = "BuildingManager";
        const string BUILDING_VIEWER = "BuildingViewer";
        const string BUILDING_FLOOR_MANAGER = "BuildingFloorManager";
        const string BUILDING_FLOOR_VIEWER = "BuildingFloorViewer";
        const string EQUIPMENT_MANAGER = "EquipmentManager";
        const string EQUIPMENT_VIEWER = "EquipmentViewer";
        const string MATERIAL_MANAGER = "MaterialManager";
        const string MATERIAL_VIEWER = "MaterialViewer";
        
        public static bool HasPermissionToManageBuildings => IsAdmin || AppearitionGate.Instance.CurrentUser.ContainsRoleForSelectedTenant(BUILDING_MANAGER);
        public static bool HasPermissionToViewBuildings => IsAdmin || AppearitionGate.Instance.CurrentUser.ContainsRoleForSelectedTenant(BUILDING_VIEWER);
        public static bool HasPermissionToManageBuildingFloors => IsAdmin || AppearitionGate.Instance.CurrentUser.ContainsRoleForSelectedTenant(BUILDING_FLOOR_MANAGER);
        public static bool HasPermissionToViewBuildingFloors => IsAdmin || AppearitionGate.Instance.CurrentUser.ContainsRoleForSelectedTenant(BUILDING_FLOOR_VIEWER);
        public static bool HasPermissionToManageEquipments => IsAdmin || AppearitionGate.Instance.CurrentUser.ContainsRoleForSelectedTenant(EQUIPMENT_MANAGER);
        public static bool HasPermissionToViewEquipments => IsAdmin || AppearitionGate.Instance.CurrentUser.ContainsRoleForSelectedTenant(EQUIPMENT_VIEWER);
        public static bool HasPermissionToManageMaterials => IsAdmin || AppearitionGate.Instance.CurrentUser.ContainsRoleForSelectedTenant(MATERIAL_MANAGER);
        public static bool HasPermissionToViewMaterials => IsAdmin || AppearitionGate.Instance.CurrentUser.ContainsRoleForSelectedTenant(MATERIAL_VIEWER);
    }
}