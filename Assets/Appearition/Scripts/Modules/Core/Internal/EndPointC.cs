// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: EndPointC.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Appearition.Internal.EndPoints
{
    /// <summary>
    /// Container class for a single end point towards the EMS.
    /// </summary>
    [System.Serializable]
    public class EndPoint
    {
        /// <summary>
        /// Endpoint name that can be used for display purposes.
        /// </summary>
        /// <value>The display name.</value>
        public virtual string displayName { get; protected set; }

        /// <summary>
        /// URL to the end point, which should be used for requests.
        /// </summary>
        /// <value>The end point UR.</value>
        public virtual string endPointURL { get; protected set; }

        /// <summary>
        /// URL to the web portal of this end point.
        /// </summary>
        /// <value>The portal UR.</value>
        public virtual string portalURL { get; protected set; }

        /// <summary>
        /// URL to the help page, if any.
        /// </summary>
        /// <value>The help page UR.</value>
        protected virtual string customHelpPageURL { get; set; }

        public EndPoint()
        {
        }

        /// <summary>
        /// Creates a new end point ApiData.
        /// A help page URL can be provided. If none, the default help page URL will be used. It can be accessed via the property GetHelpPageURL.
        /// </summary>
        /// <param name="newDisplayName">New display name.</param>
        /// <param name="newEndPointURL">New end point UR.</param>
        /// <param name="newPortalURL">New portal UR.</param>
        /// <param name="newHelpPageURL">New help page UR.</param>
        public EndPoint(string newDisplayName, string newEndPointURL, string newPortalURL, string helpPageURLOverride = "")
        {
            displayName = newDisplayName;
            endPointURL = newEndPointURL;
            portalURL = newPortalURL;
            if (helpPageURLOverride.Length > 0)
                customHelpPageURL = helpPageURLOverride;
        }

        /// <summary>
        /// Gets the URL to the webpage which should contain a help guide.
        /// This link will be generated using the portal URL and the channel name, but can be overrode in the constructor.
        /// </summary>
        /// <param name="channelName">Channel name.</param>
        public string GetHelpPageURL(string channelName = "")
        {
            if (customHelpPageURL.Length > 0)
                return customHelpPageURL;
            else
                return portalURL + "cdn/help/" + channelName.ToLower() + "/index.html";
        }
    }
}