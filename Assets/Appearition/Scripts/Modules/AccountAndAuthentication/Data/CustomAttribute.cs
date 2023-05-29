// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: CustomAttribute.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

namespace Appearition.AccountAndAuthentication
{
    /// <summary>
    /// Container data for a Profile custom attribute.
    /// Upon updating profile, if doDeleteAttribute is set to true, the attribute will be deleted.
    /// </summary>
    [System.Serializable]
    public class CustomAttribute
    {
        public string attributeName;
        public string attributeValue;
        public bool doDeleteAttribute = false;
    }
}