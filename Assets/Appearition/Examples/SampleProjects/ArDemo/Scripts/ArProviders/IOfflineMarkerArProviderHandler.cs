using System.Collections;
using System.Collections.Generic;
using Appearition.ArTargetImageAndMedia;
using UnityEngine;

namespace Appearition.ArDemo
{
    /// <summary>
    /// Interface to be implemented by any non-cloud image recognition provider handler.
    /// </summary>
    public interface IOfflineMarkerArProviderHandler 
    {
        List<Asset> TrackableAssets { get; }
        
        /// <summary>
        /// Contains all the trackable assets. Any handler implementing this class should handle refreshing the local library.
        /// </summary>
        /// <param name="allTrackableAssets"></param>
        void UpdateTrackableAssetCollection(List<Asset> allTrackableAssets);
        
        /// <summary>
        /// Request to empty the TrackableAsset container, including scene references.
        /// </summary>
        void ResetTrackableAssetContainer();
    }
}