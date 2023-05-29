// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: MultiPartFormParam.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

namespace Appearition.Internal
{
    /// <summary>
    /// Multipart form ApiData container used for POST requests.
    /// </summary>
    public class MultiPartFormParam
    {
        /// <summary>
        /// Name of the entry. Default is "item". Optional
        /// </summary>
        public string name = "item";
        /// <summary>
        /// The file's content. For most ApiData, a Stream is expected. Textures/Texture2Ds are also accepted for image types.
        /// </summary>
        public object value;
        /// <summary>
        /// The name of the file to upload. Optional.
        /// </summary>
        public string fileName = "";
        /// <summary>
        /// Defines the MimeType of the objects being uploaded. Refer to ApiConstants for samples.
        /// </summary>
        public string mimeType = "";

        public MultiPartFormParam()
        {
        }

        public MultiPartFormParam(string newName, object newValue, string newFilename, string newMimeType)
        {
            name = newName;
            value = newValue;
            fileName = newFilename;
            mimeType = newMimeType;
        }
    }
}