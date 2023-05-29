// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: Location.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace Appearition.Location
{
    [System.Serializable]
    public class Location
    {
        public int ProductId;
        public int Id;
        public string Name;
        public string TriggerType;
        public string ActionType;

        public List<PointOfInterest> Pois;
        public List<GlobalPoint> GlobalPoints;

        public Location()
        {
        }

        public Location(Location cc)
        {
            ProductId = cc.ProductId;
            Id = cc.Id;
            Name = cc.Name;
            TriggerType = cc.TriggerType;
            ActionType = cc.ActionType;

            Pois = new List<PointOfInterest>();
            if (cc.Pois != null)
            {
                for (int i = 0; i < cc.Pois.Count; i++)
                    Pois.Add(new PointOfInterest(cc.Pois[i]));
            }

            GlobalPoints = new List<GlobalPoint>();
            if (cc.GlobalPoints != null)
            {
                for (int i = 0; i < cc.GlobalPoints.Count; i++)
                    GlobalPoints.Add(new GlobalPoint(cc.GlobalPoints[i]));
            }
        }
    }
}