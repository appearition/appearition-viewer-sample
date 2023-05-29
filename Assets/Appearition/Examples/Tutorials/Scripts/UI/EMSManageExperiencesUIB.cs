// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: "EMSManageExperiencesUIB.cs" 
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Appearition.Common;
using Appearition.ArTargetImageAndMedia;
using System.Linq;

namespace Appearition.Example
{
    /// <summary>
    /// Displays and handles the UI events for the experience management.
    /// </summary>
    [RequireComponent(typeof(EMSManageExperiencesB))]
    public class EMSManageExperiencesUIB : BaseEMSUITab
    {
        //References
        public Transform targetImageLinkersHolder;
        public Transform mediaLinkersHolder;
        public GameObject targetImageLinkerPrefab;
        public GameObject mediaLinkerPrefab;
        public InputField experienceNameIF;
        public Button refreshBT;
        public Text refreshBTText;
        public Text assetIdText;
        public Text productIdText;
        public Text arTargetIdText;
        public Image arTargetImage;
        public Button publishBT;
        public Text publishBTText;
        public Button createNewMediaBT;
        public Button syncWithEMSBT;
        public Text syncWithEMSBTText;
        public Text syncOutcomeText;
        private List<Linker_SingleManageTargetImage> _allExperiences = new List<Linker_SingleManageTargetImage>();
        private List<Linker_SingleMediaManagementEntry> _allMediasForCurrentExperience = new List<Linker_SingleMediaManagementEntry>();

        //Internal Variables
        public const string publishBTTextPublish = "Publish";
        public const string publishBTTextUnpublish = "Unpublish";
        public const string publishBTTextUnknown = "Check Publish";
        public Color publishBTColPublish = Color.green;
        public Color publishBTColUnpublish = Color.red;
        public Color publishBTColUnknown = Color.white;
        public float delayBetweenPoints = 0.8f;

        //Handy Properties
        private EMSManageExperiencesB _manageExperiencesB;

        /// <summary>
        /// Reference to the script in charge of talking with the EMS to manage the experiences.
        /// </summary>
        /// <value>The manage experiences b.</value>
        private EMSManageExperiencesB ManageExperiencesB
        {
            get
            {
                if (_manageExperiencesB == null)
                    _manageExperiencesB = GetComponent<EMSManageExperiencesB>();
                return _manageExperiencesB;
            }
        }

        /// <summary>
        /// Fetch the main storage for the current ApiData.
        /// </summary>
        /// <value>The current ApiData.</value>
        private List<ArTarget> CurrentData
        {
            get
            {
                if (ManageExperiencesB == null)
                    return null;
                return ManageExperiencesB.currentData;
            }
        }

        /// <summary>
        /// Fetch the Asset ID of the experiences being displayed for quering purpose.
        /// </summary>
        /// <value>The experience currently selected asset identifier.</value>
        private string ExperienceCurrentlySelectedAssetId
        {
            get
            {
                if (ManageExperiencesB == null)
                    return null;
                return ManageExperiencesB.experienceCurrentlySelectedAssetId;
            }
        }

        void Start()
        {
            //Init. Display some empty ApiData.
            DisplayExperience(null);
        }

        #region UI Button Event

        /// <summary>
        /// Occurs whenever the Refresh button has been pressed.
        /// Re-fetches the ApiData from the cloud, and repopulates the UI with the new ApiData.
        /// </summary>
        public void OnRefreshButtonPressed()
        {
            if (ManageExperiencesB != null)
                ManageExperiencesB.OnRefreshButtonPressed();
        }

        /// <summary>
        /// Occurs whenever the Publish button has been pressed. 
        /// Goes through a series of check, and according to the experience's current state, checks, publishes or unpublishes the experience.
        /// </summary>
        public void OnPublishButtonPressed()
        {
            if (publishBT == null || publishBTText == null)
                return;

            if (ManageExperiencesB != null)
                ManageExperiencesB.OnPublishButtonPressed();
        }


        public void OnCreateNewMediaButtonPressed()
        {
            if (CurrentData == null || CurrentData.Count == 0 || ExperienceCurrentlySelectedAssetId.Length == 0)
                return;

            //Fetch the asset
            int arTargetIndex = CurrentData.FindIndex(o => o.assetId == ExperienceCurrentlySelectedAssetId);
            if (arTargetIndex >= 0)
            {
                //Create some new ApiData with default stats
                MediaFile[] newMediaFile =
                {
                    new MediaFile()
                    {
                        arMediaId = Random.Range(0, int.MaxValue),
                        isPrivate = false,
                        scaleX = 1,
                        scaleY = 1,
                        scaleZ = 1
                    }
                };

                //Store it
                CurrentData[arTargetIndex].mediaFiles = CurrentData[arTargetIndex].mediaFiles.Concat(newMediaFile).ToList();

                //Display it too
                DisplayExperience(CurrentData[arTargetIndex]);

                Debug.Log(string.Format("Media successfully created for ArTarget of id {0}", CurrentData[arTargetIndex].arTargetId));
            }
        }

        /// <summary>
        /// Occurs whenever the Delete button has been pressed on the UI.
        /// Removes a MediaFile from the local storage. Do note that the Media will be removed if the local ApiData gets synced.
        /// </summary>
        /// <param name="arMediaId">Ar media identifier.</param>
        public void OnDeleteMediaButtonPressed(int arMediaId)
        {
            if (CurrentData == null || CurrentData.Count == 0 || ExperienceCurrentlySelectedAssetId.Length == 0)
                return;

            int arTargetIndex = CurrentData.FindIndex(o => o.assetId == ExperienceCurrentlySelectedAssetId);

            if (arTargetIndex >= 0)
            {
                if (CurrentData[arTargetIndex].mediaFiles.Any(o => o.arMediaId == arMediaId))
                {
                    List<MediaFile> tmpFiles = new List<MediaFile>();

                    for (int i = 0; i < CurrentData[arTargetIndex].mediaFiles.Count; i++)
                    {
                        if (CurrentData[arTargetIndex].mediaFiles[i].arMediaId == arMediaId)
                            continue;
                        tmpFiles.Add(CurrentData[arTargetIndex].mediaFiles[i]);
                    }

                    CurrentData[arTargetIndex].mediaFiles = tmpFiles;

                    DisplayExperience(CurrentData[arTargetIndex]);
                }
            }
        }

        /// <summary>
        /// Occurs whenever an AR target button has been pressed. Displays the content of the asset.
        /// </summary>
        /// <param name="assetId">Asset identifier.</param>
        void OnArTargetButtonPressed(string assetId)
        {
            if (CurrentData == null)
                return;

            //Store the current name inside the currently loaded ApiData.
            if (experienceNameIF != null && ExperienceCurrentlySelectedAssetId != null)
            {
                ArTarget tmpTarget = CurrentData.FirstOrDefault(o => o.assetId == ExperienceCurrentlySelectedAssetId);

                if (tmpTarget != null)
                    tmpTarget.name = experienceNameIF.text;
            }


            //Just display its content.
            DisplayExperience(CurrentData.FirstOrDefault(o => o.assetId == assetId));
        }

        #endregion

        #region UI Check

        /// <summary>
        /// Should be called once the publish state of an experience has been swapped. 
        /// </summary>
        /// <param name="isPublished">Is published.</param>
        public void TargetPublishStateHasChanged(bool? isPublished)
        {
            if (publishBT != null)
                publishBT.image.color = (!isPublished.HasValue ? publishBTColUnknown : (!isPublished.Value ? publishBTColPublish : publishBTColUnpublish));
            if (arTargetImage != null)
                arTargetImage.color = new Color(arTargetImage.color.r, arTargetImage.color.b, arTargetImage.color.g, (isPublished.HasValue && isPublished.Value ? 1 : 0.5f));
            if (publishBTText != null)
                publishBTText.text = (!isPublished.HasValue ? publishBTTextUnknown : (!isPublished.Value ? publishBTTextPublish : publishBTTextUnpublish));

            //Finally, re-enable the button.
            if (publishBT != null)
                publishBT.interactable = true;
        }

        /// <summary>
        /// Returns the publish state of the current experience as a text like on the UI.
        /// </summary>
        /// <returns>The publish state text.</returns>
        public string GetPublishStateText()
        {
            if (publishBTText != null)
                return publishBTText.text;
            return "";
        }

        #endregion

        #region UI Display and Update

        /// <summary>
        /// Rebuilts the experience list completely. Destroys existing and rebuilds the allExperiences container.
        /// </summary>
        public void UpdateExperiencesContainer()
        {
            //Clean existing ones
            for (int i = _allExperiences.Count - 1; i >= 0; i--)
            {
                if (_allExperiences[i] != null)
                    Destroy(_allExperiences[i].gameObject);
            }

            _allExperiences.Clear();

            //Retardation test 
            if (targetImageLinkersHolder == null || targetImageLinkerPrefab == null)
                return;

            if (CurrentData != null)
            {
                //Create content !
                for (int i = 0; i < CurrentData.Count; i++)
                {
                    Linker_SingleManageTargetImage tmp = Instantiate(targetImageLinkerPrefab, targetImageLinkersHolder).GetComponent<Linker_SingleManageTargetImage>();

                    //Give it an image
                    if (CurrentData[i].targetImages.Count == 0)
                        continue;
                    Sprite tmpTargetImage = CurrentData[i].targetImages.First().image;

                    //Feed it some ApiData
                    tmp.Setup(CurrentData[i].assetId, tmpTargetImage, OnArTargetButtonPressed);

                    //Store it
                    _allExperiences.Add(tmp);
                }
            }

            //Clear the current experience display.
            DisplayExperience(null);
        }

        /// <summary>
        /// Destroys all the UI display of the medias inside the current experience.
        /// </summary>
        public void ClearArTargetList()
        {
            for (int i = _allMediasForCurrentExperience.Count - 1; i >= 0; i--)
            {
                if (_allMediasForCurrentExperience[i] != null)
                    Destroy(_allMediasForCurrentExperience[i].gameObject);
            }

            _allMediasForCurrentExperience.Clear();
        }

        /// <summary>
        /// Displays an experience's content on the main panel. If the param given is null, will clear up the content.
        /// </summary>
        /// <param name="tmpAsset">Tmp asset.</param>
        public void DisplayExperience(ArTarget tmpAsset)
        {
            //Get the previous target, if any. 
            if (CurrentData != null && ExperienceCurrentlySelectedAssetId != null)
            {
                int arTargetIndex = CurrentData.FindIndex(o => o.assetId == ExperienceCurrentlySelectedAssetId);
                if (arTargetIndex >= 0)
                {
                    if (experienceNameIF != null)
                        CurrentData[arTargetIndex].name = experienceNameIF.text;
                }
            }

            //Handle the asset content
            ManageExperiencesB.experienceCurrentlySelectedAssetId = (tmpAsset == null ? "" : tmpAsset.assetId);
            if (experienceNameIF != null)
                experienceNameIF.text = (tmpAsset == null ? "" : tmpAsset.name);
            if (assetIdText != null)
                assetIdText.text = (tmpAsset == null ? "" : tmpAsset.assetId);
            if (productIdText != null)
                productIdText.text = (tmpAsset == null ? "" : tmpAsset.productId.ToString());
            if (arTargetIdText != null)
                arTargetIdText.text = (tmpAsset == null ? "" : tmpAsset.targetImages.First().arImageId.ToString());
            if (arTargetImage != null)
            {
                TargetImage tmpTargetImage = (tmpAsset == null ? null : tmpAsset.targetImages.First());
                arTargetImage.sprite = (tmpAsset == null || tmpTargetImage == null ? null : tmpTargetImage.image);
            }

            TargetPublishStateHasChanged(tmpAsset == null ? default(bool?) : tmpAsset.isPublished);

            if (createNewMediaBT != null)
                createNewMediaBT.interactable = (tmpAsset != null);

            //Handle the media content. Clear it first.
            for (int i = _allMediasForCurrentExperience.Count - 1; i >= 0; i--)
            {
                if (_allMediasForCurrentExperience[i] != null)
                    Destroy(_allMediasForCurrentExperience[i].gameObject);
            }

            _allMediasForCurrentExperience.Clear();

            //If there's an asset, populate its content! if not, end here.
            if (tmpAsset == null || tmpAsset.mediaFiles == null || tmpAsset.mediaFiles.Count == 0 || mediaLinkersHolder == null)
                return;

            for (int i = 0; i < tmpAsset.mediaFiles.Count; i++)
            {
                if (tmpAsset.mediaFiles[i] == null)
                    continue;

                //Create the object
                Linker_SingleMediaManagementEntry tmpMedia = Instantiate(mediaLinkerPrefab, mediaLinkersHolder).GetComponent<Linker_SingleMediaManagementEntry>();

                //Set it up!
                tmpMedia.Setup(this, tmpAsset.mediaFiles[i]);

                //Store it
                _allMediasForCurrentExperience.Add(tmpMedia);
            }
        }

        #endregion

        public DotAnimation GetDotAnimationObject()
        {
            List<Button> buttonsToDisable = new List<Button>();
            if (syncWithEMSBT != null)
                buttonsToDisable.Add(syncWithEMSBT);
            if (publishBT != null)
                buttonsToDisable.Add(publishBT);
            if (refreshBT != null)
                buttonsToDisable.Add(refreshBT);

            return new DotAnimation(buttonsToDisable, syncWithEMSBTText, delayBetweenPoints, 3);
        }
    }
}