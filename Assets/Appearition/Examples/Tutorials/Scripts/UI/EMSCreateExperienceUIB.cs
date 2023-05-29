// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: "EMSCreateExperienceUIB.cs" 
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using Appearition.ChannelManagement;
using Appearition.Common;
using UnityEngine;
using UnityEngine.UI;

namespace Appearition.Example
{
    [RequireComponent(typeof(EMSCreateExperienceB))]
    public class EMSCreateExperienceUIB : BaseEMSUITab
    {
        //References
        public InputField targetNameIF;
        public InputField targetImageToUploadPathText;
        public InputField mediaToUploadPathText;
        public Dropdown mediaTypeDropdown;
        public Button uploadExperienceBT;
        public Text uploadExperienceBTText;
        public Text feedbackText;

        //Internal Variables
        public float delayBetweenPoints = 0.8f;
        
        //Handy Properties
        private EMSCreateExperienceB _createExperiencesB;

        private EMSCreateExperienceB CreateExperiencesB
        {
            get
            {
                if (_createExperiencesB == null)
                    _createExperiencesB = GetComponent<EMSCreateExperienceB>();
                return _createExperiencesB;
            }
        }

        protected override void Awake()
        {
            base.Awake();



            //Set the upload experience's state to disabled.
            RefreshIfUploadButtonIsInteractible();
        }

        IEnumerator Start()
        {
            List<MediaType> availableMediaTypes = new List<MediaType>();
            yield return ChannelHandler.GetMediaTypesFromEmsProcess(success => availableMediaTypes.AddRange(success));

            if (availableMediaTypes.Count == 0)
            {
                AppearitionLogger.LogError("No mediafile found in the selected channel.");
                yield break;
            }

            while (CreateExperiencesB == null || CreateExperiencesB.availableMediaTypes.Count == 0)
                yield return null;
            
            if (mediaTypeDropdown != null)
            {
                //Prepare the dropdown's content
                mediaTypeDropdown.options.Add(new Dropdown.OptionData("None"));
                mediaTypeDropdown.AddOptions(MediaTypeUtility.GetAllMediaTypesByDisplayName(CreateExperiencesB.availableMediaTypes));
                mediaTypeDropdown.value = 0;
            }
        }

        void LateUpdate()
        {
            RefreshIfUploadButtonIsInteractible();
        }

        /// <summary>
        /// Occurs wheenver the Upload Experience button is pressed. Handles the uploading of an experience, including creating the target image.
        /// </summary>
        public void OnUploadExperienceButtonPressed()
        {
            if (CreateExperiencesB == null || CreateExperiencesB.availableMediaTypes.Count == 0)
                return;
            
            //Fetch the variables
            string mediaType = "", targetName = "", mediaPath = "", targetPath = "";

            if (mediaTypeDropdown != null && mediaTypeDropdown.value > 0)
                mediaType = MediaTypeUtility.FindMediaTypeFromDisplayName(mediaTypeDropdown.options[mediaTypeDropdown.value].text, CreateExperiencesB.availableMediaTypes).Name;
            if (targetNameIF != null)
                targetName = targetNameIF.text;
            if (mediaToUploadPathText != null)
                mediaPath = mediaToUploadPathText.text;
            if (targetImageToUploadPathText != null)
                targetPath = targetImageToUploadPathText.text;

            //Let the EMSCreateExperienceB handle the rest.
            CreateExperiencesB.OnUploadExperienceButtonPressed(mediaType, targetName, targetPath, mediaPath);
        }

        /// <summary>
        /// Returns a dot animation object using the proper UI.
        /// </summary>
        /// <returns>The dot animation object.</returns>
        public DotAnimation GetDotAnimationObject()
        {
            return new DotAnimation(uploadExperienceBT, uploadExperienceBTText, delayBetweenPoints, 3);
        }

        /// <summary>
        /// Called once the experience creation process has completed.
        /// </summary>
        /// <param name="success">If set to <c>true</c> success.</param>
        public void OnExperienceCreationComplete(bool success)
        {
            //Give some feedback message
            if (feedbackText != null)
            {
                if (success)
                    feedbackText.text = "Your experience has been successfully created! Feel free to visit the EMS portal to check it out!";
                else
                    feedbackText.text = "An error occured when trying to upload your experience.. Oh no..";
            }
        }

        #region UI Utilities

        /// <summary>
        /// Occurs whenever a dropdown or text field has been touched.
        /// Verifies if the Upload button should be available.
        /// </summary>
        public void RefreshIfUploadButtonIsInteractible()
        {
            //Make the upload experience button available if not none
            if (uploadExperienceBT != null)
            {
                //The requirements for the Upload button to be interactible
                uploadExperienceBT.interactable = (targetNameIF == null || targetNameIF.text.Length > 0);
            }
        }

        public void OnResetFieldsButtonPressed()
        {
            if (targetNameIF != null)
                targetNameIF.text = "";
            if (targetImageToUploadPathText != null)
                targetImageToUploadPathText.text = "";
            if (mediaToUploadPathText != null)
                mediaToUploadPathText.text = "";
            if (mediaTypeDropdown != null)
                mediaTypeDropdown.value = 0;
        }

        #endregion
    }
}