using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Appearition.EditorUtilities
{
    public class AppearitionControlWindow : EditorWindow
    {
        #region Item Box

        /// <summary>
        /// Container for a single button that can be used by the Tutorial and Sample Project views.
        /// </summary>
        struct ItemBox
        {
            public enum TypeOfItem
            {
                Scene,
                Package
            }

            public string name;
            public string description;
            public int descriptionLineCount;
            public string path;
            public string urlButtonIfAny;
            public bool isGreyedOut;
        }

        #endregion

        //Consts 
//        const string EDITOR_PREF_POPUP_CHECK = "Appearition_GettingStarted";
        const float WINDOW_WIDTH = 500f;
        const float WINDOW_HEIGHT = 640f;
        const float SEPARATOR_HEIGHT = 10f;
        static Color BG_BOX_COLOR_NOT_PRO = Color.white * 0.8f;
        static Color BG_BOX_COLOR_PRO = Color.white * 0.3f;

        static Color BG_BOX_COLOR
        {
            get
            {
                return EditorGUIUtility.isProSkin ? BG_BOX_COLOR_PRO : BG_BOX_COLOR_NOT_PRO;
            }
        }

        const float BG_BOX_MARGIN = 5f;
        const float BG_BOX_PADDING = 5f;

        const float HEADER_LOGO_SIZE = 150f;

        const float TUTORIAL_ITEM_HEIGHT = 100f;

        const float SAMPLE_PROJECT_ITEM_HEIGHT = 100f;
        const float SAMPLE_PROJECT_URL_ITEM_HEIGHT = 20f;
        const float SAMPLE_PROJECT_URL_ITEM_WIDTH = 70f;
        const string SAMPLE_PROJECT_URL_BUTTON_TEXT = "Read more";

        const float FOOTER_LINK_ITEM_HEIGHT = 30f;

        #region GUIStyles

        static GUIStyle _centeredTitleGuiStyle;

        static GUIStyle CenteredTitleGuiStyle
        {
            get
            {
                if (_centeredTitleGuiStyle == null)
                {
                    _centeredTitleGuiStyle = new GUIStyle(GUI.skin.label);
                    _centeredTitleGuiStyle.alignment = TextAnchor.MiddleCenter;
                    _centeredTitleGuiStyle.fontStyle = FontStyle.Bold;
                }

                return _centeredTitleGuiStyle;
            }
        }

        static GUIStyle _centeredLabelGuiStyle;

        static GUIStyle CenteredLabelGuiStyle
        {
            get
            {
                if (_centeredLabelGuiStyle == null)
                {
                    _centeredLabelGuiStyle = new GUIStyle(GUI.skin.label);
                    _centeredLabelGuiStyle.alignment = TextAnchor.MiddleCenter;
                }

                return _centeredLabelGuiStyle;
            }
        }

        static GUIStyle _centeredDescriptionGuiStyle;

        static GUIStyle CenteredDescriptionGuiStyle
        {
            get
            {
                if (_centeredDescriptionGuiStyle == null)
                {
                    _centeredDescriptionGuiStyle = new GUIStyle(GUI.skin.label);
                    _centeredDescriptionGuiStyle.alignment = TextAnchor.MiddleCenter;
                    _centeredDescriptionGuiStyle.fontStyle = FontStyle.Italic;
                }

                return _centeredDescriptionGuiStyle;
            }
        }

        #endregion

        //Main variables
        static AppearitionControlWindow _currentWindow;

        //Internal Variables
        List<ItemBox> _tutorialItems = new List<ItemBox>();
        List<ItemBox> _sampleProjectItems = new List<ItemBox>();
        List<ItemBox> _footerLinkItems = new List<ItemBox>();

        [MenuItem("Tools/Appearition Window")]
        public static void ShowAppearitionControlWindow()
        {
            if (EditorApplication.isCompiling || EditorApplication.isUpdating)
            {
                //Mark the editor window as "waiting to be displayed"
                EditorApplication.update += UpdateWaitForCompileUpdatingCompleteToShowWindow;
                return;
            }

            if (AppearitionEditorSettings.Instance != null)
                AppearitionEditorSettings.Instance.HasDisplayedGettingStartedPopup = true;


            _currentWindow = GetWindow<AppearitionControlWindow>(true,
                "Getting Started with the Appearition SDK for Unity", true);
            _currentWindow.minSize = _currentWindow.maxSize = new Vector2(WINDOW_WIDTH + 20, WINDOW_HEIGHT);
            _currentWindow.ShowAuxWindow();
        }

        static void UpdateWaitForCompileUpdatingCompleteToShowWindow()
        {
            if (!EditorApplication.isCompiling && !EditorApplication.isUpdating)
            {
                ShowAppearitionControlWindow();
                EditorApplication.update -= UpdateWaitForCompileUpdatingCompleteToShowWindow;
            }
        }

        [UnityEditor.Callbacks.DidReloadScripts]
        private static void OnEditorRecompiled()
        {
            // do something
            //if (!EditorPrefs.HasKey(EDITOR_PREF_POPUP_CHECK))
            //{
            //    ShowAppearitionControlWindow();
            //    EditorPrefs.SetInt(EDITOR_PREF_POPUP_CHECK, 1);
            //}
            if (AppearitionEditorSettings.Instance != null && !AppearitionEditorSettings.Instance.HasDisplayedGettingStartedPopup)
            {
                ShowAppearitionControlWindow();
            }
            else if (_currentWindow != null)
                _currentWindow.OnGUI();
        }

        void OnGUI()
        {
            //No window, no goodies!
            if (_currentWindow == null)
                return;

            //Ongoing variables
            float windowWidth = EditorGUILayout.GetControlRect().width;
            float currentXPosition = 0;
            float currentYPosition = 0;
            Rect currentBoxRect;

            //Refresh Containers
            if (_tutorialItems.Count == 0)
                RefreshTutorialContent();

            if (_sampleProjectItems.Count == 0)
                RefreshSampleProjectContent();

            //Draw logo centered inside the box.

            #region HEADER

            currentBoxRect = new Rect(currentXPosition + BG_BOX_MARGIN, currentYPosition + BG_BOX_MARGIN,
                windowWidth - BG_BOX_MARGIN * 2, HEADER_LOGO_SIZE + EditorGUIUtility.singleLineHeight * 3 + BG_BOX_MARGIN * 2);

            EditorGUI.DrawRect(currentBoxRect, BG_BOX_COLOR);

            Sprite appearitionLogo = Resources.Load<Sprite>(EditorGUIUtility.isProSkin ? "UI/AppearitionLogo_Black" : "UI/AppearitionLogo_White");

            if (appearitionLogo == null)
                return;

            EditorGUI.LabelField(new Rect(windowWidth / 2 - HEADER_LOGO_SIZE / 2,
                    currentYPosition,
                    HEADER_LOGO_SIZE, HEADER_LOGO_SIZE),
                new GUIContent(appearitionLogo.texture));

            currentYPosition += HEADER_LOGO_SIZE;

            EditorGUI.LabelField(new Rect(currentBoxRect.position.x + BG_BOX_PADDING,
                    currentYPosition,
                    currentBoxRect.width, EditorGUIUtility.singleLineHeight),
                "Welcome to the Appearition SDK for Unity", CenteredTitleGuiStyle);

            currentYPosition += EditorGUIUtility.singleLineHeight;

            EditorGUI.LabelField(new Rect(currentBoxRect.position.x + BG_BOX_PADDING,
                    currentYPosition,
                    currentBoxRect.width, EditorGUIUtility.singleLineHeight * 2),
                "To get started using our SDK to create your own AR/VR/MR app using our EMS,\ncheck our tutorials and sample projects.",
                CenteredLabelGuiStyle);

            currentYPosition = currentBoxRect.y + currentBoxRect.height;

            #endregion

            currentYPosition += SEPARATOR_HEIGHT;

            if (_tutorialItems.Count > 0)
            {
                #region Tutorial Holder

                //Tutorial box. First, 1 line of text, then a scrollview with the different scenes.
                currentBoxRect = new Rect(currentXPosition + BG_BOX_MARGIN, currentYPosition + BG_BOX_MARGIN,
                    windowWidth - BG_BOX_MARGIN, EditorGUIUtility.singleLineHeight * 3 + BG_BOX_MARGIN * 2 + TUTORIAL_ITEM_HEIGHT);

                currentYPosition += EditorGUIUtility.singleLineHeight;

                EditorGUI.DrawRect(currentBoxRect, BG_BOX_COLOR);

                EditorGUI.LabelField(new Rect(currentBoxRect.position.x + BG_BOX_PADDING,
                        currentYPosition,
                        currentBoxRect.width, EditorGUIUtility.singleLineHeight),
                    "Tutorials", CenteredTitleGuiStyle);

                currentYPosition += EditorGUIUtility.singleLineHeight;

                EditorGUI.LabelField(new Rect(currentBoxRect.position.x + BG_BOX_PADDING,
                        currentYPosition,
                        currentBoxRect.width, EditorGUIUtility.singleLineHeight),
                    "How the SDK and EMS work", CenteredLabelGuiStyle);

                currentYPosition += EditorGUIUtility.singleLineHeight + BG_BOX_PADDING;

                Rect tutorialFullButtonView = new Rect(currentBoxRect.position.x + BG_BOX_PADDING, currentYPosition, currentBoxRect.width - BG_BOX_PADDING * 2, TUTORIAL_ITEM_HEIGHT);


                for (int i = 0; i < _tutorialItems.Count; i++)
                {
                    Rect tmpButtonRect = new Rect(
                        tutorialFullButtonView.position.x + tutorialFullButtonView.width - tutorialFullButtonView.width * (1 - (float) i / _tutorialItems.Count) + BG_BOX_PADDING * i / 2,
                        tutorialFullButtonView.position.y, tutorialFullButtonView.width / _tutorialItems.Count - BG_BOX_PADDING * (_tutorialItems.Count - 1) / 2, TUTORIAL_ITEM_HEIGHT);

                    //First display the button, then display the text on the top.
                    if (GUI.Button(tmpButtonRect, ""))
                    {
                        EditorSceneManager.OpenScene(_tutorialItems[i].path, OpenSceneMode.Single);
                    }

                    Rect tmpLabelRect = new Rect(tmpButtonRect);
                    tmpLabelRect.y = tmpButtonRect.y + tmpButtonRect.height / 5;
                    tmpLabelRect.height = EditorGUIUtility.singleLineHeight;
                    EditorGUI.LabelField(tmpLabelRect, _tutorialItems[i].name, CenteredTitleGuiStyle);
                    tmpLabelRect.y += EditorGUIUtility.singleLineHeight * (4f - _tutorialItems[i].descriptionLineCount);
                    tmpLabelRect.height = EditorGUIUtility.singleLineHeight * _tutorialItems[i].descriptionLineCount;
                    EditorGUI.LabelField(tmpLabelRect, _tutorialItems[i].description, CenteredDescriptionGuiStyle);
                }

                currentYPosition = currentBoxRect.y + currentBoxRect.height;

                #endregion

                currentYPosition += SEPARATOR_HEIGHT;
            }

            if (_sampleProjectItems.Count > 0)
            {
                #region Sample Projects

                //Sample Project box
                currentBoxRect = new Rect(currentXPosition + BG_BOX_MARGIN, currentYPosition + BG_BOX_MARGIN,
                    windowWidth - BG_BOX_MARGIN, EditorGUIUtility.singleLineHeight * 3 + BG_BOX_MARGIN * 2 + SAMPLE_PROJECT_ITEM_HEIGHT + SAMPLE_PROJECT_URL_ITEM_HEIGHT);

                currentYPosition += EditorGUIUtility.singleLineHeight;

                EditorGUI.DrawRect(currentBoxRect, BG_BOX_COLOR);

                EditorGUI.LabelField(new Rect(currentBoxRect.position.x + BG_BOX_PADDING,
                        currentYPosition,
                        currentBoxRect.width, EditorGUIUtility.singleLineHeight),
                    "Sample Projects", CenteredTitleGuiStyle);

                currentYPosition += EditorGUIUtility.singleLineHeight;

                EditorGUI.LabelField(new Rect(currentBoxRect.position.x + BG_BOX_PADDING,
                        currentYPosition,
                        currentBoxRect.width, EditorGUIUtility.singleLineHeight),
                    "Get a head start making apps using our EMS and pre-built projects", CenteredLabelGuiStyle);

                currentYPosition += EditorGUIUtility.singleLineHeight + BG_BOX_PADDING;

                Rect sampleProjectFullButtonView = new Rect(currentBoxRect.position.x + BG_BOX_PADDING, currentYPosition, currentBoxRect.width - BG_BOX_PADDING * 2,
                    SAMPLE_PROJECT_ITEM_HEIGHT + SAMPLE_PROJECT_URL_ITEM_HEIGHT + BG_BOX_PADDING);

                for (int i = 0; i < _sampleProjectItems.Count; i++)
                {
                    Rect tmpButtonRect = new Rect(
                        sampleProjectFullButtonView.position.x + sampleProjectFullButtonView.width - sampleProjectFullButtonView.width * (1 - (float) i / _sampleProjectItems.Count) +
                        BG_BOX_PADDING * i / 2,
                        sampleProjectFullButtonView.position.y, sampleProjectFullButtonView.width / _sampleProjectItems.Count - BG_BOX_PADDING * (_sampleProjectItems.Count - 1) / 2,
                        SAMPLE_PROJECT_ITEM_HEIGHT);

                    //First display the button, then display the text on the top.
                    if (GUI.Button(tmpButtonRect, ""))
                        AssetDatabase.ImportPackage(_sampleProjectItems[i].path, true);

                    Rect tmpLabelRect = new Rect(tmpButtonRect);
                    tmpLabelRect.y = tmpButtonRect.y + tmpButtonRect.height / 5;
                    tmpLabelRect.height = EditorGUIUtility.singleLineHeight;
                    EditorGUI.LabelField(tmpLabelRect, _sampleProjectItems[i].name, CenteredTitleGuiStyle);
                    tmpLabelRect.y += EditorGUIUtility.singleLineHeight * (4f - _sampleProjectItems[i].descriptionLineCount);
                    tmpLabelRect.height = EditorGUIUtility.singleLineHeight * _sampleProjectItems[i].descriptionLineCount;
                    EditorGUI.LabelField(tmpLabelRect, _sampleProjectItems[i].description, CenteredDescriptionGuiStyle);

                    if (!string.IsNullOrEmpty(_sampleProjectItems[i].urlButtonIfAny))
                    {
                        //Then, display the URL button
                        tmpButtonRect.y += SAMPLE_PROJECT_ITEM_HEIGHT + BG_BOX_PADDING;
                        tmpButtonRect.x += tmpButtonRect.width / 2 - SAMPLE_PROJECT_URL_ITEM_WIDTH / 2;
                        tmpButtonRect.width = SAMPLE_PROJECT_URL_ITEM_WIDTH;
                        tmpButtonRect.height = SAMPLE_PROJECT_URL_ITEM_HEIGHT;

                        if (GUI.Button(tmpButtonRect, ""))
                            Application.OpenURL(_sampleProjectItems[i].urlButtonIfAny);

                        //And the text on the top
                        EditorGUI.LabelField(tmpButtonRect, SAMPLE_PROJECT_URL_BUTTON_TEXT, CenteredLabelGuiStyle);
                    }
                }

                currentYPosition = currentBoxRect.y + currentBoxRect.height;

                #endregion

                currentYPosition += SEPARATOR_HEIGHT;
            }

            #region Footer

            //Footer with links. In this order: dev website, portal, zendesk, forum.
            currentBoxRect = new Rect(currentXPosition + BG_BOX_MARGIN, currentYPosition + BG_BOX_MARGIN,
                windowWidth - BG_BOX_MARGIN, EditorGUIUtility.singleLineHeight * 3 + BG_BOX_MARGIN * 2 + FOOTER_LINK_ITEM_HEIGHT);

            if (_footerLinkItems.Count == 0)
                RefreshFooterLinksContent();

            for (int i = 0; i < _footerLinkItems.Count; i++)
            {
                Rect tmpButtonRect = new Rect(
                    currentBoxRect.position.x + currentBoxRect.width - currentBoxRect.width * (1 - (float) i / _footerLinkItems.Count) + BG_BOX_PADDING * i / 2,
                    currentBoxRect.position.y, currentBoxRect.width / _footerLinkItems.Count - BG_BOX_PADDING * (_footerLinkItems.Count - 1) / 2, FOOTER_LINK_ITEM_HEIGHT);

                //First display the button, then display the text on the top.
                if (_footerLinkItems[i].isGreyedOut)
                    GUI.enabled = false;

                if (GUI.Button(tmpButtonRect, ""))
                {
                    Application.OpenURL(_footerLinkItems[i].urlButtonIfAny);
                }

                if (_footerLinkItems[i].isGreyedOut)
                    GUI.enabled = true;

                EditorGUI.LabelField(tmpButtonRect, _footerLinkItems[i].name, CenteredTitleGuiStyle);
            }

            #endregion
        }

        #region Refresh Content

        void RefreshTutorialContent()
        {
            _tutorialItems.Clear();

            string[] tmpGuids = AssetDatabase.FindAssets("Introduction t:scene");
            if (tmpGuids != null && tmpGuids.Length > 0)
                _tutorialItems.Add(new ItemBox
                {
                    name = "Introduction",
                    description = "Basics of the\nEMS and SDK.",
                    descriptionLineCount = 2,
                    path = AssetDatabase.GUIDToAssetPath(tmpGuids[0])
                });

            tmpGuids = AssetDatabase.FindAssets("GetExperiences t:scene");
            if (tmpGuids != null && tmpGuids.Length > 0)
                _tutorialItems.Add(new ItemBox
                {
                    name = "Get Experiences",
                    description = "Fetching EMS data\nand displaying it.",
                    descriptionLineCount = 2,
                    path = AssetDatabase.GUIDToAssetPath(tmpGuids[0])
                });

            tmpGuids = AssetDatabase.FindAssets("Login t:scene");
            if (tmpGuids != null && tmpGuids.Length > 0)
                _tutorialItems.Add(new ItemBox
                {
                    name = "Login",
                    description = "Login, Logout\nand why.",
                    descriptionLineCount = 2,
                    path = AssetDatabase.GUIDToAssetPath(tmpGuids[0])
                });

            tmpGuids = AssetDatabase.FindAssets("ManageExperiences t:scene");
            if (tmpGuids != null && tmpGuids.Length > 0)
                _tutorialItems.Add(new ItemBox
                {
                    name = "Manage Content",
                    description = "Fetch, modify\nand update\nEMS content.",
                    descriptionLineCount = 3,
                    path = AssetDatabase.GUIDToAssetPath(tmpGuids[0])
                });
        }

        void RefreshSampleProjectContent()
        {
            _sampleProjectItems.Clear();

            string[] tmpGuids = AssetDatabase.FindAssets("ArDemo_v");
            if (tmpGuids != null && tmpGuids.Length > 0)
            {
                if (!string.IsNullOrEmpty(tmpGuids[0]))
                    _sampleProjectItems.Add(new ItemBox
                    {
                        name = "Ar Demo",
                        description = "Simple AR app\naugmenting experiences and\nlive data from the EMS.",
                        descriptionLineCount = 3,
                        path = AssetDatabase.GUIDToAssetPath(tmpGuids[0]),
                        urlButtonIfAny = "https://staging.developer.appearition.com/ar-demo/"
                    });
            }

            tmpGuids = AssetDatabase.FindAssets("LocationDemo_v");
            if (tmpGuids != null && tmpGuids.Length > 0)
            {
                if (!string.IsNullOrEmpty(tmpGuids[0]))
                    _sampleProjectItems.Add(new ItemBox
                    {
                        name = "Location Demo",
                        description = "Location-based demo\nusing Google Maps and\nlive data from the EMS.",
                        descriptionLineCount = 3,
                        path = AssetDatabase.GUIDToAssetPath(tmpGuids[0]),
                        urlButtonIfAny = "https://staging.developer.appearition.com/location-demo/"
                    });
            }
        }

        void RefreshFooterLinksContent()
        {
            _footerLinkItems.Clear();

            _footerLinkItems.Add(new ItemBox
            {
                name = "Dev Website",
                urlButtonIfAny = "http://docs.appearition.com/",
            });

            _footerLinkItems.Add(new ItemBox
            {
                name = "EMS Portal",
                urlButtonIfAny = "https://loginusa.appearition.com/"
            });

            _footerLinkItems.Add(new ItemBox
            {
                name = "Support",
                urlButtonIfAny = "https://www.appearition.com/contact-us/",
            });

            _footerLinkItems.Add(new ItemBox
            {
                name = "Forum",
                urlButtonIfAny = "",
                isGreyedOut = true
            });
        }

        #endregion
    }
}