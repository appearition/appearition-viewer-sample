using Appearition.ArTargetImageAndMedia;
using UnityEngine;
using UnityEngine.UI;

namespace Appearition.ArDemo
{
    /// <summary>
    /// Base class for any ArMedia which uses a Canvas to render its content.
    /// </summary>
    //[RequireComponent(typeof(RectTransform))]
    //[RequireComponent(typeof(Canvas))]
    //[RequireComponent(typeof(CanvasScaler))]
    //[RequireComponent(typeof(GraphicRaycaster))]
    public abstract class BaseCanvasMedia : BaseMedia
    {
        //References
        protected RectTransform RectTransformRef { get; set; }
        protected Canvas CanvasRef { get; set; }
        protected CanvasScaler CanvasScalerRef { get; set; }

        protected BaseCanvasMedia(MediaFile cc) : base(cc)
        {
        }

        public override void Setup(MediaHolder holder, Experience associatedExperience, MediaFile media)
        {
            base.Setup(holder, associatedExperience, media);

            //8AR fix, if the on target media's translationX is absurdly high, the object is not on the grid and should not be spawned.
            if (translationX < -9000)
            {
                AppearitionLogger.LogInfo(
                    string.Format("Media of id {0} is not located on the grid, but is onTarget (8AR hack). Disabling media.", arMediaId));

                HolderRef.enabled = false;
                HolderRef.gameObject.SetActive(false);
                return;
            }

            //Setup the common settings for the canvas objects
            RectTransformRef = HolderRef.GetComponent<RectTransform>();
            if (RectTransformRef == null)
                RectTransformRef = HolderRef.gameObject.AddComponent<RectTransform>();
            CanvasRef = HolderRef.GetComponent<Canvas>();
            if (CanvasRef == null)
                CanvasRef = HolderRef.gameObject.AddComponent<Canvas>();
            CanvasScalerRef = HolderRef.GetComponent<CanvasScaler>();
            if (CanvasScalerRef == null)
                CanvasScalerRef = HolderRef.gameObject.AddComponent<CanvasScaler>();
            if (HolderRef.GetComponent<GraphicRaycaster>() == null)
                HolderRef.gameObject.AddComponent<GraphicRaycaster>();

            //Setup the canvas object based on the mediafile ApiData.
            if (isTracking)
            {
                RectTransformRef.sizeDelta = Vector2.one;
                CanvasRef.renderMode = RenderMode.WorldSpace;
                HolderRef.transform.localRotation = Quaternion.identity; //Quaternion.Euler(Vector3.right * 90);
                HolderRef.transform.localScale = Vector3.one / 100f;
                CanvasScalerRef.dynamicPixelsPerUnit = 100; //10000;

                //Attach camera.
                CanvasRef.worldCamera = AppearitionArHandler.ProviderHandler.ProviderCamera;
            }
            else
            {
                CanvasRef.renderMode = RenderMode.ScreenSpaceOverlay;
                CanvasScalerRef.referenceResolution = new Vector2(Screen.height, Screen.width);
                CanvasScalerRef.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
                CanvasScalerRef.matchWidthOrHeight = 1;

                //if (AppearitionArHandler.ShouldFullscreenMediaHaveBlackBackground)
                //{
                Image tmp = new GameObject("Black BG").AddComponent<Image>();
                tmp.transform.SetParent(HolderRef.transform);
                tmp.transform.SetAsFirstSibling();
                tmp.rectTransform.anchorMin = Vector2.zero;
                tmp.rectTransform.anchorMax = Vector2.one;
                tmp.rectTransform.pivot = Vector2.one / 2;
                tmp.rectTransform.sizeDelta = RectTransformRef.sizeDelta;
                tmp.color = new Color(0, 0, 0, 0.7f);
                //}
            }

            //CanvasRef.sortingOrder = -1 - arMediaId;
        }

        /// <summary>
        /// Using the MediaFile's ApiData, sets the given RectTransform's 
        /// </summary>
        /// <param name="mediaRectTransform"></param>
        /// <param name="tracking"></param>
        protected void SetMediaRectPosition(RectTransform mediaRectTransform, bool tracking)
        {
            mediaRectTransform.anchorMin = Vector2.zero;
            mediaRectTransform.anchorMax = Vector2.one;
            mediaRectTransform.pivot = Vector2.one / 2;

            if (tracking)
            {
                //mediaRectTransform.Translate(new Vector3(Data.GetPosition.x * (1 / CanvasRef.transform.localScale.x),
                //    Data.GetPosition.y * (1 / CanvasRef.transform.localScale.y),
                //    Data.GetPosition.z * (1 / CanvasRef.transform.localScale.z)));
                mediaRectTransform.localRotation = GetRotation;
                mediaRectTransform.localScale = GetScale;
                mediaRectTransform.localPosition = CanvasRef.transform.rotation * new Vector3(GetPosition.x * (1 / CanvasRef.transform.localScale.x),
                    GetPosition.y * (1 / CanvasRef.transform.localScale.y),
                    GetPosition.z * (1 / CanvasRef.transform.localScale.z));
            }
            else
            {
                mediaRectTransform.anchoredPosition3D = Vector3.zero;
            }
        }
    }
}