// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: MopSyncManifest.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace Appearition.Mop
{
    [System.Serializable]
    public class MopSyncManifest
    {
        public int ProjectId = FloARConstants.FLOAR_INSTANCE_CHANNEL_ID;
        public List<Mop> Mops;
    }
}