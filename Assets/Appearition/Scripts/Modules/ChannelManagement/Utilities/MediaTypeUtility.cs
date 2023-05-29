// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: MediaTypeHandler.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using Appearition.ArTargetImageAndMedia;
using Appearition.Common.ListExtensions;

namespace Appearition.Common
{
    /// <summary>
    /// Static utility class to help with MediaType queries.
    /// </summary>
    public static class MediaTypeUtility
    {
        #region Creation

        /// <summary>
        /// Create a new instance of a MediaFile using a template MediaType.
        /// </summary>
        /// <param name="mediaType"></param>
        /// <returns></returns>
        public static MediaFile CreateMediaFileFromMediaType(MediaType mediaType)
        {
            //Just the MediaType for now
            return new MediaFile()
            {
                mediaType = mediaType.Name
            };
        }
        
        #endregion

        #region Queries

        #region Find MediaType

        /// <summary>
        /// Returns a list containing the display names of all the active MediaTypes.
        /// </summary>
        /// <returns>The all media types by display name.</returns>
        public static List<string> GetAllMediaTypesByDisplayName(IEnumerable<MediaType> mediaTypes)
        {
            return (from tmpMediaType in mediaTypes
                select tmpMediaType.DisplayName).ToList();
        }

        /// <summary>
        /// Returns a list containing the names (as on the EMS) of all the active MediaTypes.
        /// </summary>
        /// <returns>The all media types by name as on EM.</returns>
        public static List<string> GetAllMediaTypesByNameAsOnEms(IEnumerable<MediaType> mediaTypes)
        {
            return (from tmpMediaType in mediaTypes
                select tmpMediaType.DisplayName).ToList();
        }

        /// <summary>
        /// Fetches the MediaType corresponding to a given display name.
        /// </summary>
        /// <returns>The media type from display name.</returns>
        /// <param name="displayName">Display name.</param>
        /// <param name="mediaTypes"></param>
        public static MediaType FindMediaTypeFromDisplayName(string displayName, IEnumerable<MediaType> mediaTypes)
        {
            return mediaTypes.FirstOrDefault(o => o.DisplayName == displayName);
        }

        /// <summary>
        /// Fetches the MediaType corresponding to a given name as displayed on the EMS.
        /// </summary>
        /// <returns>The media type from EMS name.</returns>
        /// <param name="nameAsOnEms">Name as on EM.</param>
        /// <param name="mediaTypes"></param>
        public static MediaType FindMediaTypeFromEmsName(string nameAsOnEms, IEnumerable<MediaType> mediaTypes)
        {
            return mediaTypes.FirstOrDefault(o => o.Name == nameAsOnEms);
        }

        /// <summary>
        /// From a given extension (which includes the dot, ie ".png"), fetches all MediaTypes that allow that extension.
        /// </summary>
        /// <returns>The media types from extension.</returns>
        /// <param name="extension">Extension.</param>
        /// <param name="mediaTypes"></param>
        public static List<MediaType> FindMediaTypesFromExtension(string extension, IEnumerable<MediaType> mediaTypes)
        {
            return (from tmpType in mediaTypes
                from tmpFileExtension in tmpType.FileValidations
                where tmpFileExtension.ContainsExtension(extension)
                select tmpType).ToList();
        }

        #endregion

        #region Extensions

        /// <summary>
        /// Given a name, whether it's the name as on the EMS or a display name, fetches all the extensions for this MediaType.
        /// By default, the given name is the display name.
        /// </summary>
        /// <returns>Lists all extensions from this mediatype.</returns>
        /// <param name="mediaTypeGivenName">Media type given name.</param>
        /// <param name="mediaTypes"></param>
        /// <param name="isGivenStringDisplayName">If set to <c>true</c> is given string display name.</param>
        public static List<string> FindAllExtensionsFromMediaType(string mediaTypeGivenName, IEnumerable<MediaType> mediaTypes, bool isGivenStringDisplayName = true)
        {
            //Handle for display name
            if (isGivenStringDisplayName)
                return FindAllExtensionsFromMediaType(FindMediaTypeFromDisplayName(mediaTypeGivenName, mediaTypes));

            //Handle for EMS name
            return FindAllExtensionsFromMediaType(FindMediaTypeFromEmsName(mediaTypeGivenName, mediaTypes));
        }

        /// <summary>
        /// For a given MediaType, fetches all the associated extensions, with the dot before the extension.
        /// Can provide a list which will be filled with those new values.
        /// </summary>
        /// <returns>The all extensions from media type.</returns>
        /// <param name="mediaType">Media type.</param>
        /// <param name="allExtensionsContainer">All extensions container.</param>
        public static List<string> FindAllExtensionsFromMediaType(MediaType mediaType, List<string> allExtensionsContainer = null)
        {
            if (mediaType == null || mediaType.FileValidations == null)
                return new List<string>();

            if (allExtensionsContainer == null)
                allExtensionsContainer = new List<string>();
            else
                allExtensionsContainer.Clear();

            for (int i = 0; i < mediaType.FileValidations.Count; i++)
            {
                string[] tmpExtensions = mediaType.FileValidations[i].GetFileExtensions(true);
                if (tmpExtensions != null && tmpExtensions.Length > 0)
                    allExtensionsContainer.AddRange(tmpExtensions);
            }

            //Remove duplicates
            allExtensionsContainer.HiziRemoveDuplicates();
            return allExtensionsContainer;
        }

        #endregion

        #region Mime Types

        /// <summary>
        /// From a given MediaType and an extension, finds the MimeType as on the EMS.
        /// </summary>
        /// <returns>The MIME type from extension.</returns>
        /// <param name="mediaType">Media type.</param>
        /// <param name="extension">Extension.</param>
        public static string FindMimeTypeFromExtension(MediaType mediaType, string extension)
        {
            if (mediaType == null)
                return "";

            return mediaType.GetMimeTypeForGivenExtension(extension);
        }

        #endregion

        #endregion
    }
}