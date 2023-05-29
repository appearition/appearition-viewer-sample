using System;
using System.Collections;
using System.Collections.Generic;
using Appearition.ArTargetImageAndMedia;
using Appearition.Common;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace Appearition.ArDemo
{
    public class VideoMedia : BaseCanvasMedia
    {
        public delegate void VideoLoaded(string videoMediaId);

        /// <summary>
        /// Occurs whenever the video has done loading. Can be caught for debugging, analytic or UI.
        /// </summary>
        public event VideoLoaded OnVideoLoaded;

        //References
        protected VideoMediaHolder VideoMediaHolder { get; private set; }

        public VideoPlayer VideoPlayerRef => VideoMediaHolder.videoPlayer;
        public RawImage DisplayImageRef => VideoMediaHolder.displayImage;
        public RenderTexture TemplateRenderTextureRef => VideoMediaHolder.templateRenderTexture;
        public RectTransform PanelControlHolderRef => VideoMediaHolder.panelControlHolder;
        public AspectRatioFitter RatioFitterRef => VideoMediaHolder.ratioFitter;

        //Internal Variables
        public override bool IsMediaReadyAndDownloaded => _isVideoReadyToPlay;

        private bool _isVideoReadyToPlay;

        public VideoMedia(MediaFile cc) : base(cc)
        {
        }
        
        public override float GenerateMediaAssociationScore()
        {
            return mediaType.Equals("video", StringComparison.InvariantCultureIgnoreCase) ? 1.0f : 0f;
        }

        public override string PathToPrefab => $"{ArDemoConstants.ARPREFAB_FOLDER_NAME_IN_RESOURCES}/video";

        public override void Setup(MediaHolder holder, Experience associatedExperience, MediaFile media)
        {
            //Let the rest of the setup happen
            base.Setup(holder, associatedExperience, media);
            VideoMediaHolder = (VideoMediaHolder) holder;

            //Setup the video object
            VideoPlayerRef.targetTexture = new RenderTexture(TemplateRenderTextureRef);
            PanelControlHolderRef.localScale = Vector3.one * (isTracking ? 0.1f : 1);
            if (DisplayImageRef.material != null)
            {
                DisplayImageRef.material = new Material(DisplayImageRef.material);

                Sprite placeholderSprite = ArDemoConstants.GetImagePlaceholder();

                if (placeholderSprite != null)
                    DisplayImageRef.material.mainTexture = placeholderSprite.texture;
            }

            SetMediaRectPosition(RatioFitterRef.GetComponent<RectTransform>(), isTracking);

            if (GameObject.FindObjectOfType<AudioListener>() == null)
                GameObject.FindObjectOfType<Camera>().gameObject.AddComponent<AudioListener>();
        }

        public override void ChangeDisplayState(AppearitionArHandler.TargetState state)
        {
            base.ChangeDisplayState(state);

            if (state.IsTracking())
            {
                //Check if the video has been loaded or not.
                if (LoadingState == MediaLoadingState.LoadingSuccessful)
                    //Launch the video
                    VideoPlayerRef.Play();

                //Disable the display either way
                PanelControlHolderRef.gameObject.SetActive(false);
            }
            else
            {
                //If disabling the object, stop the video player.
                VideoPlayerRef.Stop();
            }
        }

        public override IEnumerator DownloadAndLoadMedia(Action<BaseMedia, MediaLoadingState> onComplete)
        {
            LoadingState = MediaLoadingState.Loading;
            yield return ArTargetHandler.LoadMediaFileContentProcess(ExperienceRef.Data, this, OnVideoDownloaded);
            onComplete?.Invoke(this, LoadingState);
        }

        /// <summary>
        /// Occurs whenever the video has finished downloading. Just plug the url in the video player, and play if object active.
        /// </summary>
        /// <param name="obj"></param>
        private void OnVideoDownloaded(Dictionary<string, byte[]> obj)
        {
            if (ExperienceRef == null || ExperienceRef.Data == null)
                return;

            if (obj != null && obj.Count > 0)
            {
                try
                {
                    VideoPlayerRef.url = ArTargetHandler.GetPathToMedia(ExperienceRef.Data, this);
                } catch (MissingReferenceException)
                {
                    return;
                }

                //Handle the new image's content and ratio
                DisplayImageRef.texture = VideoPlayerRef.targetTexture;

                //Handle scaling once the video is prepared
                VideoPlayerRef.prepareCompleted += RescaleVideoForGrid;


                //If the object is currently visible, start playing (if autoplay)
                if (isAutoPlay && HolderRef.gameObject.activeInHierarchy)
                    ChangeVideoPlayState(true);

                if (OnVideoLoaded != null)
                    OnVideoLoaded(arMediaId.ToString());

                ChangeVideoPlayState(true);
                LoadingState = MediaLoadingState.LoadingSuccessful;
            }
            else
            {
                //Display error while loading video
                AppearitionLogger.LogError(string.Format("Error while loading the video of id {0} and of filename {1}", arMediaId, fileName));
                LoadingState = MediaLoadingState.LoadingFailed;
            }
        }

        public void ChangeVideoPlayState(bool shouldBePlaying)
        {
            if (LoadingState != MediaLoadingState.LoadingSuccessful)
                return;

            if (shouldBePlaying)
            {
                _isVideoReadyToPlay = true;
                VideoPlayerRef.Play();
            }
            else
            {
                VideoPlayerRef.Pause();
            }

            PanelControlHolderRef.gameObject.SetActive(!shouldBePlaying);
        }

        /// <summary>
        /// Called once the video is prepared. Uses the video's data to rescale based on the grid.
        /// Only applies to tracking.
        /// </summary>
        /// <param name="source">Source.</param>
        void RescaleVideoForGrid(VideoPlayer source)
        {
            if (!isTracking || _isVideoReadyToPlay)
                return;

            float refWidth = source.texture.width;
            float refHeight = source.texture.height;

            RatioFitterRef.aspectRatio = (float) refWidth / (float) refHeight;

            _isVideoReadyToPlay = true;
        }

        void OnDisable()
        {
            VideoPlayerRef.Stop();
        }
    }
}