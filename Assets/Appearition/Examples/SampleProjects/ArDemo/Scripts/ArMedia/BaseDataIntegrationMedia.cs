using System.Collections;
using System.Collections.Generic;
using Appearition.ArTargetImageAndMedia;
using Appearition.Common;
using Appearition.Common.TypeExtensions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Appearition.ArDemo
{
    public abstract class BaseDataIntegrationMedia : BaseCanvasMedia
    {
        //Consts
        const string BG_WIDTH_KEY = "canvas-width";
        const string BG_HEIGHT_KEY = "canvas-height";
        const string BG_COLOR_KEY = "background-color";
        const string FONT_COLOR_KEY = "font-color";
        const string FONT_SIZE_KEY = "font-size";
        const string FONT_WEIGHT_KEY = "font-weight";
        const string FONT_STYLE_KEY = "font-style";

        const string DATA_REFRESH_KEY = "datarefreshseconds";//"dataRefreshSeconds";
        const string IS_BILLBOARD_KEY = "alwaysfacingcamera";


        //References
        protected Image backgroundImage;
        protected Text queryText;

        //Data
        protected Dictionary<string, string> customParams = new Dictionary<string, string>();
        public override bool IsMediaReadyAndDownloaded => true;
        protected virtual bool UseBackgroundImage => true;
        protected virtual bool UseQueryText => true;
        Coroutine _observerProcess;
        Coroutine _refreshProcess;

        #region Default Customs

        protected virtual Color BackgroundColor
        {
            get
            {
                Color? bgCol = customParams.ContainsKey(BG_COLOR_KEY) ? customParams[BG_COLOR_KEY].TryConvertToColor() : default;
                return bgCol ?? Color.white;
            }
        }

        protected virtual float BackgroundWidth
        {
            get
            {
                if (customParams.ContainsKey(BG_WIDTH_KEY) && float.TryParse(customParams[BG_WIDTH_KEY], out float newValue))
                    return newValue;
                return 512;
            }
        }

        protected virtual float BackgroundHeight
        {
            get
            {
                if (customParams.ContainsKey(BG_HEIGHT_KEY) && float.TryParse(customParams[BG_HEIGHT_KEY], out float newValue))
                    return newValue;
                return 512;
            }
        }

        protected virtual Color FontColor
        {
            get
            {
                Color? fontCol = customParams.ContainsKey(FONT_COLOR_KEY) ? customParams[FONT_COLOR_KEY].TryConvertToColor() : default;
                return fontCol ?? Color.black;
            }
        }

        protected virtual int FontSize
        {
            get
            {
                //if (customParams.ContainsKey(FONT_SIZE_KEY) && int.TryParse(customParams[FONT_SIZE_KEY], out int newValue))
                //    return newValue;
                GameObject[] gameObjects = SceneManager.GetActiveScene().GetRootGameObjects();
                foreach (GameObject root in gameObjects)
                {

                    if (customParams.ContainsKey(FONT_SIZE_KEY) && int.TryParse(customParams[FONT_SIZE_KEY].Replace("px", ""), out int newValue))
                        return newValue;

                }

                //if (customParams.ContainsKey(FONT_SIZE_KEY) && int.TryParse(customParams[FONT_SIZE_KEY].Replace("px",""), out int newValue))
                //    return newValue;
                return 10;
            }
        }

        protected virtual FontStyle FontStyleAndWeight
        {
            get
            {
                string fontStyle = customParams.ContainsKey(FONT_STYLE_KEY) ? customParams[FONT_STYLE_KEY].ToLower() : "";
                string fontWeight = customParams.ContainsKey(FONT_WEIGHT_KEY) ? customParams[FONT_WEIGHT_KEY].ToLower() : "";

                if (fontWeight.Equals("bold") && !fontStyle.Equals("italic"))
                    return FontStyle.Bold;
                if (!fontWeight.Equals("bold") && fontStyle.Equals("italic"))
                    return FontStyle.Italic;
                if (fontWeight.Equals("bold") && fontStyle.Equals("italic"))
                    return FontStyle.BoldAndItalic;

                return FontStyle.Normal;
            }
        }

        bool? _isBillboard;
        protected virtual bool IsBillboard
        {
            get
            {
                if (_isBillboard.HasValue)
                    return _isBillboard.Value;

                if (customParams.ContainsKey(IS_BILLBOARD_KEY) && bool.TryParse(customParams[IS_BILLBOARD_KEY], out bool newValue))
                    _isBillboard = newValue;
                else
                    _isBillboard = false;

                return _isBillboard.Value;
            }
            set => _isBillboard = value;
        }

        protected virtual float? RefreshTimer
        {
            get
            {
                if (customParams.ContainsKey(DATA_REFRESH_KEY) && float.TryParse(customParams[DATA_REFRESH_KEY], out float newValue))
                {
                    if (newValue > 0)
                        return newValue;
                }

                return default;
            }
        }

        #endregion

        float _refreshTimer = 0;
        protected bool IsRefreshProcessOngoing => _refreshProcess != null;
        protected virtual bool AllowRefresh => true;

        protected BaseDataIntegrationMedia(MediaFile cc) : base(cc)
        {
        }

        public override void Setup(MediaHolder holder, Experience associatedExperience, MediaFile media)
        {
            base.Setup(holder, associatedExperience, media);

            //Get custom params, if any
            if (!string.IsNullOrEmpty(media.custom))
                customParams = media.custom.DeserializeDictionary();

            //BackgroundImage
            if (UseBackgroundImage)
            {
                backgroundImage = new GameObject("Background").AddComponent<Image>();
                backgroundImage.transform.SetParent(HolderRef.transform);
                SetMediaRectPosition(backgroundImage.rectTransform, true);

                backgroundImage.color = BackgroundColor;
                GameObject[] gameObjects = SceneManager.GetActiveScene().GetRootGameObjects();
                foreach (GameObject root in gameObjects)
                {

                    //backgroundImage.rectTransform.sizeDelta = new Vector2(BackgroundWidth, BackgroundHeight);

                }
                //backgroundImage.rectTransform.sizeDelta = new Vector2(BackgroundWidth, BackgroundHeight);
            }
            

            //Font
            if (UseQueryText)
            {
                queryText = new GameObject("Query Text").AddComponent<RectTransform>().gameObject.AddComponent<Text>();
                queryText.transform.SetParent(backgroundImage.transform);
                queryText.transform.localPosition = UseBackgroundImage ? Vector3.zero : GetPosition;
                queryText.transform.localRotation = UseBackgroundImage ? Quaternion.identity : GetRotation;
                if (BackgroundHeight < BackgroundWidth)
                    queryText.transform.localScale = UseBackgroundImage ? new Vector3(1.0f, BackgroundWidth / BackgroundHeight, 1.0f) : GetScale;
                else if (BackgroundHeight > BackgroundWidth)
                    queryText.transform.localScale = UseBackgroundImage ? new Vector3(BackgroundHeight / BackgroundWidth, 1.0f, 1.0f) : GetScale;
                else
                    queryText.transform.localScale = UseBackgroundImage ? Vector3.one : GetScale;
                queryText.rectTransform.pivot = Vector2.one / 2;
                queryText.rectTransform.anchorMin = Vector2.zero;
                queryText.rectTransform.anchorMax = Vector2.one;
                queryText.rectTransform.sizeDelta = Vector2.zero;

                queryText.fontStyle = FontStyleAndWeight;
                queryText.color = FontColor;
                queryText.fontSize = 10;
                queryText.alignment = TextAnchor.MiddleCenter;
                queryText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");

                //OVERRIDE DYNAMIC
                CanvasScalerRef.dynamicPixelsPerUnit = 5;
                if (!isDataQuery)
                {
                    ApplyDataToVisuals(text);
                }
            }

            //Start observer process
            if (_observerProcess != null)
                AppearitionGate.Instance.StopCoroutine(_observerProcess);
            _observerProcess = AppearitionGate.Instance.StartCoroutine(HolderStateObserverProcess());
        }

        public override void ChangeDisplayState(AppearitionArHandler.TargetState state)
        {
            base.ChangeDisplayState(state);
            if (!isDataQuery)
                return;
            //Upon opening, always refresh.
            if (AllowRefresh && state.IsTracking())
            {
                if (_refreshProcess != null)
                    HolderRef.StopCoroutine(_refreshProcess);
                _refreshProcess = HolderRef.StartCoroutine(RefreshDataProcess());
            }
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            Debug.Log(isDataQuery + HolderRef.gameObject.name);
            if (RefreshTimer.HasValue && !IsRefreshProcessOngoing && HolderRef.Media.isDataQuery)
            {
                if (_refreshTimer < 0)
                {
                    _refreshProcess = HolderRef.StartCoroutine(RefreshDataProcess());
                    _refreshTimer = RefreshTimer.GetValueOrDefault();

                }
                else
                    _refreshTimer -= Time.deltaTime;
            }

            //Billboard
            if (IsBillboard && AppearitionArHandler.ProviderHandler != null && AppearitionArHandler.ProviderHandler.ProviderCamera != null)
            {
                if (backgroundImage != null)
                    //backgroundImage.transform.LookAt(backgroundImage.transform.position - AppearitionArHandler.ProviderHandler.ProviderCamera.transform.position);
                    backgroundImage.transform.rotation = Quaternion.LookRotation(backgroundImage.transform.position - AppearitionArHandler.ProviderHandler.ProviderCamera.transform.position);
                else
                    CanvasRef.transform.LookAt(CanvasRef.transform.position - AppearitionArHandler.ProviderHandler.ProviderCamera.transform.position);
            }
        }


        /// <summary>
        /// Get the latest query content.
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerator RefreshDataProcess()
        {
            string data = "";
            yield return ArTargetHandler.GetDataIntegrationResultProcess(ExperienceRef.Data, this, success => data = success);
            ApplyDataToVisuals(data);
            _refreshTimer = RefreshTimer.GetValueOrDefault();
            _refreshProcess = null;
        }

        IEnumerator HolderStateObserverProcess()
        {
            bool currentState = false;

            while (true)
            {
                if (HolderRef == null || HolderRef.gameObject == null)
                {
                    if (_observerProcess != null)
                        AppearitionGate.Instance.StopCoroutine(_observerProcess);
                    break;
                }

                if (currentState != HolderRef.gameObject.activeInHierarchy)
                {
                    if (HolderRef.gameObject.activeInHierarchy)
                        OnEnableOccurred();
                    else
                    {
                        //Stop refresh process
                        if (_refreshProcess != null)
                            HolderRef.StopCoroutine(_refreshProcess);
                        _refreshProcess = null;

                        OnDisableOccurred();
                    }
                }

                currentState = HolderRef.gameObject.activeInHierarchy;
                yield return null;
            }
        }

        protected virtual void ApplyDataToVisuals(string data)
        {
            //Just apply text
            queryText.text = data;
        }

        protected virtual void OnEnableOccurred()
        {
        }

        protected virtual void OnDisableOccurred()
        {
        }
    }
}