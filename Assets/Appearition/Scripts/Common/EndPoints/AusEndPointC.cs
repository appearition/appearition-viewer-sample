// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: AusEndPointC.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Appearition.Internal.EndPoints
{
    /// <summary>
    /// Definition of the login.appearition.com End Point.
    /// </summary>
    public class AUSEndPoint : EndPoint
    {
        /// <summary>
        /// Endpoint name that can be used for display purposes.
        /// </summary>
        /// <value>The display name.</value>
        public override string displayName
        {
            get { return "Appearition Australia"; }
        }

        /// <summary>
        /// URL to the end point, which should be used for requests.
        /// </summary>
        /// <value>The end point UR.</value>
        public override string endPointURL
        {
            get { return "https://api.appearition.com"; }
        }

        /// <summary>
        /// URL to the web portal of this end point.
        /// </summary>
        /// <value>The portal UR.</value>
        public override string portalURL
        {
            get { return "https://www.login.appearition.com"; }
        }
    }
}