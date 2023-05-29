// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: EndPointUtility.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System;

namespace Appearition.Internal.EndPoints
{
    public static class EndPointUtility
    {
        static List<EndPoint> _activeEndPoints;

        /// <summary>
        /// All EMS End points currently loaded and active.
        /// </summary>
        /// <value>The active end points.</value>
        public static List<EndPoint> activeEndPoints
        {
            get
            {
                if (_activeEndPoints == null)
                    RefreshEndPoints();
                return _activeEndPoints;
            }
        }

        /// <summary>
        /// Refreshes the Active End Points container, by fetching all currently loaded end points.
        /// </summary>
        public static void RefreshEndPoints()
        {
            if (_activeEndPoints == null)
                _activeEndPoints = new List<EndPoint>();
            _activeEndPoints.Clear();

            try
            {
                Type[] endPointTypes = (from tmpAssembly in AppDomain.CurrentDomain.GetAssemblies()
                    from tmpModule in tmpAssembly.GetModules()
                    from tmpType in tmpModule.GetTypes()
                    //where tmpType.BaseType != null && tmpType.BaseType.Name.Equals("EndPoint")
                    where tmpType.BaseType != null && tmpType.BaseType == typeof(EndPoint)
                    select tmpType).ToArray();

                for (int i = 0; i < endPointTypes.Length; i++)
                {
                    try
                    {
                        _activeEndPoints.Add((EndPoint) Activator.CreateInstance(endPointTypes[i]));
                    } catch
                    {
                    }
                }
            } catch
            {
                AppearitionLogger.LogWarning("Unable to fetch any End Points.");
            }
        }

        #region Queries and Utilities

        /// <summary>
        /// Using a display name, fetches the relevant end point.
        /// </summary>
        /// <returns>The end point from display name.</returns>
        /// <param name="displayName">Display name.</param>
        public static EndPoint GetEndPointFromDisplayName(string displayName)
        {
            return activeEndPoints.FirstOrDefault(o => o.displayName == displayName);
        }

        /// <summary>
        /// Using an end point URL, fetches the relevant end point.
        /// </summary>
        /// <returns>The end point from end point UR.</returns>
        /// <param name="endPointURL">End point UR.</param>
        public static EndPoint GetEndPointFromEndPointURL(string endPointURL)
        {
            return activeEndPoints.FirstOrDefault(o => o.endPointURL == endPointURL);
        }

        /// <summary>
        /// Using a portal URL, fetches the relevant end point.
        /// </summary>
        /// <returns>The end point from portal UR.</returns>
        /// <param name="portalURL">Portal UR.</param>
        public static EndPoint GetEndPointFromPortalURL(string portalURL)
        {
            return activeEndPoints.FirstOrDefault(o => o.portalURL == portalURL);
        }

        /// <summary>
        /// Fetches the display names of all the existing end points.
        /// </summary>
        /// <returns>The the names of active all end points.</returns>
        public static List<string> GetTheNamesOfActiveAllEndPoints()
        {
            return (from tmpEndPoint in activeEndPoints
                select tmpEndPoint.displayName).ToList();
        }

        /// <summary>
        /// Fetches the end point URLs of all the existing end points.
        /// </summary>
        /// <returns>The the end point UR ls of all active end points.</returns>
        public static List<string> GetTheEndPointURLsOfAllActiveEndPoints()
        {
            return (from tmpEndPoint in activeEndPoints
                select tmpEndPoint.endPointURL).ToList();
        }

        #endregion
    }
}