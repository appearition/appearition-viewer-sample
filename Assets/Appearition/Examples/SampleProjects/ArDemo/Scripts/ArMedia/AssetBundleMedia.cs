#pragma warning disable 0162
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Appearition.ArTargetImageAndMedia;
using Appearition.Common.ListExtensions;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Appearition.ArDemo
{
    /// <summary>
    /// Base AssetBundle ArMedia. For any different implementation of AssetBundle content,
    /// feel free to create a class which inherit from it, and override LoadAssetBundle to handle the logic of how it should be loaded.
    /// Additionally, in order for the ArExperience to choose your child class over this one, remember to override GenerateMediaAssociationScore with your own logic.
    /// </summary>
    public class AssetBundleMedia : BaseMedia
    {
        protected Dictionary<string, RuntimePlatform> MediaTypePerPlatform = new Dictionary<string, RuntimePlatform>(StringComparer.InvariantCultureIgnoreCase) {
            {"assetBundle_android", RuntimePlatform.Android},
            {"assetBundle_ios", RuntimePlatform.IPhonePlayer},
            {"assetBundle_windows", RuntimePlatform.WindowsPlayer},
            {"assetBundle_osx", RuntimePlatform.OSXPlayer},
            {"assetBundle_webgl", RuntimePlatform.WebGLPlayer},
        };

        #region AssetBundle References

        protected GameObject contentHolder;


        private AssetBundle _currentBundle;

        /// <summary>
        /// Reference and storage for the bundle associated to that mediafile.
        /// </summary>
        public AssetBundle CurrentBundle
        {
            get { return _currentBundle; }
            private set
            {
                if (value == null && _currentBundle != null)
                    _currentBundle.Unload(true);

                _currentBundle = value;
            }
        }

        #endregion

        public AssetBundleMedia(MediaFile cc) : base(cc)
        {
        }

        /// <summary>
        /// Whether or not the current build platform is suitable for this assetbundle
        /// </summary>
        public virtual bool IsPlatformAllowedForAssetBundle
        {
            get
            {
                if (MediaTypePerPlatform == null)
                    return false;

                #if UNITY_EDITOR
                //If editor, check if this assetbundle is the first one in the list. Only load one at once.
                var types = MediaTypePerPlatform.Keys;
                for (int i = 0; i < ExperienceRef.Data.mediaFiles.Count; i++)
                {
                    if (types.Any(o => o.Equals(ExperienceRef.Data.mediaFiles[i].mediaType, StringComparison.InvariantCultureIgnoreCase)))
                        return ExperienceRef.Data.mediaFiles[i].arMediaId == arMediaId || arMediaId == 0;
                }

                return false;
                #endif

                //Otherwise, just find the one with the matching platform.
                var entry = MediaTypePerPlatform.FirstOrDefault(o => o.Key.Equals(mediaType, StringComparison.InvariantCultureIgnoreCase));
                return entry.Value == Application.platform;
            }
        }

        public override float GenerateMediaAssociationScore()
        {
            return MediaTypePerPlatform.ContainsKey(mediaType) ? 1.0f : 0;
        }

        public override IEnumerator DownloadAndLoadMedia(Action<BaseMedia, MediaLoadingState> onComplete)
        {
            if (!IsPlatformAllowedForAssetBundle)
            {
                LoadingState = MediaLoadingState.Unavailable;
                onComplete?.Invoke(this, LoadingState);
                yield break;
            }

            LoadingState = MediaLoadingState.Loading;

            Dictionary<string, byte[]> downloadData = new Dictionary<string, byte[]>();
            yield return ArTargetHandler.LoadMediaFileContentProcess(ExperienceRef.Data, this, download => downloadData = download);

            if (downloadData.Count == 0)
            {
                AppearitionLogger.LogError($"No content was found after downloading the AssetBundle of id {arMediaId}.");
                LoadingState = MediaLoadingState.LoadingFailed;
                onComplete?.Invoke(this, MediaLoadingState.LoadingFailed);
                yield break;
            }

            //Handle the loading logic
            LoadAssetBundle(downloadData);

            if (LoadingState == MediaLoadingState.Loading)
                LoadingState = MediaLoadingState.Unavailable;

            if(CurrentBundle != null)
                CurrentBundle.Unload(false);
            onComplete?.Invoke(this, LoadingState);
        }

        /// <summary>
        /// Logic of how the AssetBundle should be loaded. Override this script for different loading operations.
        /// </summary>
        /// <param name="downloadData"></param>
        protected virtual void LoadAssetBundle(Dictionary<string, byte[]> downloadData)
        {
            CurrentBundle = AssetBundle.LoadFromMemory(downloadData.First().Value);

            //Bundle not found? Most likely in the editor due to duplicate loading.
            if (CurrentBundle == null)
            {
                AppearitionLogger.LogError($"An error occurred when trying to load the assetBundle from the media of id {arMediaId}. " +
                                           "If your experience has several experiences for different platforms and you currently are running in editor, disregard this message.");

                LoadingState = MediaLoadingState.LoadingFailed;
                return;
            }
            
            GameObject bundleContentToSpawn = null;
            Object[] bundleContent = CurrentBundle.LoadAllAssets();

            if (bundleContent.Length > 0)
            {
                //Spawn the first item found
                for (int i = 0; i < bundleContent.Length; i++)
                {
                    if (bundleContent[i] == null)
                        continue;

                    try
                    {
                        bundleContentToSpawn = (GameObject) bundleContent[i];

                        if (bundleContentToSpawn != null)
                            break;
                    } catch
                    {
                        //Keep looping
                    }
                }
            }

            if (bundleContentToSpawn == null)
            {
                AppearitionLogger.LogWarning($"Unable to find content to instantiate in the AssetBundle of id {arMediaId}, and name + {fileName}");
                LoadingState = MediaLoadingState.LoadingFailed;
            }
            //If the experience is null, then the object was about to be created after the user decided to move on.. 
            else
            {
                contentHolder = GameObject.Instantiate(bundleContentToSpawn, HolderRef.transform);
                contentHolder.transform.localPosition = GetPosition;
                contentHolder.transform.localRotation = GetRotation;
                contentHolder.transform.localScale = GetScale * ArDemoConstants.ASSETBUNDLE_SCALE_MULTIPLIER;

                //Apply cameras
                Canvas[] allCanvases = contentHolder.GetComponentsInChildren<Canvas>();

                for (int i = 0; i < allCanvases.Length; i++)
                {
                    if (allCanvases[i].renderMode == RenderMode.WorldSpace)
                        allCanvases[i].worldCamera = AppearitionArHandler.ProviderHandler?.ProviderCamera;
                }


                Debug.Log($"Asset downloaded ! Name: {fileName}, Mediatype: {mediaType}");

                LoadingState = MediaLoadingState.LoadingSuccessful;
            }
        }

        public override void Dispose()
        {
            if (CurrentBundle != null)
                CurrentBundle.Unload(true);
        }
    }
}