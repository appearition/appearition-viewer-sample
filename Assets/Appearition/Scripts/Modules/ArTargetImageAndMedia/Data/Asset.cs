// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: Asset.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using Appearition.Common;

namespace Appearition.ArTargetImageAndMedia
{
    /// <summary>
    /// Container of an Appearition Asset JSON ApiData model.
    /// </summary>
    [System.Serializable]
    public class Asset
    {
        //Variables
        public string assetId;
        public int productId;
        public string name;
        public string createdByUsername;
        public int mediaGridWidth;
        public List<MediaFile> mediaFiles;
        public List<TargetImage> targetImages;
        public List<string> tags;

        public string createdUtcDate;
        public string createdUtcDateStr;
        public System.DateTime CreatedUtcDate => AppearitionGate.ConvertStringToDateTime(createdUtcDateStr);
        public string modifiedUtcDate;
        public string modifiedUtcDateStr;
        public System.DateTime ModifiedUtcDate => AppearitionGate.ConvertStringToDateTime(modifiedUtcDateStr);

        public string shortDescription;
        public string longDescription;
        /// <summary>
        /// The copyright needs to be fetched when required using ArTargetHandler.GetExperienceCopyrightInfoProcess.
        /// </summary>
        public string copyrightInfo;
        
        public Asset()
        {
        }

        /// <summary>
        /// Copy Constructor
        /// </summary>
        /// <param name="cc">C.</param>
        public Asset(Asset cc)
        {
            CopyValuesFrom(cc);
        }

        public virtual void CopyValuesFrom(Asset cc)
        {
            assetId = cc.assetId;
            productId = cc.productId;
            name = cc.name;
            createdByUsername = cc.createdByUsername;
            mediaGridWidth = cc.mediaGridWidth;
            if (cc.mediaFiles != null)
            {
                mediaFiles = new List<MediaFile>();
                for (int i = 0; i < cc.mediaFiles.Count; i++)
                    mediaFiles.Add(new MediaFile(cc.mediaFiles[i]));
            }

            if (cc.targetImages != null)
            {
                targetImages = new List<TargetImage>();
                for (int i = 0; i < cc.targetImages.Count; i++)
                    targetImages.Add(new TargetImage(cc.targetImages[i]));
            }

            if (cc.tags != null)
            {
                tags = new List<string>(cc.tags);
            }

            createdUtcDate = cc.createdUtcDate;
            createdUtcDateStr = cc.createdUtcDateStr;
            modifiedUtcDate = cc.modifiedUtcDate;
            modifiedUtcDateStr = cc.modifiedUtcDateStr;

            shortDescription = cc.shortDescription;
            longDescription = cc.longDescription;
            copyrightInfo = cc.copyrightInfo;
        }

        public bool IsImageDownloaded(int targetIndex = 0)
        {
            if (targetImages == null || targetIndex >= targetImages.Count)
                return false;

            return File.Exists(ArTargetHandler.GetPathToTargetImage(this, targetImages[targetIndex]));
        }

        public bool IsImageDownloaded(TargetImage target)
        {
            if (targetImages == null || target == null)
                return false;

            return File.Exists(ArTargetHandler.GetPathToTargetImage(this, target));
        }

        /// <summary>
        /// Whether this asset contains a tag as defined in the EMS.
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public bool ContainsTag(string tag)
        {
            if (tags == null)
            {
                tags = new List<string>();
                return false;
            }

            for (int i = 0; i < tags.Count; i++)
            {
                if (tags[i].Equals(tag, StringComparison.InvariantCultureIgnoreCase))
                    return true;
            }

            return false;
        }
    }
}