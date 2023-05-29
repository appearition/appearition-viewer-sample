using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Appearition.ArDemo.Editor
{
    [CustomEditor(typeof(MediaHolder))]
    public class MediaHolderCustomEditor : UnityEditor.Editor
    {
        static bool fold = true;
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var script = (MediaHolder)target;

            if (script == null || script.Media == null)
                return;

            EditorGUILayout.Separator();
            
            fold = EditorGUILayout.Foldout(fold, "ArMedia Content");
            if (fold)
            {
                GUI.enabled = false;

                EditorGUILayout.TextField("Implementation: ", script.Media.GetType().ToString());
                EditorGUILayout.Separator();

                EditorGUILayout.TextField("ArMediaId: ", script.Media.arMediaId.ToString());
                EditorGUILayout.TextField("Filename: ", script.Media.fileName);
                EditorGUILayout.TextField("MediaType: ", script.Media.mediaType);
                EditorGUILayout.TextField("URL: ", script.Media.url);
                EditorGUILayout.TextField("Text: ", script.Media.text);
                EditorGUILayout.TextField("Animation Name: ", script.Media.animationName);
                EditorGUILayout.Separator();

                EditorGUILayout.TextField("CI Provider: ", script.Media.contentItemProviderName);
                EditorGUILayout.TextField("CI Key: ", script.Media.contentItemKey);
                EditorGUILayout.Separator();

                EditorGUILayout.Toggle("IsAutoplay: ", script.Media.isAutoPlay);
                EditorGUILayout.Toggle("IsDataQuery: ", script.Media.isDataQuery);
                EditorGUILayout.Toggle("IsInteractive: ", script.Media.isInteractive);
                EditorGUILayout.Toggle("IsPreDownload: ", script.Media.isPreDownload);
                EditorGUILayout.Toggle("IsTracking: ", script.Media.isTracking);

                EditorGUILayout.TextField("Pos: ", script.Media.GetPosition.ToString());
                EditorGUILayout.TextField("Rot: ", script.Media.GetRotation.eulerAngles.ToString());
                EditorGUILayout.TextField("Sca: ", script.Media.GetScale.ToString());
                EditorGUILayout.Separator();
                
                GUI.enabled = true;
            }
        }

    }
}