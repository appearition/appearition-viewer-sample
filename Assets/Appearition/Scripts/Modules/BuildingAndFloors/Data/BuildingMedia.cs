using System;
using System.Collections;
using System.Collections.Generic;
using Appearition.Common;
using UnityEngine;

namespace Appearition.BuildingsAndFloors
{
    [System.Serializable]
    public class BuildingMedia
    {
        public int id;
        public int buildingId;
        public int productId;
        public string mediaFileKey;

        public string fileName;
        public string originalFileName;
        public string mediaType;
        public string mimeType;
        public string checksum;
        public long fileSizeBytes;
        public string createdUtcDate;
        public System.DateTime CreatedUtcDate => AppearitionGate.ConvertStringToDateTime(createdUtcDate);
        public string modifiedUtcDate;
        public System.DateTime ModifiedUtcDate => AppearitionGate.ConvertStringToDateTime(modifiedUtcDate);

        public string text;
        public float resolution;
        public bool isPrivate;

        public string url;

        //Properties
        [System.Serializable]
        class PropertyContainer
        {
            public List<Property> properties = new List<Property>();
        }

        public List<Property> Properties { get; private set; } = new List<Property>();

        #region Constructors

        public BuildingMedia()
        {
            ExtractPropertiesFromText();
        }

        public BuildingMedia(BuildingMedia cc)
        {
            CopyFrom(cc);
        }

        public void CopyFrom(BuildingMedia cc)
        {
            id = cc.id;
            buildingId = cc.buildingId;
            productId = cc.productId;
            mediaFileKey = cc.mediaFileKey;

            fileName = cc.fileName;
            originalFileName = cc.originalFileName;
            mediaType = cc.mediaType;
            mimeType = cc.mimeType;
            checksum = cc.checksum;
            fileSizeBytes = cc.fileSizeBytes;
            createdUtcDate = cc.createdUtcDate;
            modifiedUtcDate = cc.modifiedUtcDate;

            text = cc.text;
            resolution = cc.resolution;
            url = cc.url;
            ExtractPropertiesFromText();
        }

        /// <summary>
        /// Creates a new Building Media from a given mediatype
        /// </summary>
        /// <param name="givenFilename"></param>
        /// <param name="givenMediaType"></param>
        public BuildingMedia(string givenFilename, MediaType givenMediaType)
        {
            fileName = originalFileName = givenFilename;
            mediaType = givenMediaType.Name;
            mimeType = givenMediaType.GetMimeTypeForGivenExtension(givenFilename);
        }

        /// <summary>
        /// Creates a new Building Media from a given mediatype and data template
        /// </summary>
        /// <param name="givenFilename"></param>
        /// <param name="cc"></param>
        /// <param name="givenMediaType"></param>
        public BuildingMedia(string givenFilename, BuildingMedia cc, MediaType givenMediaType)
        {
            CopyFrom(cc);
            fileName = originalFileName = givenFilename;
            mediaType = givenMediaType.Name;
            mimeType = givenMediaType.GetMimeTypeForGivenExtension(givenFilename);
        }

        #endregion

        #region Text KVP

        protected void ExtractPropertiesFromText()
        {
            if (!string.IsNullOrEmpty(text) && text.Length > 0 && text.Contains("{"))
            {
                PropertyContainer container = AppearitionConstants.DeserializeJson<PropertyContainer>(text);

                if (container != null && container.properties != null && container.properties.Count > 0)
                    Properties = new List<Property>(container.properties);
            }
        }

        public void DebugPropertiesToJson(List<Property> properties)
        { 
            Debug.Log(AppearitionConstants.SerializeJson(new PropertyContainer { properties = new List<Property>(properties)}));
        }

        #endregion
    }
}