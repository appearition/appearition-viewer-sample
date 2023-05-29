// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: "EMSInformationUIB.cs" 
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;

namespace Appearition.Example
{
    /// <summary>
    /// Displays the information of the EMS, fetched and provided by EMSInformationB.
    /// </summary>
    [RequireComponent(typeof(EMSInformationB))]
    public class EMSInformationUIB : BaseEMSUITab
    {
        //References
        public Text endPointText;
        public Text tenantNameText;
        public Text channelIdText;
        public Text channelNameText;
        public Text amountOfExperiencesText;

        public void DisplayBaseContent()
        {
            //Display the variables already stored on the current user.
            if (endPointText != null)
            {
                if (AppearitionGate.Instance != null)
                    endPointText.text += " " + AppearitionGate.Instance.CurrentUser.SelectedEndPoint;
                else
                    endPointText.text += " N/A";
            }

            if (tenantNameText != null)
            {
                if (AppearitionGate.Instance != null)
                    tenantNameText.text += " " + AppearitionGate.Instance.CurrentUser.selectedTenant;
                else
                    tenantNameText.text += " N/A";
            }

            if (channelIdText != null)
            {
                if (AppearitionGate.Instance != null)
                    channelIdText.text += " " + AppearitionGate.Instance.CurrentUser.selectedChannel;
                else
                    channelIdText.text += " N/A";
            }
        }

        public void DisplayChannelName(string channelName)
        {
            //Displays the information fetched from the EMS.
            if (channelNameText != null)
            {
                if (!string.IsNullOrEmpty(channelName))
                    channelNameText.text += " " + channelName;
                else
                    channelNameText.text += "N/A";
            }
        }

        public void DisplayAmountOfExperiences(int amountOfExperiences)
        {
            if (amountOfExperiencesText != null)
                amountOfExperiencesText.text += " " + amountOfExperiences;
        }
    }
}