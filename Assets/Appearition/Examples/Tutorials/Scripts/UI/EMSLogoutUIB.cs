// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: "EMSLogoutUIB.cs" 
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;

namespace Appearition.Example
{
    [RequireComponent(typeof(EMSLogoutB))]
    public class EMSLogoutUIB : BaseEMSUITab
    {
        //References
        public Button logoutButton;
        public Text logoutOutcomeText;

        new void Update()
        {
            base.Update();

            if (logoutButton != null)
                logoutButton.interactable = AppearitionGate.Instance.CurrentUser.IsUserLoggedIn;
        }

        public void OnLogoutButtonPressed()
        {
            transform.GetComponent<EMSLogoutB>().Logout(obj =>
            {
                if (logoutOutcomeText != null)
                    logoutOutcomeText.text = obj ? "Logout successfully." : "Unable to logout. The user was most likely not authenticated.";
            });
        }
    }
}