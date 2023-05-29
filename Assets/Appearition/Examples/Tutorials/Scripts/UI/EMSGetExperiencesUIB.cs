// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: "EMSGetExperiencesUIB.cs" 
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Appearition.ArTargetImageAndMedia;

namespace Appearition.Example
{
    /// <summary>
    /// Displays and utilizes the experiences coming down from the EMS in a simple way. Works hand-in-hand with the EMSGetExperiencesB, which gets the experiences.
    /// </summary>
    [RequireComponent(typeof(EMSGetExperiencesB))]
    public class EMSGetExperiencesUIB : BaseEMSUITab
    {
        //References
        public RectTransform allExperiencesContainer;
        public GameObject singleExperiencePrefab;
        public Button getAllExperiencesBT;
        public Text previewNameText;
        public Text previewMediaTypeText;
        public Text previewIsPreDownloadText;
        public Text previewTextText;
        public Image previewTargetImage;
        List<Linker_SingleGetExperiencesEntry> allLoadedExperiences = new List<Linker_SingleGetExperiencesEntry>();

        //Internal Variables
        private string _nameDefaultText = "Name: ";
        private string _mediaTypeDefaultText = "Media Type: ";
        private string _isPreDownloadDefaultText = "Is Pre-Download: ";
        private string _textDefaultText = "Text: ";

        //Handy properties
        private EMSGetExperiencesB _getExperiencesB;

        /// <summary>
        /// Reference to the object in charge of talking with the EMS.
        /// </summary>
        /// <value>The get experiences b.</value>
        private EMSGetExperiencesB GetExperiencesB
        {
            get
            {
                if (_getExperiencesB == null)
                    _getExperiencesB = GetComponent<EMSGetExperiencesB>();
                return _getExperiencesB;
            }
        }

        void Start()
        {
            //Backup Preview
            if (previewNameText != null)
                _nameDefaultText = previewNameText.text;
            if (previewMediaTypeText != null)
                _mediaTypeDefaultText = previewMediaTypeText.text;
            if (previewIsPreDownloadText != null)
                _isPreDownloadDefaultText = previewIsPreDownloadText.text;
            if (previewTextText != null)
                _textDefaultText = previewTextText.text;
        }


        /// <summary>
        /// Pressed whenever requesting to get all the experiences from the stored channel. Fetches and display them!
        /// </summary>
        public void OnGetAllExperiencesButtonPressed()
        {
            if (singleExperiencePrefab == null || allExperiencesContainer == null)
                return;

            if (getAllExperiencesBT != null)
                getAllExperiencesBT.interactable = false;

            GetExperiencesB.GetAllExperiences(outcome =>
            {
                if (outcome != null)
                {
                    //Firstly, clean the display.
                    ClearPreview();
                    ClearAllExperiences();

                    //Then, create the new preview !
                    for (int i = 0; i < outcome.Count; i++)
                    {
                        Linker_SingleGetExperiencesEntry tmp = Instantiate(singleExperiencePrefab, allExperiencesContainer).GetComponent<Linker_SingleGetExperiencesEntry>();
                        tmp.Setup(this, outcome[i]);
                        allLoadedExperiences.Add(tmp);
                    }

                    //Allow to click again.
                    if (getAllExperiencesBT != null)
                        getAllExperiencesBT.interactable = true;
                }
            });
        }

        /// <summary>
        /// Occurs whenever an experience button is pressed. Display the experience on the preview.
        /// </summary>
        /// <param name="entry">Entry.</param>
        public void OnExperienceButtonPressed(Linker_SingleGetExperiencesEntry entry)
        {
            //Display the given asset in the preview!
            previewNameText.text = _nameDefaultText + entry.storedAsset.name;

            MediaFile tmpMainMediafile = entry.storedAsset.mediaFiles.FirstOrDefault();
            if (tmpMainMediafile == null)
            {
                if (previewMediaTypeText != null)
                    previewMediaTypeText.text = "N/A";
                if (previewIsPreDownloadText != null)
                    previewIsPreDownloadText.text = "N/A";
                if (previewTextText != null)
                    previewTextText.text = "N/A";
            }
            else
            {
                if (previewMediaTypeText != null)
                    previewMediaTypeText.text = _mediaTypeDefaultText + tmpMainMediafile.mediaType;
                if (previewIsPreDownloadText != null)
                    previewIsPreDownloadText.text = _isPreDownloadDefaultText + tmpMainMediafile.isPreDownload.ToString();
                if (previewTextText != null)
                    previewTextText.text = _textDefaultText + tmpMainMediafile.text;
            }

            if (previewTargetImage != null && entry.mainBT != null && entry.mainBT.image != null)
                previewTargetImage.sprite = entry.mainBT.image.sprite;
        }

        #region Utilties

        /// <summary>
        /// Sets default values to the preview display.
        /// </summary>
        public void ClearPreview()
        {
            if (previewNameText != null)
                previewNameText.text = _nameDefaultText;
            if (previewMediaTypeText != null)
                previewMediaTypeText.text = _mediaTypeDefaultText;
            if (previewIsPreDownloadText != null)
                previewIsPreDownloadText.text = _isPreDownloadDefaultText;
            if (previewTextText != null)
                previewTextText.text = _textDefaultText;
            if (previewTargetImage != null)
                previewTargetImage.sprite = null;
        }


        /// <summary>
        /// Destroys and clears all the loaded experiences in the display panel.
        /// </summary>
        public void ClearAllExperiences()
        {
            for (int i = allLoadedExperiences.Count - 1; i >= 0; i--)
                Destroy(allLoadedExperiences[i].gameObject);
            allLoadedExperiences.Clear();
        }

        #endregion
    }
}