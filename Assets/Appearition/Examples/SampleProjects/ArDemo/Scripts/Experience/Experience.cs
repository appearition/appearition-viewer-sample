using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Appearition;
using Appearition.ArDemo;
using Appearition.ArTargetImageAndMedia;
using Appearition.Common.ListExtensions;
using UnityEngine;

public class Experience : MonoBehaviour
{
    //References
    public BaseArProviderHandler ProviderRef { get; protected set; }
    
    /// <summary>
    /// ApiData of this experience.
    /// </summary>
    public Asset Data { get; protected set; }

    /// <summary>
    /// Contains all the media objects attached to this experience.
    /// </summary>
    public List<BaseMedia> Medias { get; protected set; }

    public Transform MediaContentHolder { get; protected set; }

    //Internal Variables
    public bool HasLoadingBegun { get; protected set; }
    public float OverallLoadingProgress { get; private set; }
    public bool AreMediaLoaded { get; protected set; }

    public Vector3 BackupSetupPosition { get; protected set; }
    protected virtual Vector3 PositionOffset { get; set; } = Vector3.zero;
    protected virtual Quaternion RotationOffset { get; set; } = Quaternion.identity;
    protected virtual Vector3 ScaleOffset  { get; set; } =Vector3.one;

    #region Setup

    /// <summary>
    /// Setup a single experience and store the new data. To view the content, start the LoadContent coroutine.
    /// </summary>
    /// <param name="newData"></param>
    /// <param name="provider"></param>
    /// <param name="position"></param>
    public virtual void SetupExperience(Asset newData, BaseArProviderHandler provider, Vector3? position = default)
    {
        //Create containers
        Data = newData;
        ProviderRef = provider;
        Medias = new List<BaseMedia>();

        try
        {
            if (newData == null || transform == null)
                return;
        } catch
        {
            return;
        }

        //Before handling any media, create the container
        MediaContentHolder = new GameObject("Media Content Holder").transform;
        MediaContentHolder.SetParent(transform);

        MediaContentHolder.localPosition = BackupSetupPosition = position.GetValueOrDefault();
        MediaContentHolder.localRotation = Quaternion.identity;
        MediaContentHolder.localScale = Vector3.one * (provider == null ? 1.0f : provider.ScaleMultiplier);

        //Create media items
        if (Data.mediaFiles != null)
        {
            for (int i = 0; i < Data.mediaFiles.Count; i++)
            {
                if (Data.mediaFiles[i] != null)
                    CreateSingleMediaContent(Data.mediaFiles[i]);
            }
            
            ChangeMediaDisplayState(AppearitionArHandler.TargetState.None);
        }
    }

    #endregion

    #region Content Loading

    /// <summary>
    /// Load the content of this current experience.
    /// </summary>
    /// <returns></returns>
    public virtual IEnumerator LoadContent()
    {
        if (HasLoadingBegun)
            yield break;

        HasLoadingBegun = true;

        for (int i = 0; i < Medias.Count; i++)
        {
            if (Medias[i].LoadingState == BaseMedia.MediaLoadingState.NotLoaded)
                yield return Medias[i].DownloadAndLoadMedia(OnMediaLoaded);
        }
    }

    public IEnumerator RefreshExperienceContent()
    {
        var query = ArTargetConstant.GetDefaultArTargetListQuery();
        query.AssetId = Data.assetId;

        ArTarget updatedAsset = null;
        yield return ArTargetHandler.GetSpecificArTargetByQueryProcess(query, success => updatedAsset = success.FirstOrDefault());

        if (updatedAsset == null)
            AppearitionLogger.LogWarning($"Tried to update the experience of name {Data.name} and id {Data.assetId} but no matching record was found.");
        else
            yield return UpdateExperienceContent(updatedAsset);
    }
    
    public IEnumerator UpdateExperienceContent(Asset newData)
    {
        var oldData = new Asset(Data);
        Data.CopyValuesFrom(newData);
        yield return UpdateExperienceContent(oldData.mediaFiles, Data.mediaFiles, true);
    }

    public IEnumerator UpdateExperienceContent(List<MediaFile> newMedias, bool loadContent)
    {
        yield return UpdateExperienceContent(Medias.Cast<MediaFile>().ToList(), newMedias, loadContent);
    }

    public IEnumerator UpdateExperienceContent(List<MediaFile> oldMedias, List<MediaFile> newMedias, bool loadContent)
    {
        if(newMedias == null)
            newMedias = new List<MediaFile>();
        
        //If the media has a different modified date, it's safe to assume the one from the EMS is more recent.
        var manifest = newMedias.HiziUpdateListWithManifest(oldMedias, (x, y) => x.arMediaId == y.arMediaId, (x, y) => x.lastModified.Equals(y.lastModified) ? 0 : 1);

        Debug.LogError(AppearitionConstants.SerializeJson(manifest));

        for (int i = 0; i < manifest.itemsUpdated.Count; i++)
            Debug.Log($"Old: {AppearitionConstants.SerializeJson(manifest.itemsUpdated[i][0])} - New: {AppearitionConstants.SerializeJson(manifest.itemsUpdated[i][1])}");

        //Apply the changes to the scene
        for (int i = 0; i < manifest.itemsUpdated.Count; i++)
        {
            BaseMedia associatedMedia = Medias.FirstOrDefault(o => o.arMediaId == manifest.itemsUpdated[i][1].arMediaId);

            //If the Media disappeared, consider it as if it was added
            if (associatedMedia == null)
            {
                manifest.itemsAdded.Add(manifest.itemsUpdated[i][1]);
                continue;
            }

            associatedMedia.UpdateContent(manifest.itemsUpdated[i][1]);
        }

        for (int i = 0; i < manifest.itemsAdded.Count; i++)
            CreateSingleMediaContent(manifest.itemsAdded[i]);

        for (int i = 0; i < manifest.itemsRemoved.Count; i++)
        {
            int index = Medias.FindIndex(o => o.arMediaId == manifest.itemsRemoved[i].arMediaId);

            if (index >= 0)
            {
                Medias[index].Dispose();
                if (Medias[index].HolderRef != null && Medias[index].HolderRef.gameObject != null)
                    Destroy(Medias[index].HolderRef.gameObject);
                Medias.RemoveAt(index);
            }
        }

        if (loadContent)
        {
            for (int i = 0; i < Medias.Count; i++)
            {
                if (Medias[i].LoadingState == BaseMedia.MediaLoadingState.NotLoaded)
                    yield return Medias[i].DownloadAndLoadMedia(OnMediaLoaded);
            }
        }
    }

    protected virtual void OnMediaLoaded(BaseMedia media, BaseMedia.MediaLoadingState state)
    { 
        //Nothing?
    }

    protected virtual BaseMedia CreateSingleMediaContent(MediaFile media)
    {
        BaseMedia tmpMedia = TryToCreateArMediaFromMediaType(media);

        if (tmpMedia == null)
        {
            AppearitionLogger.LogWarning($"{media.mediaType} was not recognized and was skipped.");
            return null;
        }
        
        MediaHolder tmpArMediaGameObject = null;

        //Try to load the prefab, if any
        if (!string.IsNullOrEmpty(tmpMedia.PathToPrefab))
        {
            GameObject tmpMediaPrefab = (GameObject) Resources.Load(tmpMedia.PathToPrefab);

            if (tmpMediaPrefab != null)
            {
                var instantiatedObject = Instantiate(tmpMediaPrefab);
                instantiatedObject.name = GetMediaNameBasedOnMediaFile(media);
                tmpArMediaGameObject = instantiatedObject.GetComponent<MediaHolder>();

                //No MediaHolder on the prefab? Discard.
                if (tmpArMediaGameObject == null)
                {
                    AppearitionLogger.LogWarning($"A prefab at path {tmpMedia.PathToPrefab} was found but no MediaHolder component is located on the root.");
                    Destroy(instantiatedObject);
                }
            }
            else
            {
                AppearitionLogger.LogWarning(
                    $"Prefab located at {tmpMedia.PathToPrefab} was not found in the resource folder. Media info:\n{AppearitionConstants.SerializeJson(media)}");
            }
        }

        //If no prefab was found, create a blank GameObject with the MediaHolder on it.
        if (tmpArMediaGameObject == null)
            tmpArMediaGameObject = new GameObject(GetMediaNameBasedOnMediaFile(media)).AddComponent<MediaHolder>();

        //Attach it to this object.
        tmpArMediaGameObject.transform.SetParent(MediaContentHolder);
        tmpArMediaGameObject.transform.localPosition = PositionOffset;
        tmpArMediaGameObject.transform.localRotation = RotationOffset;
        tmpArMediaGameObject.transform.localScale = ScaleOffset; // * 0.8f; // * 400; //WORLD SIZE;

        Medias.Add(tmpMedia);
        tmpArMediaGameObject.Setup(this, tmpMedia);

        return tmpMedia;
    }

    void Update()
    {
        if (HasLoadingBegun && !AreMediaLoaded && Medias != null && Medias.Count != 0)
        {
            float progressSum = 0;
            for (int i = 0; i < Medias.Count; i++)
                progressSum += Medias[i].LoadingProgress;

            OverallLoadingProgress = progressSum / Medias.Count;

            if (Mathf.Approximately(OverallLoadingProgress, 1.0f))
                AreMediaLoaded = true;
        }
    }

    /// <summary>
    /// Unloads and disposes of all the loaded media content.
    /// </summary>
    public virtual void UnloadContent()
    {
        //Those will dispose themselves.
        for (int i = Medias.Count - 1; i >= 0; i--)
        {
            if (Medias[i] != null)
            {
                Medias[i].Dispose();
                if(Medias[i].HolderRef != null && Medias[i].HolderRef.gameObject != null)
                    Destroy(Medias[i].HolderRef.gameObject);
            }
        }
        
        if(MediaContentHolder != null)
            Destroy(MediaContentHolder.gameObject);
        
        Medias.Clear();
        AreMediaLoaded = false;
    }

    /// <summary>
    /// Finds the most appropriate ArMedia type, and creates an instance of it.
    /// </summary>
    /// <param name="media"></param>
    /// <returns></returns>
    BaseMedia TryToCreateArMediaFromMediaType(MediaFile media)
    {
        //Punch the assembly and get the type regardless of its namespace.
        return (from tmpType in Assembly.GetExecutingAssembly().GetTypes()
            where tmpType.IsSubclassOf(typeof(BaseMedia))
            from tmpConstructor in tmpType.GetConstructors()
            where tmpConstructor.GetParameters().Length == 1 && typeof(MediaFile).IsAssignableFrom(tmpConstructor.GetParameters()[0].ParameterType)
            let tmpArMedia = Activator.CreateInstance(tmpType, media)
            where ((BaseMedia) tmpArMedia).GenerateMediaAssociationScore() > 0
            select tmpArMedia).Cast<BaseMedia>().OrderByDescending(o => o.GenerateMediaAssociationScore()).FirstOrDefault();
    }

    /// <summary>
    /// Uses a common naming system for how a GameObject's name should be called using a MediaFile's ApiData.
    /// </summary>
    /// <param name="mediaFile"></param>
    /// <returns></returns>
    string GetMediaNameBasedOnMediaFile(MediaFile mediaFile)
    {
        return string.Format("Media Id: {0}, Type: {1}", mediaFile.arMediaId, mediaFile.mediaType);
    }

    #endregion

    protected virtual void ChangeMediaDisplayState(AppearitionArHandler.TargetState trackingState)
    {
        if (Medias != null)
        {
            for (int i = 0; i < Medias.Count; i++)
                Medias[i].ChangeDisplayState(trackingState);
        }
    }

    protected virtual void OnDestroy()
    {
        Resources.UnloadUnusedAssets();
    }
}