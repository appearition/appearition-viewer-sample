// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: ChannelImage.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using UnityEngine;

namespace Appearition.ChannelManagement
{
    /// <summary>
    /// Container of an Appearition Channel Image JSON ApiData model.
    /// </summary>
    [System.Serializable]
    public class ChannelImage
    {
        //Variables
        public int channelImageId;
        public string fileName;
        public string url;
        public string checksum;
        [System.NonSerialized] public Sprite image;

        public ChannelImage()
        {
        }

        /// <summary>
        /// Copy Constructor
        /// </summary>
        /// <param name="cc">C.</param>
        public ChannelImage(ChannelImage cc)
        {
            channelImageId = cc.channelImageId;
            fileName = cc.fileName;
            url = cc.url;
            checksum = cc.checksum;
        }
    }
}