// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: "EMSInformationB.cs" 
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using System.Collections;
using UnityEngine;
using Appearition.Common;
using System.Linq;
using Appearition.ArTargetImageAndMedia;
using Appearition.ChannelManagement;

namespace Appearition.Example
{
    /// <summary>
    /// Fetches a sample information from the EMS. 
    /// In this case, the channel's name and the amount of experiences it holds.
    /// </summary>
    public class EMSInformationB : MonoBehaviour
    {
        private EMSInformationUIB _infoUib;

        /// <summary>
        /// Lazy referencing to the UI object.
        /// </summary>
        /// <value>The info user interface.</value>
        private EMSInformationUIB InfoUib
        {
            get
            {
                if (_infoUib == null)
                    _infoUib = GetComponent<EMSInformationUIB>();
                return _infoUib;
            }
        }

        private IEnumerator Start()
        {
            //Wait for the gate before setting any UI.
            while (AppearitionGate.Instance == null)
                yield return null;

            //Display the ApiData on the UI object.
            if (InfoUib != null)
                InfoUib.DisplayBaseContent();

            // Handle the channel name display. Here we are using the GetAllChannel because it does not require any session token (login).
            // We get the one with the ID plugged in the user, and simply display the result on the screen. 
            ChannelHandler.GetAllChannels(outcome =>
            {
                if (InfoUib != null)
                {
                    if (outcome != null && outcome.Count > 0)
                    {
                        Channel relatedChannel = outcome.FirstOrDefault(o => o.channelId == AppearitionGate.Instance.CurrentUser.selectedChannel);

                        InfoUib.DisplayChannelName(relatedChannel == null ? null : relatedChannel.name);
                    }
                    else
                        InfoUib.DisplayChannelName(null);
                }
            }, error =>
            {
                //In case of error, just display a default null value.
                if (InfoUib != null)
                    InfoUib.DisplayChannelName(null);
            });

            // Fetch all the experiences on the current channel.
            // This request is one of the most popular, since it fetches the ApiData on the EMS, and returns it in a presentable way.
            // This method contains several overloads allowing utilities for storage as well.
            ArTargetHandler.GetChannelExperiences(outcome =>
            {
                //Find all the experiences associated with the given channel.
                if (outcome != null && InfoUib != null)
                    InfoUib.DisplayAmountOfExperiences(outcome.Count);
            }, error =>
            {
                //Display a debug number.
                if (InfoUib != null)
                    InfoUib.DisplayAmountOfExperiences(-1);
            });
        }
    }
}