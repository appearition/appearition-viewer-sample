using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Appearition.AccountAndAuthentication;
using Appearition.ChannelManagement;
using Appearition.Packaging;
using Appearition.Packaging.API;
using UnityEngine;

namespace Appearition.Common
{
    public class AccountVerificationProcess : ICommonProcess, IDisposable
    {
        //Const messages
        public const string CHECK_REGISTRATION_MESSAGE = "Checking account details..";
        public const string REGISTRATION_TIMEOUT = "Your account activation seems to be taking longer than expected. Please restart the application later, or contact our support on the EMS Portal.";
        public const string NO_PACKAGES_AVAILABLE = "No package is available. Please contact our support on the Ems portal.";
        public const string PLEASE_SELECT_PACKAGE_MESSAGE = "Please select a package.";
        public const string SELECTING_PACKAGE_MESSAGE = "Selecting a package, please wait..";
        public const string PACKAGE_CHECK_TIMEOUT = "Your account is being activated, but is taking more time than expected. Please come back later.";
        public const string CHECKING_PACKAGE_MESSAGE = "Finalizing your account creation, please wait..";
        public const string ACCOUNT_VERIFICATION_COMPLETE_MESSAGE = "Account verification complete!";
        
        //Storage
        string _username;
        Action _onAccountVerified;
        Action<PackageStatus> _onPackageStatusReceived;
        Action<List<BasicPackage>> _packagesToSelect;
        Action<string> _onMessageReceived;
        Action<EmsError> _onFailure;
        Action<bool> _onComplete;
        string _backupSessionToken;

        //Configuration
        public float delayBetweenEachStatusCheck = 5.0f;
        public int maxAmountOfRegistrationStatusCheck = 10;

        //User input
        BasicPackage _selectedPackage;

        #region Constructor

        public AccountVerificationProcess(string username,
            Action onAccountVerified,
            Action<string> onMessageReceived,
            Action<List<BasicPackage>> packagesToSelect,
            Action<EmsError> onFailure,
            Action<bool> onComplete)
        {
            //Store all the goodies.
            _username = username;
            _onAccountVerified = onAccountVerified;
            _onMessageReceived = onMessageReceived;
            _packagesToSelect = packagesToSelect;
            _onFailure = onFailure;
            _onComplete = onComplete;
        }

        public AccountVerificationProcess(string username,
            Action onAccountVerified,
            Action<string> onMessageReceived,
            Action<PackageStatus> onPackageStatusReceived,
            Action<List<BasicPackage>> packagesToSelect,
            Action<EmsError> onFailure,
            Action<bool> onComplete)
        {
            //Store all the goodies.
            _username = username;
            _onMessageReceived = onMessageReceived;
            _onAccountVerified = onAccountVerified;
            _onPackageStatusReceived = onPackageStatusReceived;
            _packagesToSelect = packagesToSelect;
            _onFailure = onFailure;
            _onComplete = onComplete;
        }

        #endregion

        #region Main Process

        public IEnumerator ExecuteMainProcess()
        {
            //Backup the session token, if any.
            if (AppearitionGate.Instance.CurrentUser.IsUserLoggedIn && !string.IsNullOrWhiteSpace(AppearitionGate.Instance.CurrentUser.SessionToken))
            {
                _backupSessionToken = AppearitionGate.Instance.CurrentUser.SessionToken;
                AppearitionGate.Instance.CurrentUser.RemoveTokenByNameIfExisting(AppearitionConstants.PROFILE_SESSION_TOKEN_NAME);
            }
            
            #region Registration Check 

            //First step, check the registration status.
            AccountStatus accountStatus = null;
            EmsError? onFailureError = default;

            SendMessageCallback(CHECK_REGISTRATION_MESSAGE);
            int registrationStatusCheck = maxAmountOfRegistrationStatusCheck;

            while (!IsAccountStatusValid(accountStatus))
            {
                //Fire check
                yield return AccountHandler.CheckRegistrationStatusProcess(_username, onSuccess => accountStatus = onSuccess, onFailure => onFailureError = onFailure, null);

                //First check skips the timers
                if (!IsAccountStatusValid(accountStatus))
                {
                    registrationStatusCheck--;

                    //Check for timeout
                    if (registrationStatusCheck <= 0)
                        break;

                    //Handle wait timer between each request
                    float timeBetweenEachStatusCheck = delayBetweenEachStatusCheck;
                    while (timeBetweenEachStatusCheck > 0)
                    {
                        timeBetweenEachStatusCheck -= Time.deltaTime;
                        yield return null;
                    }
                }
            }

            //Handle timeout failure
            if (registrationStatusCheck <= 0)
            {
                onFailureError = new EmsError(REGISTRATION_TIMEOUT);
                OnProcessFailure(onFailureError.GetValueOrDefault());
                yield break;
            }

            if (_onAccountVerified != null)
                _onAccountVerified();

            #endregion

            //After registration, apply token.
            AppearitionGate.Instance.CurrentUser.AddOrModifyAuthenticationToken(AppearitionConstants.PROFILE_SESSION_TOKEN_NAME, accountStatus.SessionToken);


            //HACK, if a user has been given rights via the EMS portal by admins, skip the package check.
            //This hack is made by checking if the user has access to any channels. Don't handle error either.
            bool isGivenChannels = false;
            if (!string.IsNullOrWhiteSpace(_backupSessionToken))
            {
                Debug.Log("Checking channels hack..");
                //AppearitionGate.Instance.CurrentUser.AddOrModifyAuthenticationToken(AppearitionConstants.PROFILE_SESSION_TOKEN_NAME, accountStatus.SessionToken);
                yield return ChannelHandler.GetAllChannelsProcess(success => isGivenChannels = (success != null && success.Count > 0));
                //AppearitionGate.Instance.CurrentUser.RemoveTokenIfExisting(accountStatus.SessionToken);
            }

            if (!isGivenChannels)
            {
                //Check package status
                PackageStatusContainer packageStatusContainer = null;
                int packageId = 0;

                yield return PackageHandler.CheckPackageStatusProcess(success => packageStatusContainer = success, failure => onFailureError = failure);

                if (onFailureError.HasValue)
                {
                    OnProcessFailure(onFailureError.GetValueOrDefault());
                    yield break;
                }

                if (packageStatusContainer.Packages == null || packageStatusContainer.Packages.Length == 0)
                {
                    #region Select Package 

                    //If there is no package enabled, fetch all packages, and prompt the player to choose one; unless one is auto-select.
                    Package_AvailablePackages.ApiData allPackages = null;
                    yield return PackageHandler.GetAllAvailablePackagesProcess(success => allPackages = success, failure => onFailureError = failure);

                    if (onFailureError.HasValue)
                    {
                        OnProcessFailure(onFailureError.GetValueOrDefault());
                        yield break;
                    }

                    //Check if it's possible to auto-select a package. If so, do it. Otherwise, let the user choose one.
                    if (allPackages.SystemCanAutoSelectPackage)
                    {
                        SendMessageCallback(SELECTING_PACKAGE_MESSAGE);

                        //Auto select package!
                        yield return PackageHandler.SelectPackageAutomaticallyProcess(success => packageId = success.TenantPackageId, failure => onFailureError = failure);

                        if (onFailureError.HasValue)
                        {
                            OnProcessFailure(onFailureError.GetValueOrDefault());
                            yield break;
                        }

                        //All good? Proceed to the package status check then.
                    }
                    else if (allPackages.Packages != null && allPackages.Packages.Count > 0)
                    {
                        SendMessageCallback(PLEASE_SELECT_PACKAGE_MESSAGE);

                        if (_packagesToSelect != null)
                            _packagesToSelect.Invoke(allPackages.Packages);

                        while (_selectedPackage == null)
                            yield return null;

                        //Package received, submit it!
                        yield return PackageHandler.SelectPackageManuallyProcess(_selectedPackage, success => packageId = success.TenantPackageId, failure => onFailureError = failure);

                        if (onFailureError.HasValue)
                        {
                            OnProcessFailure(onFailureError.GetValueOrDefault());
                            yield break;
                        }

                        //All good? Proceed to the package status check then.
                    }
                    else
                    {
                        //No package??
                        onFailureError = new EmsError(NO_PACKAGES_AVAILABLE);
                        OnProcessFailure(onFailureError.GetValueOrDefault());
                        yield break;
                    }

                    #endregion
                }

                #region Check Package Status 

                SendMessageCallback(CHECKING_PACKAGE_MESSAGE);

                //Now just wait for the package to be activated. Upon timeout, let the user know it failed. Upon success, IT'S ALL GOOD AND DONE!
                float totalTimerUntilPackageCheckTimeout = packageStatusContainer.TooLongThresholdInSeconds;
                float delayUntilNextpackageCheck;
                float timerUntilNextPackageCheck = delayUntilNextpackageCheck = packageStatusContainer.StatusCheckIntervalInSeconds;

                while (totalTimerUntilPackageCheckTimeout > 0)
                {
                    //Handle timers
                    totalTimerUntilPackageCheckTimeout -= Time.deltaTime;
                    timerUntilNextPackageCheck -= Time.deltaTime;

                    if (timerUntilNextPackageCheck <= 0)
                    {
                        //Time to check the package!
                        yield return PackageHandler.CheckPackageStatusProcess(packageId, success => packageStatusContainer = success, failure => onFailureError = failure);

                        if (onFailureError.HasValue)
                            break;
                        
                        //Forward message
                        SendMessageCallback(packageStatusContainer.Message);
                        if (_onPackageStatusReceived != null)
                            _onPackageStatusReceived(packageStatusContainer.Packages.FirstOrDefault());

                        if (packageStatusContainer.Packages != null && packageStatusContainer.Packages.Length != 0)
                        {
                            PackageStatus status = packageStatusContainer.Packages.FirstOrDefault(o => o.PackageId == packageId);

                            Debug.Log(status + ", " + packageId);
                            
                            //Package found, check if it's ready
                            if (status != null && status.IsActive)
                                break;
                        }
                        
                        Debug.Log("=(");

                        //Otherwise, reset timer..
                        timerUntilNextPackageCheck = delayUntilNextpackageCheck;
                    }

                    yield return null;
                }

                //Handle timeout and failure.
                if (onFailureError.HasValue)
                {
                    OnProcessFailure(onFailureError.GetValueOrDefault());
                    yield break;
                }
                
                if (totalTimerUntilPackageCheckTimeout <= 0)
                {
                    onFailureError = new EmsError(PACKAGE_CHECK_TIMEOUT);
                    OnProcessFailure(onFailureError.GetValueOrDefault());
                    yield break;
                }

                #endregion
            }
            
            SendMessageCallback(ACCOUNT_VERIFICATION_COMPLETE_MESSAGE);

            //Output status!
            if (_onComplete != null)
                _onComplete(true);
        }

        #endregion

        #region User Input

        /// <summary>
        /// Method to call after receiving all the packages to select.
        /// Should contain the package that the user has selected for their account.
        /// Upon calling this method, the process will resume.
        /// </summary>
        /// <param name="package"></param>
        public void SelectPackage(BasicPackage package)
        {
            _selectedPackage = package;
        }

        #endregion

        #region utilities

        void SendMessageCallback(string message)
        {
            if (_onMessageReceived != null)
                _onMessageReceived(message);
        }

        /// <summary>
        /// Utility to quickly output failure. Saves copy-pasta.
        /// </summary>
        /// <param name="failure"></param>
        void OnProcessFailure(EmsError failure)
        {
            if (_onFailure != null)
                _onFailure(failure);
            if (_onComplete != null)
                _onComplete(false);
        }

        /// <summary>
        /// Whether or not the given account is considered as valid based on EMS standards.
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        bool IsAccountStatusValid(AccountStatus status)
        {
            if (status == null)
                return false;

            if (status.IsAccountLocked || !status.IsAccountVerified)
                return false;

            return true;
        }

        #endregion

        public void Dispose()
        {
            //Revert the tokens?
            if(!string.IsNullOrWhiteSpace(_backupSessionToken))
                AppearitionGate.Instance.CurrentUser.AddOrModifyAuthenticationToken(AppearitionConstants.PROFILE_SESSION_TOKEN_NAME, _backupSessionToken);
        }
    }
}