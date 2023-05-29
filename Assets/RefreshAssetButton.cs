using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Appearition.ArTargetImageAndMedia;
using Appearition.ArDemo;
public class RefreshAssetButton : MonoBehaviour
{
    public void Refr()
    {
        AppearitionArHandler.ResetTracking();
    }
}
