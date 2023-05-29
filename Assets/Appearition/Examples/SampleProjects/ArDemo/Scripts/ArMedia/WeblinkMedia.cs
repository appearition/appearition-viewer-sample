#pragma warning disable 0414
using System;
using System.Collections;
using Appearition.ArTargetImageAndMedia;
using UnityEngine;

namespace Appearition.ArDemo
{
    public class WeblinkMedia : BaseMedia
    {
        //Weblinks are always successful so long they have a url.
        public override MediaLoadingState LoadingState => !string.IsNullOrEmpty(url) ? MediaLoadingState.LoadingSuccessful : MediaLoadingState.Unavailable;

        bool _isWaitingForCleansing = false;

        public WeblinkMedia(MediaFile cc) : base(cc)
        {
        }
        
        public override float GenerateMediaAssociationScore()
        {
            return mediaType.Equals("weblink", StringComparison.InvariantCultureIgnoreCase) ? 1.0f : 0f;
        }

        public override IEnumerator DownloadAndLoadMedia(Action<BaseMedia, MediaLoadingState> onComplete)
        {
            onComplete?.Invoke(this, MediaLoadingState.LoadingSuccessful);
            yield break;
        }

        public override void ChangeDisplayState(AppearitionArHandler.TargetState state)
        {
            //if (state.IsTracking() && !_isWaitingForCleansing)
            //{
            //    HolderRef.Invoke("Cleanse", 1.5f);
            //    _isWaitingForCleansing = true;
            //    Application.OpenURL(url);
            //}
        }

        void Cleanse()
        {
            Debug.LogError("THE CLEANSING IS HAPPENING");
            AppearitionArHandler.ClearCurrentTargetBeingTracked(true);
            _isWaitingForCleansing = false;
        }
    }
}