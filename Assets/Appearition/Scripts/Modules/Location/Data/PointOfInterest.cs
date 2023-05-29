// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: PointOfInterest.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using UnityEngine;

namespace Appearition.Location
{
    [System.Serializable]
    public class PointOfInterest
    {
        public int Id;
        public string Language;

        public string MarkerImageFileName;
        public string MarkerImageUrl;
        public string MarkerImageChecksum;
        [System.NonSerialized] public Sprite MarkerImageSprite;

        public bool IsDisplayLabel;
        public string LabelText;
        public string LabelImageFileName;
        public string LabelImageUrl;
        public string LabelImageChecksum;
        [System.NonSerialized] public Sprite LabelImageSprite;

        public bool IsDisplayInfo;
        public string InfoTitle;
        public string InfoHtml;
        public string InfoImageFileName;
        public string InfoImageUrl;
        public string InfoImageChecksum;
        [System.NonSerialized] public Sprite InfoImageSprite;

        public PointOfInterest()
        {
        }

        /// <summary>
        /// Copy Constructor
        /// </summary>
        /// <param name="cc"></param>
        public PointOfInterest(PointOfInterest cc)
        {
            Id = cc.Id;
            Language = cc.Language;

            MarkerImageFileName = cc.MarkerImageFileName;
            MarkerImageUrl = cc.MarkerImageUrl;
            MarkerImageChecksum = cc.MarkerImageChecksum;

            IsDisplayLabel = cc.IsDisplayLabel;
            LabelText = cc.LabelText;
            LabelImageFileName = cc.LabelImageFileName;
            LabelImageUrl = cc.LabelImageUrl;
            LabelImageChecksum = cc.LabelImageChecksum;

            IsDisplayInfo = cc.IsDisplayInfo;
            InfoTitle = cc.InfoTitle;
            InfoHtml = cc.InfoHtml;
            InfoImageFileName = cc.InfoImageFileName;
            InfoImageUrl = cc.InfoImageUrl;
            InfoImageChecksum = cc.InfoImageChecksum;
        }
    }
}