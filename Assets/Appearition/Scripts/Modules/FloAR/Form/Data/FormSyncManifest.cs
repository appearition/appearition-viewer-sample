// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: FormSyncManifest.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace Appearition.Form
{
    [System.Serializable]
    public class FormSyncManifest
    {
        public int ProjectId = FloARConstants.FLOAR_INSTANCE_CHANNEL_ID;
        public List<FormGroup> FormGroups;
    }
}