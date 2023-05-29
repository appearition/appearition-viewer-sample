using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Appearition.ArDemo;
using Appearition.ArDemo.Vuforia;
using Appearition.ArTargetImageAndMedia;
using System.Linq;
using Vuforia;

public class ToggleMarkerlessView : MonoBehaviour
{
    //public GameObject loadingScreen;
    //public GameObject findScreen;
    //public GameObject placeScreen;
    //public GameObject autoRotateButton;
    Asset _assetToAugment = null;

    void Awake()
    {
        AppearitionArHandler.OnProviderStateChanged += AppearitionArHandler_OnProviderStateChanged;
        AppearitionArHandler.OnTargetStateChanged += AppearitionArHandler_OnTargetStateChanged;

        var markerlessHandler = FindObjectOfType<VuforiaMarkerlessProviderHandler>();

        if (markerlessHandler != null)
        {
            var anchorListener = markerlessHandler.GetComponentInChildren<AnchorInputListenerBehaviour>(true);
            if (anchorListener != null)
                anchorListener.OnInputReceivedEvent.AddListener(OnAnchorListenerInputReceived);
            var contentPlacer = markerlessHandler.GetComponentInChildren<ContentPositioningBehaviour>();
            if (contentPlacer != null)
                contentPlacer.OnContentPlaced.AddListener(OnContentPlaced);
        }
    }

    private void AppearitionArHandler_OnTargetStateChanged(ArExperience arasset, AppearitionArHandler.TargetState newstate)
    {
        if (AppearitionArHandler.ProviderHandler is VuforiaMarkerlessProviderHandler)
        {
            //if (arasset != null && arasset.IsCurrentlyTracking)
            //{
            //    findScreen.SetActive(false);
            //    placeScreen.SetActive(false);
            //    autoRotateButton.SetActive(true);
            //}
            //else
            //{
            //    findScreen.SetActive(arasset == null && !placeScreen.activeInHierarchy);
            //    placeScreen.SetActive(!findScreen.activeInHierarchy);
            //}
        }
    }

    private void OnAnchorListenerInputReceived(Vector2 arg0)
    {
        //findScreen.SetActive(false);
        //placeScreen.SetActive(true);
        //autoRotateButton.SetActive(false);
    }

    private void OnContentPlaced(GameObject arg0)
    {
        //findScreen.SetActive(false);
        //placeScreen.SetActive(false);
        //autoRotateButton.SetActive(true);
    }

    void OnEnable()
    {
        //findScreen.SetActive(false);
        //placeScreen.SetActive(false);
        //autoRotateButton.SetActive(false);
        StartProvider();
    }

    void OnDisable()
    {
        AppearitionArHandler.UnloadArProvider();
    }

    public void StartProvider()
    {
        //HACK; create a blank asset and have the creation process load its content upon getting placed (offline AR style)
        //loadingScreen.SetActive(true);
        _assetToAugment = new Asset() {assetId = SessionSettings.Instance.currentMarkerlessExpId};
        AppearitionArHandler.ChangeArProviderAndSelectAsset<VuforiaMarkerlessProviderHandler>(_assetToAugment);
    }

    private void AppearitionArHandler_OnProviderStateChanged(BaseArProviderHandler provider, bool isActive)
    {
        if (provider is VuforiaMarkerlessProviderHandler && isActive)
        {
            //loadingScreen.SetActive(false);
            //if (!placeScreen.activeInHierarchy)
            //    findScreen.SetActive(true);
        }
    }

    //IEnumerator StartProviderProcess()
    //{
    //    _assetToAugment = null;
    //    loadingScreen.SetActive(true);

    //    ////Get some asset
    //    //if (_assetToAugment == null)
    //    //{
    //    //    var query = ArTargetConstant.GetDefaultArTargetListQuery();
    //    //    query.AssetId = SessionSettings.Instance.currentMarkerlessExpId;

    //    //    Debug.LogError($"Fetching {query.AssetId}");
    //    //    //yield return ArTargetHandler.GetSpecificExperiencesByQueryProcess(query, false, true, true, success => { /*_assetToAugment = success.FirstOrDefault();*/ Debug.Log(success.Count); }, onFailure => Debug.LogError(onFailure.errorCode));
    //    //    yield return ArTargetHandler.GetSpecificArTargetByQueryProcess(query, false, true, success => { _assetToAugment = success.FirstOrDefault(); Debug.Log(success.Count); }, onFailure => Debug.LogError(onFailure.errorCode));

    //    //    Debug.LogError($"{_assetToAugment.assetId}    {_assetToAugment.mediaFiles.Length}");
    //    //}

    //    //HACK; create a blank asset and have the creation process load its content upon getting placed (offline AR style)
    //    _assetToAugment = new Asset() { assetId = SessionSettings.Instance.currentMarkerlessExpId };

    //    AppearitionArHandler.ChangeArProviderAndSelectAsset<VuforiaMarkerlessProviderHandler>(_assetToAugment);
    //    loadingScreen.SetActive(false);
    //    yield return null;
    //}

    public void DisableProvider()
    {
        AppearitionArHandler.ClearCurrentTargetBeingTracked(_assetToAugment, true);
        AppearitionArHandler.UnloadArProvider();
    }
}