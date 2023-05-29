// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: ExtendedProfile.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace Appearition.AccountAndAuthentication
{
    /// <summary>
    /// Contains all the standard profile information and more advanced data such as the tenants this profile is allowed to use.
    /// </summary>
    [System.Serializable]
    public class ExtendedProfile : Profile
    {
        public List<string> tenantKeys;
        public List<TenantData> tenants;

        public ExtendedProfile() : base()
        {
        }

        /// <summary>
        /// Copy Constructor
        /// </summary>
        /// <param name="cc"></param>
        public ExtendedProfile(Profile cc) : base(cc)
        {
            tenantKeys = new List<string>();
            tenants = new List<TenantData>();
        }

        /// <summary>
        /// Copy Constructor
        /// </summary>
        /// <param name="cc"></param>
        public ExtendedProfile(ExtendedProfile cc) : base(cc)
        {
            tenantKeys = new List<string>(cc.tenantKeys);
            tenants = new List<TenantData>(cc.tenants);
        }
    }
}