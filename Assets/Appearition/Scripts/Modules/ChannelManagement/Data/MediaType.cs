// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: MediaType.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System;
using System.Diagnostics;
using System.Linq;
using Appearition.ArTargetImageAndMedia;
using Appearition.Common.TypeExtensions;

namespace Appearition.Common
{
    /// <summary>
    /// Base container class for a mediatype definition. 
    /// A MediaType contains information about the type of media of a specific experience. 
    /// This includes how to display the media, but also how to load and upload it.
    /// </summary>
    [Serializable]
    public class MediaType
    {
        //Variables
        public string Name;
        public int MediaTypeId;
        public string DataProviderName;
        public bool ShowPlatform;
        public bool IsDataQuery;
        public bool ShowDisplayMode;
        public bool ShowLanguage;
        public bool ShowFileUpload;
        public string FileLabel;
        public bool ShowUrl;
        public string UrlLabel;
        public bool ShowSingleLineText;
        public bool ShowMultiLineText;
        public int MaxTextChars;
        public int MaxTransformTemplateChars;
        public List<OtherProperty> OtherProperties;
        public List<PropertyDefault> PropertyDefaults;
        public List<FileValidation> FileValidations;

        #region Extras

        private string _displayName;

        public virtual string DisplayName
        {
            get
            {
                if (string.IsNullOrEmpty(_displayName))
                    _displayName = Name.FirstCharToUpper();
                return _displayName;
            }
        }

        #endregion

        public MediaType()
        {
        }

        /// <summary>
        /// Copy Constructor
        /// </summary>
        /// <param name="cc">C.</param>
        public MediaType(MediaType cc)
        {
            Name = cc.Name;
            MediaTypeId = cc.MediaTypeId;
            DataProviderName = cc.DataProviderName;
            ShowPlatform = cc.ShowPlatform;
            IsDataQuery = cc.IsDataQuery;
            ShowDisplayMode = cc.ShowDisplayMode;
            ShowLanguage = cc.ShowLanguage;
            ShowFileUpload = cc.ShowFileUpload;
            FileLabel = cc.FileLabel;
            ShowUrl = cc.ShowUrl;
            UrlLabel = cc.UrlLabel;
            ShowSingleLineText = cc.ShowSingleLineText;
            ShowMultiLineText = cc.ShowMultiLineText;
            MaxTextChars = cc.MaxTextChars;
            MaxTransformTemplateChars = cc.MaxTransformTemplateChars;
            OtherProperties = new List<OtherProperty>(cc.OtherProperties);
            PropertyDefaults = new List<PropertyDefault>(cc.PropertyDefaults);
            FileValidations = new List<FileValidation>(cc.FileValidations);
        }

        //SubClasses
        [Serializable]
        public class OtherProperty
        {
            //Variables
            public int Id;
            public string MediaTypeName;
            public string DisplayMode;
            public bool ShowCustom;
            public bool ShowChecksum;
            public bool ShowAutoPlay;
            public bool ShowPreDownload;
            public bool ShowInteractive;
            public bool ShowAnimationName;
            public bool ShowTranslation;
            public bool ShowRotation;
            public bool ShowScale;

            public OtherProperty()
            {
            }

            /// <summary>
            /// Copy Constructor
            /// </summary>
            /// <param name="cc">C.</param>
            public OtherProperty(OtherProperty cc)
            {
                Id = cc.Id;
                MediaTypeName = cc.MediaTypeName;
                DisplayMode = cc.DisplayMode;
                ShowCustom = cc.ShowCustom;
                ShowChecksum = cc.ShowChecksum;
                ShowAutoPlay = cc.ShowAutoPlay;
                ShowPreDownload = cc.ShowPreDownload;
                ShowInteractive = cc.ShowInteractive;
                ShowAnimationName = cc.ShowAnimationName;
                ShowTranslation = cc.ShowTranslation;
                ShowRotation = cc.ShowRotation;
                ShowScale = cc.ShowScale;
            }
        }

        /// <summary>
        /// Contains the default properties of a MediaFile of this MediaType
        /// </summary>
        [Serializable]
        public class PropertyDefault
        {
            //Variables
            public int Id;
            public string MediaTypeName;
            public string DisplayMode;
            public string PlatformType;
            public string Checksum;
            public bool IsAutoPlay;
            public bool IsPreDownload;
            public bool IsInteractive;
            public string AnimationName;
            public float TranslationX;
            public float TranslationY;
            public float TranslationZ;
            public float RotationX;
            public float RotationY;
            public float RotationZ;
            public float ScaleX;
            public float ScaleY;
            public float ScaleZ;

            public PropertyDefault()
            {
            }

            /// <summary>
            /// Copy Constructor
            /// </summary>
            /// <param name="cc">C.</param>
            public PropertyDefault(PropertyDefault cc)
            {
                Id = cc.Id;
                MediaTypeName = cc.MediaTypeName;
                DisplayMode = cc.DisplayMode;
                PlatformType = cc.PlatformType;
                Checksum = cc.Checksum;
                IsAutoPlay = cc.IsAutoPlay;
                IsPreDownload = cc.IsPreDownload;
                IsInteractive = cc.IsInteractive;
                AnimationName = cc.AnimationName;
                TranslationX = cc.TranslationX;
                TranslationY = cc.TranslationY;
                TranslationZ = cc.TranslationZ;
                RotationX = cc.RotationX;
                RotationY = cc.RotationY;
                RotationZ = cc.RotationZ;
                ScaleX = cc.ScaleX;
                ScaleY = cc.ScaleY;
                ScaleZ = cc.ScaleZ;
            }

            public void ApplyDefaultsOnMediafile(MediaFile media)
            {
                if (media == null)
                    return;

                media.animationName = AnimationName;
                media.checksum = Checksum;
                media.isAutoPlay = IsAutoPlay;
                media.isInteractive = IsInteractive;
                media.isPreDownload = IsPreDownload;
                media.isTracking = !DisplayMode.ToLower().Equals("fullscreen");
                media.translationX = TranslationX;
                media.translationY = TranslationY;
                media.translationZ = TranslationZ;
                media.rotationX = RotationX;
                media.rotationY = RotationY;
                media.rotationZ = RotationZ;
                media.scaleX = ScaleX;
                media.scaleY = ScaleY;
                media.scaleZ = ScaleZ;
                media.language = media.custom = media.text = "";
                media.resolution = 0;
            }
        }

        /// <summary>
        /// Used to determine whether the MediaFile is respecting the EMS's rulesets for this MediaType.
        /// </summary>
        [Serializable]
        public class FileValidation
        {
            //Variables
            public int Id;
            public string MediaTypeName;
            public string PlatformType;
            public bool AllowFileNameWhitespace;
            public bool AllowSameNamedFile;
            public bool CheckForInvalidFileNameChars;
            public string InvalidFileNameCharsCsv;
            public string ValidFileNameExtensionCsv;
            public string ValidFileMimeTypeCsv;
            public long ValidFileSizeInBytes;
            public long ValidFileSizeInKBytes;
            public long ValidFileSizeInMBytes;
            public int MinWidth;
            public int MaxWidth;
            public int MinHeight;
            public int MaxHeight;
            public string Notes;

            public FileValidation()
            {
            }

            /// <summary>
            /// Copy Constructor
            /// </summary>
            /// <param name="cc">C.</param>
            public FileValidation(FileValidation cc)
            {
                Id = cc.Id;
                MediaTypeName = cc.MediaTypeName;
                PlatformType = cc.PlatformType;
                AllowFileNameWhitespace = cc.AllowFileNameWhitespace;
                AllowSameNamedFile = cc.AllowSameNamedFile;
                CheckForInvalidFileNameChars = cc.CheckForInvalidFileNameChars;
                InvalidFileNameCharsCsv = cc.InvalidFileNameCharsCsv;
                ValidFileNameExtensionCsv = cc.ValidFileNameExtensionCsv;
                ValidFileMimeTypeCsv = cc.ValidFileMimeTypeCsv;
                ValidFileSizeInBytes = cc.ValidFileSizeInBytes;
                ValidFileSizeInKBytes = cc.ValidFileSizeInKBytes;
                ValidFileSizeInMBytes = cc.ValidFileSizeInMBytes;
                MinWidth = cc.MinWidth;
                MaxWidth = cc.MaxWidth;
                MinHeight = cc.MinHeight;
                MaxHeight = cc.MaxHeight;
                Notes = cc.Notes;
            }

            #region Utilities

            /// <summary>
            /// Returns a container with all available mimetypes for this file validation.
            /// </summary>
            /// <returns>The MIME types.</returns>
            public string[] GetMimeTypes()
            {
                if (ValidFileMimeTypeCsv == null)
                    return null;
                return ValidFileMimeTypeCsv.DeserializeCSV();
            }

            /// <summary>
            /// From a given extension, finds and fetches a mimetype.
            /// </summary>
            /// <returns>The MIME type for extension.</returns>
            /// <param name="extension">Extension.</param>
            public string GetMimeTypeForExtension(string extension)
            {
                if (string.IsNullOrEmpty(extension))
                    return null;

                if (extension[0].Equals('.'))
                    extension = extension.Substring(1);

                //Check if contains the extension to begin with
                if (!ContainsExtension(extension))
                    return null;

                //Find it in the mimeTypes
                string[] mimeTypes = GetMimeTypes();

                if (mimeTypes == null || mimeTypes.Length == 0)
                    return null;

                for (int i = 0; i < mimeTypes.Length; i++)
                {
                    if (mimeTypes[i] == null || mimeTypes[i].Length == 0)
                        continue;

                    string[] splits = mimeTypes[i].Split(new[] {'/', '\\'}, StringSplitOptions.RemoveEmptyEntries);

                    if (splits.Length > 0)
                    {
                        if (splits[1].Equals(extension))
                            return mimeTypes[i];
                    }
                }

                return null;
            }

            /// <summary>
            /// Returns a list of all the file extensions allowed for this file validation.
            /// </summary>
            /// <returns>The file extensions.</returns>
            /// <param name="includeDot">eg .mp4</param>
            /// <param name="includeStar"> eg *.mp4</param>
            public string[] GetFileExtensions(bool includeDot = true, bool includeStar = false)
            {
                if (ValidFileNameExtensionCsv == null)
                    return null;
                string[] output = ValidFileNameExtensionCsv.DeserializeCSV();

                for (int i = 0; i < output.Length; i++)
                {
                    bool alreadyHasDot = output[i].IndexOf('.') > 0;
                    if (alreadyHasDot)
                        output[i] = $"{(includeStar && includeDot ? "*" : "")}{(!includeDot ? output[i].Substring(1) : "")}";
                    else
                        output[i] = $"{(includeStar && includeDot ? "*" : "")}{(includeDot ? "." : "")}{output[i]}";
                }

                return output;
            }

            /// <summary>
            /// Returns whether or not this File Validation contains the given extension as a rule.
            /// </summary>
            /// <returns><c>true</c>, if extension was containsed, <c>false</c> otherwise.</returns>
            /// <param name="extension">Extension.</param>
            public bool ContainsExtension(string extension)
            {
                string[] allExtensions = GetFileExtensions(false);

                //If both have empty content, it technically is a valid extension..
                if (string.IsNullOrEmpty(extension) && (allExtensions == null || allExtensions.Length == 0))
                    return true;

                //If both aren't empty, then do a valid check.
                if ((allExtensions == null || allExtensions.Length == 0) || string.IsNullOrEmpty(extension))
                    return false;

                if (extension[0].Equals('.'))
                    extension = extension.Substring(1);

                return allExtensions.Any(o => o.Equals(extension));
            }

            /// <summary>
            /// Returns all the invalid characters a filename cannot have, as a char array.
            /// </summary>
            /// <returns></returns>
            public char[] GetAllInvalidCharacters()
            {
                if (InvalidFileNameCharsCsv == null)
                    return null;
                string[] invalidCharactersString = InvalidFileNameCharsCsv.DeserializeCSV();

                char[] output = new char[invalidCharactersString.Length];

                for (int i = 0; i < invalidCharactersString.Length; i++)
                {
                    try
                    {
                        output[i] = Convert.ToChar(invalidCharactersString[i]);
                    } catch
                    {
                        // ignored
                    }
                }

                return output;
            }

            /// <summary>
            /// Handle the file validation for this specific file validation content.
            /// The MediaType should be having the final word on whether the file is valid or not.
            /// </summary>
            /// <returns><c>true</c> if this instance is file valid the specified fileNameWithExtension fileSizeInBytes; otherwise, <c>false</c>.</returns>
            /// <param name="fileNameWithExtension">File name with extension.</param>
            /// <param name="fileSizeInBytes">File size in bytes.</param>
            public bool IsFileValid(string fileNameWithExtension, int fileSizeInBytes)
            {
                //No file = valid file?
                if (string.IsNullOrEmpty(fileNameWithExtension))
                    return true;

                //Fetch the different names
                string extension = System.IO.Path.GetExtension(fileNameWithExtension);
                //string fileName = System.IO.Path.GetFileNameWithoutExtension (fileNameWithExtension);

                if (string.IsNullOrEmpty(extension))
                    return false;

                bool outcome = true;

                if (ContainsExtension(extension))
                {
                    //Check the name. NVM LET THE EMS HANDLE THAT
                    if (!AllowFileNameWhitespace && fileNameWithExtension.Contains(' '))
                        outcome = false;

                    char[] invalidCharacters = GetAllInvalidCharacters();
                    if (invalidCharacters != null && invalidCharacters.Length > 0)
                    {
                        if (fileNameWithExtension.Any(o => invalidCharacters.Contains(o)))
                            outcome = false;
                    }


                    //Check the fileWeight
                    if (outcome && fileSizeInBytes > ValidFileSizeInBytes)
                        outcome = false;

                    //Final check
                    if (outcome)
                        return true;
                }

                //Debug.LogWarning(string.Format("The given file\"{0}\"'s properties did not match any FileValidation systems for the MediaType {1}", fileNameWithExtension);
                return false;
            }

            #endregion
        }

        #region Media Handling

        #endregion

        #region Utilities

        /// <summary>
        /// Returns of all the extensions valid for this MediaType.
        /// </summary>
        /// <returns>The all possible extensions.</returns>
        public List<string> GetAllPossibleExtensions(bool removeExtensionDot = false)
        {
            List<string> output = new List<string>();

            if (FileValidations != null)
            {
                for (int i = 0; i < FileValidations.Count; i++)
                {
                    if (FileValidations[i] != null)
                    {
                        string[] validExtensions = FileValidations[i].GetFileExtensions(!removeExtensionDot);
                        if (validExtensions != null && validExtensions.Length > 0)
                            output.AddRange(validExtensions);
                    }
                }
            }

            return output;
        }

        /// <summary>
        /// Given an extension, returns the associated mime type.
        /// </summary>
        /// <returns>The MIME type for given extension.</returns>
        /// <param name="extension">Extension.</param>
        public string GetMimeTypeForGivenExtension(string extension)
        {
            if (FileValidations == null || extension == null)
                return null;

            for (int i = 0; i < FileValidations.Count; i++)
            {
                if (FileValidations[i] == null)
                    continue;

                string tmpMimeType = FileValidations[i].GetMimeTypeForExtension(extension);
                if (tmpMimeType != null && tmpMimeType.Length > 0)
                    return tmpMimeType;
            }

            return null;
        }

        /// <summary>
        /// Shortcut to fetch the max upload file size in bytes.
        /// </summary>
        /// <returns></returns>
        public long GetMaxSizeInBytes()
        { 
            //Default to 3mb if no file validation.
            if(FileValidations == null || FileValidations.Count == 0)
                return 3000000;
            return FileValidations.First().ValidFileSizeInBytes;
        }

        #endregion
    }
}