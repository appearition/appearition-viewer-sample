using System;
using System.Collections;
using System.Collections.Generic;
using Appearition.Common;
using Appearition.Packaging.API;
using UnityEngine;

namespace Appearition.Packaging
{
    public class PackageHandler : BaseHandler
    {
        #region Get All Packages

        public static void GetAllAvailablePackages(Action<Package_AvailablePackages.ApiData> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(GetAllAvailablePackagesProcess(onSuccess, onFailure, onComplete));
        }

        public static IEnumerator GetAllAvailablePackagesProcess(Action<Package_AvailablePackages.ApiData> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            //Online request. Launch Request
            var getAvailablePackagesRequest =
                AppearitionRequest<Package_AvailablePackages>.LaunchAPICall_GET<PackageHandler>(AppearitionGate.Instance.CurrentUser.selectedChannel,
                    GetReusableApiRequest<Package_AvailablePackages>());

            while (!getAvailablePackagesRequest.IsDone)
                yield return null;

            //Handle response
            if (getAvailablePackagesRequest.RequestResponseObject.IsSuccess)
            {
                AppearitionLogger.LogInfo("All available packages have been successfully fetched!");

                if (onSuccess != null)
                    onSuccess(getAvailablePackagesRequest.RequestResponseObject.Data);
            }
            else
            {
                AppearitionLogger.LogError("An error occured when trying to fetch all available packages for this current user.");

                if (onFailure != null)
                    onFailure(getAvailablePackagesRequest.Errors);
            }

            if (onComplete != null)
                onComplete(getAvailablePackagesRequest.RequestResponseObject.IsSuccess);
        }

        #endregion

        #region Check Status

        public static void CheckPackageStatus(Action<PackageStatusContainer> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            CheckPackageStatus(0, onSuccess, onFailure, onComplete);
        }

        public static IEnumerator CheckPackageStatusProcess(Action<PackageStatusContainer> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            yield return CheckPackageStatusProcess(0, onSuccess, onFailure, onComplete);
        }

        public static void CheckPackageStatus(int packageId, Action<PackageStatusContainer> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(CheckPackageStatusProcess(packageId, onSuccess, onFailure, onComplete));
        }

        public static IEnumerator CheckPackageStatusProcess(int packageId, Action<PackageStatusContainer> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            var requestContent = new Package_CheckStatus.RequestContent() {
                packageId = packageId
            };

            //Online request. Launch Request
            var packageStatusCheckRequest =
                AppearitionRequest<Package_CheckStatus>.LaunchAPICall_GET<PackageHandler>(AppearitionGate.Instance.CurrentUser.selectedChannel, GetReusableApiRequest<Package_CheckStatus>(), requestContent);

            while (!packageStatusCheckRequest.IsDone)
                yield return null;

            //Handle response
            if (packageStatusCheckRequest.RequestResponseObject.IsSuccess)
            {
                AppearitionLogger.LogInfo("Package status successfully fetched!");

                if (onSuccess != null)
                    onSuccess(packageStatusCheckRequest.RequestResponseObject.Data);
            }
            else
            {
                AppearitionLogger.LogError("An error occured when trying to fetch the user's package status.");

                if (onFailure != null)
                    onFailure(packageStatusCheckRequest.Errors);
            }

            if (onComplete != null)
                onComplete(packageStatusCheckRequest.RequestResponseObject.IsSuccess);
        }

        #endregion

        #region Select Package Automatically

        public static void SelectPackageAutomatically(Action<PackageProcessingStatus> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(SelectPackageAutomaticallyProcess(onSuccess, onFailure, onComplete));
        }

        public static IEnumerator SelectPackageAutomaticallyProcess(Action<PackageProcessingStatus> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            //Online request. Launch Request
            var autoSelectPackageRequest =
                AppearitionRequest<Package_AutoSelectPackage>.LaunchAPICall_POST<PackageHandler>(AppearitionGate.Instance.CurrentUser.selectedChannel,
                    GetReusableApiRequest<Package_AutoSelectPackage>(), null);

            while (!autoSelectPackageRequest.IsDone)
                yield return null;

            //Handle response
            if (autoSelectPackageRequest.RequestResponseObject.IsSuccess)
            {
                AppearitionLogger.LogInfo("Package automatically selected!");

                if (onSuccess != null)
                    onSuccess(autoSelectPackageRequest.RequestResponseObject.Data);
            }
            else
            {
                AppearitionLogger.LogError("An error occured when trying to auto-select a package for the user.");

                if (onFailure != null)
                    onFailure(autoSelectPackageRequest.Errors);
            }

            if (onComplete != null)
                onComplete(autoSelectPackageRequest.RequestResponseObject.IsSuccess);
        }

        #endregion

        #region Select Package Manually

        public static void SelectPackageManually(BasicPackage packageToSelect, Action<PackageProcessingStatus> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(SelectPackageManuallyProcess(packageToSelect, onSuccess, onFailure, onComplete));
        }

        public static IEnumerator SelectPackageManuallyProcess(BasicPackage packageToSelect, Action<PackageProcessingStatus> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            var requestContent = new Package_SelectPackage.PostApi() {
                TenantPackageId = packageToSelect.Id
            };

            //Online request. Launch Request
            var manualSelectPackageRequest =
                AppearitionRequest<Package_SelectPackage>.LaunchAPICall_POST<PackageHandler>(AppearitionGate.Instance.CurrentUser.selectedChannel, GetReusableApiRequest<Package_SelectPackage>(), requestContent);

            while (!manualSelectPackageRequest.IsDone)
                yield return null;

            //Handle response
            if (manualSelectPackageRequest.RequestResponseObject.IsSuccess)
            {
                AppearitionLogger.LogInfo("Package manually selected!");

                if (onSuccess != null)
                    onSuccess(manualSelectPackageRequest.RequestResponseObject.Data);
            }
            else
            {
                AppearitionLogger.LogError("An error occured when trying to select a package for the user by id.");

                if (onFailure != null)
                    onFailure(manualSelectPackageRequest.Errors);
            }

            if (onComplete != null)
                onComplete(manualSelectPackageRequest.RequestResponseObject.IsSuccess);
        }

        #endregion
    }
}