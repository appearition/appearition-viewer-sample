// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: TargetImage.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using UnityEngine;

namespace Appearition.ArTargetImageAndMedia
{
    /// <summary>
    /// Container of an Appearition Target Image JSON ApiData model.
    /// </summary>
    [System.Serializable]
    public class TargetImage
    {
        //Variables
        public int arImageId;
        public string checksum;
        public string fileName;
        public string url;
        [System.NonSerialized] public Sprite image;

        public TargetImage()
        {
        }

        /// <summary>
        /// Copy Constructor
        /// </summary>
        /// <param name="cc">C.</param>
        public TargetImage(TargetImage cc)
        {
            arImageId = cc.arImageId;
            checksum = cc.checksum;
            fileName = cc.fileName;
            url = cc.url;
        }
    }
}