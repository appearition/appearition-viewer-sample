// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: "EMSCreateExperienceB.cs" 
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Appearition.ArTargetImageAndMedia;
using Appearition.ChannelManagement;
using Appearition.Common;

namespace Appearition.Example
{
    /// <summary>
    /// Loads, creates and uploads a new experience on the EMS.
    /// </summary>
    public class EMSCreateExperienceB : MonoBehaviour
    {
        //References
        private EMSCreateExperienceUIB _createExperiencesUIB;

        /// <summary>
        /// Lazy reference to the UI object containing all the input fields and buttons required to help the user create an experience.
        /// </summary>
        /// <value>The create experiences user interface.</value>
        private EMSCreateExperienceUIB CreateExperiencesUib
        {
            get
            {
                if (_createExperiencesUIB == null)
                    _createExperiencesUIB = GetComponent<EMSCreateExperienceUIB>();
                return _createExperiencesUIB;
            }
        }

        //Data
        [HideInInspector] public List<MediaType> availableMediaTypes = new List<MediaType>();

        IEnumerator Start()
        {
            //Get MediaTypes
            availableMediaTypes = new List<MediaType>();
            yield return ChannelHandler.GetMediaTypesFromEmsProcess(success => availableMediaTypes.AddRange(success));
        }

        #region Upload Process

        /// <summary>
        /// Occurs whenever the Upload Experience button is pressed. Handles the uploading of an experience, including creating the target image.
        /// </summary>
        public void OnUploadExperienceButtonPressed(string mediaType, string targetName, string targetImagePath, string mediaPath)
        {
            StartCoroutine(UploadExperience_PROCESS(mediaType, targetName, targetImagePath, mediaPath));
        }

        IEnumerator UploadExperience_PROCESS(string mediaType, string targetName, string targetImagePath, string mediaPath)
        {
            //Disable the uploadExperience button and do some dot animation during the key waiting times.
            using (DotAnimation dotAnim = GetDotAnimationObject())
            {
                //To begin with, load the target image, if any.
                Texture targetImageTexture = null;
                if (!string.IsNullOrEmpty(targetImagePath))
                    targetImageTexture = ImageUtility.LoadOrCreateTexture(targetImagePath);


                //Upload the target image!
                ArTarget tmpArTarget = null;
                bool? isArTargetCreationSuccess = null;
                if (string.IsNullOrEmpty(targetName))
                    targetName = "Tmp Target";



                ArTargetHandler.CreateExperience(targetName, targetImageTexture, "tmpTarget.jpg", onSuccess: tmpOutcome => { tmpArTarget = tmpOutcome; },
                    onComplete: isSuccess => { isArTargetCreationSuccess = isSuccess; });

                while (!isArTargetCreationSuccess.HasValue)
                {
                    dotAnim.UpdateDisplay();
                    yield return null;
                }

                //Check if it didn't manage uploading it..
                if (tmpArTarget == null || !isArTargetCreationSuccess.Value)
                {
                    AppearitionLogger.LogError("The AR target could not be created.");
                    if (CreateExperiencesUib != null)
                        CreateExperiencesUib.OnExperienceCreationComplete(false);
                    yield break;
                }

                AppearitionLogger.LogInfo("Target image created !");

                Debug.Log("Droppin in the PUBLIC tag");
                yield return ArTargetHandler.AddTagToExperienceProcess(tmpArTarget, "PUBLIC");

                //Now that the Target Image uploaded and all's well, fill in the experience with information. Start by loading the media, if any.
                if (!string.IsNullOrEmpty(mediaType))
                {
                    object mediaToUpload = null;

                    if (!string.IsNullOrEmpty(mediaPath))
                    {
                        AppearitionLogger.LogDebug("Loading media..");
                        //Fetch the file!
                        bool isMediaDoneFetching = false;

                        yield return FileHandler.LoadBytesFromFileProcess(mediaPath, tmpOutcome =>
                            //AppearitionGate.LoadObjectFromPath(mediaPath, tmpOutcome =>
                        {
                            mediaToUpload = tmpOutcome;
                            isMediaDoneFetching = true;
                        });

                        while (!isMediaDoneFetching)
                        {
                            dotAnim.UpdateDisplay();
                            yield return null;
                        }

                        AppearitionLogger.LogDebug("Media done loading !" + mediaToUpload);
                    }


                    //Make a media to upload
                    MediaFile tmpMediaToUpload = new MediaFile();
                    string mediaExtension = "";

                    if (availableMediaTypes.Count == 0)
                    {
                        availableMediaTypes = new List<MediaType>();
                        yield return ChannelHandler.GetMediaTypesFromEmsProcess(success => availableMediaTypes.AddRange(success));

                        if (availableMediaTypes.Count == 0)
                        {
                            AppearitionLogger.LogError("No mediafile found in the selected channel.");
                            if (CreateExperiencesUib != null)
                                CreateExperiencesUib.OnExperienceCreationComplete(false);
                            yield break;
                        }
                    }

                    //Ignore None for mediatype
                    MediaType tmpMediaType = MediaTypeUtility.FindMediaTypeFromEmsName(mediaType, availableMediaTypes);

                    if (mediaToUpload != null && tmpMediaType != null)
                    {
                        mediaExtension = System.IO.Path.GetExtension(mediaPath);
                        tmpMediaToUpload.mimeType = tmpMediaType.GetMimeTypeForGivenExtension(mediaExtension);
                    }

                    tmpMediaToUpload.fileName = "TmpFile" + mediaExtension;
                    if (tmpMediaType != null)
                        tmpMediaToUpload.mediaType = tmpMediaType.Name;
                    tmpMediaToUpload.isPrivate = false;

                    //Upload the experience!
                    bool? isExperienceCreationSuccess = null;

                    AppearitionLogger.LogDebug("Launching upload media !");
                    ArTargetHandler.AddMediaToExistingExperience(tmpArTarget, tmpMediaToUpload, mediaToUpload, true, (arTargetOutcome, mediaFileOutcome) => { tmpMediaToUpload = mediaFileOutcome; },
                        onComplete: isSuccess => { isExperienceCreationSuccess = isSuccess; });

                    while (!isExperienceCreationSuccess.HasValue)
                    {
                        dotAnim.UpdateDisplay();
                        yield return null;
                    }

                    if (isExperienceCreationSuccess.Value)
                        AppearitionLogger.LogInfo("Media successfully uploaded !");
                    else
                        AppearitionLogger.LogError("Media was not successfully uploaded..");
                }
                else
                {
                    //The media being dealt with is empty, Publish it anyway!
                    bool? isPublishRequestComplete = null;

                    ArTargetHandler.PublishExperience(tmpArTarget, onComplete: isSuccess => { isPublishRequestComplete = isSuccess; });

                    while (!isPublishRequestComplete.HasValue)
                    {
                        dotAnim.UpdateDisplay();
                        yield return null;
                    }

                    if (isPublishRequestComplete.Value)
                    {
                        AppearitionLogger.LogDebug("Empty target successfully published !");
                    }
                    else
                        AppearitionLogger.LogError("An issue occured when trying to publish this empty target..");
                }

                //All done! Display the results
                if (CreateExperiencesUib != null)
                    CreateExperiencesUib.OnExperienceCreationComplete(true);
                AppearitionLogger.LogInfo("All good! Experience fully created !");
            }
        }

        #endregion

        #region Utilities

        DotAnimation GetDotAnimationObject()
        {
            if (CreateExperiencesUib == null)
                return null;
            return CreateExperiencesUib.GetDotAnimationObject();
        }

        #endregion
    }
}