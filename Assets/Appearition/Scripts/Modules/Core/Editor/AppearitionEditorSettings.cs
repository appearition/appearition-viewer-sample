using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Appearition.EditorUtilities
{
    /// <summary>
    /// Container of all the editor settings used on the Appearition SDK.
    /// </summary>
    [System.Serializable]
    public class AppearitionEditorSettings
    {
        #region Asset Singleton

        const string EDITOR_SETTINGS_FILE_NAME = "AppearitionEditorSettings.settings";

        static string ProjectPath
        {
            get { return Application.dataPath.Substring(0, Application.dataPath.Length - "/Assets".Length); }
        }

        static AppearitionEditorSettings _instance;

        /// <summary>
        /// Instance to the current application settings.
        /// </summary>
        public static AppearitionEditorSettings Instance
        {
            get
            {
                if (_instance == null)
                {
                    //Try to load it.
                    string fullPath = ProjectPath + "/" + EDITOR_SETTINGS_FILE_NAME;
                    if (File.Exists(fullPath))
                    {
                        try
                        {
                            _instance = JsonUtility.FromJson<AppearitionEditorSettings>(File.ReadAllText(fullPath));
                        }
                        catch
                        {
                            _instance = null;
                        }
                    }

                    //No file found? Create a new instance and save it.
                    if (_instance == null)
                    {
                        //Create a new one
                        _instance = new AppearitionEditorSettings();

                        //Save it
                    }
                }

                return _instance;
            }
        }

        #endregion

        //Paths
        public string profileSavingDirectory = "Appearition";

        /// <summary>
        /// Full path where the settings are saved.
        /// </summary>
        static string FullPath
        {
            get { return ProjectPath + "/" + EDITOR_SETTINGS_FILE_NAME; }
        }

        //Flags
        [SerializeField]
        bool _hasDisplayedGettingStartedPopup;

        /// <summary>
        /// A Getting Started popup should be shown to users that just imported the package. This flag is its check.
        /// </summary>
        public bool HasDisplayedGettingStartedPopup
        {
            get { return _hasDisplayedGettingStartedPopup; }
            set
            {
                _hasDisplayedGettingStartedPopup = value;
                SaveEditorSettings();
            }
        }

        public static void SaveEditorSettings()
        {
            File.WriteAllText(FullPath, JsonUtility.ToJson(_instance));
            //Debug.Log("Appearition Settings created at: " + FullPath);
        }
    }
}