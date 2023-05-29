// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: GlobalPoint.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

namespace Appearition.Location
{
    [System.Serializable]
    public class GlobalPoint
    {
        public int Id;
        public string StreetAddress;
        public float Latitude;
        public float Longitude;
        public float Altitude;

        public GlobalPoint()
        {
        }

        /// <summary>
        /// Copy Constructor
        /// </summary>
        /// <param name="cc"></param>
        public GlobalPoint(GlobalPoint cc)
        {
            Id = cc.Id;
            StreetAddress = cc.StreetAddress;
            Latitude = cc.Latitude;
            Longitude = cc.Longitude;
            Altitude = cc.Altitude;
        }
    }
}