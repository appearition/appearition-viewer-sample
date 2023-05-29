// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: "Linker_SingleMediaManagementEntry.cs" 
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;
using Appearition.ArTargetImageAndMedia;
using Appearition.ChannelManagement;
using Appearition.Common;

namespace Appearition.Example
{
    public class Linker_SingleMediaManagementEntry : MonoBehaviour
    {
        //References
        /// <summary>
        /// Reference to the manager in charge of handling the experience management, for nicer communication.
        /// </summary>
        EMSManageExperiencesUIB managerRef;
        public RectTransform mainHolderRect;
        public InputField mediaNameIF;
        public Text mediaIdText;
        public Text showHideBTText;
        public Dropdown mediaTypeDropdown;
        public Text mimeTypeText;
        public InputField pathText;
        public Toggle deleteExistingToggle;
        public Toggle isPreDownloadToggle;
        public Toggle isAutoplayToggle;
        public Toggle isInteractiveToggle;
        public Toggle isTrackingToggle;
        public Linker_Vector3InputField positionVector3IF;
        public Linker_Vector3InputField rotationVector3IF;
        public Linker_Vector3InputField scaleVector3IF;
        public InputField resolutionIF;
        public InputField textIF;
        public InputField customIF;

        //Internal Variables
        List<MediaType> _mediaTypesAvailable = new List<MediaType>();
        /// <summary>
        /// Current media being modified.
        /// </summary>
        public MediaFile media;
        public string showHideBTShowText = "Show";
        public string showHideBTHideText = "Hide";

        //Retraction/Expansion variables
        public Vector2 retractedAndExpandedHeights = new Vector2(60, 500);
        float currentExpansionTarget = 0;
        float currentExpansionProgress = 0;
        float lerpSpeed = 8;

        #region Init

        public void Setup(EMSManageExperiencesUIB experienceManager, MediaFile tmpMedia, bool startExpanded = false)
        {
            //Store the manager's ref
            managerRef = experienceManager;
            media = tmpMedia;

            //Handle the show/hide thingy.
            currentExpansionProgress = currentExpansionTarget = (startExpanded ? 1 : 0);

            //Set default expansion state.
            if (mainHolderRect != null)
            {
                //Set default value
                mainHolderRect.sizeDelta = new Vector2(mainHolderRect.sizeDelta.x, Mathf.Lerp(retractedAndExpandedHeights.x, retractedAndExpandedHeights.y, currentExpansionProgress));
            }

            //Retardation test
            if (tmpMedia == null)
                return;

            //Set media content!
            if (mediaNameIF != null)
                mediaNameIF.text = tmpMedia.fileName;
            if (mediaIdText != null)
                mediaIdText.text = tmpMedia.arMediaId.ToString();

            //Mediatype
            StartCoroutine(SetupMediaTypesWhenReadyProcess(tmpMedia));
            
           if (pathText != null)
                pathText.text = "";
            if (deleteExistingToggle != null)
                deleteExistingToggle.isOn = false;
            if (pathText != null)
                pathText.interactable = (string.IsNullOrEmpty(media.url) || !media.url.Contains("http"));
            if (isPreDownloadToggle != null)
                isPreDownloadToggle.isOn = tmpMedia.isPreDownload;
            if (isAutoplayToggle != null)
                isAutoplayToggle.isOn = tmpMedia.isAutoPlay;
            if (isInteractiveToggle != null)
                isInteractiveToggle.isOn = tmpMedia.isInteractive;
            if (isTrackingToggle != null)
                isTrackingToggle.isOn = tmpMedia.isTracking;
            if (positionVector3IF != null)
                positionVector3IF.SetValue(tmpMedia.translationX, tmpMedia.translationY, tmpMedia.translationZ);
            if (rotationVector3IF != null)
                rotationVector3IF.SetValue(tmpMedia.rotationX, tmpMedia.rotationY, tmpMedia.rotationZ);
            if (scaleVector3IF != null)
                scaleVector3IF.SetValue(tmpMedia.scaleX, tmpMedia.scaleY, tmpMedia.scaleZ);
            if (resolutionIF != null)
                resolutionIF.text = tmpMedia.resolution.ToString();
            if (textIF != null)
                textIF.text = tmpMedia.text;
            if (customIF != null)
                customIF.text = tmpMedia.custom;
        }

        IEnumerator SetupMediaTypesWhenReadyProcess(MediaFile tmpMedia)
        {
            _mediaTypesAvailable = new List<MediaType>();
            yield return ChannelHandler.GetMediaTypesFromEmsProcess(success => _mediaTypesAvailable.AddRange(success));

            if (mediaTypeDropdown != null)
            {
                mediaTypeDropdown.options.Add(new Dropdown.OptionData("None"));
                mediaTypeDropdown.AddOptions(MediaTypeUtility.GetAllMediaTypesByDisplayName(_mediaTypesAvailable));
                mediaTypeDropdown.value = Mathf.Clamp(_mediaTypesAvailable.FindIndex(o => o.Name == media.mediaType) + 1, 0, mediaTypeDropdown.options.Count);
            }

            if (mimeTypeText != null)
                SetMimetypeText(!string.IsNullOrEmpty(media.mediaType) ? media.mediaType : _mediaTypesAvailable.First().Name, tmpMedia.url);

        }

        #endregion

        #region Expand Retract

        public void OnShowHideButtonPressed()
        {
            //Flip it!
            currentExpansionTarget = Mathf.Abs(1 - currentExpansionTarget);
        }

        void Update()
        {
            if (mainHolderRect == null)
                return;

            //Change the retraction / expansion !
            if ((currentExpansionProgress < 1 && currentExpansionTarget >= 1) || (currentExpansionProgress > 0 && currentExpansionTarget <= 0))
            {
                //handle _progress !
                currentExpansionProgress = Mathf.Clamp01(Mathf.Lerp(currentExpansionProgress, currentExpansionTarget, Time.deltaTime * lerpSpeed));
                mainHolderRect.sizeDelta = new Vector2(mainHolderRect.sizeDelta.x, Mathf.Lerp(retractedAndExpandedHeights.x, retractedAndExpandedHeights.y, currentExpansionProgress));
            }

            if (mediaNameIF != null)
                mediaNameIF.interactable = Mathf.Abs(currentExpansionTarget - currentExpansionProgress) < 0.1f;
            if (showHideBTText != null)
                showHideBTText.text = (currentExpansionTarget > 0.5f ? showHideBTHideText : showHideBTShowText);
        }

        #endregion

        #region Events and Display

        public void OnMediatypeDropdownValueChanged(int index)
        {
            if (_mediaTypesAvailable.Count == 0)
                return;

            //Change the mimetype
            if (mediaTypeDropdown != null && mimeTypeText != null && mediaTypeDropdown.value > 0)
            {
                MediaType mediaType = MediaTypeUtility.FindMediaTypeFromDisplayName(mediaTypeDropdown.options[mediaTypeDropdown.value].text, _mediaTypesAvailable);
                SetMimetypeText(mediaType.Name, (pathText == null || pathText.text == null || pathText.text.Length == 0 ? media.url : pathText.text));
            }
            else
                SetMimetypeText("", (pathText == null || pathText.text == null || pathText.text.Length == 0 ? media.url : pathText.text));
        }


        /// <summary>
        /// Sets the text field of the mime type.
        /// Should be called anytime when changing the MediaType or when the path of the file has been changed.
        /// </summary>
        /// <param name="mediaTypeText">Media type text.</param>
        /// <param name="filePath">File path.</param>
        void SetMimetypeText(string mediaTypeText, string filePath)
        {
            if (_mediaTypesAvailable.Count == 0)
                return;
            
            string fileExtension = "";
            if (filePath != null && filePath.Length >= 0)
                fileExtension += Path.GetExtension(filePath);

//			Debug.Log (mediaTypeText + ", " + fileExtension + ", " + filePath);
            if (mimeTypeText != null)
            {
                string mimeType = "N/A";
                List<MediaType> tmpMediaTypes = MediaTypeUtility.FindMediaTypesFromExtension(fileExtension, _mediaTypesAvailable);
                MediaType tmpMediaType = tmpMediaTypes.FirstOrDefault(o => o.Name == mediaTypeText);
                if (tmpMediaType != null)
                {
                    mimeType = tmpMediaType.GetMimeTypeForGivenExtension(fileExtension);
                }

                mimeTypeText.text = mimeType;
            }
        }

        /// <summary>
        /// Occurs whenever the Delete UI button is pressed. Lets the manager handle it.
        /// </summary>
        public void OnDeleteButtonPressed()
        {
            if (managerRef != null && media != null)
                managerRef.OnDeleteMediaButtonPressed(media.arMediaId);
        }

        #endregion

        #region Export

        /// <summary>
        /// Applies all the modification to the media, and exports it.
        /// This will be called most likely when trying to update the Media's content with the EMS.
        /// </summary>
        /// <returns>The media file.</returns>
        public MediaFile ExportMediaFile()
        {
            if (_mediaTypesAvailable.Count == 0)
                return null;
            
            //Retardation test
            if (media == null)
                return null;

            if (mediaNameIF != null)
                media.fileName = mediaNameIF.text;

            MediaType tmp = null;
            if (mediaTypeDropdown != null && mediaTypeDropdown.options.Count > mediaTypeDropdown.value)
                tmp = MediaTypeUtility.FindMediaTypeFromDisplayName(mediaTypeDropdown.options[mediaTypeDropdown.value].text, _mediaTypesAvailable);

            if (mediaTypeDropdown != null && mediaTypeDropdown.value > 0)
            {
                if (tmp != null)
                    media.mediaType = tmp.Name;
            }

            //Flag for new file to upload
            if (pathText != null && pathText.text.Length > 0)
            {
                media.url = pathText.text;
                media.mimeType = MediaTypeUtility.FindMimeTypeFromExtension(tmp, Path.GetExtension(pathText.text));
            }

            if (deleteExistingToggle != null)
                deleteExistingToggle.isOn = false;
            if (isPreDownloadToggle != null)
                media.isPreDownload = isPreDownloadToggle.isOn;
            if (isAutoplayToggle != null)
                media.isAutoPlay = isAutoplayToggle.isOn;
            if (isInteractiveToggle != null)
                media.isInteractive = isInteractiveToggle.isOn;
            if (isTrackingToggle != null)
                media.isTracking = isTrackingToggle.isOn;
            if (positionVector3IF != null)
            {
                Vector3 tmpValue = positionVector3IF.GetValueVector3;
                media.translationX = tmpValue.x;
                media.translationY = tmpValue.y;
                media.translationZ = tmpValue.z;
            }

            if (rotationVector3IF != null)
            {
                Vector3 tmpValue = rotationVector3IF.GetValueVector3;
                media.rotationX = tmpValue.x;
                media.rotationY = tmpValue.y;
                media.rotationZ = tmpValue.z;
            }

            if (scaleVector3IF != null)
            {
                Vector3 tmpValue = scaleVector3IF.GetValueVector3;
                media.scaleX = tmpValue.x;
                media.scaleY = tmpValue.y;
                media.scaleZ = tmpValue.z;
            }

            if (resolutionIF != null)
                media.resolution = int.Parse(resolutionIF.text);
            if (textIF != null)
                media.text = textIF.text;
            if (customIF != null)
                media.custom = customIF.text;

            return media;
        }

        #endregion

        void OnDestroy()
        {
            ExportMediaFile();
        }
    }
}