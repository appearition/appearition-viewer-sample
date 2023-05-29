// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: Profile.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace Appearition.AccountAndAuthentication
{
    /// <summary>
    /// Contains the main information of a profile as on the EMS.
    /// </summary>
    [System.Serializable]
    public class Profile
    {
        public string firstName;
        public string lastName;
        public string emailAddress;
        public List<CustomAttribute> customAttributes;

        public Profile()
        {
        }

        /// <summary>
        /// Copy Constructor
        /// </summary>
        /// <param name="cc"></param>
        public Profile(Profile cc)
        {
            firstName = cc.firstName;
            lastName = cc.lastName;
            emailAddress = cc.emailAddress;
            customAttributes = new List<CustomAttribute>(cc.customAttributes);
        }
    }
}