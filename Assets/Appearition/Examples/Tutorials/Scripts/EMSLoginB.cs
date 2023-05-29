// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: "EMSLoginB.cs" 
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using System.Collections;
using UnityEngine;
using Appearition.AccountAndAuthentication;
using Appearition.AccountAndAuthentication.API;
using Appearition.Common;

namespace Appearition.Example
{
    /// <summary>
    /// Example script of a use of a login.
    /// The script is divided between a process (EMSLoginB) and a UI script (EMSloginUIB).
    /// Here are shown two ways of handling a login: Using the AppearitionGate, which is at a higher level, 
    /// and using the AppearitionRequest, which happens at a lower level but with more control.
    /// </summary>
    public class EMSLoginB : MonoBehaviour
    {
        //Internal Variables
        public bool useAppearitionGate = true;

        //Handy Properties
        EMSLoginUIB _loginUib;

        /// <summary>
        /// Lazy referencing to the object handling the EMS requests for the login.
        /// </summary>
        /// <value>The login uib.</value>
        private EMSLoginUIB LoginUib
        {
            get
            {
                if (_loginUib == null)
                    _loginUib = GetComponent<EMSLoginUIB>();
                return _loginUib;
            }
        }

        //ReusableVariables
        private readonly Account_Authenticate _authenticateRequestRv = null;

        public void StartLoginProcess(string username, string password)
        {
            StartCoroutine(useAppearitionGate
                ? LoginUsingGate_PROCESS(username, password)
                : LoginProcess(username, password));
        }

        #region Appearition Gate Method (simpler)

        /// <summary>
        /// Example of relying on the Appearition Gate to letting the login be taken cared of.
        /// </summary>
        /// <returns>The appearition gate handle login.</returns>
        /// <param name="username">Username.</param>
        /// <param name="password">Password.</param>
        IEnumerator LoginUsingGate_PROCESS(string username, string password)
        {
            //Reset / prepare vars.
            bool? isLoginSuccess = null;
            EmsError errorIfAny = new EmsError();
            if (LoginUib != null && LoginUib.outcomeText != null)
                LoginUib.outcomeText.text = "";

            //Launch request
            AccountHandler.Login(username, password, onFailure: tmpError => { errorIfAny = tmpError; }, onComplete: outcome => { isLoginSuccess = outcome; });

            //Do some little waiting animation?
            using (DotAnimation dotAnimation = GetDotAnimationObject())
            {
                while (!isLoginSuccess.HasValue)
                {
                    if (dotAnimation != null)
                        dotAnimation.UpdateDisplay();
                    yield return null;
                }
            }

            //Complete! Update the UI.
            if (LoginUib != null)
                LoginUib.DisplayOutcome(isLoginSuccess.Value, errorIfAny.Errors);
        }

        #endregion

        #region Appearition Gate Callback (simplest, no callback handling)

        /// <summary>
        /// Logs in to the EMS in the simplest way.
        /// </summary>
        /// <param name="username">Username.</param>
        /// <param name="password">Password.</param>
        private void LoginUsingGate_Simplest(string username, string password)
        {
            AccountHandler.Login(username, password, onComplete: outcome =>
            {
                if (LoginUib != null)
                    LoginUib.DisplayOutcome(outcome, null);
            });
        }

        #endregion

        #region Appearition Request Method

        /// <summary>
        /// Example of login with full control
        /// </summary>
        /// <returns>The PROCESS.</returns>
        /// <param name="username">Username.</param>
        /// <param name="password">Password.</param>
        IEnumerator LoginProcess(string username, string password)
        {
            //Reset / prepare vars.
            if (LoginUib != null && LoginUib.outcomeText != null)
                LoginUib.outcomeText.text = "";

            Account_Authenticate loginOutcome = null;

            //Launch request
            AppearitionRequest<Account_Authenticate> loginRequest =
                AppearitionRequest<Account_Authenticate>.LaunchAPICall_POST<AccountHandler>(AppearitionGate.Instance.CurrentUser.selectedChannel,
                    _authenticateRequestRv,
                    new Account_Authenticate.PostData()
                    {
                        username = username,
                        password = password,
                        appId = Application.identifier
                    },
                    null,
                    outcome => { loginOutcome = outcome; }
                );

            //Do some little waiting animation?
            using (DotAnimation dotAnimation = GetDotAnimationObject())
            {
                while (!loginRequest.IsDone)
                {
                    if (dotAnimation != null)
                        dotAnimation.UpdateDisplay();
                    yield return null;
                }
            }

            //Apply the token.
            if (loginOutcome.IsSuccess)
            {
                AppearitionGate.Instance.CurrentUser.AddOrModifyAuthenticationToken(AppearitionConstants.PROFILE_SESSION_TOKEN_NAME, loginOutcome.Data.sessionToken);
            }

            //Complete !
            if (LoginUib != null)
                LoginUib.DisplayOutcome(loginRequest.RequestResponseObject.IsSuccess, loginRequest.Errors.Errors);
        }

        #endregion


        DotAnimation GetDotAnimationObject()
        {
            if (LoginUib == null)
                return null;
            return LoginUib.GetDotAnimationObject();
        }
    }
}