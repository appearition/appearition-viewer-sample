using Appearition.ArTargetImageAndMedia;

namespace Appearition.ArDemo
{
    /// <summary>
    /// Asset Extensions used mainly for the ArDemo.
    /// </summary>
    public static class ArDemoAssetExtensions
    {
        public static bool IsMarker(this Asset asset)
        {
            if (asset == null)
                return false;

            if (asset.ContainsTag(ArDemoConstants.TAG_MARKER))
                return true;

            if (asset.tags == null || (!asset.IsMarkerless() && asset.targetImages != null && asset.targetImages.Count > 0))
                return true;
            return false;
        }

        public static bool IsMarkerless(this Asset asset)
        {
            if (asset == null)
                return false;
            if (asset.ContainsTag(ArDemoConstants.TAG_MARKERLESS))
                return true;
            if (asset.targetImages == null || asset.targetImages.Count == 0)
                return true;
            return false;
        }

        public static bool IsInteractive(this Asset asset)
        {
            return asset != null && asset.ContainsTag(ArDemoConstants.TAG_INTERACTIVE);
        }
    }
}