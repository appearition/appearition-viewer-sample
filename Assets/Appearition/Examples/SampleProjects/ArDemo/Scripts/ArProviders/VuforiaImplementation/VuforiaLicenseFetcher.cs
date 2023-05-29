using System;
using System.Collections;
using System.Collections.Generic;
using Appearition.ImageRecognition;
using UnityEngine;
using System.Linq;
using Vuforia;


namespace Appearition.ArDemo.Vuforia
{
    public static class VuforiaLicenseFetcher
    {
        const string CLIENT_ACCESS_KEY = "Client Access Key";
        const string CLIENT_SECRET_KEY = "Client Secret Key";
        const string LICENSE_KEY = "License Key";

        public static IEnumerator FetchVuforiaLicense()
        {
            List<DataStore> providerData = new List<DataStore>();
            yield return ImageRecognitionHandler.GetAllAvailableProvidersProcess(onSuccess => providerData.AddRange(onSuccess));

            DataStore validDataStore = null;

            for (int i = 0; i < providerData.Count; i++)
            {
                if (!string.IsNullOrEmpty(providerData[i].provider) && providerData[i].configSettings?.Count > 0)
                {
                    //Make sure that the provider contains all 3 pieces of information, otherwise no.
                    if (providerData[i].configSettings.Any(o => o.key.Equals(CLIENT_ACCESS_KEY, StringComparison.InvariantCultureIgnoreCase)) &&
                        providerData[i].configSettings.Any(o => o.key.Equals(CLIENT_SECRET_KEY, StringComparison.InvariantCultureIgnoreCase)) &&
                        providerData[i].configSettings.Any(o => o.key.Equals(LICENSE_KEY, StringComparison.InvariantCultureIgnoreCase)))
                    {
                        validDataStore = providerData[i];
                        break;
                    }
                }
            } 

            if (validDataStore != null)
            {
                //Populate the vuforia stuff!
                VuforiaCloudProviderHandler provider = GameObject.FindObjectOfType<VuforiaCloudProviderHandler>();
                CloudRecoBehaviour reco = GameObject.FindObjectOfType<CloudRecoBehaviour>();

                bool wasDeInit = false;

                if (provider != null)
                {
                    provider.ClearExperiencesBeingTracked();
                    wasDeInit = provider.DeInitTargetFinder();
                }
                
                if (reco != null)
                {
                    reco.AccessKey = validDataStore.configSettings.First(o => o.key.Equals(CLIENT_ACCESS_KEY, StringComparison.InvariantCultureIgnoreCase)).name;
                    reco.SecretKey = validDataStore.configSettings.First(o => o.key.Equals(CLIENT_SECRET_KEY, StringComparison.InvariantCultureIgnoreCase)).name;
                    VuforiaConfiguration.Instance.Vuforia.LicenseKey = validDataStore.configSettings.First(o => o.key.Equals(LICENSE_KEY, StringComparison.InvariantCultureIgnoreCase)).name;
                    Debug.Log($"Vuforia configuration successfully set!\nAccessKey:{reco.AccessKey}, SecretKey:{reco.SecretKey}.\nLicense:{VuforiaConfiguration.Instance.Vuforia.LicenseKey}");
                }
                else
                {
                    VuforiaConfiguration.Instance.Vuforia.LicenseKey = validDataStore.configSettings.First(o => o.key.Equals(LICENSE_KEY, StringComparison.InvariantCultureIgnoreCase)).name;
                    Debug.Log($"Vuforia configuration successfully set!\nAccessKey:{reco.AccessKey}, SecretKey:{reco.SecretKey}.\nLicense:{VuforiaConfiguration.Instance.Vuforia.LicenseKey}");
                }

                if (wasDeInit)
                    provider.InitTargetFinder();
            }
        }
    }
}