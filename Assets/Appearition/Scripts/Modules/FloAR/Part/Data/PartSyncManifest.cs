// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: PartSyncManifest.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace Appearition.Part
{
    [System.Serializable]
    public class PartSyncManifest
    {
        public int ProjectId = FloARConstants.FLOAR_INSTANCE_CHANNEL_ID;
        public List<Part> Parts;
    }
}