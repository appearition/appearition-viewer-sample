// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: JobFormFilePost.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using Appearition.Internal;

namespace Appearition.Job
{
    [System.Serializable]
    public class JobFormFilePost
    {
        public long FieldKey;
        public long FormKey;
        public MultiPartFormParam multiPartForm;
    }
}