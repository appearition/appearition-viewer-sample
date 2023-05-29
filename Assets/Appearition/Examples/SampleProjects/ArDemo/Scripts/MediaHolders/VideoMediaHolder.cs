using System.Collections;
using System.Collections.Generic;
using Appearition.ArDemo;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace Appearition.ArDemo
{
    public class VideoMediaHolder : MediaHolder
    {
        public VideoPlayer videoPlayer;
        public RawImage displayImage;
        public RenderTexture templateRenderTexture;
        public RectTransform panelControlHolder;
        public AspectRatioFitter ratioFitter;

        public void OnPauseButtonPressed()
        {
            VideoMedia videoMedia = (VideoMedia) Media;
            if (videoMedia != null)
                videoMedia.ChangeVideoPlayState(false);
        }
        
        public void OnPlayButtonPressed()
        {
            VideoMedia videoMedia = (VideoMedia)Media;
            if (videoMedia != null)
                videoMedia.ChangeVideoPlayState(true);
        }
    }
}