using System;
using System.Reflection;
using System.Linq;
using UnityEngine;

namespace Appearition.ArDemo
{
    /// <summary>
    /// Contains all the settings for the ArDemo Example.
    /// </summary>
    public static class ArDemoConstants 
    {
        public const string ARPREFAB_FOLDER_NAME_IN_RESOURCES = "ArPrefabs";
        public const bool USING_CLOUD_RECO = true;
        public const float ASSETBUNDLE_SCALE_MULTIPLIER = 0.125f;

        //Tags
        public const string TAG_MARKER = "Marker";
        public const string TAG_MARKERLESS = "Markerless";
        public const string TAG_INTERACTIVE = "Interactive";

        /// <summary>
        /// Fetches a placeholder sprite which can be used while waiting for an image to load.
        /// </summary>
        /// <returns></returns>
        public static Sprite GetImagePlaceholder ()
        {
           //return Resources.Load("ArDemo_Placeholder", typeof(Sprite)) as Sprite;
           return Resources.Load("Transparent_Empty", typeof(Sprite)) as Sprite;
        }

        public static Sprite GetTargetImagePlaceholder()
        {
            return Resources.Load("EducART_MarkerlessTargetImage", typeof(Sprite)) as Sprite;
        }
    }
}