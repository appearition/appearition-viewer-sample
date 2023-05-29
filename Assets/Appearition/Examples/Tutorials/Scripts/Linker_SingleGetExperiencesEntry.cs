// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: "Linker_SingleGetExperiencesEntry.cs" 
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Appearition.ArTargetImageAndMedia;
using Appearition.Common;

namespace Appearition.Example
{
    public class Linker_SingleGetExperiencesEntry : MonoBehaviour
    {
        //References
        public Button mainBT;
        public Text titleText;

        //Internal Variables
        [HideInInspector] public Asset storedAsset;

        /// <summary>
        /// Setup the visuals for a single experience entry.
        /// </summary>
        /// <param name="getExperienceHandler">Get experience handler.</param>
        /// <param name="tmpAsset">Tmp asset.</param>
        public void Setup(EMSGetExperiencesUIB getExperienceHandler, Asset tmpAsset)
        {
            storedAsset = tmpAsset;

            //Bind event
            if (mainBT != null)
                mainBT.onClick.AddListener(() => { getExperienceHandler.OnExperienceButtonPressed(this); });

            //Display !
            if (mainBT != null && mainBT.image != null && tmpAsset.targetImages != null && tmpAsset.targetImages.Count > 0 &&
                !string.IsNullOrEmpty(tmpAsset.targetImages[0].url) && tmpAsset.targetImages[0].image != null)
            {
                mainBT.image.sprite = tmpAsset.targetImages[0].image;
            }

            if (titleText != null && tmpAsset.name != null)
                titleText.text = tmpAsset.name;
        }

    }
}