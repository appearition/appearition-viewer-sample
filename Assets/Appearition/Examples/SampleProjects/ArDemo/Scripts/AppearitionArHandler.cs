using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Appearition.ArTargetImageAndMedia;

namespace Appearition.ArDemo
{
    /// <summary>
    /// Main handle which controls anything related to the AR implementation.
    /// It contains all events, states and settings related to the Ar implementation.
    /// </summary>
    public class AppearitionArHandler : MonoBehaviour
    {
        #region Enums

        /// <summary>
        /// Contains the different possible states for an Target being tracked.
        /// </summary>
        [Flags]
        public enum TargetState
        {
            None = 0,
            TargetFound = 1,
            TargetExtendedTracking = 2,
            TargetLost = 4,
            Unknown = 8,

            Tracking = TargetFound | TargetExtendedTracking,
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs whenever both Vuforia and the AppearitionArHandler are done initializing, and ready to do some AR.
        /// </summary>
        public static event Action OnInitializationComplete;

        public delegate void ProviderStateChanged(BaseArProviderHandler provider, bool isActive);

        /// <summary>
        /// Occurs when the current ProviderHandler has been changed.
        /// </summary>
        public static event ProviderStateChanged OnProviderStateChanged;

        public delegate void ScanStateChanged(bool isScanning);

        /// <summary>
        /// Called anytime the scan value has been changed.
        /// </summary>
        public static event ScanStateChanged OnScanStateChanged;


        public delegate void TargetStateChanged(ArExperience arAsset, TargetState newState);

        /// <summary>
        /// Event triggered whenever the given target has been 
        /// </summary>
        public static event TargetStateChanged OnTargetStateChanged;

        #endregion

        #region Singleton

        static AppearitionArHandler _instance;

        /// <summary>
        /// Singleton of the Appearition ArHandler.
        /// </summary>
        static AppearitionArHandler Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<AppearitionArHandler>();

                return _instance;
            }
        }

        #endregion

        //References
        private static BaseArProviderHandler _providerHandler;

        /// <summary>
        /// The currently active ArProvider.
        /// </summary>
        public static BaseArProviderHandler ProviderHandler
        {
            get => _providerHandler;
            protected set
            {
                var previousValue = _providerHandler;
                _providerHandler = value;
                if (_providerHandler == null && previousValue != null)
                    AppearitionLogger.LogDebug($"Provider \'{previousValue.GetType().Name}\' has been unloaded.");
                else if (_providerHandler != null)
                    AppearitionLogger.LogDebug($"Provider \'{_providerHandler.GetType().Name}\' has been loaded");
            }
        }


        //References
        [SerializeField] List<BaseArProviderHandler> _arProviders = new List<BaseArProviderHandler>();
        public static List<BaseArProviderHandler> RegisteredArProviders => Instance._arProviders;

        //Settings Variables
        public bool refreshOfflineLibraryDuringInitialization = false;

        public static bool IsCurrentProviderOffline => (ProviderHandler as IOfflineMarkerArProviderHandler != null) || (ProviderHandler as IOfflineMarkerlessArProviderHandler != null);
        public static bool IsCurrentProviderMarkerless => ProviderHandler as IOfflineMarkerlessArProviderHandler != null;
        public static bool IsAnyProviderActive => ProviderHandler != null;
        public static bool IsProviderScanning => ProviderHandler != null && ProviderHandler.IsScanning;

        /// <summary>
        /// Whether or not the Appearition Ar Handler's initialization is complete.
        /// </summary>
        public static bool IsInitialized { get; protected set; }

        IEnumerator Start()
        {
            for (int i = 0; i < _arProviders.Count; i++)
                yield return _arProviders[i].SetupLicense();

            if (refreshOfflineLibraryDuringInitialization)
                yield return RefreshOfflineExperienceLibraryProcess(_arProviders.Any(o => o is IOfflineMarkerArProviderHandler), true);

            IsInitialized = true;

            OnInitializationComplete?.Invoke();
        }

        #region Experience Handling

        /// <summary>
        /// Refreshes the local experience library. The JSON will be stored locally for offline purpose, but you can choose to download Target Images and Medias optionally.
        /// By default, all pre-downloads will be downloaded.
        /// If any Offline Marker experiences are attached, will ask them to update their trackable collection.
        /// </summary>
        /// <param name="downloadTargetImages"></param>
        /// <param name="downloadMedias"></param>
        /// <param name="onComplete"></param>
        /// <returns></returns>
        public static IEnumerator RefreshOfflineExperienceLibraryProcess(bool downloadTargetImages, bool downloadMedias, Action onComplete = null)
        {
            bool isSuccess = true;
            List<Asset> assets = new List<Asset>();
            List<ArTarget> arTargets = new List<ArTarget>();

            yield return ArTargetHandler.GetChannelExperiencesProcess(downloadTargetImages, downloadMedias, true,
                success => assets.AddRange(success), null, complete => isSuccess = complete);

            yield return ArTargetHandler.GetChannelArTargetsProcess(downloadTargetImages, downloadMedias,
                success => arTargets.AddRange(success), null, complete => isSuccess = isSuccess && complete);

            //Add non-duplicates.
            for (int i = 0; i < arTargets.Count; i++)
            {
                if (assets.All(o => !o.assetId.Equals(arTargets[i].assetId)))
                    assets.Add(arTargets[i]);
            }

            if (downloadTargetImages && isSuccess)
            {
                for (int i = 0; i < Instance._arProviders.Count; i++)
                {
                    if (Instance._arProviders[i] is IOfflineMarkerArProviderHandler markerProvider)
                        markerProvider.UpdateTrackableAssetCollection(assets);
                }
            }

            onComplete?.Invoke();
        }

        /// <summary>
        /// Initializes a single target using the Asset container of said target.
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="asset"></param>
        /// <param name="loadExperience"></param>
        /// <param name="onComplete"></param>
        /// <param name="position">Includes a position offset where you want the experience to be placed</param>
        public static T CreateExperienceFromAsset<T>(BaseArProviderHandler provider, Asset asset, bool loadExperience, Action<bool> onComplete = null, Vector3? position = default) where T : Experience
        {
            if (asset == null)
            {
                AppearitionLogger.LogWarning("No asset provided.");
                onComplete?.Invoke(false);
                return null;
            }

            var tmpExperience = new GameObject($"Experience Name: {asset.name}, Id: {asset.assetId}");

            return CreateExperienceFromAsset<T>(provider, asset, loadExperience, tmpExperience, onComplete, position);
        }

        /// <summary>
        /// Initializes a single target using the Asset container of said target.
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="asset"></param>
        /// <param name="loadExperience"></param>
        /// <param name="holder"></param>
        /// <param name="onComplete"></param>
        /// <param name="position">Includes a position offset where you want the experience to be placed</param>
        public static T CreateExperienceFromAsset<T>(BaseArProviderHandler provider, Asset asset, bool loadExperience, GameObject holder, Action<bool> onComplete = null, Vector3? position = default)
            where T : Experience
        {
            if (asset == null)
            {
                AppearitionLogger.LogWarning("No asset provided.");
                onComplete?.Invoke(false);
                return null;
            }

            var tmpExperience = holder.AddComponent<T>();
            tmpExperience.transform.SetParent(Instance.transform);

            if (asset.mediaFiles != null && asset.mediaFiles.Count > 0)
            {
                tmpExperience.SetupExperience(asset, provider, position);
                if (!loadExperience)
                    onComplete?.Invoke(true);
            }
            
            Instance.StartCoroutine(CreateExperienceFromAssetProcess<T>(provider, asset, tmpExperience, loadExperience, onComplete, position));

            return tmpExperience;
        }

        static IEnumerator CreateExperienceFromAssetProcess<T>(BaseArProviderHandler provider, Asset asset, T experience, bool loadExperience, Action<bool> onComplete = null,
            Vector3? position = default) where T : Experience
        {
            bool success = false;
            if (asset.mediaFiles == null || asset.mediaFiles.Count == 0)
                yield return GetMediafilesProcess(provider, experience, asset, getMediaSuccess => success = getMediaSuccess, position);
            else
                success = true;

            if (loadExperience)
                yield return experience.LoadContent();

            onComplete?.Invoke(success); // && experience.AreMediaLoaded);
        }

        static IEnumerator GetMediafilesProcess<T>(BaseArProviderHandler provider, T experience, Asset asset, Action<bool> onComplete, Vector3? position = default) where T : Experience
        {
            string assetId = asset.assetId;

            Asset newAsset = null;
            var assetQuery = ArTargetConstant.GetDefaultAssetListQuery();
            assetQuery.AssetId = asset.assetId;
            yield return ArTargetHandler.GetSpecificExperiencesByQueryProcess(0, assetQuery, false, true, false, success => newAsset = success.FirstOrDefault());


            if (newAsset == null)
            {
                var arTargetQuery = ArTargetConstant.GetDefaultArTargetListQuery();
                arTargetQuery.AssetId = assetId;
                yield return ArTargetHandler.GetSpecificArTargetByQueryProcess(0, arTargetQuery, false, true, success => newAsset = success.FirstOrDefault());
            }

            if (newAsset != null)
            {
                asset.CopyValuesFrom(newAsset);
                experience.SetupExperience(asset, provider, position);
            }

            onComplete?.Invoke(newAsset != null);
        }

        /// <summary>
        /// Retrieves the current tracking state of the first experience.
        /// </summary>
        public static TargetState GetExperienceTargetState()
        {
            return GetExperienceTargetState(null);
        }

        /// <summary>
        /// Retrieves the current tracking state of a given experience.
        /// </summary>
        /// <param name="experience"></param>
        public static TargetState GetExperienceTargetState(Experience experience)
        {
            if (ProviderHandler == null)
                return TargetState.None;

            return ProviderHandler.GetExperienceTargetState(experience);
        }

        #endregion

        #region Event Handling

        /// <summary>
        /// Should be called by the ArProvider whenever an experience has been found or lost. Handles the events.
        /// </summary>
        /// <param name="experienceTracked"></param>
        /// <param name="newState"></param>
        public static void ArProviderTargetStateChanged(ArExperience experienceTracked, TargetState newState)
        {
            if (Instance != null)
                OnTargetStateChanged?.Invoke(experienceTracked, newState);
        }

        #endregion

        #region Ar Provider Handling

        /// <summary>
        /// Activates the ArProvider of a given type. Make sure that the ArProvider is present in the list of ArProvider on the AppearitionArHandler's component.
        /// If an existing provider was already loaded, unloads it first.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static T ChangeArProvider<T>() where T : BaseArProviderHandler
        {
            return (T)ChangeArProvider(typeof(T));
            //if (Instance == null)
            //    return null;

            //if (ProviderHandler != null)
            //    UnloadArProvider();

            //var provider = Instance._arProviders.FirstOrDefault(o => o.GetType() == typeof(T));

            //if (provider != null)
            //{
            //    ProviderHandler = provider;
            //    if (ProviderHandler != null)
            //    {
            //        ProviderHandler.ChangeArProviderActiveState(true);
            //        ChangeArProviderScanningState(true);
            //        OnProviderStateChanged?.Invoke(provider, true);
            //        return (T) provider;
            //    }
            //}
            //else
            //    AppearitionLogger.LogError($"ArProvider of type {typeof(T).Name} was not found. Make sure it is present in the attached list of providers.");

            //return null;
        }

        /// <summary>
        /// Activates the ArProvider of a given type. Make sure that the ArProvider is present in the list of ArProvider on the AppearitionArHandler's component.
        /// If an existing provider was already loaded, unloads it first.
        /// <param name="providerType"></param>
        /// </summary>
        public static BaseArProviderHandler ChangeArProvider(Type providerType)
        {
            if (Instance == null)
                return null;

            if (ProviderHandler != null)
                UnloadArProvider();

            var provider = Instance._arProviders.FirstOrDefault(o => o.GetType() == providerType);

            if (provider != null)
            {
                ProviderHandler = provider;
                if (ProviderHandler != null)
                {
                    ProviderHandler.ChangeArProviderActiveState(true);
                    ChangeArProviderScanningState(true);
                    OnProviderStateChanged?.Invoke(provider, true);
                    return provider;
                }
            }
            else
                AppearitionLogger.LogError($"ArProvider of type {providerType.Name} was not found. Make sure it is present in the attached list of providers.");

            return null;
        }

        /// <summary>
        /// Changes the current provider to an Offline Markerless type, and passes an asset to set it up.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="asset"></param>
        public static void ChangeArProviderAndSelectAsset<T>(Asset asset) where T : BaseArProviderHandler, IOfflineMarkerlessArProviderHandler
        {
            ChangeArProviderAndSelectAsset(typeof(T), asset);

            //if (Instance == null)
            //    return;

            ////Firstly, change provider
            //IOfflineMarkerlessArProviderHandler provider = ChangeArProvider<T>();

            //if (provider != null)
            //{
            //    //Then, load asset
            //    Instance.StartCoroutine(provider.LoadSelectedExperience(asset));
            //}
        }

        /// <summary>
        /// Changes the current provider to an Offline Markerless type, and passes an asset to set it up.
        /// </summary>
        /// <param name="providerType"></param>
        /// <param name="asset"></param>
        public static void ChangeArProviderAndSelectAsset(Type providerType, Asset asset)
        {
            if (Instance == null || providerType == null)
                return;

            //Firstly, change provider
            try
            {
                IOfflineMarkerlessArProviderHandler provider = (IOfflineMarkerlessArProviderHandler) ChangeArProvider(providerType);

                if (provider != null)
                {
                    //Then, load asset
                    Instance.StartCoroutine(provider.LoadSelectedExperience(asset));
                }
            } catch (InvalidCastException e)
            {
                AppearitionLogger.LogError(
                    $"Unable to load the provider of type {providerType} because it doesn't inherit from BaseArProviderHandler and/or implements IOfflineMarkerlessArProviderHandler.\n{e}");
            }
        }

        /// <summary>
        /// If any provider is loaded, will unload them and remove all their content.
        /// </summary>
        public static void UnloadArProvider()
        {
            if (ProviderHandler != null)
            {
                var provider = ProviderHandler;
                ProviderHandler.ClearExperiencesBeingTracked();
                ProviderHandler.ChangeArProviderActiveState(false);
                OnProviderStateChanged?.Invoke(provider, false);

                if (provider is IOfflineMarkerArProviderHandler offlineProvider)
                    offlineProvider.ResetTrackableAssetContainer();
            }

            ProviderHandler = null;
        }

        /// <summary>
        /// Changes the scanning state of the current Ar Provider, if setup properly..
        /// </summary>
        /// <param name="desiredState"></param>
        public static void ChangeArProviderScanningState(bool desiredState)
        {
            if (ProviderHandler != null)
            {
                ProviderHandler.ChangeScanningState(desiredState);
                OnScanStateChanged?.Invoke(IsProviderScanning);
            }
        }

        /// <summary>
        /// If any target is currently being tracked, removes it and clears it. Additionally, can start scanning again.
        /// </summary>
        public static void ClearCurrentTargetBeingTracked(bool doScanAgain)
        {
            if (ProviderHandler != null)
            {
                ProviderHandler.ClearExperiencesBeingTracked();
                if (doScanAgain)
                    ChangeArProviderScanningState(true);
            }
        }

        /// <summary>
        /// If any target is currently being tracked, removes it and clears it. Additionally, can start scanning again.
        /// </summary>
        public static void ClearCurrentTargetBeingTracked(Asset assetToLoad, bool doScanAgain)
        {
            if (ProviderHandler != null)
            {
                ProviderHandler.ClearExperiencesBeingTracked();
                if (ProviderHandler is IOfflineMarkerlessArProviderHandler offlineProvider)
                    Instance.StartCoroutine(offlineProvider.LoadSelectedExperience(assetToLoad));

                if (doScanAgain)
                    ChangeArProviderScanningState(true);
            }
        }

        /// <summary>
        /// Resets the experience. In the case of Markerless providers, this might mean "reset the placement of the experience";
        /// while for Marker-based provides, this might mean "reset and look for a new target".
        /// </summary>
        public static void ResetTracking()
        {
            if (ProviderHandler != null)
                ProviderHandler.ResetTracking();
        }

        #endregion
    }
}