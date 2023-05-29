// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: Tenant.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace Appearition.AccountAndAuthentication
{
    /// <summary>
    /// Basic information a Tenant as on the EMS contains.
    /// </summary>
    [System.Serializable]
    public class TenantData
    {
        public string tenantKey;
        public string tenantName;
        public List<string> roles;
    }
}