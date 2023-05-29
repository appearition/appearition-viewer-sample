// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: "EMSGetExperiencesB.cs" 
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;
using Appearition.ArTargetImageAndMedia;
using System;

namespace Appearition.Example
{
    /// <summary>
    /// Fetches all the experiences from the EMS, and lets a UI display them.
    /// </summary>
    public class EMSGetExperiencesB : MonoBehaviour
    {
        /// <summary>
        /// Launches a request to fetch all the experiences attached on a given channel. This method is called by the UI GetAllExperiences button.
        /// </summary>
        /// <param name="onComplete">On complete.</param>
        public void GetAllExperiences(Action<List<Asset>> onComplete)
        {
            // Here we are getting all the experiences and making use of the downloading features.
            // We are downloading and saving the TargetImages as they show on the EMS to be re-used on buttons, and saving the ApiData locally for offline reusability purposes.
            ArTargetHandler.GetChannelExperiences(true, false, false, (onComplete));
        }
    }
}