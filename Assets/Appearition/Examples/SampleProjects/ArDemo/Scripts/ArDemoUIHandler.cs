using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Appearition.ArTargetImageAndMedia;
using UnityEngine;
using UnityEngine.UI;

namespace Appearition.ArDemo
{
    public class ArDemoUIHandler : MonoBehaviour
    {
        //References
        [SerializeField] Dropdown _providerSelection;
        [SerializeField] Button _resetExperienceButton;
        [SerializeField] GameObject _scanningRecticle;

        //Internal Variables
        [SerializeField] string _testAssetId;
        Asset _testAsset;
        bool _isInitialized;

        #region Setup

        void Awake()
        {
            //Register Events
            AppearitionArHandler.OnScanStateChanged += AppearitionArHandler_OnScanStateChanged;
            AppearitionArHandler.OnTargetStateChanged += AppearitionArHandler_OnTargetStateChanged;

            //Setup dropdown 
            var providerOptions = new List<Dropdown.OptionData>();
            providerOptions.Add(new Dropdown.OptionData("None"));

            for (int i = 0; i < AppearitionArHandler.RegisteredArProviders.Count; i++)
                providerOptions.Add(new Dropdown.OptionData(GetProviderNameToString(AppearitionArHandler.RegisteredArProviders[i])));

            _providerSelection.options = providerOptions;
            _providerSelection.onValueChanged.AddListener(OnProviderDropdownValueChanged);
            _providerSelection.interactable = false;

            //Setup the rest of the UI
            _resetExperienceButton.interactable = false;
            _resetExperienceButton.onClick.AddListener(OnResetButtonClicked);
            _scanningRecticle.gameObject.SetActive(false);
        }


        IEnumerator Start()
        {
            //Get a sample Experience to be able to test markerless provider.
            var query = ArTargetConstant.GetDefaultAssetListQuery();
            query.AssetId = _testAssetId;
            yield return ArTargetHandler.GetSpecificExperiencesByQueryProcess(query, false, false, false, success => _testAsset = success.FirstOrDefault());

            while (!AppearitionArHandler.IsInitialized)
                yield return null;

            _providerSelection.interactable = true;
            _isInitialized = true;
        }


        string GetProviderNameToString(BaseArProviderHandler provider)
        {
            string outcome = "";
            string fullName = provider.GetType().Name;

            for (int i = 0; i < fullName.Length; i++)
            {
                outcome += fullName[i];
                if (i + 1 < fullName.Length && char.IsUpper(fullName[i + 1]))
                    outcome += " ";
            }

            return outcome;
        }

        #endregion

        #region Events

        private void OnProviderDropdownValueChanged(int value)
        {
            if (!_isInitialized)
                return;

            if (value == 0)
                AppearitionArHandler.UnloadArProvider();
            else
            {
                //Value starts at 1, since the first entry is None.
                var provider = AppearitionArHandler.RegisteredArProviders[Mathf.Clamp(value - 1, 0, AppearitionArHandler.RegisteredArProviders.Count)];

                if (provider != AppearitionArHandler.ProviderHandler)
                {
                    if (provider is IOfflineMarkerlessArProviderHandler)
                        AppearitionArHandler.ChangeArProviderAndSelectAsset(provider.GetType(), _testAsset);
                    else
                        AppearitionArHandler.ChangeArProvider(provider.GetType());
                }
            }
        }
        
        
        private void OnResetButtonClicked()
        {
            if (AppearitionArHandler.ProviderHandler != null)
            {
                AppearitionArHandler.ResetTracking();
            }
        }

        private void AppearitionArHandler_OnScanStateChanged(bool isScanning)
        {
            _scanningRecticle.gameObject.SetActive(isScanning);
        }

        private void AppearitionArHandler_OnTargetStateChanged(ArExperience arAsset, AppearitionArHandler.TargetState newState)
        {
            _resetExperienceButton.interactable = arAsset != null;
        }

        #endregion
    }
}