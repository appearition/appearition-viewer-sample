using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Appearition.Common
{
    /// <summary>
    /// Generic database for API response, featuring both an override or additive data implementation.
    /// This system does not support removing specific items, but upon clearing the cache, its content will be cleared off.
    /// 
    /// </summary>
    /// <typeparam name="T">Object Type</typeparam>
    /// <typeparam name="K">Handler Type</typeparam>
    public static class CustomApiStorageDatabase<T,K> where K : BaseHandler
    {
        #region Defines

        static string PathToDatabase => $"{BaseHandler.GetHandlerStoragePath<K>()}/{typeof(T).Name}Database.json";

        [System.Serializable]
        public class DatabaseContent
        {
            public List<T> content = new List<T>();
        }

        #endregion

        #region Common

        static List<T> LoadDatabase()
        {
            List<T> outcome = new List<T>();

            if (!AppearitionConstants.enableApiResponseStorage)
                return outcome;

            if (File.Exists(PathToDatabase))
            {
                var databaseJson = File.ReadAllText(PathToDatabase);

                if (!string.IsNullOrEmpty(databaseJson))
                {
                    DatabaseContent database;

                    try
                    {
                        database = AppearitionConstants.DeserializeJson<DatabaseContent>(AppearitionConstants.shouldEncryptJson
                            ? AppearitionConstants.DecryptData(databaseJson)
                            : databaseJson);
                    } catch
                    {
                        database = null;
                    }

                    if (database != null && database.content != null && database.content.Count > 0)
                        outcome.AddRange(database.content);
                }
            }

            return outcome;
        }

        static void SaveDatabase(List<T> content)
        {
            if (!AppearitionConstants.enableApiResponseStorage || content == null)
                return;

            if (File.Exists(PathToDatabase))
                File.Delete(PathToDatabase);

            DatabaseContent database = new DatabaseContent() {content = content};

            string json = AppearitionConstants.SerializeJson(database);
            File.WriteAllText(PathToDatabase, AppearitionConstants.shouldEncryptJson ? AppearitionConstants.EncryptData(json) : json);
        }

        #endregion

        #region Write

        /// <summary>
        /// Override the current stored content of the database with newly provided content.
        /// </summary>
        /// <param name="newContent"></param>
        public static void UpdateStorageWithNewLiveContent(List<T> newContent)
        {
            UpdateStorageWithNewLiveContent(newContent, null);
        }

        /// <summary>
        /// Updates the content of the local database using a comparer. If no existing item was found, will add a new one.
        /// This system doesn't support removing items; but upon clearing the cache, the database will be reset.
        /// </summary>
        /// <param name="newContent"></param>
        /// <param name="equalComparer"></param>
        public static void UpdateStorageWithNewLiveContent(List<T> newContent, Func<T, T, bool> equalComparer)
        {
            List<T> allContent = new List<T>();
            if (equalComparer == null)
            {
                //Override all, and save
                allContent.AddRange(newContent);
            }
            else
            {
                //Based on the comparer, override or add new content.
                allContent = LoadDatabase();
                var newBatch = new List<T>();
                for (int i = 0; i < newContent.Count; i++)
                {
                    bool hasFoundExisting = false;
                    for (int k = 0; k < allContent.Count; k++)
                    {
                        if (equalComparer.Invoke(newContent[i], allContent[k]))
                        {
                            allContent[k] = newContent[i];
                            hasFoundExisting = true;
                            break;
                        }
                    }
                    
                    if(!hasFoundExisting)
                        newBatch.Add(newContent[i]);
                }
                
                //Add the new batch
                allContent.AddRange(newBatch);
            }

            //Save the content locally
            SaveDatabase(allContent);
        }

        #endregion

        #region Read

        /// <summary>
        /// Load the content of the database.
        /// </summary>
        /// <returns></returns>
        public static List<T> GetStoredContent()
        {
            return LoadDatabase();
        }

        #endregion
    }
}