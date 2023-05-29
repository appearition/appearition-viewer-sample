using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Appearition.ArTargetImageAndMedia;
using Appearition.Common;
using UnityEngine;
using UnityEngine.UI;

namespace Appearition.ArDemo
{
    public class ImageMedia : BaseCanvasMedia
    {
        /// <summary>
        /// Image displaying the content of the media.
        /// </summary>
        protected Image ImageRef { get; set; }

        public ImageMedia(MediaFile cc) : base(cc)
        {
        }

        public override float GenerateMediaAssociationScore()
        {
            return mediaType.Equals("image", StringComparison.InvariantCultureIgnoreCase) ? 1.0f : 0;
        }

        public override void Setup(MediaHolder holder, Experience associatedExperience, MediaFile media)
        {
            base.Setup(holder, associatedExperience, media);

            //Create an image object as a child, which will contain the image itself.
            ImageRef = new GameObject("Image").AddComponent<Image>();
            ImageRef.transform.SetParent(HolderRef.transform);

            ImageRef.type = Image.Type.Simple;
            ImageRef.preserveAspect = true; // !isTracking;

            if (!string.IsNullOrEmpty(text))
            {
                Button attachedButton = ImageRef.gameObject.AddComponent<Button>();
                attachedButton.onClick.AddListener(OnAttachedButtonPressed);
            }

            //Position the image
            SetMediaRectPosition(ImageRef.rectTransform, isTracking);
            ImageRef.gameObject.SetActive(false);
        }

        public override IEnumerator DownloadAndLoadMedia(Action<BaseMedia, MediaLoadingState> onComplete)
        {
            Dictionary<string, byte[]> mediaOutcome = new Dictionary<string, byte[]>();
            yield return ArTargetHandler.LoadMediaFileContentProcess(ExperienceRef.Data, this, success => mediaOutcome = success);

            if (ImageRef != null)
            {
                if (mediaOutcome != null && mediaOutcome.Count > 0)
                {
                    Sprite tmpSprite = ImageUtility.LoadOrCreateSprite(mediaOutcome.First().Value);
                    if (tmpSprite != null)
                        ImageRef.sprite = tmpSprite;
                    else
                        ImageRef.sprite = null;
                }
                else
                {
                    Debug.LogWarning("No sprite found.");
                    ImageRef.sprite = null;
                }

                ImageRef.gameObject.SetActive(ImageRef.sprite != null);
                LoadingState = ImageRef.sprite != null ? MediaLoadingState.LoadingSuccessful : MediaLoadingState.LoadingFailed;
            }
            else
                LoadingState = MediaLoadingState.LoadingFailed;

            //Flag!
            onComplete?.Invoke(this, LoadingState);
        }

        /// <summary>
        /// Called when clicking on the Image, if the MediaFile contains a URL in the Text field.
        /// </summary>
        void OnAttachedButtonPressed()
        {
            Application.OpenURL(text);
        }

        public override void Dispose()
        {
            if (ImageRef != null && ImageRef.sprite != null && LoadingState == MediaLoadingState.LoadingSuccessful)
                Texture2D.Destroy(ImageRef.sprite.texture);
        }
    }
}