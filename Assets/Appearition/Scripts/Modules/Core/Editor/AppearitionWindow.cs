//// -----------------------------------------------------------------------
//// Company:"Appearition Pty Ltd"
//// File: "AppearitionWindow.cs" 
//// Copyright (c) 2019. All rights reserved.
//// -----------------------------------------------------------------------

//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;
//using System;
//using UnityEditorInternal;
//using System.Text;
//using System.Linq;
//using System.IO;

//public class AppearitionWindow : EditorWindow
//{
//    #region INIT

//    private AppearitionWindow TmpWindow
//    {
//        get { return (AppearitionWindow) GetWindow(typeof(AppearitionWindow)); }
//    }

//    string getScriptPath
//    {
//        get { return AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets("AppearitionWindow")[0]); }
//    }

//    string GetScriptFolderPath
//    {
//        get
//        {
//            string tmp = getScriptPath;
//            return tmp.Substring(0, tmp.Length - (TmpWindow.GetType().ToString().Length + 3));
//        }
//    }

//    [MenuItem("Window/Appearition Editor")]
//    public static void CreateEditorWindow()
//    {
//        AppearitionWindow wee = (AppearitionWindow) GetWindow(typeof(AppearitionWindow));
//        wee.titleContent = new GUIContent("Appearition");
//        wee.Show();
//    }


//    //void OnEnable()
//    //{
//    //    currentEditorData = LoadEditorData();
//    //    currentState = currentEditorData.currentState;
//    //}

//    //void OnDisable()
//    //{
//    //    SaveEditorData(currentEditorData);
//    //}

//    #endregion

//    #region FSM

//    public enum WindowState
//    {
//        Manage_AR,
//        Manage_Profiles,
//        API_Generator,
//    }

//    public WindowState CurrentState { get; set; }

//    private Action[] _subMenus;

//    Action[] SubMenus
//    {
//        get
//        {
//            if (_subMenus == null)
//                _subMenus = new Action[] {Handle_APIGenerator};
//            return _subMenus;
//        }
//    }

//    #endregion

//    static GUIStyle _boldMiddleCenterGui;

//    static GUIStyle BoldMiddleCenterGui
//    {
//        get
//        {
//            if (_boldMiddleCenterGui == null)
//            {
//                _boldMiddleCenterGui = new GUIStyle(GUI.skin.label);
//                _boldMiddleCenterGui.fontStyle = FontStyle.Bold;
//            }

//            return _boldMiddleCenterGui;
//        }
//    }

//    void OnGUI()
//    {
//        EditorGUILayout.LabelField("Appearition EMS/API Editor", BoldMiddleCenterGui);
//        DrawSeparatorLine();

//        EditorGUILayout.BeginHorizontal();

//        for (int i = 0; i < Enum.GetNames(typeof(WindowState)).Length; i++)
//        {
//            GUI.enabled = (CurrentState != (WindowState) i);
//            GUI.color = (CurrentState == (WindowState) i ? Color.green : Color.white);
//            if (GUILayout.Button((Enum.GetNames(typeof(WindowState))[i]).Replace('_', ' ')))
//                CurrentState = (WindowState) i;
//        }

//        GUI.enabled = true;
//        GUI.color = Color.white;

//        EditorGUILayout.EndHorizontal();

//        DrawSeparatorLine();

//        //Draw the rest of the screen
//        if ((int) CurrentState < SubMenus.Length)
//            SubMenus[(int) CurrentState]();
//    }


//    #region Handle API Generator

//    public enum ApiRequestType
//    {
//        None,
//        Get,
//        Post,
//        PostMultiform
//    }

//    //Consts
//    const string API_JSON_ROOT = "JSON";
//    const string API_JSON_DATA = "ApiData";
//    const string API_JSON_CONTAINER = "CONTAINER";
//    const string API_JSON_ARRAY = "ARRAY";
//    const string API_JSON_TYPE = "TYPE";

//    //Internal Variables
//    private ApiRequestType _apiRequestType;
//    private string _apiJsonString;
//    private bool _containsPostJson = false;
//    private string _apiPostJsonString;
//    private string _apiRequestUrl;
//    private string _apiSummary;
//    private ReorderableList _apiRlAdditionalRequestParam;
//    private List<ReorderableListParam> _apiAdditionalRequestParam;
//    private Appearition.API.AuthenticationOverrideType _apiAuthenticationOverrideType = Appearition.API.AuthenticationOverrideType.None;
//    private int _apiVersionNumber = 1;

//    private void Handle_APIGenerator()
//    {
//        EditorGUILayout.LabelField("Create API from JSON", BoldMiddleCenterGui);
//        EditorGUILayout.Space();

//        bool isReadyToBuild = true;

//        _apiRequestType = (ApiRequestType) EditorGUILayout.EnumPopup("Request Type", _apiRequestType);
//        if (_apiRequestType == ApiRequestType.None)
//            isReadyToBuild = false;

//        EditorGUILayout.LabelField("Input JSON");
//        Rect jsonBodyRect = EditorGUILayout.GetControlRect(false, 70);
//        _apiJsonString = EditorGUI.TextArea(jsonBodyRect, _apiJsonString);
//        if (string.IsNullOrEmpty(_apiJsonString))
//            isReadyToBuild = false;
//        EditorGUILayout.Space();


//        //Display secondary JSON 
//        if (_apiRequestType == ApiRequestType.Post || _apiRequestType == ApiRequestType.PostMultiform)
//        {
//            _containsPostJson = EditorGUILayout.Toggle("Contains POST JSON?", _containsPostJson);
//            if (_containsPostJson)
//            {
//                Rect jsonBodyRect2 = EditorGUILayout.GetControlRect(false, 70);
//                _apiPostJsonString = EditorGUI.TextArea(jsonBodyRect2, _apiPostJsonString);
//                if (string.IsNullOrEmpty(_apiPostJsonString))
//                    isReadyToBuild = false;
//            }

//            EditorGUILayout.Space();
//        }

//        //Request URL
//        EditorGUILayout.LabelField("Request URL (ie \"api/Asset/ListByChannel/\"");
//        EditorGUILayout.BeginHorizontal();
//        EditorGUIUtility.labelWidth = 110;
//        _apiRequestUrl = EditorGUILayout.TextField(_apiRequestUrl);
//        if (GUILayout.Button(new GUIContent("[~]", "Set default value"), GUILayout.Width(30)))
//            _apiRequestUrl = "api/APICategory/APICall/";
//        EditorGUILayout.EndHorizontal();
//        EditorGUILayout.Space();
//        if (string.IsNullOrEmpty(_apiRequestUrl) || !_apiRequestUrl.Contains("api") || _apiRequestUrl[_apiRequestUrl.Length - 1] != '/')
//        {
//            //Help the user say what's wrong
//            if (_apiRequestUrl == null)
//                _apiRequestUrl = "";
//            EditorGUILayout.HelpBox("" + (_apiRequestUrl.Length < 2 ? "Missing request URL. " : "Incorrect request. ") +
//                                    (!_apiRequestUrl.Contains("api") ? "Missing \"api/\". " : "") +
//                                    (_apiRequestUrl.Length > 0 && _apiRequestUrl[_apiRequestUrl.Length - 1] != '/' ? "Missing \"/\" at the end of the request." : ""),
//                MessageType.Error);

//            isReadyToBuild = false;
//        }

//        EditorGUILayout.Space();

//        //Summary
//        EditorGUILayout.BeginHorizontal();
//        EditorGUIUtility.labelWidth = 110;
//        //EditorGUILayout.LabelField("API description");
//        _apiSummary = EditorGUILayout.TextField("API description", _apiSummary);
//        EditorGUIUtility.labelWidth = 0;
//        if (GUILayout.Button(new GUIContent("[~]", "Set default value"), GUILayout.Width(30)))
//            _apiSummary = "https://api.appearition.com/TenantName/" + _apiRequestUrl + "0 , where 0 is Channel ID ";
//        EditorGUILayout.EndHorizontal();
//        EditorGUILayout.Space();

//        //API Request Parameters
//        if (_apiAdditionalRequestParam == null)
//            _apiAdditionalRequestParam = new List<ReorderableListParam>();
//        if (_apiRlAdditionalRequestParam == null)
//        {
//            _apiRlAdditionalRequestParam = new ReorderableList(_apiAdditionalRequestParam, typeof(ReorderableListParam))
//            {
//                drawHeaderCallback = (rect => { EditorGUI.LabelField(rect, "Additional URL Params"); })
//            };
//            _apiRlAdditionalRequestParam.drawElementCallback = ((rect, index, isActive, isFocused) =>
//            {
//                rect.width /= 2;
//                EditorGUIUtility.labelWidth = 35;
//                _apiAdditionalRequestParam[index].key = EditorGUI.TextField(rect, "Key", ((ReorderableListParam) _apiRlAdditionalRequestParam.list[index]).key);
//                rect.position = rect.position + Vector2.right * (rect.width + 2);
//                EditorGUIUtility.labelWidth = 43;
//                _apiAdditionalRequestParam[index].value = (ReorderableListParam.ParamType) EditorGUI.EnumPopup(rect, "Value", ((ReorderableListParam) _apiRlAdditionalRequestParam.list[index]).value);
//                EditorGUIUtility.labelWidth = 0;
//            });
//        }

//        _apiRlAdditionalRequestParam.DoLayoutList();

//        EditorGUILayout.Space();

//        //Override
//        _apiAuthenticationOverrideType = (Appearition.API.AuthenticationOverrideType) EditorGUILayout.EnumPopup("API auth override", _apiAuthenticationOverrideType);

//        //Version
//        _apiVersionNumber = EditorGUILayout.IntField("API Version", _apiVersionNumber);

//        EditorGUILayout.BeginHorizontal();
//        if (GUILayout.Button("Reset"))
//        {
//            _apiRequestType = ApiRequestType.None;
//            _apiJsonString = "";
//            _apiPostJsonString = "";
//            _containsPostJson = false;
//            _apiRequestUrl = "";
//            _apiSummary = "";
//            _apiAuthenticationOverrideType = Appearition.API.AuthenticationOverrideType.None;
//            _apiVersionNumber = 1;
//            _apiAdditionalRequestParam.Clear();
//        }

//        if (GUILayout.Button("Create JSON API"))
//        {
//            if (isReadyToBuild)
//                GenerateApiData();
//            else
//                Debug.LogError("Not all fields contain information.");
//        }

//        EditorGUILayout.EndHorizontal();
//        EditorUtility.SetDirty (TmpWindow);
//    }

//    public void GenerateApiData()
//    {
//        //Parse API request URL
//        string[] splitUrl = _apiRequestUrl.Split('/');
//        string apiCategory = "";
//        string apiClass = "";
//        int count = -1;
//        for (int i = 0; i < splitUrl.Length; i++)
//        {
//            if (splitUrl[i].Length < 1)
//                continue;

//            if (splitUrl[i] == "api")
//                count = 0;

//            if (count < 0)
//                continue;

//            switch (count)
//            {
//                case 1:
//                    apiCategory = splitUrl[i];
//                    break;
//                case 2:
//                    apiClass = splitUrl[i];
//                    break;
//            }

//            count++;
//        }

//        string fullClassName = apiCategory + "_" + apiClass;

//        //Unbuild JSON
//        Dictionary<string, Dictionary<string, string>> extractedJson = ParseJsonIntoContainer(_apiJsonString);

//        //foreach (var wee in extractedJson)
//        //{
//        //    foreach (var nyah in extractedJson[wee.Key])
//        //        Debug.LogError("Holder: " + wee.Key + ", " + nyah.Key + ":" + nyah.Value);
//        //}

//        #region GENERATE CLASS

//        //Create script header
//        StringBuilder sb = new StringBuilder();
//        int tmpIndent = 0;
//        sb.AppendLineIndent("namespace Appearition.API");
//        sb.AppendLineIndent("{"); //begin namespace
//        tmpIndent++;
//        if (!string.IsNullOrEmpty(_apiSummary))
//        {
//            sb.AppendLineIndent("/// <summary>");
//            sb.AppendLineIndent("/// " + _apiSummary);
//            sb.AppendLineIndent("/// </summary>");
//        }

//        sb.AppendLineIndent("[System.Serializable]");
//        sb.AppendLineIndent("public class " + fullClassName + " : " + (_apiRequestType == ApiRequestType.Get ? "BaseApiGet" : "BaseApiPost"), tmpIndent);
//        sb.AppendLineIndent("{", tmpIndent); //Begin main class
//        tmpIndent++;

//        //Override authentication override type if not default
//        if (_apiAuthenticationOverrideType != Appearition.API.AuthenticationOverrideType.None)
//        {
//            sb.AppendLineIndent("public override AuthenticationOverrideType AuthenticationOverride {", tmpIndent);
//            sb.AppendLineIndent("get {", ++tmpIndent);
//            sb.AppendLineIndent(string.Format("return AuthenticationOverrideType.{0};", _apiAuthenticationOverrideType.ToString()), ++tmpIndent);
//            sb.AppendLineIndent("}", --tmpIndent);
//            sb.AppendLineIndent("}", --tmpIndent);
//            sb.AppendLineIndent();
//        }

//        //Override version number if not default
//        if (_apiVersionNumber != 1)
//        {
//            sb.AppendLineIndent("public override int ApiVersion {", tmpIndent);
//            sb.AppendLineIndent("get {", ++tmpIndent);
//            sb.AppendLineIndent("return " + _apiVersionNumber + ";", ++tmpIndent);
//            sb.AppendLineIndent("}", --tmpIndent);
//            sb.AppendLineIndent("}", --tmpIndent);
//            sb.AppendLineIndent();
//        }

//        if (_apiAdditionalRequestParam.Count > 0)
//        {
//            //Additional URL params
//            sb.AppendLineIndent("//Request Params", tmpIndent);
//            sb.AppendLineIndent("public class RequestContent : BaseRequestContent", tmpIndent);
//            sb.AppendLineIndent("{", tmpIndent);
//            tmpIndent++;

//            for (int i = 0; i < _apiAdditionalRequestParam.Count; i++)
//                sb.AppendLineIndent(GetJsonEntryString(
//                    new KeyValuePair<string, string>(_apiAdditionalRequestParam[i].key, _apiAdditionalRequestParam[i].value.ToString().ToLower()), "", false), tmpIndent);

//            sb.AppendLineIndent("}", --tmpIndent);
//            sb.AppendLineIndent();
//        }

//        if (_apiRequestType == ApiRequestType.PostMultiform)
//        {
//            //Add multiform info
//            sb.AppendLineIndent("public override TypeOfPost FormType {", tmpIndent);
//            sb.AppendLineIndent("get {", ++tmpIndent);
//            sb.AppendLineIndent("return TypeOfPost.MultiForms;", ++tmpIndent);
//            sb.AppendLineIndent("}", --tmpIndent);
//            sb.AppendLineIndent("}", --tmpIndent);
//        }

//        //WRITE ALL THE JSON !
//        WriteClassFromJson(ref sb, extractedJson, ref tmpIndent);

//        int indentBackup = tmpIndent;
//        if (_apiRequestType == ApiRequestType.Get || !_containsPostJson)
//        {
//            for (int i = 0; i < indentBackup; i++)
//                sb.AppendLineIndent("}", --tmpIndent);
//        }
//        else
//        {
//            //POST JSON !
//            for (int i = 0; i < indentBackup - 2; i++)
//                sb.AppendLineIndent("}", --tmpIndent);

//            //Inset 2nd JSON
//            sb.AppendLineIndent();
//            sb.AppendLineIndent("/// <summary>", tmpIndent);
//            sb.AppendLineIndent("/// Post ApiData", tmpIndent);
//            sb.AppendLineIndent("/// </summary>", tmpIndent);
//            sb.AppendLineIndent("public class PostData : IPostParam", tmpIndent);
//            sb.AppendLineIndent("{", tmpIndent);
//            tmpIndent++;
//            WriteClassFromJson(ref sb, ParseJsonIntoContainer(_apiPostJsonString), ref tmpIndent);
//            sb.AppendLineIndent("}", --tmpIndent);
//            sb.AppendLineIndent();

//            //close indent again
//            indentBackup = tmpIndent;
//            for (int i = 0; i < indentBackup; i++)
//                sb.AppendLineIndent("}", --tmpIndent);
//        }

//        #endregion

//        Debug.LogError(sb.ToString());

//        //Save the file
//        string folderName = GetScriptFolderPath.Substring(0, GetScriptFolderPath.Length - "Editor/".Length) + "/API/";
//        if (!Directory.Exists(folderName))
//            Directory.CreateDirectory(folderName);
//        //System.IO.File.Create(getScriptFolderPath + "/GeneratedAPI/" + api_category + "_" + api_class + ".cs");
//        using (FileStream fs = new FileStream(folderName + apiCategory + "_" + apiClass + ".cs", FileMode.Create, FileAccess.ReadWrite))
//        {
//            using (StreamWriter sw = new StreamWriter(fs))
//            {
//                sw.Write(sb.ToString());
//            }
//        }

//        AssetDatabase.Refresh();
//    }

//    /// <summary>
//    /// Takes an extract JSON and writes its content parsed out in C# inside the given string builder.
//    /// Uses a given indent to keep it all proper.
//    /// </summary>
//    /// <param name="sb">Sb.</param>
//    /// <param name="extractedJson">Extracted JSO.</param>
//    /// <param name="tmpIndent">Tmp indent.</param>
//    static void WriteClassFromJson(ref StringBuilder sb, Dictionary<string, Dictionary<string, string>> extractedJson, ref int tmpIndent)
//    {
//        //Variables
//        sb.AppendLineIndent("//Variables", tmpIndent);
//        foreach (var jsonLayerContent in extractedJson[API_JSON_ROOT])
//        {
//            if (jsonLayerContent.Key == "\"IsSuccess\"" || jsonLayerContent.Key == "\"Errors\"")
//                continue;

//            //Common classes

//            //New sub-clases
//            if (extractedJson.ContainsKey(jsonLayerContent.Key))
//            {
//                string customType = extractedJson[jsonLayerContent.Key].FirstOrDefault(o => o.Key == API_JSON_TYPE).Key;
//                if (customType == null)
//                    customType = "";
//                sb.AppendLineIndent(GetJsonEntryString(jsonLayerContent, customType), tmpIndent);
//            }
//            else
//                sb.AppendLineIndent(GetJsonEntryString(jsonLayerContent), tmpIndent);
//        }

//        sb.AppendLineIndent();

//        //Empty ApiData handling
//        if (extractedJson.Keys.Any(o => o.Contains(API_JSON_DATA)) && extractedJson[extractedJson.Keys.First(o => o.Contains(API_JSON_DATA))].Count == 0)
//        {
//            sb.AppendLineIndent("public class ApiData { }", tmpIndent);
//        }

//        //SubClasses (loop)
//        foreach (var jsonLayer in extractedJson)
//        {
//            //The root has already been handled
//            if (jsonLayer.Key == API_JSON_ROOT || jsonLayer.Value.Any(o => o.Value == API_JSON_TYPE) || jsonLayer.Value.Count == 0 || IsCommonApiClass(jsonLayer.Key))
//                continue;

//            Debug.LogError(jsonLayer.Key + " first item: " + jsonLayer.Value.First().Value);
//            sb.AppendLineIndent("//SubClasses", tmpIndent);

//            string newKey = jsonLayer.Key.Substring(1, jsonLayer.Key.Length - 2);
//            sb.AppendLineIndent("public class " + GetJsonClassName(newKey), tmpIndent);
//            sb.AppendLineIndent("{", tmpIndent);
//            tmpIndent++;

//            //Variables
//            sb.AppendLineIndent("//Variables", tmpIndent);
//            foreach (var jsonLayerContent in jsonLayer.Value)
//            {
//                if (extractedJson.ContainsKey(jsonLayerContent.Key))
//                {
//                    string customType = extractedJson[jsonLayerContent.Key].FirstOrDefault(o => o.Key == API_JSON_TYPE).Key;
//                    if (customType == null)
//                        customType = "";
//                    sb.AppendLineIndent(GetJsonEntryString(jsonLayerContent, customType), tmpIndent);
//                }
//                else
//                    sb.AppendLineIndent(GetJsonEntryString(jsonLayerContent), tmpIndent);
//            }

//            sb.AppendLineIndent();

//            //Subclasses

//            //If no variable inside contain the class, then close the curvy brace
//            bool anyChildClasses = false;
//            int amountOfClassesToClose = 0;
//            foreach (var layers in extractedJson)
//            {
//                //Current layer, check if there's any custom types
//                if (layers.Key == jsonLayer.Key)
//                {
//                    anyChildClasses = jsonLayer.Value.Any(o => ((o.Value == API_JSON_ARRAY || o.Key == API_JSON_CONTAINER)));
//                }
//                else
//                {
//                    //Find where this layer appears on other layers
//                    var listOfLayerContent = layers.Value.ToList();

//                    if (listOfLayerContent.FindAll(o => o.Key == jsonLayer.Key).Count > 0)
//                    {
//                        // *appears* OwO
//                        for (int i = 0; i < listOfLayerContent.Count; i++)
//                        {
//                            if (listOfLayerContent[i].Key != jsonLayer.Key && string.Compare(jsonLayer.Key, listOfLayerContent[i].Key, StringComparison.Ordinal) == 1
//                                                                           && (listOfLayerContent[i].Value == API_JSON_ARRAY || listOfLayerContent[i].Value == API_JSON_CONTAINER))
//                                amountOfClassesToClose++;
//                        }
//                    }
//                }
//            }

//            if (!anyChildClasses)
//            {
//                //Include current
//                amountOfClassesToClose++;
//                Debug.LogError(jsonLayer.Key + " has no child classes, closing the curve brace!" + amountOfClassesToClose);
//                for (int i = 0; i < amountOfClassesToClose; i++)
//                {
//                    tmpIndent--;
//                    sb.AppendLineIndent("}", tmpIndent);
//                }
//            }
//        }
//    }

//    static Dictionary<string, Dictionary<string, string>> ParseJsonIntoContainer(string json)
//    {
//        var layerContent = new Dictionary<string, Dictionary<string, string>>();

//        string fullJson = System.Text.RegularExpressions.Regex.Replace(json, @"\s", "");
//        string[] split = fullJson.Split(new[] {'[', ']', '{', '}'}, StringSplitOptions.RemoveEmptyEntries);
//        List<string> cleanSplit = new List<string>(split);
//        cleanSplit.RemoveAll(o => o.Length < 1);

//        //Set ROOT level
//        layerContent.Add("JSON", new Dictionary<string, string>());
//        List<string> preClassName = new List<string>() {"JSON"};

//        //Temp params
//        string paramName = "";
//        int classDepth = 0;

//        for (int i = 0; i < cleanSplit.Count; i++)
//        {
//            string[] comaSplit = cleanSplit[i].Split(',');

//            //Split between variables
//            for (int k = 0; k < comaSplit.Length; k++)
//            {
//                if (comaSplit[k].Length < 1)
//                    continue;

//                string[] colonSplit = comaSplit[k].Split(':');

//                //Extract per KEY/VALUE
//                paramName = "";
//                for (int j = 0; j < colonSplit.Length; j++)
//                {
//                    if (colonSplit[j].Length < 1)
//                        continue;

//                    if (paramName.Length < 1)
//                        paramName = colonSplit[j];
//                    else
//                    {
//                        if (fullJson.Contains("}," + paramName) || fullJson.Contains("}]," + paramName))
//                        {
//                            string preContain = (fullJson.Contains("}," + paramName) ? "}" : "}]") + ",";

//                            Debug.LogError(paramName + " " + preClassName[preClassName.Count - 1]);
//                            if (fullJson.Contains("}," + paramName) && preClassName.Count > 1)
//                            {
//                                classDepth--;
//                                preClassName.RemoveAt(preClassName.Count - 1);
//                            }

//                            do
//                            {
//                                try
//                                {
//                                    if (preClassName.Count > 1)
//                                    {
//                                        classDepth--;
//                                        preClassName.RemoveAt(preClassName.Count - 1);
//                                    }
//                                    else
//                                        break;

//                                    if (fullJson.Contains("}" + preContain + paramName))
//                                    {
//                                        preContain = preContain.Insert(0, "}");
//                                    }
//                                    else
//                                        break;
//                                }
//                                catch
//                                {
//                                    Debug.LogError(paramName);
//                                    break;
//                                }
//                            } while (fullJson.Contains(preContain + paramName));
//                        }


//                        Debug.LogError("Holder: " + preClassName[preClassName.Count - 1] + ", key: " + paramName + ", Value: " + colonSplit[j]);
//                        layerContent[preClassName[preClassName.Count - 1]].Add(paramName, colonSplit[j]);

//                        paramName = "";
//                    }
//                }
//            }

//            //Check if unfinished entry. It means that a new depth is starting.
//            if (paramName.Length > 0)
//            {
//                if (fullJson.Contains(paramName + ":{"))
//                {
//                    Debug.LogError("Container: " + preClassName[preClassName.Count - 1] + ", key: " + paramName + ", Value: " + "CONTAINER");
//                    layerContent[preClassName[preClassName.Count - 1]].Add(paramName, API_JSON_CONTAINER);
//                }
//                else if (fullJson.Contains(paramName + ":["))
//                {
//                    Debug.LogError("Array: " + preClassName[preClassName.Count - 1] + ", key: " + paramName + ", Value: " + "ARRAY");
//                    layerContent[preClassName[preClassName.Count - 1]].Add(paramName, API_JSON_ARRAY);
//                }
//                else if (fullJson.Contains("[" + paramName + "]"))
//                {
//                    //Array element
//                    Debug.LogError("Array: " + preClassName[preClassName.Count - 1] + ", key: " + paramName + ", Value: " + "TYPE");
//                    layerContent[preClassName[preClassName.Count - 1]].Add(paramName, API_JSON_TYPE);
//                }
//                else
//                    Debug.LogError("ENTRY NOT RECOGNIZED");

//                try
//                {
//                    preClassName.Add(paramName);
//                    layerContent.Add(paramName, new Dictionary<string, string>());
//                }
//                catch
//                {
//                    Debug.LogError(paramName);
//                }

//                classDepth++;
//            }
//            else
//            {
//                classDepth--;
//                preClassName.RemoveAt(preClassName.Count - 1);
//            }
//        }

//        return layerContent;
//    }

//    static string GetJsonEntryString(KeyValuePair<string, string> entry, string arrayEntryType = "", bool removeQuotes = true)
//    {
//        //Remove the quotes
//        string newVarName;
//        string newKey;

//        if (removeQuotes)
//            newKey = entry.Key.Substring(1, entry.Key.Length - 2);
//        else
//            newKey = entry.Key;

//        //Change the key into one of the exceptions if needed
//        IsJsonException(newKey, out newVarName);

//        string output = "";

//        switch (entry.Value)
//        {
//            case API_JSON_CONTAINER:
//                if (arrayEntryType.Length < 1)
//                    output = "public " + GetJsonClassName(newVarName) + " " + newVarName + ";";
//                break;
//            case API_JSON_ARRAY:
//                if (arrayEntryType.Length < 1)
//                    output = "public " + GetJsonClassName(newVarName) + "[] " + newVarName + ";";
//                else
//                    output = "public " + arrayEntryType.ToLower() + "[] " + newVarName + ";";
//                break;
//            case "\"string\"":
//                output = "public string " + newVarName + ";";
//                break;
//            case "0":
//            case "int":
//                if (entry.Key.ToLower().Contains("translation") || entry.Key.ToLower().Contains("rotation") || entry.Key.ToLower().Contains("scale"))
//                    output = "public float " + newVarName + ";";
//                else
//                    output = "public int " + newVarName + ";";
//                break;
//            case "0.0":
//            case "0.0f":
//            case "float":
//                output = "public float " + newVarName + ";";
//                break;
//            case "true":
//            case "bool":
//                output = "public bool " + newVarName + ";";
//                break;
//            case API_JSON_TYPE:
//                break;
//            default:
//                Debug.LogError("Value : " + entry.Value + " not recognized! (Key:" + newKey + " output)");
//                break;
//        }

//        return output;
//    }

//    static void IsJsonException(string key, out string properKey, bool isVariableName = false)
//    {
//        if (isVariableName)
//        {
//            if (key == "ApiData")
//                properKey = "ApiData";
//            else if (key == "parameters")
//                properKey = "params";
//            else
//                properKey = key;
//        }
//        else
//        {
//            if (key == "ApiData")
//                properKey = "ApiData";
//            else if (key == "params")
//                properKey = "parameters";
//            else
//                properKey = key;
//        }
//    }

//    static string GetJsonClassName(string variableName)
//    {
//        IsJsonException(variableName, out variableName, true);
//        //Uppercase first letter
//        variableName = char.ToUpper(variableName[0]) + variableName.Substring(1);

//        //Remove plural
//        if (variableName.Substring(0, variableName.Length - 3) == "ies")
//            variableName = variableName.Substring(0, variableName.Length - 3);
//        else if (variableName.Substring(0, variableName.Length - 2) == "es")
//            variableName = variableName.Substring(0, variableName.Length - 2);
//        else if (variableName[variableName.Length - 1] == 's')
//            variableName = variableName.Substring(0, variableName.Length - 1);

//        return variableName;
//    }

//    /// <summary>
//    /// Returns whether or not this json layer key already has a class created for it in the API commons.
//    /// </summary>
//    /// <returns><c>true</c> if is common API class the specified jsonKey; otherwise, <c>false</c>.</returns>
//    /// <param name="jsonKey">Json key.</param>
//    static bool IsCommonApiClass(string jsonKey)
//    {
//        jsonKey = jsonKey.ToLower();
//        return jsonKey.Contains("mediafiles") || jsonKey.Contains("targetimages") || jsonKey.Contains("settings");
//    }

//    #endregion

//    #region Utilities

//    [Serializable]
//    public class ReorderableListParam
//    {
//        public enum ParamType
//        {
//            Int,
//            String,
//            Float,
//            Bool
//        }

//        public string key;
//        public ParamType value;
//    }


//    public static void DrawSeparatorLine()
//    {
//        GUILayout.Space(-8);
//        EditorGUILayout.LabelField("_________________________________________________________________________________________________________________________________________________________");
//    }

//    #endregion
//}

//#region Extensions

//public static class WindowExtensions
//{
//    public static StringBuilder AppendLineIndent(this StringBuilder sb, int indent = 0)
//    {
//        string indentText = "";
//        for (int i = 0; i < indent; i++)
//            indentText += "\t";

//        return sb.AppendLine(indentText);
//    }

//    public static StringBuilder AppendLineIndent(this StringBuilder sb, string value, int indent = 0)
//    {
//        string indentText = "";
//        for (int i = 0; i < indent; i++)
//            indentText += "\t";

//        return sb.AppendLine(indentText + value);
//    }
//}

//#endregion