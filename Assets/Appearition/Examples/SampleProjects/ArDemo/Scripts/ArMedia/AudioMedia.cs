using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Appearition.ArTargetImageAndMedia;
using Appearition.Common;
using UnityEngine;
using UnityEngine.Networking;

namespace Appearition.ArDemo
{
    public class AudioMedia : BaseMedia
    {
        //References
        AudioSource _mediaAs;

        public AudioMedia(MediaFile cc) : base(cc)
        {
        }

        public override float GenerateMediaAssociationScore()
        {
            return mediaType.Equals("audio", StringComparison.InvariantCultureIgnoreCase) ? 1.0f : 0f;
        }

        public override void Setup(MediaHolder holder, Experience associatedExperience, MediaFile media)
        {
            base.Setup(holder, associatedExperience, media);

            if (GameObject.FindObjectOfType<AudioListener>() == null)
                GameObject.FindObjectOfType<Camera>().gameObject.AddComponent<AudioListener>();
        }

        public override IEnumerator DownloadAndLoadMedia(Action<BaseMedia, MediaLoadingState> onComplete)
        {
            LoadingState = MediaLoadingState.Loading;
            Dictionary<string, byte[]> downloadData = new Dictionary<string, byte[]>();
            yield return ArTargetHandler.LoadMediaFileContentProcess(ExperienceRef.Data, this, data => downloadData = data);

            if (downloadData == null || downloadData.Count == 0)
            {
                LoadingState = MediaLoadingState.LoadingFailed;
                onComplete?.Invoke(this, LoadingState);
                yield break;
            }

            string fileLocation = ArTargetHandler.GetPathToMedia(ExperienceRef.Data, this);

            if (!File.Exists(fileLocation))
                yield return ArTargetHandler.LoadMediaFileContentProcess(ExperienceRef.Data, this);

            AudioType audioType = AudioType.UNKNOWN;

            switch (Path.GetExtension(fileName))
            {
                case ".ogg":
                    audioType = AudioType.OGGVORBIS;
                    break;
                case ".wav":
                    audioType = AudioType.WAV;
                    break;
                default:
                    Debug.LogError("Audio format not implemented.");
                    LoadingState = MediaLoadingState.Unavailable;
                    onComplete?.Invoke(this, LoadingState);
                    yield break;
            }

            string fullLocation = fileLocation;

            if (Application.platform == RuntimePlatform.Android)
                fullLocation = string.Format("file:///{0}", fullLocation);
            else
                fullLocation = string.Format("file://{0}", fullLocation);


            var loadRequest = UnityWebRequestMultimedia.GetAudioClip(fullLocation, audioType);

            yield return loadRequest.SendWebRequest();

            AudioClip tmp = DownloadHandlerAudioClip.GetContent(loadRequest);

            if (tmp == null)
            {
                Debug.LogError($"An error occurred when trying to load the AudioClip of name {fileName}.");
                LoadingState = MediaLoadingState.LoadingFailed;
            }
            else
            {
                _mediaAs = HolderRef.gameObject.AddComponent<AudioSource>();
                _mediaAs.clip = tmp;
                _mediaAs.volume = 1.0f;
                _mediaAs.loop = true;
                _mediaAs.Play();
                LoadingState = MediaLoadingState.LoadingSuccessful;
            }
            onComplete?.Invoke(this, LoadingState);
        }

        public override void ChangeDisplayState(AppearitionArHandler.TargetState state)
        {
            //NEVER GOING AWAY, MUAAAAHAHAHAHAHHAHAHAHAHAHAHA
            HolderRef.gameObject.SetActive(true);
        }

        public override void Dispose()
        {
            if(_mediaAs != null && _mediaAs.clip != null)
                AudioClip.Destroy(_mediaAs.clip);
        }
    }
}