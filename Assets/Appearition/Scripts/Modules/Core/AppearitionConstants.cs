// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: AppearitionConstants.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using System;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace Appearition
{
    /// <summary>
    /// Different type of AR technologies which might be used through apps.
    /// </summary>
    public enum ArMode
    {
        None,
        Marker,
        Markerless,
        Location,
        FaceRecognition
    }

    /// <summary>
    /// Repertory of all the constants used throughout the Appearition SDK.
    /// This utility class avoids hard-coding any names, and allows to change any in a single place.
    /// </summary>
    public static class AppearitionConstants
    {
        #region File Saving

        /// <summary>
        /// The root directory where all the cloud data will be saved.
        /// Each Handler has their own way of saving data. If you want a different file structure, the first region of each module (where applicable) resolves around file structure.
        /// It is possible that the handler itself has a parent class which derives from the BaseHandler, in which case, this parent class will be in charge of the file structure.
        /// </summary>
        public static string RootDirectory
        {
            get { return Application.persistentDataPath; }
        }

        /// <summary>
        /// Allows the APIs to be stored locally to enable offline capability.
        /// </summary>
        public static bool enableApiResponseStorage = true;
        
        /// <summary>
        /// Whether cloud files should get downloaded locally.
        /// </summary>
        public static bool enableLocalFileStorage = true;

        #endregion

        #region Json Serialization

        /// <summary>
        /// Whether or not the JSON files should be encrypted or left as UTF8 text. If true, you can change the encryption key stored in JSON_ENCRYPTION_KEY.
        /// </summary>
        public static bool shouldEncryptJson = false;

        /// <summary>
        /// If EncryptJsonFiles is true, this key will be used to md5 encrypt and decrypt the JSON files.
        /// </summary>
        public const string JSON_ENCRYPTION_KEY = "SuperToughMd5EncryptionKey";

        /// <summary>
        /// Method of deserialization to JSON used for requests.
        /// If you want to use a different plugin to deserialize JSON, change the content of this method.
        /// Do note that JSON encryption (if any) happens at a different location.
        /// </summary>
        /// <returns>The deserialized class from the given JSON.</returns>
        /// <param name="json">The JSON data.</param>
        /// <typeparam name="T">The type of the object to be deserialized.</typeparam>
        public static T DeserializeJson<T>(string json)
        {
            //return JsonConvert.DeserializeObject<T> (json, new JsonSerializerSettings {
            //	NullValueHandling = NullValueHandling.Ignore
            //});
            return JsonUtility.FromJson<T>(json);
        }

        /// <summary>
        /// Method of serialization to JSON used for requests.
        /// If you want to use a different plugin to serialize JSON, change the content of this method.
        /// Do note that JSON encryption (if any) happens at a different location.
        /// </summary>
        /// <returns>The JSON.</returns>
        /// <param name="obj">The object to be serialized.</param>
        /// <typeparam name="T">The type of the object to be serialized.</typeparam>
        public static string SerializeJson<T>(T obj)
        {
            //return JsonConvert.SerializeObject (obj);
            return JsonUtility.ToJson(obj);
        }

        #endregion

        #region JSON File Encryption

        /// <summary>
        /// Encrypts the given data using MD5 encryption, using the encryptionKey as a key.
        /// </summary>
        /// <returns>The data.</returns>
        /// <param name="data">Data.</param>
        public static string EncryptData(string data)
        {
            //Prepare data
            byte[] byteData = UTF8Encoding.UTF8.GetBytes(data);

            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                //Prepare key
                byte[] key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(AppearitionConstants.JSON_ENCRYPTION_KEY));

                using (TripleDESCryptoServiceProvider tripleDes = new TripleDESCryptoServiceProvider() {
                    Key = key,
                    Mode = CipherMode.ECB,
                    Padding = PaddingMode.PKCS7
                })
                {
                    //Prepare and encrypt final data
                    ICryptoTransform cryptoTransform = tripleDes.CreateEncryptor();
                    byte[] output = cryptoTransform.TransformFinalBlock(byteData, 0, byteData.Length);

                    //Finally, output encrypted data
                    return Convert.ToBase64String(output);
                }
            }
        }

        /// <summary>
        /// Decrypts the given data using MD5 encryption, using the encryptionKey as a key.
        /// </summary>
        /// <returns>The data.</returns>
        /// <param name="data">Data.</param>
        public static string DecryptData(string data)
        {
            //Prepare data
            byte[] byteData = Convert.FromBase64String(data);

            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                //Prepare key
                byte[] key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(AppearitionConstants.JSON_ENCRYPTION_KEY));

                using (TripleDESCryptoServiceProvider tripleDes = new TripleDESCryptoServiceProvider() {
                    Key = key,
                    Mode = CipherMode.ECB,
                    Padding = PaddingMode.PKCS7
                })
                {
                    //Prepare and decrypt final data
                    ICryptoTransform cryptoTransform = tripleDes.CreateDecryptor();
                    byte[] output = cryptoTransform.TransformFinalBlock(byteData, 0, byteData.Length);

                    //Finally, output decrypted data
                    return UTF8Encoding.UTF8.GetString(output);
                }
            }
        }

        #endregion

        #region EMS SETTINGS

        /// <summary>
        /// Delay before an EMS request is considered as timeout.
        /// </summary>
        public const int EMS_PING_TIMEOUT_SECONDS = 30;

        /// <summary>
        /// Delay between each check to ensure the program can reach the EMS.
        /// </summary>
        public const int DELAY_BETWEEN_EACH_EMS_REACHABILITY_CHECK_IN_SECONDS = 50;

        public const string EMS_UNREACHABLE_ERROR_MESSAGE = "This process requires internet connection in order to work. Please try again later.";

        public const string EMS_UNREACHABLE_NO_LOCAL_DATA_ERROR_MESSAGE = "Internet is not reachable and no offline ApiData is available. No asset can be found.";

        #endregion

        #region Profile Settings

        /// <summary>
        /// Key used in the UserProfile authenticationTokens dictionary to access the Anonymous token directly.
        /// </summary>
        public const string PROFILE_APPLICATION_TOKEN_NAME = "ApplicationToken";
        /// <summary>
        /// Key used in the UserProfile authenticationTokens dictionary to access the Session token directly.
        /// </summary>
        public const string PROFILE_SESSION_TOKEN_NAME = "SessionToken";

        public const string PROFILE_PREFERRED_TENANT = "PreferredTenant";

        #endregion
    }
}