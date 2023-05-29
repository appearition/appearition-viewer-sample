// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: UserProfile.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

namespace Appearition.AccountAndAuthentication
{
    /// <summary>
    /// Currently used for the FloAR Apps mainly. Will be obsoleted once FloAr modules are updated onto the current EMS instance.
    /// </summary>
    [System.Serializable]
    public class UserProfile
    {
        public long UserProfileId;
        public string FirstName;
        public string LastName;
    }
}