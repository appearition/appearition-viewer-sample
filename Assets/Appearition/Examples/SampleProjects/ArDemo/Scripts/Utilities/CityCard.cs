using Appearition.ArTargetImageAndMedia;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CityCard : MonoBehaviour
{
    public Text title;
    public GameObject panelToDisable;
    public GameObject panelToEnableMarker;
    public GameObject panelToEnable;
    public Image imageThumbnail;

    public virtual void Setup(Asset asset)
    {
        title.text = asset.name;
        if(asset.ContainsTag("marker"))
        {
            Debug.Log("Marker");
            GetComponent<Button>().onClick.AddListener(delegate { SetExperienceMarker(asset.assetId); });
        }
        else
        {
            Debug.Log("Markerless");
            GetComponent<Button>().onClick.AddListener(delegate { SetExperience(asset.assetId); });
        }

        if (asset.targetImages.Count > 0)
            imageThumbnail.sprite = asset.targetImages[0].image;
    }

    public void SetExperience(string id)
    {
        SessionSettings.Instance.currentMarkerlessExpId = id;
        MakeTransitions();
    }
    public void SetExperienceMarker(string id)
    {
        SessionSettings.Instance.currentMarkerlessExpId = id;
        MakeTransitionsMarker();
    }

    public virtual void MakeTransitions()
    {
        OpenViewModeInMarkerless();
    }
    public virtual void MakeTransitionsMarker()
    {
        OpenViewModeInMarker();
    }

    public void OpenViewModeInMarkerless()
    {
        panelToDisable.SetActive(false);
        panelToEnable.SetActive(true);
    }
    
    public void OpenViewModeInMarker()
    {
        Debug.LogError("OpeningMarker");
        panelToDisable.SetActive(false);
        panelToEnableMarker.SetActive(true);
    }
}
