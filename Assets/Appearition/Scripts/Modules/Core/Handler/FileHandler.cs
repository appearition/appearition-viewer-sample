// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: FileHandler.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using System;
using System.Collections;
using Appearition.Common;

namespace Appearition
{
    /// <summary>
    /// Handle in charge of exposing the file functionalities, such as loading, saving, copying and deleting.
    /// This handler is to simplify the process of non-module file loading.
    /// If you wish to load the content of an experience, please refer to ArTargetHandler.LoadMediaFileContent(asset,mediafile,Action<byte[]>);
    /// </summary>
    public class FileHandler : BaseHandler
    {
        #region Get 

        /// <summary>
        /// Fetches the content of a local file at a given path.
        /// </summary>
        /// <param name="pathToFile">Full path to the file to load.</param>
        /// <param name="onComplete">Called once the file loading process has completed. Contains the file content.</param>
        public static void LoadBytesFromFile(string pathToFile, Action<byte[]> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(LoadBytesFromFileProcess(pathToFile, onComplete));
        }

        /// <summary>
        /// Fetches the content of a local file at a given path.
        /// </summary>
        /// <param name="pathToFile">Full path to the file to load.</param>
        /// <param name="onComplete">Called once the file loading process has completed. Contains the file content.</param>
        public static IEnumerator LoadBytesFromFileProcess(string pathToFile, Action<byte[]> onComplete = null)
        {
            yield return GetContentFromFileProcess(pathToFile, onComplete);
        }

        #endregion

        #region Save

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="pathToFile"></param>
        /// <param name="onComplete"></param>
        public static void SaveBytesToFile(byte[] data, string pathToFile, Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(SaveBytesToFileProcess(data, pathToFile, onComplete));
        }

        /// <summary>
        /// Saves given content to a file at a given full path.
        /// </summary>
        /// <param name="data">The content that the file should contain.</param>
        /// <param name="pathToFile">The full path to the file, including filename and extension.</param>
        /// <param name="onComplete">Called once the process is complete. Includes whether or not the action was successful.</param>
        public static IEnumerator SaveBytesToFileProcess(byte[] data, string pathToFile, Action<bool> onComplete = null)
        {
            yield return SaveContentToFileProcess(data, pathToFile, onComplete);
        }

        #endregion

        #region Copy

        /// <summary>
        /// Copies a local file at a given path to a destination at a given path.
        /// </summary>
        /// <param name="pathToSourceFile">The full path to the original file, including filename and extension.</param>
        /// <param name="pathToDestinationFile">The full path to the destination file location, including filename and extension.</param>
        /// <param name="onComplete">Called once the process is complete. Includes whether or not the action was successful.</param>
        public static void CopyExistingFileToDestination(string pathToSourceFile, string pathToDestinationFile, Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(CopyExistingFileToDestinationProcess(pathToSourceFile, pathToDestinationFile, onComplete));
        }

        /// <summary>
        /// Copies a local file at a given path to a destination at a given path.
        /// </summary>
        /// <param name="pathToSourceFile">The full path to the original file, including filename and extension.</param>
        /// <param name="pathToDestinationFile">The full path to the destination file location, including filename and extension.</param>
        /// <param name="onComplete">Called once the process is complete. Includes whether or not the action was successful.</param>
        public static IEnumerator CopyExistingFileToDestinationProcess(string pathToSourceFile, string pathToDestinationFile, Action<bool> onComplete = null)
        {
            yield return CopyFileToDestinationProcess(pathToSourceFile, pathToDestinationFile, onComplete);
        }

        #endregion

        #region Delete

        /// <summary>
        /// Deletes the file at a given location.
        /// </summary>
        /// <param name="pathToFile">The full path to the file, including filename and extension.</param>
        /// <param name="onComplete">Called once the process is complete. Includes whether or not the action was successful.</param>
        public static void DeleteExistingFile(string pathToFile, Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(DeleteExistingFileProcess(pathToFile, onComplete));
        }

        /// <summary>
        /// Deletes the file at a given location.
        /// </summary>
        /// <param name="pathToFile">The full path to the file, including filename and extension.</param>
        /// <param name="onComplete">Called once the process is complete. Includes whether or not the action was successful.</param>
        public static IEnumerator DeleteExistingFileProcess(string pathToFile, Action<bool> onComplete = null)
        {
            yield return DeleteFileProcess(pathToFile, onComplete);
        }

        #endregion
    }
}