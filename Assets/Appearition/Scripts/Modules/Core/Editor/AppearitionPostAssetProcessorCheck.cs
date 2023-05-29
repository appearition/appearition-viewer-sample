using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

namespace Appearition.EditorUtilities
{
    public class AppearitionPostAssetProcessorCheck : AssetPostprocessor
    {
        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            if (AppearitionEditorSettings.Instance != null && !AppearitionEditorSettings.Instance.HasDisplayedGettingStartedPopup && importedAssets.Any(o => o.Contains("Appearition")))
                AppearitionControlWindow.ShowAppearitionControlWindow();
        }
    }
}