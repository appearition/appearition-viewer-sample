// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: "EMSLoginUIB.cs" 
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.EventSystems;

namespace Appearition.Example
{
    [RequireComponent(typeof(EMSLoginB))]
    public class EMSLoginUIB : BaseEMSUITab
    {
        //References
        public InputField usernameIF;
        public InputField passwordIF;
        public Button loginBT;
        public Button autologinBT;
        public Text loginBTText;
        public Text outcomeText;
        public Text currentSessionTokenText;

        //Internal Variables
        float delayBetweenDots = 0.8f;
        public string loginSuccessString = "You have successfully logged in !";
        public string loginFailureString = "Your login and password were incorrect, and you were not able to login. " +
                                           "Do ensure that the EMS Token, end point and tenant settings are correct. In case of doubt, please contact our support team. ";

        public string autoLoginUsername = "a@a.a";
        public string autoLoginPassword = "aaaa";

        //Handy Properties
        EMSLoginB _loginB;

        /// <summary>
        /// Lazy referencing to the object handling the EMS requests for the login.
        /// </summary>
        /// <value>The login b.</value>
        EMSLoginB loginB
        {
            get
            {
                if (_loginB == null)
                    _loginB = this.GetComponent<EMSLoginB>();
                return _loginB;
            }
        }

        void Start()
        {
            if (currentSessionTokenText != null)
                currentSessionTokenText.text = "(Application)" + AppearitionGate.Instance.CurrentUser.applicationToken;
            if (loginBT != null)
                loginBT.interactable = false;
        }

        new void Update()
        {
            base.Update();

            if (currentSessionTokenText != null)
                currentSessionTokenText.text = (AppearitionGate.Instance.CurrentUser.IsUserLoggedIn ? "(session)" : "(Application)") +
                                               AppearitionGate.Instance.CurrentUser.GetHighestLevelAuthenticationToken;

            //Enable tabbing
            if (Input.GetKeyDown(KeyCode.Tab) && usernameIF != null && passwordIF != null)
            {
                if (usernameIF.isFocused)
                    passwordIF.Select();
                else if (passwordIF.isFocused)
                    usernameIF.Select();
            }
        }

        #region Button & UI Events

        /// <summary>
        /// Occurs whenever an input field (username or password) has been changed. Used to determine whether the Login button should be available or not.
        /// </summary>
        /// <param name="text">Text.</param>
        public void OnInputFieldValueChanged(string text)
        {
            if (loginBT != null)
                loginBT.interactable = (usernameIF != null && passwordIF != null && usernameIF.text.Length > 0 && passwordIF.text.Length > 0);
        }

        /// <summary>
        /// Occurs whenever the Login button is pressed. Handles the login.
        /// </summary>
        public void OnLoginButtonPressed()
        {
            if (usernameIF != null && passwordIF != null)
                loginB.StartLoginProcess(usernameIF.text, passwordIF.text);
        }

        /// <summary>
        /// As a shortcut, there's an autologin feature, because typing is exhausting.
        /// </summary>
        public void OnAutoLoginButtonPressed()
        {
            if (usernameIF != null)
                usernameIF.text = autoLoginUsername;
            if (passwordIF != null)
                passwordIF.text = autoLoginPassword;
            OnLoginButtonPressed();
        }

        #endregion

        /// <summary>
        /// Returns a dot animation object using the proper UI.
        /// </summary>
        /// <returns>The dot animation object.</returns>
        public DotAnimation GetDotAnimationObject()
        {
            Text mainText = loginBTText;
            List<Button> buttonsToDisable = new List<Button>();
            if (loginBT != null)
                buttonsToDisable.Add(loginBT);
            if (autologinBT != null)
                buttonsToDisable.Add(autologinBT);

            return new DotAnimation(buttonsToDisable, mainText, delayBetweenDots, 3);
        }


        public void DisplayOutcome(bool isLoginSuccess, List<string> errors)
        {
            if (outcomeText != null)
            {
                if (isLoginSuccess)
                    outcomeText.text = loginSuccessString + " Your session token is: " + AppearitionGate.Instance.CurrentUser.SessionToken;
                else
                    outcomeText.text = (errors == null ? loginFailureString : "<b>" + errors.FirstOrDefault() + "</b>. " + loginFailureString);
            }
        }
    }
}