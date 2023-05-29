// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: "CustomEditor_ActiveUserProfile.cs" 
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using Appearition.Profile;
using UnityEngine;
using UnityEditor;

namespace Appearition.Internal
{
    [CustomEditor(typeof(UserProfile))]
    public class CustomEditor_ActiveUserProfile : Editor
    {
        GUIStyle _style;

        GUIStyle style
        {
            get
            {
                if (_style == null)
                {
                    _style = GUI.skin.label;
                    _style.wordWrap = true;
                }

                return _style;
            }
        }

        public override void OnInspectorGUI()
        {
            GUI.enabled = false;
            EditorGUILayout.LabelField("Please use the AppearitionManager to edit profiles.\nIf it is not present in your scene, please find \nit in the Prefab folder, and drop it in your scene.",
                style);
            GUI.enabled = true;
        }
    }
}