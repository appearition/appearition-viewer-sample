using System;
using System.Collections;
using System.Collections.Generic;
using Appearition.ArTargetImageAndMedia;
using UnityEngine;

namespace Appearition.ArDemo
{
    public class MiscDataIntegrationMedia : BaseDataIntegrationMedia
    {
        public MiscDataIntegrationMedia(MediaFile cc) : base(cc)
        {
        }
        
        public override float GenerateMediaAssociationScore()
        {
            //Debug.LogError()
            return isDataQuery ? 1.0f : mediaType.Equals("label", StringComparison.InvariantCultureIgnoreCase) ? 1.0f : 0f;
        }

        public override IEnumerator DownloadAndLoadMedia(Action<BaseMedia, MediaLoadingState> onComplete)
        {
            onComplete?.Invoke(this, MediaLoadingState.LoadingSuccessful);
            yield break;
        }
    }
}