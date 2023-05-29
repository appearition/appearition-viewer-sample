// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: Form.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace Appearition.Form
{
    [System.Serializable]
    public class Form
    {
        public string FormKey;
        public string FormName;
        public string Instructions;
        public int Ordinal;
        public List<Field> Fields;
    }
}