// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: SiteSyncManifest.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace Appearition.Site
{
    [System.Serializable]
    public class SiteSyncManifest
    {
        public int ProjectId = FloARConstants.FLOAR_INSTANCE_CHANNEL_ID;
        public List<Site> Sites;
    }
}