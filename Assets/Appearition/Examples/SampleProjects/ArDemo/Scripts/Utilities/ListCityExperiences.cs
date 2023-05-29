using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Appearition.ArTargetImageAndMedia;
using System.Linq;
using UnityEngine.UI;

public class ListCityExperiences : MonoBehaviour
{
    public GameObject emptyExperience;
    public GameObject experiencesParent;
    public List<GameObject> experiences = new List<GameObject>();
    public Text messageText;
    public GameObject emptyMessageText;
    public GameObject splashScreen;

    public bool needToBePinned = false;

    private void OnEnable()
    {
    //    emptyMessageText.gameObject.SetActive(false);
        //messageText.gameObject.SetActive(true);
        StartCoroutine(FetchExperiences());
    }

    private void OnDisable()
    {
        //emptyMessageText.gameObject.SetActive(false);
        //messageText.gameObject.SetActive(true);
        StopAllCoroutines();
        for (int i = experiences.Count - 1; i >= 0; i--)
        {
            Destroy(experiences[i]);
        }
        experiences.Clear();
    }

    IEnumerator FetchExperiences()
    {
        bool isSuccess = true;
        List<Asset> assets = new List<Asset>();
        List<ArTarget> arTargets = new List<ArTarget>();

        var arTargetQuery = ArTargetConstant.GetDefaultArTargetListQuery();
        arTargetQuery.Tags = new List<string>();
        yield return ArTargetHandler.GetSpecificArTargetByQueryProcess(0, arTargetQuery, true, false,
            success => arTargets.AddRange(success), null, complete => isSuccess = complete);

        var arAssetQuery = ArTargetConstant.GetDefaultAssetListQuery();
        arAssetQuery.Tags = new List<string>();
        yield return ArTargetHandler.GetSpecificExperiencesByQueryProcess(0, arAssetQuery, true, false, false,
            success => assets.AddRange(success), null, complete => isSuccess = complete);



        Debug.LogError(arTargets.Count);
        //Add non-duplicates.
        for (int i = 0; i < arTargets.Count; i++)
        {

            if (assets.All(o => !o.assetId.Equals(arTargets[i].assetId)))
                assets.Add(arTargets[i]);
        }

        //for (int i = assets.Count - 1; i >= 0; i--)
        //{
        //    if (!assets[i].ContainsTag(MonashConstants.CityTag))
        //    {
        //        assets.RemoveAt(i);
        //    }
        //}
        //assets = assets.OrderByDescending(o => o.CreatedUtcDate).ToList();
        foreach (Asset exp in assets)
        {

            GameObject go = Instantiate(emptyExperience, experiencesParent.transform);
            go.SetActive(true);
            go.GetComponent<CityCard>().Setup(exp);
            experiences.Add(go);
        }

        //if (experiences.Count > 0)
        //{
        //    messageText.gameObject.SetActive(false);
        //}
        //else
        //{
        //    messageText.gameObject.SetActive(false);
        //    emptyMessageText.gameObject.SetActive(true);
        //}

    }
}
