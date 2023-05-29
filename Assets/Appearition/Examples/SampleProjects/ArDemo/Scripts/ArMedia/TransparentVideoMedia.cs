using System;
using System.Collections;
using System.Collections.Generic;
using Appearition.ArTargetImageAndMedia;
using UnityEngine;

namespace Appearition.ArDemo
{
    public class TransparentVideoMedia : VideoMedia
    {
        public TransparentVideoMedia(MediaFile cc) : base(cc)
        {
        }

        public override float GenerateMediaAssociationScore()
        {
            return mediaType.Equals("transparent_video", StringComparison.InvariantCultureIgnoreCase) ? 1.0f : 0f;
        }
        
        public override string PathToPrefab => $"{ArDemoConstants.ARPREFAB_FOLDER_NAME_IN_RESOURCES}/transparent_video";
    }
}