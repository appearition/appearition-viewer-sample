// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: BaseDocument.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using System.IO;

namespace Appearition.Common
{
    /// <summary>
    /// Base class for a document of type FloAR.
    /// </summary>
    [System.Serializable]
    public class BaseDocument
    {
        public long DocumentId;
        /// <summary>
        /// Contains the byte stream of the document if loaded during runtime.
        /// </summary>
        [System.NonSerialized] public Stream DocumentContent;
        public int VersionNo;
        public string DocumentName;
        public System.DateTime DateModified;
        public string FileName;
        public bool IsActive;
    }
}