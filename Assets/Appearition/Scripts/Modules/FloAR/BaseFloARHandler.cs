// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: BaseFloARHandler.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Appearition.API;
using Appearition.Common.ObjectExtensions;
using UnityEngine;

namespace Appearition.Common
{
    /// <summary>
    /// Base class for a FloAR-like handler. Contains utilities to deal with handlers of that generation.
    /// </summary>
    public class BaseFloARHandler : BaseHandler
    {
        #region Document Saving/Loading and Utilities

        #region Paths

        /// <summary>
        /// Using a given Document Data, generates a filename that will be used for the json data stored locally.
        /// </summary>
        /// <typeparam name="T">The type of document.</typeparam>
        /// <param name="document"></param>
        /// <returns>The filename of the data of a given document.</returns>
        protected static string GetDocumentDataFileName<T>(T document) where T : BaseDocument
        {
            return string.Format("id{0}_docName-{1}_v{2}.txt", document.DocumentId, document.DocumentName, document.VersionNo);
        }

        /// <summary>
        /// Returns the expected full path to the data file of a given document. 
        /// </summary>
        /// <typeparam name="T">The type of document.</typeparam>
        /// <typeparam name="K">The type of handler</typeparam>
        /// <param name="document">The document content.</param>
        /// <returns>The expected full path to the data file of a given document.</returns>
        public static string GetDocumentDataFullPath<T, K>(T document) where T : BaseDocument where K : BaseFloARHandler
        {
            return string.Format("{0}/{1}", GetHandlerStoragePath<K>(), GetDocumentDataFileName<T>(document));
        }

        /// <summary>
        /// Returns the expected full path to a given document. 
        /// </summary>
        /// <typeparam name="T">The type of document.</typeparam>
        /// <typeparam name="K">The type of handler</typeparam>
        /// <param name="document">The document content.</param>
        /// <returns>The expected full path to the given document.</returns>
        public static string GetDocumentFullPath<T, K>(T document) where T : BaseDocument where K : BaseFloARHandler
        {
            return string.Format("{0}/{1}", GetHandlerStoragePath<K>(), document.FileName);
        }

        #endregion

        /// <summary>
        /// Saves the data of a specific document locally for offline usage and download efficiency purpose.
        /// </summary>
        /// <typeparam name="T">Document Type</typeparam>
        /// <typeparam name="K">Handler class</typeparam>
        /// <param name="documentData"></param>
        protected static void SaveDocumentDataLocally<T, K>(T documentData) where T : BaseDocument, new() where K : BaseFloARHandler
        {
            if (!AppearitionConstants.enableApiResponseStorage || Application.platform == RuntimePlatform.WebGLPlayer)
                return;

            string jsonFolderPath = GetHandlerJsonFolderPath<K>();

            if (!Directory.Exists(jsonFolderPath))
                Directory.CreateDirectory(jsonFolderPath);
            
            string jsonFileName = GetDocumentDataFileName(documentData);

            if (string.IsNullOrEmpty(jsonFileName))
            {
                AppearitionLogger.LogError("Please provide a valid filename to save JSON data.");
                return;
            }

            string data = AppearitionConstants.SerializeJson(documentData);

            string jsonFullPath = string.Format("{0}/{1}", jsonFolderPath, jsonFileName);

            //Encrypt if required
            if (AppearitionConstants.shouldEncryptJson)
                data = AppearitionConstants.EncryptData(data);

            File.WriteAllText(jsonFullPath, data);
        }

        /// <summary>
        /// Using a given Document Data object, loads the existing matching Document Data, if any.
        /// </summary>
        /// <typeparam name="T">Document Type</typeparam>
        /// <typeparam name="K">Handler class</typeparam>
        /// <param name="documentData">Document Data</param>
        /// <returns></returns>
        protected static T LoadDocumentLocalData<T, K>(T documentData) where T : BaseDocument where K : BaseFloARHandler
        {
            if (!AppearitionConstants.enableApiResponseStorage || Application.platform == RuntimePlatform.WebGLPlayer)
                return null;
            
            string jsonFileName = GetDocumentDataFileName(documentData);

            string jsonFolderPath = GetHandlerJsonFolderPath<K>();
            string jsonFullPath = string.Format("{0}/{1}", jsonFolderPath, jsonFileName);

            //No file, no data
            if (!File.Exists(jsonFullPath))
                return null;

            string jsonData = null;

            try
            {
                jsonData = File.ReadAllText(jsonFullPath);

                if (AppearitionConstants.shouldEncryptJson)
                    jsonData = AppearitionConstants.DecryptData(jsonData);
            }
            catch
            {
                AppearitionLogger.LogError("An error occured when trying to read JSON data. Was data in this file not text?");
            }

            if (string.IsNullOrEmpty(jsonFileName))
                return null;

            T outcome = null;

            try
            {
                outcome = AppearitionConstants.DeserializeJson<T>(jsonData);
            } catch
            {
                AppearitionLogger.LogError("An error happened when trying to deserialize the following Document Data: " + jsonData);
            }

            return outcome;
        }

        /// <summary>
        /// Whether or not this document was previously downloaded and exists locally.
        /// </summary>
        /// <typeparam name="T">Document Type</typeparam>
        /// <typeparam name="K">Handler class</typeparam>
        /// <param name="document">Document Data</param>
        /// <returns>Whether or not a local version of the document exists and has a matching version.</returns>
        protected static bool DoesDocumentExistsAndMatchesLocalVersion<T, K>(T document) where T : BaseDocument where K : BaseFloARHandler
        {
            //Firstly, load the local data, if any.
            T localDocumentData = LoadDocumentLocalData<T, K>(document);

            if (localDocumentData != null)
            {
                //If a document was found, compare the version numbers. If equal, just load the local document.
                if (localDocumentData.VersionNo == document.VersionNo)
                {
                    if (File.Exists(GetDocumentFullPath<T, K>(document)))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Loads the content of a document into memory, located inside the document object which will be updated.
        /// In case of failure, will return false.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="K"></typeparam>
        /// <param name="document">Original document</param>
        /// <param name="onComplete"></param>
        protected static IEnumerator LoadLocalDocumentOntoMemoryInDocumentContent<T, K>(T document, Action<byte[]> onComplete = null) where T : BaseDocument where K : BaseFloARHandler
        {
            //Firstly, load the local data, if any.
            byte[] loadedBytes = null;
            yield return GetContentFromFileProcess(GetDocumentFullPath<T, K>(document), bytes => loadedBytes = bytes);

            if (loadedBytes != null)
            {
                document.DocumentContent = loadedBytes.ToStream();
                AppearitionLogger.LogInfo("File successfully loaded locally.");
            }
            else
                AppearitionLogger.LogInfo("No matching document was found or loaded properly.");

            //Callback null if the document failed loading.
            if (onComplete != null)
                onComplete(loadedBytes);
        }

        /// <summary>
        /// Deletes both the document file and the document data json, if they exist.
        /// </summary>
        /// <typeparam name="T">Document Type</typeparam>
        /// <typeparam name="K">Handler class</typeparam>
        /// <param name="document">Document data</param>
        protected static IEnumerator DeleteDocumentAndDocumentJsonData<T, K>(T document) where T : BaseDocument where K : BaseFloARHandler
        {
            //Find the document based on its filename.
            string fullPathToDocument = GetDocumentFullPath<T, K>(document);

            if (File.Exists(fullPathToDocument))
                yield return DeleteFileProcess(fullPathToDocument);

            //Then, delete the document data json
            string jsonFullPath = GetDocumentDataFullPath<T, K>(document);

            if (File.Exists(jsonFullPath))
                yield return DeleteFileProcess(jsonFullPath);
        }

        #endregion
    }
}