// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: GeneralSyncManifest.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace Appearition.GeneralDocument
{
    [System.Serializable]
    public class GeneralSyncManifest
    {
        public int ProjectId = FloARConstants.FLOAR_INSTANCE_CHANNEL_ID;
        public List<General> GeneralDocs;
    }
}