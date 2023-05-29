// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: General_GetDocument.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using Appearition.API;
using System.IO;

namespace Appearition.GeneralDocument.API
{
    /// <summary>
    /// https://api.appearition.com/TenantName/api/General/GetDocument/1 , where 1 is document Id
    /// </summary>
    [System.Serializable]
    public class General_GetDocument : BaseApiGet
    {
        #region FloAR module override 

        public override bool RequiresTenant
        {
            get { return false; }
        }

        public override string ForcedEndPoint
        {
            get { return FloARConstants.FLOAR_INSTANCE_END_POINT_LOCATION; }
        }

        public override AuthenticationOverrideType AuthenticationOverride
        {
            get { return AuthenticationOverrideType.Custom; }
        }

        public override string AuthenticationTokenCustomKey
        {
            get { return FloARConstants.PROFILE_FLOAR_TOKEN_NAME; }
        }

        //Cheat, cancel the channel Id and have a document id instead.
        public override int? ChannelIdOverride
        {
            get { return -1; }
        }

        #endregion


        public FileStream documentContent;
        public string savedDocumentPath;

        /// <summary>
        /// URL parameter content
        /// </summary>
        public class GetDocumentRequestContent : BaseRequestContent
        {
            public long DocumentId;

            public override string GetUrlExtraParameters()
            {
                return DocumentId.ToString();
            }
        }
    }
}