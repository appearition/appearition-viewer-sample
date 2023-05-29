// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: QAEndPointC.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Appearition.Internal.EndPoints
{
    /// <summary>
    /// Definition for the aakhaar end point.
    /// </summary>
    public class QAEndPoint : EndPoint
    {
        /// <summary>
        /// Endpoint name that can be used for display purposes.
        /// </summary>
        /// <value>The display name.</value>
        public override string displayName
        {
            get { return "Appearition QA"; }
        }

        /// <summary>
        /// URL to the end point, which should be used for requests.
        /// </summary>
        /// <value>The end point UR.</value>
        public override string endPointURL
        {
            get { return "https://www.aakhaar.com:5101"; }
        }

        /// <summary>
        /// URL to the web portal of this end point.
        /// </summary>
        /// <value>The portal UR.</value>
        public override string portalURL
        {
            get { return "https://www.aakhaar.com:5100"; }
        }
    }
}