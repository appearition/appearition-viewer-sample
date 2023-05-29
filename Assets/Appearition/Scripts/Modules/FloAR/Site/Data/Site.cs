// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: Site.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace Appearition.Site
{
    [System.Serializable]
    public class Site
    {
        public long SiteId;
        public string SiteName;
        public long MainsHybridGenset;
        public long ShelterType;
        public string AtlType;
        public string EquipmentDisposition;
        public string UpgradeSpecialRequirements;
        public string Region;
        public string SiteType;
        public string Housing;
        public string AccessDetails;
        public string Map;
        public string MapRef;
        public string Address;
        public string AmgGrid;
        public string Easting;
        public string Northing;
        public string MobileCover;
        public string KeyAccess;
        public string Owner;
        public bool IsKeySafeInstalled;
        public string CompoundKey;
        public string ShelterKey;
        public string CabinetKey;
        public float Latitude;
        public float Longitude;
        public List<SiteDocument> Docs;


        static Dictionary<long, string> _hybridGensetTypeNameDictionary = new Dictionary<long, string>
            {{901, "Standard"}, {902, "FTR"}, {903, "Generator"}, {904, "HPA"}, {905, "LGA"}, {906, "Solar Hybrid"}};

        static Dictionary<long, string> _getShelterTypeNameDictionary = new Dictionary<long, string>
            {{101, "Insitu"}, {102, "Shelter"}, {103, "ODU"}};

        /// <summary>
        /// Fetches the name of a hybrid genset type from a given id.
        /// </summary>
        /// <param name="mainsHybridGenset"></param>
        /// <returns></returns>
        public static string GetHybridGensetTypeName(long mainsHybridGenset)
        {
            if (_hybridGensetTypeNameDictionary.ContainsKey(mainsHybridGenset))
                return _hybridGensetTypeNameDictionary[mainsHybridGenset];
            return "";
        }

        /// <summary>
        /// Fetches the name of a shelter type from a given id.
        /// </summary>
        /// <param name="shelterType"></param>
        /// <returns></returns>
        public static string GetShelterTypeName(long shelterType)
        {
            if (_getShelterTypeNameDictionary.ContainsKey(shelterType))
                return _getShelterTypeNameDictionary[shelterType];
            return "";
        }
    }
}