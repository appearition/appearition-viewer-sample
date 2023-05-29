// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: "HandyGuiStyles.cs" 
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using UnityEngine;

namespace Appearition.EditorUtilities
{
    public static class HandyGuiStyles
    {
        #region Standard Labels

        private static GUIStyle _centeredGui = null;

        public static GUIStyle CenteredGui
        {
            get
            {
                if (_centeredGui == null)
                {
                    _centeredGui = new GUIStyle(GUI.skin.label);
                    _centeredGui.alignment = TextAnchor.MiddleCenter;
                }

                return _centeredGui;
            }
        }

        private static GUIStyle _boldGui = null;

        public static GUIStyle BoldGui
        {
            get
            {
                if (_boldGui == null)
                {
                    _boldGui = new GUIStyle(GUI.skin.label);
                    _boldGui.fontStyle = FontStyle.Bold;
                }

                return _boldGui;
            }
        }

        private static GUIStyle _boldCenteredGui = null;

        public static GUIStyle BoldCenteredGui
        {
            get
            {
                if (_boldCenteredGui == null)
                {
                    _boldCenteredGui = new GUIStyle(GUI.skin.label);
                    _boldCenteredGui.fontStyle = FontStyle.Bold;
                    _boldCenteredGui.alignment = TextAnchor.MiddleCenter;
                }

                return _boldCenteredGui;
            }
        }

        #endregion

        #region Standard Buttons / others

        private static GUIStyle _centeredButtonGui = null;

        public static GUIStyle CenteredButtonGui
        {
            get
            {
                if (_centeredButtonGui == null)
                {
                    _centeredButtonGui = new GUIStyle(GUI.skin.button);
                    _centeredButtonGui.alignment = TextAnchor.MiddleCenter;
                }

                return _centeredButtonGui;
            }
        }

        private static GUIStyle _foldoutBoldGui = null;

        public static GUIStyle FoldoutBoldGui
        {
            get
            {
                if (_foldoutBoldGui == null)
                {
                    _foldoutBoldGui = new GUIStyle(UnityEditor.EditorStyles.foldout);
                    _foldoutBoldGui.fontStyle = FontStyle.Bold;
                }

                return _foldoutBoldGui;
            }
        }

        #endregion

        #region Boxed ApiData

        private static GUIStyle _greenBoxGui = null;

        public static GUIStyle GreenBoxGui
        {
            get
            {
                if (_greenBoxGui == null)
                {
                    _greenBoxGui = new GUIStyle(GUI.skin.textField);
                    _greenBoxGui.alignment = TextAnchor.MiddleCenter;
                    //_greenBoxGUI.
                }

                return _greenBoxGui;
            }
        }

        #endregion

        #region Utilities

        public static string screenSeparator =
            "____________________________________________________________________________________________________________________________________________________________________________________________";

        #endregion
    }
}