using System;
using System.Collections;
using System.Collections.Generic;
using Appearition.Common;
using Appearition.Internal;
using Appearition.Tenant.API;

namespace Appearition.Tenant
{
    public class TenantHandler : BaseHandler
    {
        public static void GetTenantSettings(Action<Dictionary<string, string>> onSuccess = null, 
            Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(GetTenantSettingsProcess(onSuccess, onFailure, onComplete));
        }

        public static IEnumerator GetTenantSettingsProcess(Action<Dictionary<string, string>> onSuccess = null, 
            Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            //Online request
            var getTenantSettingsRequest = AppearitionRequest<TenantSettings_List>.LaunchAPICall_GET<TenantHandler>(0, GetReusableApiRequest<TenantSettings_List>());

            while (!getTenantSettingsRequest.IsDone)
                yield return null;

            if (getTenantSettingsRequest.RequestResponseObject.IsSuccess)
            {
                if (getTenantSettingsRequest.CurrentState == AppearitionBaseRequest<TenantSettings_List>.RequestState.SuccessOnline)
                    AppearitionLogger.LogInfo(string.Format(TenantConstants.GET_TENANT_SETTINGS_SUCCESS_ONLINE, AppearitionGate.Instance.CurrentUser.selectedTenant));
                else
                    AppearitionLogger.LogInfo(string.Format(TenantConstants.GET_TENANT_SETTINGS_SUCCESS_OFFLINE, AppearitionGate.Instance.CurrentUser.selectedTenant));

                Dictionary<string, string> outcome = new Dictionary<string, string>();
                if (getTenantSettingsRequest.RequestResponseObject.Data != null)
                {
                    for (int i = 0; i < getTenantSettingsRequest.RequestResponseObject.Data.Count; i++)
                    {
                        if (!outcome.ContainsKey(getTenantSettingsRequest.RequestResponseObject.Data[i].key))
                            outcome.Add(getTenantSettingsRequest.RequestResponseObject.Data[i].key,
                                getTenantSettingsRequest.RequestResponseObject.Data[i].value);
                    }
                }

                if (onSuccess != null)
                    onSuccess(outcome);
            }
            else
            {
                AppearitionLogger.LogError(string.Format(TenantConstants.GET_TENANT_SETTINGS_SUCCESS_FAILURE, AppearitionGate.Instance.CurrentUser.selectedTenant));

                if (onFailure != null)
                    onFailure(new EmsError(getTenantSettingsRequest.RequestResponseObject.Errors));
            }

            if (onComplete != null)
                onComplete(getTenantSettingsRequest.RequestResponseObject.IsSuccess);
        }
    }
}