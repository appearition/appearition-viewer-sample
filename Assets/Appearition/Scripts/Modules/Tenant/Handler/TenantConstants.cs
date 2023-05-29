using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Appearition.Tenant
{
    public static class TenantConstants
    {
        public const string GET_TENANT_SETTINGS_SUCCESS_ONLINE = "Tenant settings for tenant of key {0} were successfully fetched";
        public const string GET_TENANT_SETTINGS_SUCCESS_OFFLINE = "Tenant settings for tenant of key {0} were loaded offline successfully.";
        public const string GET_TENANT_SETTINGS_SUCCESS_FAILURE = "An error occured when trying to fetch the settings of tenant of key {0}";

    }
}