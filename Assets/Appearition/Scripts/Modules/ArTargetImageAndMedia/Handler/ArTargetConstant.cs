using Appearition.ArTargetImageAndMedia.API;

namespace Appearition.ArTargetImageAndMedia
{
    public static class ArTargetConstant
    {
        #region Log Messages

        public const string GET_CHANNEL_EXPERIENCES_SUCCESS_OFFLINE = "Experiences of the channel {0} have been successfully fetched offline!";
        public const string GET_CHANNEL_EXPERIENCES_SUCCESS = "Experiences of the channel {0} have been successfully fetched!";
        public const string GET_CHANNEL_EXPERIENCES_FAILURE = "An error occured when trying to fetch the experiences from the channel of id {0}";

        public const string GET_SPECIFIC_MEDIAS_IN_ASSET_BY_QUERY_EMPTY_ID = "An empty AssetId was provided when trying to excecute GetSpecificMediasInAssetByQueryProcess.";
        public const string GET_SPECIFIC_MEDIAS_IN_ASSET_BY_QUERY_SUCCESS = "The request to fetch the medias of the asset of id {0} was successful, and fetched {1} mediafiles!";
        public const string GET_SPECIFIC_MEDIAS_IN_ASSET_BY_QUERY_SUCCESS_NO_RESULTS =
            "The request to fetch the medias of the asset of id {0} was successful, but no medias came out from the given query.";
        public const string GET_SPECIFIC_MEDIAS_IN_ASSET_BY_QUERY_FAILURE = "An error occured when trying to fetch the medias from the asset of id {0}.";

        public const string GET_SPECIFIC_EXPERIENCES_BY_QUERY_SUCCESS = "The query was successful and {0} results were found!";
        public const string GET_SPECIFIC_EXPERIENCES_BY_QUERY_SUCCESS_OFFLINE = "The query was successfully completed offline and {0} results were found!";
        public const string GET_SPECIFIC_EXPERIENCES_BY_QUERY_SUCCESS_EMPTY = "The query was successful but no items were found.";
        public const string GET_SPECIFIC_EXPERIENCES_BY_QUERY_FAILURE = "An error occured when trying to find experiences using the given query.";
        public const string GET_SPECIFIC_EXPERIENCES_BY_QUERY_FAILURE_EMPTY_QUERY = "No query was included. The request was not submitted.";

        public const string GET_DATA_INTEGRATION_RESULTS_NULL = "The given asset {0} or media {1} given is null.";
        public const string GET_DATA_INTEGRATION_RESULTS_SUCCESS = "Data integration from media of id {0} was successfully fetched!";
        public const string GET_DATA_INTEGRATION_RESULTS_FAILURE = "An error occured when trying to get results from data integration from the media of id {0}.";

        public const string CREATE_EXPERIENCE_SUCCESS = "ArTarget successfully created (code {0} with ID: {1})";
        public const string CREATE_EXPERIENCE_FAILURE = "An error occured while creating the ArTarget of name {0}.";

        public const string ADD_MEDIA_TO_EXISTING_EXPERIENCE_NULL = "Null ArTarget provided.";
        public const string ADD_MEDIA_TO_EXISTING_EXPERIENCE_SUCCESS = "Given media has been successfully added to the ArTarget.";
        public const string ADD_MEDIA_TO_EXISTING_EXPERIENCE_FAILURE = "An error occured when trying to add the given Media to the ArTarget.";

        public const string GET_SINGLE_ARTARGET_SUCCESS = "The ArTarget of id {0} was successfully fetched!";
        public const string GET_SINGLE_ARTARGET_FAILURE = "An error occured when trying to fetch the ArTarget of id {0}.";

        public const string GET_CHANNEL_ARTARGET_SUCCESS_OFFLINE = "ArTargets successfully loaded offline!";
        public const string GET_CHANNEL_ARTARGET_SUCCESS = "ArTargets successfully loaded online!";
        public const string GET_CHANNEL_ARTARGET_FAILURE = "An error occured when trying to fetch all the ArTargets from the channel of id {0}";

        public const string PUBLISH_EXPERIENCE_NULL = "Null ArTarget provided.";
        public const string PUBLISH_EXPERIENCE_SUCCESS = "ArTarget published successfully!";
        public const string PUBLISH_EXPERIENCE_FAILURE = "Error while trying to publish the given ArTarget.";

        public const string UNPUBLISH_EXPERIENCE_NULL = "Null ArTarget provided.";
        public const string UNPUBLISH_EXPERIENCE_SUCCESS = "ArTarget unpublished successfully!";
        public const string UNPUBLISH_EXPERIENCE_FAILURE = "Error while trying to unpublish the given ArTarget.";

        public const string DELETE_EXPERIENCE_NULL = "Null ArTarget provided.";
        public const string DELETE_EXPERIENCE_SUCCESS = "ArTarget deleted successfully!";
        public const string DELETE_EXPERIENCE_FAILURE = "Error while trying to deleted the given ArTarget.";

        public const string ADD_TARGET_IMAGE_TO_ARTARGET_NULL = "Null Texture provided when trying to add a new Target Image to ArTarget.";
        public const string ADD_TARGET_IMAGE_TO_ARTARGET_SUCCESS = "Successfully added a Target Image to the ArTarget of id {0}";
        public const string ADD_TARGET_IMAGE_TO_ARTARGET_FAILURE = "An error occured while uploading the Target Image.";

        public const string REMOVE_TARGET_IMAGE_FROM_ARTARGET_NULL_ARTARGET = "Null ArTarget provided.";
        public const string REMOVE_TARGET_IMAGE_FROM_ARTARGET_NULL_TARGETIMAGE = "Null TargetImage provided and/or none was found on the ArTarget.";
        public const string REMOVE_TARGET_IMAGE_FROM_ARTARGET_SUCCESS = "The TargetImage was successfully removed.";
        public const string REMOVE_TARGET_IMAGE_FROM_ARTARGET_FAILURE = "An error occured when trying to remove the given TargetImage.";
        public const string REMOVE_TARGET_IMAGE_FROM_ARTARGET_DELETE_SUCCESS = "The TargetImage was successfully removed from the EMS's library.";
        public const string REMOVE_TARGET_IMAGE_FROM_ARTARGET_DELETE_FAILURE = "An error occured when trying to remove the given TargetImage from the EMS's library.";

        public const string REPLACE_TARGET_IMAGE_NULL_ARTARGET = "Null ArTarget provided.";
        public const string REPLACE_TARGET_IMAGE_NULL_IMAGE = "Null new TargetImage texture provided.";

        public const string FETCH_AVAILABLE_TAGS_SUCCESS_OFFLINE = "Experience tags successfully loaded offline!";
        public const string FETCH_AVAILABLE_TAGS_SUCCESS = "Experience tags fetched successfully!";
        public const string FETCH_AVAILABLE_TAGS_FAILURE = "An error occured when trying to fetch the available tags of the channel of id {0}";

        public const string FETCH_RELATED_TAGS_SUCCESS = "Experience tags related to {0} fetched successfully!";
        public const string FETCH_RELATED_TAGS_SUCCESS_OFFLINE = "Experience tags related to {0} fetched successfully offline!";
        public const string FETCH_RELATED_TAGS_FAILURE = "An error occured when trying to fetch the related tags to {0}";


        public const string LOCK_ARTARGET_SUCCESS = "Experience of ArTargetId {0} is successfully locked!";
        public const string LOCK_ARTARGET_FAILURE = "An error occured when trying to lock the experience of ArTargetId {0}";

        public const string UNLOCK_ARTARGET_SUCCESS = "Experience of ArTargetId {0} is successfully unlocked!";
        public const string UNLOCK_ARTARGET_FAILURE = "An error occured when trying to unlock the experience of ArTargetId {0}";

        public const string ASSET_NULL = "Null Asset provided.";
        public const string ASSET_COPYRIGHT_SUCCESS = "Experience copyright info successfully fetched!";
        public const string ASSET_COPYRIGHT_SUCCESS_OFFLINE = "Experience copyright info successfully loaded offline!";
        public const string ASSET_COPYRIGHT_FAILURE = "An error occured when trying to fetch the copyright info of the asset of id {0}";

        public const string ARTARGET_COPYRIGHT_SUCCESS = "ArTarget copyright info successfully fetched!";
        public const string ARTARGET_COPYRIGHT_SUCCESS_OFFLINE = "ArTarget copyright info successfully loaded offline!";
        public const string ARTARGET_COPYRIGHT_FAILURE = "An error occured when trying to fetch the copyright info of the ArTarget of id {0}";


        public const string ADD_TAG_TO_ARTARGET_SUCCESS = "The tag {0} was successfully added to the ArTarget of id {1}!";
        public const string ADD_TAG_TO_ARTARGET_FAILURE_INVALID_PARAMS = "The process failed because the given tag or ArTarget are invalid.";
        public const string ADD_TAG_TO_ARTARGET_FAILURE_EXIST = "The tag {0} already existed on the ArTarget of id {1} and was not added.";
        public const string ADD_TAG_TO_ARTARGET_FAILURE = "An error occured when trying to add the tag {0} to the ArTarget of id {1}";

        public const string REMOVE_TAG_FROM_ARTARGET_SUCCESS = "The tag {0} was successfully removed from the ArTarget of id {1}!";
        public const string REMOVE_TAG_FROM_ARTARGET_FAILURE_INVALID_PARAMS = "The process failed because the given tag or ArTarget are invalid.";
        public const string REMOVE_TAG_FROM_ARTARGET_FAILURE_NOT_EXIST = "The tag {0} did not exist on the ArTarget of id {1} and was not removed.";
        public const string REMOVE_TAG_FROM_ARTARGET_FAILURE = "An error occured when trying to remove the tag {0} from the ArTarget of id {1}";


        public const string ARTARGET_NULL = "Null ArTarget provided.";
        public const string CHANGE_EXPERIENCE_NAME_ARTARGET_SUCCESS = "ArTarget Name was successfully changed!";
        public const string CHANGE_EXPERIENCE_NAME_ARTARGET_FAILURE = "An error happened while trying to rename the ArTarget to {0}.";

        public const string ADD_PROPERTY_SUCCESS = "The property of key {0} was successfully added to the ArTarget of id {1}.";
        public const string ADD_PROPERTY_FAILURE = "An error occured when trying to add the property of key {0} to the ArTarget of id {1}\n{2}.";

        public const string GET_PROPERTY_SUCCESS = "{0} properties were successfully fetched from the ArTarget of id {1}.";
        public const string GET_PROPERTY_FAILURE = "An error occured when trying to fetch the properties from the ArTarget of id {0}\n{1}.";

        public const string UPDATE_PROPERTY_SUCCESS = "The property of key {0} was successfully updated to the ArTarget of id {1}.";
        public const string UPDATE_PROPERTY_FAILURE = "An error occured when trying to update the property of key {0} to the ArTarget of id {1}\n{2}.";

        public const string DELETE_PROPERTY_SUCCESS = "The property of key {0} was successfully deleted from the ArTarget of id {1}.";
        public const string DELETE_PROPERTY_FAILURE = "An error occured when trying to delete the property of key {0} from the ArTarget of id {1}\n{2}.";

        public const string CHANGE_EXPERIENCE_SHORT_DESCRIPTION_ARTARGET_SUCCESS = "ArTarget short description was successfully changed!";
        public const string CHANGE_EXPERIENCE_SHORT_DESCRIPTION_ARTARGET_FAILURE = "An error happened while trying to change the short description of the ArTarget of id {0}.";

        public const string CHANGE_EXPERIENCE_LONG_DESCRIPTION_ARTARGET_SUCCESS = "ArTarget long description was successfully changed!";
        public const string CHANGE_EXPERIENCE_LONG_DESCRIPTION_ARTARGET_FAILURE = "An error happened while trying to change the long description of the ArTargetof id {0}.";

        public const string CHANGE_EXPERIENCE_COPYRIGHT_INFO_ARTARGET_SUCCESS = "ArTarget copyright info was successfully changed!";
        public const string CHANGE_EXPERIENCE_COPYRIGHT_INFO_ARTARGET_FAILURE = "An error happened while trying to change the copyright info of the ArTarget of id {0}.";

        public const string UPDATE_EXPERIENCE_MEDIA_NULL = "A MediaFile is required in order to replace the media's content.";
        public const string UPDATE_EXPERIENCE_MEDIA_SUCCESS = "Media successfully updated!";
        public const string UPDATE_EXPERIENCE_MEDIA_FAILURE = "An error occured when trying to update the media of the target of id {0}";

        public const string REMOVE_MEDIA_FROM_ARTARGET_NULL = "Null ArTarget provided.";
        public const string REMOVE_MEDIA_FROM_ARTARGET_UNLINK_SUCCESS = "Media successfully unlinked!";
        public const string REMOVE_MEDIA_FROM_ARTARGET_UNLINK_FAILURE = "An error occured when trying to unlink media of id {0}";
        public const string REMOVE_MEDIA_FROM_ARTARGET_DELETE_SUCCESS = "Media of id {0} has successfully been deleted!";
        public const string REMOVE_MEDIA_FROM_ARTARGET_DELETE_FAILURE = "An error occured when trying to delete media of id {0}";

        #endregion

        #region Download Constants

        public static bool allowParallelDownloads = false;
        public const long FILE_UPLOAD_PLACEHOLDER_SIZE = 3000000;

        #endregion

        #region Query Defaults

        /// <summary>
        /// Default query for Asset/List APIs processes.
        /// </summary>
        /// <returns></returns>
        public static Asset_List.QueryContent GetDefaultAssetListQuery()
        {
            return new Asset_List.QueryContent() {
                Page = 0,
                RecordsPerPage = 999,
                IncludeTargetImages = true,
                IncludeMedia = true
            };
        }

        /// <summary>
        /// Default query for ArTarget/List APIs processes.
        /// </summary>
        /// <returns></returns>
        public static ArTarget_List.QueryContent GetDefaultArTargetListQuery()
        {
            return new ArTarget_List.QueryContent() {
                Page = 0,
                RecordsPerPage = 999,
                IncludeTargetImages = true,
                IncludeMedia = true,
                ExcludeMarket = false,
                MarketOnly = false
            };
        }

        #endregion
    }
}