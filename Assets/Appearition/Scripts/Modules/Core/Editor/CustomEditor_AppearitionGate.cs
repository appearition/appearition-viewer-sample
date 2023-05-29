// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: "CustomEditor_AppearitionGate.cs" 
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

#pragma warning disable 0414
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using Appearition.EditorUtilities;
using UnityEditor;
using Appearition.Internal.EndPoints;
using Appearition.Profile;

namespace Appearition.Internal
{
    [CustomEditor(typeof(AppearitionGate))]
    public class CustomEditor_AppearitionGate : Editor
    {
        #region Styling

        GUIStyle _boldStyle;

        GUIStyle BoldStyle
        {
            get
            {
                if (_boldStyle == null)
                {
                    _boldStyle = new GUIStyle(GUI.skin.label);
                    _boldStyle.fontStyle = FontStyle.Bold;
                }

                return _boldStyle;
            }
        }

        #endregion

        //Internal Variable
        private int _selectedEndPointIndex;
        private List<string> _availableEndPoints;
        private string _currentTenantText = "";

        public override void OnInspectorGUI()
        {
            //Prepare standard editor stuff.
            AppearitionGate script = (AppearitionGate) target;
            EditorGUI.BeginChangeCheck();

            //Draw appearition logo
            GUILayout.Space(-30);
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            string logoToUse = EditorGUIUtility.isProSkin ? "UI/AppearitionLogo_Black" : "UI/AppearitionLogo_White";
            EditorGUILayout.LabelField(new GUIContent(Resources.Load<Sprite>(logoToUse).texture),
                GUILayout.Width(Mathf.Clamp(200, 0, EditorGUIUtility.currentViewWidth)),
                GUILayout.Height(Mathf.Clamp(200, 0, EditorGUIUtility.currentViewWidth)));
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(-50);

            DrawSeparator();

            #region APPEARITION EMS

            EditorGUILayout.LabelField("Appearition Profile", BoldStyle);


            EditorGUILayout.BeginHorizontal();
            EditorGUIUtility.labelWidth = 80;
            script.CurrentUser = (UserProfile) EditorGUILayout.ObjectField(
                new GUIContent("Current User", "The storage asset which contains all information required to communicate with the EMS."), script.CurrentUser, typeof(UserProfile), false);
            EditorGUIUtility.labelWidth = 0;

            if (GUILayout.Button("Create New", GUILayout.Width(80)))
            {
                script.CurrentUser = CreateCurrentUserProfile();
            }

            if (GUILayout.Button("Reset", GUILayout.Width(50)) && script.CurrentUser != null)
            {
                script.CurrentUser = new UserProfile();
            }

            EditorGUILayout.EndHorizontal();

            if (script.CurrentUser == null)
                EditorGUILayout.HelpBox("Please create or insert a profile to use the EMS.", MessageType.Warning);
            else
            {
                //Handle the end point selection
                if (_availableEndPoints == null || _availableEndPoints.Count == 0)
                {
                    EndPointUtility.RefreshEndPoints();
                    _availableEndPoints = EndPointUtility.GetTheNamesOfActiveAllEndPoints();
                }

                EditorGUILayout.BeginHorizontal();
                _selectedEndPointIndex = Mathf.Clamp(_availableEndPoints.FindIndex(o => o.Equals(script.CurrentUser.selectedEndPointName)), 0, _availableEndPoints.Count);
                for (int i = 0; i < _availableEndPoints.Count; i++)
                    _availableEndPoints[i] = ConvertStandardWithSlashToUnicode(_availableEndPoints[i]);
                _selectedEndPointIndex = EditorGUILayout.Popup("Selected Region", _selectedEndPointIndex, _availableEndPoints.ToArray());
                if (GUILayout.Button("Refresh", GUILayout.Width(80)))
                {
                    EndPointUtility.RefreshEndPoints();
                    _availableEndPoints = EndPointUtility.GetTheNamesOfActiveAllEndPoints();
                }

                if (_availableEndPoints.Count > 0)
                {
                    for (int i = 0; i < _availableEndPoints.Count; i++)
                        _availableEndPoints[i] = ConvertUnicodeToStandardWithSlash(_availableEndPoints[i]);
                    script.CurrentUser.selectedEndPointName = _availableEndPoints[_selectedEndPointIndex];
                }

                EditorGUILayout.EndHorizontal();

                ////FORCE US END POINT
                //script.CurrentUser.selectedEndPointName = "Appearition USA";


                //Tenant and channel
                //GUI.enabled = false;
                //EditorGUILayout.TextField("Active Tenant", script.CurrentUser.GetSelectedTenant);
                //GUI.enabled = true;
                //EditorGUILayout.BeginHorizontal();
                //_currentTenantText = EditorGUILayout.TextField("Tenant To Select", _currentTenantText);
                //if (GUILayout.Button("Select", GUILayout.Width(80)))
                //    script.CurrentUser.SwitchTenant(_currentTenantText, true);
                //EditorGUILayout.EndHorizontal();
                script.CurrentUser.selectedTenant = EditorGUILayout.TextField("Tenant", script.CurrentUser.selectedTenant);

                script.CurrentUser.selectedChannel = EditorGUILayout.IntField(new GUIContent("Channel Id", "Channel Id, or Product Id, as it appears on the EMS."), script.CurrentUser.selectedChannel);

                //Token
                script.CurrentUser.applicationToken = EditorGUILayout.TextField(
                    new GUIContent("Application Token", "Your Authentication Token. It can be found on the EMS Portal, under Settings>Developer."),
                    script.CurrentUser.applicationToken);

                //WEBGL BundlePackage Name
                if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.WebGL || 
                    EditorUserBuildSettings.activeBuildTarget == BuildTarget.StandaloneWindows ||
                    EditorUserBuildSettings.activeBuildTarget == BuildTarget.StandaloneWindows64 ||
                    EditorUserBuildSettings.activeBuildTarget == BuildTarget.StandaloneOSX)
                {
                    script.appBundleIdentifier = EditorGUILayout.TextField("Package Identifier", script.appBundleIdentifier);
                }
            }

            #endregion

            DrawSeparator();
            
            script.forceSingleInstance = EditorGUILayout.Toggle("Force single instance", script.forceSingleInstance);

            DrawSeparator();

            #region Debug

            EditorGUILayout.LabelField("Common Debug Utilities", BoldStyle);
            AppearitionGate.LogLevel = (AppearitionLogger.LogLevel) EditorGUILayout.EnumPopup(
                new GUIContent("Log level", "Allows the Gate and Handlers to log any information in the console."), AppearitionGate.LogLevel);

            EditorGUILayout.LabelField("Editor Debug Utilities", BoldStyle);
            script.debugSimulateNoInternetConnection = EditorGUILayout.Toggle("Simulate No Internet", script.debugSimulateNoInternetConnection);

            #endregion

            bool anyChangesMade = EditorGUI.EndChangeCheck();

            if (anyChangesMade)
            {
                ApplyCurrentUserSettings(script);
            }
        }

        public static void ApplyCurrentUserSettings(AppearitionGate script)
        {
            //Serialize
            SerializedObject tmpScript = new SerializedObject(script);
            tmpScript.FindProperty("_currentUser").objectReferenceValue = script.CurrentUser;
            tmpScript.ApplyModifiedProperties();

            EditorUtility.SetDirty(script);

            if (script.CurrentUser != null)
                EditorUtility.SetDirty(script.CurrentUser);
            if(!EditorApplication.isPlaying)
                UnityEditor.SceneManagement.EditorSceneManager.MarkAllScenesDirty();
        }

        #region Utilties

        static void DrawSeparator()
        {
            GUILayout.Space(-10);
            EditorGUILayout.LabelField(HandyGuiStyles.screenSeparator);
        }

        /// <summary>
        /// Creates a default empty User Profile in the .../Appearition/Scripts/Profile folder.
        /// </summary>
        /// <returns>The default user profile.</returns>
        static UserProfile CreateCurrentUserProfile()
        {
            UserProfile tmpProfile = CreateInstance<UserProfile>();
            //string[] allPaths = new string[1];

            //			//Store it
            //			string[] allPaths = Directory.GetDirectories (Application.dataPath, "Appearition", SearchOption.AllDirectories);
            //
            //			if (allPaths.Length == 0 || allPaths.Length > 1) {
            //				Debug.LogError ("There is more than one Appearition folder. Cannot create working profile.");
            //				return tmpProfile;
            //			}
            //
            //			//If no Appearition/Scripts/Profile, SOMEHOW, create it.
            //			allPaths [0] += "/Scripts/Profile";
            //			if (!Directory.Exists (allPaths [0]))
            //				Directory.CreateDirectory (allPaths [0]);

            string folderPath = string.Format("{0}/Appearition/Scripts/Profile/", Application.dataPath);

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            string filePath = "Assets/Appearition/Scripts/Profile/UserProfile.asset";
            filePath = AssetDatabase.GenerateUniqueAssetPath(filePath);

            //Export its ApiData
            AssetDatabase.CreateAsset(tmpProfile, filePath);
            AssetDatabase.Refresh();

            return tmpProfile;
        }

        /// <summary>
        /// Inputs a normal string, and replaces the slash to a unicode character.
        /// </summary>
        /// <returns>The standard with slash to unicode.</returns>
        /// <param name="text">Text.</param>
        string ConvertStandardWithSlashToUnicode(string text)
        {
            if (string.IsNullOrEmpty(text))
                return "";
            return text.Replace('/', '\u2215');
        }

        /// <summary>
        /// Inputs a string with unicode characters, and replaces them with a normal string.
        /// </summary>
        /// <returns>The unicode to standard with slash.</returns>
        /// <param name="text">Text.</param>
        string ConvertUnicodeToStandardWithSlash(string text)
        {
            if (string.IsNullOrEmpty(text))
                return "";
            return text.Replace('\u2215', '/');
        }

        #endregion
    }
}