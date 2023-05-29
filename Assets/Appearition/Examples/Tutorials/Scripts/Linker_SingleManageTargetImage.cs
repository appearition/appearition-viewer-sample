// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: "Linker_SingleManageTargetImage.cs" 
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace Appearition.Example
{
    public class Linker_SingleManageTargetImage : MonoBehaviour
    {
        //References
        public Button mainBT;
        public Image image;

        //Storage Variables
        public string assetId;

        /// <summary>
        /// Setup a single target image in the Experience Management tab, and store the asset ID for future referencing.
        /// Also requires an action to call when the target image is pressed.
        /// </summary>
        /// <param name="newAssetId">New asset identifier.</param>
        /// <param name="targetImage">Target image.</param>
        /// <param name="onButtonPressed">On button pressed.</param>
        public void Setup(string newAssetId, Sprite targetImage, Action<string> onButtonPressed)
        {
            assetId = newAssetId;

            if (image != null)
                image.sprite = targetImage;

            if (mainBT != null)
                mainBT.onClick.AddListener(() => { onButtonPressed(assetId); });
        }
    }
}