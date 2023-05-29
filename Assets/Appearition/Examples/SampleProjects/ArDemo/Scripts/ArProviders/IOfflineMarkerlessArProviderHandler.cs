using System.Collections;
using Appearition.ArTargetImageAndMedia;

namespace Appearition.ArDemo
{
    /// <summary>
    /// For ArProviders which do not need Target Images and instead just display the content in their own way.
    /// This include Markerless tracking ArProviders.
    /// </summary>
    public interface IOfflineMarkerlessArProviderHandler 
    {
        IEnumerator LoadSelectedExperience(Asset asset);
    }
}