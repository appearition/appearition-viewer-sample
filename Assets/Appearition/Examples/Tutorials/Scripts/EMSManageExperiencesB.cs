// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: "EMSManageExperiencesB.cs" 
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Appearition.ArTargetImageAndMedia;
using Appearition.Common;

namespace Appearition.Example
{
    /// <inheritdoc />
    public class EMSManageExperiencesB : MonoBehaviour
    {
        [HideInInspector] public List<ArTarget> currentData;
        [HideInInspector] public string experienceCurrentlySelectedAssetId = "";
        public bool debugUpdateInfo = false;

        //Handy Properties
        private EMSManageExperiencesUIB _manageExperiencesUib;

        /// <summary>
        /// Reference to the script in charge of display the ApiData coming down from the EMS, as well as handling UI inputs.
        /// </summary>
        /// <value>The manage experiences b.</value>
        private EMSManageExperiencesUIB ManageExperiencesUib
        {
            get
            {
                if (_manageExperiencesUib == null)
                    _manageExperiencesUib = GetComponent<EMSManageExperiencesUIB>();
                return _manageExperiencesUib;
            }
        }


        #region Main Buttons

        /// <summary>
        /// Occurs whenever the Refresh button has been pressed.
        /// Re-fetches the ApiData from the cloud, and repopulates the UI with the new ApiData.
        /// </summary>
        public void OnRefreshButtonPressed()
        {
            StartCoroutine(OnRefreshButtonPressedProcess());
        }

        private IEnumerator OnRefreshButtonPressedProcess()
        {
            //Firstly, disable the button.
            using (DotAnimation dotAnim = GetDotAnimationObject())
            {
                if (AppearitionGate.Instance.CurrentUser.selectedChannel == 0)
                {
                    //Try to find a channel for that user.
                    SetUserChannelAsCurrentProcess findUserChannel = new SetUserChannelAsCurrentProcess();

                    yield return findUserChannel.ExecuteMainProcess();
                }
                
                bool isRequestDone = false;

                //API call to get all assets !
                ArTargetHandler.GetChannelArTargets(true, true, tmpArTargets =>
                {
                    if (tmpArTargets != null)
                        currentData = new List<ArTarget>(tmpArTargets);
                    //Update the experience list!
                }, null, b=> isRequestDone = true);

                //Handle the dot animation
                while (!isRequestDone)
                {
                    dotAnim.UpdateDisplay();
                    yield return null;
                }

                //All done, update the UI
                if (ManageExperiencesUib != null)
                    ManageExperiencesUib.UpdateExperiencesContainer();
            }
        }

        /// <summary>
        /// Occurs whenever the Publish button has been pressed. 
        /// Goes through a series of check, and according to the experience's current state, checks, publishes or unpublishes the experience.
        /// </summary>
        public void OnPublishButtonPressed()
        {
            //Make sure a current asset is selected and exists. Just in case.
            ArTarget selectedAsset = currentData.FirstOrDefault(o => o.assetId == experienceCurrentlySelectedAssetId);

            if (selectedAsset == null || currentData == null || currentData.Count == 0)
                return;

            StartCoroutine(OnPublishButtonPressed_PROCESS());
        }

        private IEnumerator OnPublishButtonPressed_PROCESS()
        {
            //Firstly, disable the button
            using (DotAnimation dotAnim = GetDotAnimationObject())
            {
                //Get the ArTargetId before handling the publish states.
                ArTarget tmpArTarget = null;

                ArTargetHandler.GetChannelArTargets(true, false, successOutcome =>
                {
                    if (successOutcome != null && successOutcome.Count > 0)
                    {
                        ArTarget tmp = successOutcome.FirstOrDefault(o => o.assetId == experienceCurrentlySelectedAssetId);
                        if (tmp != null)
                            tmpArTarget = tmp;
                        else
                            tmpArTarget = null;
                    }
                    else
                        tmpArTarget = null;
                }, onComplete: completeOutcome =>
                {
                    if (!completeOutcome)
                        tmpArTarget = null;
                });

                //Do the little dot animation
                while (tmpArTarget == null)
                {
                    dotAnim.UpdateDisplay();
                    yield return null;
                }

                bool isRequestDone = false;
                bool? isTargetPublishedAfterRequest = null;

                string publishText = "";
                if (ManageExperiencesUib != null)
                    publishText = ManageExperiencesUib.GetPublishStateText();

                //Handle each button state separately. Launch any API.
                switch (publishText)
                {
                    case EMSManageExperiencesUIB.publishBTTextPublish:
                        //Publish it!

                        ArTargetHandler.PublishExperience(tmpArTarget, onComplete: outcome =>
                        {
                            isTargetPublishedAfterRequest = outcome ? true : default(bool?);
                            isRequestDone = true;
                        });

                        break;
                    case EMSManageExperiencesUIB.publishBTTextUnpublish:
                        //Unpublish it!

                        ArTargetHandler.UnpublishExperience(tmpArTarget, onComplete: outcome =>
                        {
                            isTargetPublishedAfterRequest = outcome ? false : default(bool?);
                            isRequestDone = true;
                        });

                        break;
                    default:
                        Debug.LogError("Current publish button text not recognized.");
                        isRequestDone = true;
                        break;
                }

                //Do an animation while waiting
                while (!isRequestDone)
                {
                    dotAnim.UpdateDisplay();
                    yield return null;
                }

                //Finally, re-enable the button.
                if (ManageExperiencesUib != null)
                    ManageExperiencesUib.TargetPublishStateHasChanged(isTargetPublishedAfterRequest);
            }
        }


        public void OnSyncWithCloudDataButtonPressed()
        {
            if (currentData != null)
                StartCoroutine(OnSyncWithCloudDataButtonPressed_PROCESS());
        }

        private IEnumerator OnSyncWithCloudDataButtonPressed_PROCESS()
        {
            //Firstly, disable the button.
            using (DotAnimation dotAnim = GetDotAnimationObject())
            {
                //Select no ArTarget, so the ApiData is successfully saved.
                if (ManageExperiencesUib != null)
                {
                    ManageExperiencesUib.DisplayExperience(null);
                    ManageExperiencesUib.ClearArTargetList();
                }

                //Fetches brand-new ApiData to use for comparison
                bool isAssetFetchingDone = false;
                ArTarget[] dataAsOnTheEms = null;
                ArTargetHandler.GetChannelArTargets(outcome =>
                {
                    dataAsOnTheEms = outcome.ToArray();
                    isAssetFetchingDone = true;
                });

                while (!isAssetFetchingDone)
                {
                    if (dotAnim != null)
                        dotAnim.UpdateDisplay();
                    yield return null;
                }

                //For each asset and mediatype, check the differences of content. If new content, create media; if content gone, delete media; if same media, update media ApiData.
                for (int i = 0; i < currentData.Count; i++)
                {
                    if (currentData[i] == null)
                        continue;

                    //Get asset for further use.
                    ArTarget assetAsOnEms = dataAsOnTheEms.FirstOrDefault(o => o.assetId == currentData[i].assetId);

                    if (assetAsOnEms == null)
                    {
                        Debug.LogError("Asset of id " + currentData[i].assetId + " was not found.");
                        continue;
                    }


                    //Firstly, update the name if needed.
                    if (!currentData[i].name.Equals(assetAsOnEms.name))
                    {
                        bool? nameChangeRequest = null;
                        ArTargetHandler.ChangeExperienceName(currentData[i], currentData[i].name, onComplete: outcome => { nameChangeRequest = outcome; });

                        while (!nameChangeRequest.HasValue)
                        {
                            if (dotAnim != null)
                                dotAnim.UpdateDisplay();
                            yield return null;
                        }

                        //If the request was successful, change the name.
                        if (nameChangeRequest.Value)
                            assetAsOnEms.name = currentData[i].name;
                    }

                    //Then, handle the medias that the two have in common.
                    List<MediaFile> mediaInCommon = (from tmpMedia in currentData[i].mediaFiles
                        where assetAsOnEms.mediaFiles.Any(o => o.arMediaId == tmpMedia.arMediaId)
                        select tmpMedia).ToList();

                    for (int k = 0; k < mediaInCommon.Count; k++)
                    {
                        MediaFile mediaOnEms = assetAsOnEms.mediaFiles.First(o => o.arMediaId == mediaInCommon[k].arMediaId);
                        //Firstly, check if they're different on a JSON level.
                        string localMediaJson = JsonUtility.ToJson(mediaInCommon[k]);
                        string emsMediaJson = JsonUtility.ToJson(mediaOnEms);

                        //If they are the same, no need to update. Leave it for now.
                        if (localMediaJson.Equals(emsMediaJson))
                            continue;

                        //if not, update the media.
                        bool? mediaUpdateRequestOutcome = null;

                        //Check whether the user has changed the media content (upload). I used a little flag by replacing the URL.
                        if (mediaInCommon[k].url == null || mediaInCommon[k].url.Contains("http"))
                        {
                            //Still the same. Just update the media.
                            ArTargetHandler.UpdateExperienceMedia(currentData[i], mediaInCommon[k], onComplete: outcome => { mediaUpdateRequestOutcome = outcome; });
                        }
                        else
                        {
                            object newMediaContent = null;
                            bool isDoneLoadingNewMedia = false;

                            yield return FileHandler.LoadBytesFromFileProcess(mediaInCommon[k].url, loadOutcome =>
                            {
                                newMediaContent = loadOutcome;
                                isDoneLoadingNewMedia = true;
                            });

                            while (!isDoneLoadingNewMedia)
                            {
                                if (dotAnim != null)
                                    dotAnim.UpdateDisplay();
                                yield return null;
                            }

                            //It's now a local URL. Update the content.
                            ArTargetHandler.UpdateExperienceMedia(currentData[i], mediaInCommon[k], newMediaContent, false, (arTargetOutcome, mediaFileOutcome) =>
                            {
                                if (mediaFileOutcome != null)
                                {
                                    //Replace entries in the current ApiData.
                                    currentData[i].mediaFiles[currentData[i].mediaFiles.FindIndex(o => o.arMediaId == mediaInCommon[k].arMediaId)] = mediaFileOutcome;

                                    mediaInCommon[k] = mediaFileOutcome;
                                }

                                mediaUpdateRequestOutcome = (mediaFileOutcome != null);
                            });
                        }


                        while (!mediaUpdateRequestOutcome.HasValue)
                        {
                            if (dotAnim != null)
                                dotAnim.UpdateDisplay();
                            yield return null;
                        }

                        //If the request was successful, apply the changes.
                        if (mediaUpdateRequestOutcome.Value && debugUpdateInfo)
                        {
                            Debug.Log(string.Format("Media of id {0} has successfully been updated with the EMS.", mediaInCommon[k].arMediaId));
                        }
                    }

                    //Finally, handle the medias to remove from the EMS. Just unlink, no deleting.
                    List<MediaFile> mediasToRemoveFromEms = (from tmpMedia in assetAsOnEms.mediaFiles
                        where currentData[i].mediaFiles.All(o => o.arMediaId != tmpMedia.arMediaId)
                        select tmpMedia).ToList();

                    for (int k = 0; k < mediasToRemoveFromEms.Count; k++)
                    {
                        bool? isRemoveRequestComplete = null;

                        ArTargetHandler.RemoveMediaFromArTarget(currentData[i], mediasToRemoveFromEms[k], false, removeSuccess => { isRemoveRequestComplete = true; },
                            outcome => { isRemoveRequestComplete = false; });

                        while (!isRemoveRequestComplete.HasValue)
                        {
                            if (dotAnim != null)
                                dotAnim.UpdateDisplay();
                            yield return null;
                        }

                        if (isRemoveRequestComplete.Value && debugUpdateInfo)
                        {
                            Debug.Log(string.Format("Media of id {0} has successfully been removed from the EMS.", mediasToRemoveFromEms[k].arMediaId));
                        }
                    }


                    //Secondly, handle the medias to add to the EMS.
                    List<MediaFile> mediasToAddToEms = (from tmpMedia in currentData[i].mediaFiles
                        where assetAsOnEms.mediaFiles.All(o => o.arMediaId != tmpMedia.arMediaId)
                        select tmpMedia).ToList();

                    for (int k = 0; k < mediasToAddToEms.Count; k++)
                    {
                        bool? mediaAddRequestOutcome = null;

                        object newMediaContent = null;

                        //Check whether it contains a file or not.
                        if (!string.IsNullOrEmpty(mediasToAddToEms[k].url))
                        {
                            bool isDoneLoadingNewMedia = false;

                            yield return FileHandler.LoadBytesFromFileProcess(mediasToAddToEms[k].url, loadOutcome =>
                            //AppearitionGate.LoadObjectFromPath(mediasToAddToEms[k].url, loadOutcome =>
                            {
                                newMediaContent = loadOutcome;
                                isDoneLoadingNewMedia = true;
                            });

                            while (!isDoneLoadingNewMedia)
                            {
                                if (dotAnim != null)
                                    dotAnim.UpdateDisplay();
                                yield return null;
                            }
                        }

                        ArTargetHandler.AddMediaToExistingExperience(currentData[i], mediasToAddToEms[k], newMediaContent, (a,b) => { mediaAddRequestOutcome = true; });


                        while (!mediaAddRequestOutcome.HasValue)
                        {
                            if (dotAnim != null)
                                dotAnim.UpdateDisplay();
                            yield return null;
                        }

                        //If the request was successful, apply the changes.
                        if (mediaAddRequestOutcome.Value && mediaInCommon.Count >= k && mediaInCommon[k] != null && debugUpdateInfo)
                        {
                            Debug.Log(string.Format("Media of id {0} has successfully been created on the EMS.", mediaInCommon[k].arMediaId));
                        }
                    }
                }


                //Finally, fetch all the ApiData from the EMS one last time.
                yield return OnRefreshButtonPressedProcess();
            }
        }

        #endregion

        private DotAnimation GetDotAnimationObject()
        {
            return ManageExperiencesUib == null ? null : ManageExperiencesUib.GetDotAnimationObject();
        }
    }
}