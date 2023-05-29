// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: FormGroup.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace Appearition.Form
{
    [System.Serializable]
    public class FormGroup
    {
        public long FormGroupId;
        public string FormGroupName;
        public bool IsJobAllocated;
        public int Ordinal;
        public List<Form> Forms;
    }
}