// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: ImageUtility.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Threading;
using UnityEngine.Networking;

namespace Appearition.Common
{
    /// <summary>
    /// Utility component helping dealing with Texture2D and Sprite objects.
    /// </summary>
    public static class ImageUtility
    {
        #region Memory Handling

        static Dictionary<string, Sprite> _spritesLoadedInMemory = new Dictionary<string, Sprite>();
        static Dictionary<string, Texture2D> _texturesLoadedInMemory = new Dictionary<string, Texture2D>();

        public static void DeleteAllTexturesLoadedInMemory()
        {
            foreach (var tmpTex in _texturesLoadedInMemory.Values)
            {
                UnityEngine.Object.Destroy(tmpTex);
            }

            foreach (var tmpSprite in _spritesLoadedInMemory.Values)
            {
                UnityEngine.Object.Destroy(tmpSprite);
            }

            //for (int i = _texturesLoadedInMemory.Count - 1; i >= 0; i--)
            //{
            //    if (_spritesLoadedInMemory[i] != null)
            //        UnityEngine.Object.Destroy(_spritesLoadedInMemory[i]);
            //}

            //for (int i = _texturesLoadedInMemory.Count - 1; i >= 0; i--)
            //{
            //    if (_texturesLoadedInMemory != null)
            //        UnityEngine.Object.Destroy(_texturesLoadedInMemory[i]);
            //}

            _spritesLoadedInMemory.Clear();
            _texturesLoadedInMemory.Clear();
            GC.Collect();
        }

        public static Sprite TryToFindLoadedSprite(string checksum)
        {
            if (!string.IsNullOrEmpty(checksum) && _spritesLoadedInMemory.ContainsKey(checksum))
            {
                Sprite tmp = _spritesLoadedInMemory[checksum];
                
                if(tmp == null)
                    _spritesLoadedInMemory.Remove(checksum);
                return tmp;
            }
            return null;
        }

        static void AddNewlyLoadedSprite(string checksum, Sprite sprite)
        {
            if (!_spritesLoadedInMemory.ContainsKey(checksum))
                _spritesLoadedInMemory.Add(checksum, sprite);
            else
                _spritesLoadedInMemory[checksum] = sprite;
        }

        static void AddNewlyLoadedTexture(string checksum, Texture2D texture)
        {
            if (!_texturesLoadedInMemory.ContainsKey(checksum))
                _texturesLoadedInMemory.Add(checksum, texture);
            else
                _texturesLoadedInMemory[checksum] = texture;
        }

        public static Texture2D TryToFindLoadedTexture(string checksum)
        {
            if (!string.IsNullOrEmpty(checksum) && _texturesLoadedInMemory.ContainsKey(checksum))
            {
                Texture2D tex = _texturesLoadedInMemory[checksum];
                if(tex == null)
                    _texturesLoadedInMemory.Remove(checksum);
                return tex;
            }
            return null;
        }

        #endregion

        #region Load

        /// <summary>
        /// Loads a picture from a given file path, and returns it as a Sprite object.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="checksum"></param>
        /// <param name="pixelsPerUnit"></param>
        /// <returns></returns>
        public static Sprite LoadOrCreateSprite(string filePath, string checksum = "", float pixelsPerUnit = 100.0f)
        {
            //Don't bother going further if it exists.
            Sprite newSprite = TryToFindLoadedSprite(checksum);
            if (newSprite != null)
                return newSprite;

            Texture2D spriteTexture = LoadOrCreateTexture(filePath.Trim(), checksum);
            newSprite = Sprite.Create(spriteTexture, new Rect(0, 0, spriteTexture.width, spriteTexture.height), new Vector2(0, 0), pixelsPerUnit);

            if (newSprite == null)
                AppearitionLogger.LogWarning("NewSprite not created:" + filePath);
            else if (!string.IsNullOrEmpty(checksum))
                AddNewlyLoadedSprite(checksum, newSprite);

            return newSprite;
        }

        /// <summary>
        /// Using given ApiData, creates a Sprite object.
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="checksum"></param>
        /// <param name="pixelsPerUnit"></param>
        /// <returns></returns>
        public static Sprite LoadOrCreateSprite(byte[] bytes, string checksum = "", float pixelsPerUnit = 100.0f)
        {
            Sprite newSprite = TryToFindLoadedSprite(checksum);
            if (newSprite != null)
                return newSprite;

            Texture2D spriteTexture = LoadOrCreateTexture(bytes, checksum);
            newSprite = Sprite.Create(spriteTexture, new Rect(0, 0, spriteTexture.width, spriteTexture.height),
                new Vector2(0, 0), pixelsPerUnit);

            if (newSprite == null)
                AppearitionLogger.LogWarning("New sprite could not be created using the given ApiData.");
            else if (!string.IsNullOrEmpty(checksum))
                AddNewlyLoadedSprite(checksum, newSprite);

            return newSprite;
        }

        /// <summary>
        /// Using a given Texture, creates a Sprite object.
        /// </summary>
        /// <param name="tmpTexture"></param>
        /// <param name="checksum"></param>
        /// <param name="pixelPerUnit"></param>
        /// <returns></returns>
        public static Sprite LoadOrCreateSprite(Texture2D tmpTexture, string checksum = "", float pixelPerUnit = 100.0f)
        {
            Sprite newSprite = TryToFindLoadedSprite(checksum);
            if (newSprite != null)
                return newSprite;

            if (tmpTexture == null)
            {
                AppearitionLogger.LogWarning("Unable to create a sprite from an empty texture");
                return null;
            }

            newSprite = Sprite.Create(tmpTexture, new Rect(0, 0, tmpTexture.width, tmpTexture.height), new Vector2(0, 0), pixelPerUnit);

            if (!string.IsNullOrEmpty(checksum))
                AddNewlyLoadedSprite(checksum, newSprite);

            return newSprite;
        }

        /// <summary>
        /// Loads a texture from a given path.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="checksum"></param>
        /// <returns></returns>
        public static Texture2D LoadOrCreateTexture(string filePath, string checksum = "")
        {
            Texture2D newTexture = TryToFindLoadedTexture(checksum);
            if (newTexture != null)
                return newTexture;

            if (File.Exists(filePath))
                newTexture = LoadOrCreateTexture(File.ReadAllBytes(filePath), checksum);
            else
                Debug.Log(filePath + " does not exist");
            return newTexture;
        }

        /// <summary>
        /// Creates a texture from given ApiData.
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="checksum"></param>
        /// <returns></returns>
        public static Texture2D LoadOrCreateTexture(byte[] bytes, string checksum = "")
        {
            Texture2D newTexture = TryToFindLoadedTexture(checksum);
            if (newTexture != null)
                return newTexture;

            var tex2D = new Texture2D(2, 2);
            if (tex2D.LoadImage(bytes))
            {
                if (!string.IsNullOrEmpty(checksum))
                    AddNewlyLoadedTexture(checksum, tex2D);
                return tex2D;
            }

            AppearitionLogger.LogWarning("The texture could not be created using the given ApiData.");
            return null;
        }

        /// <summary>
        /// Loads a sprite at a given file path.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="checksum"></param>
        /// <param name="onTextureLoaded"></param>
        /// <param name="pixelPerUnit"></param>
        /// <returns></returns>
        public static IEnumerator LoadOrCreateSpriteAsync(string filePath, string checksum = "", Action<Sprite> onTextureLoaded = null, float pixelPerUnit = 100)
        {
            Sprite tmp = TryToFindLoadedSprite(checksum);
            
            if(tmp != null)
                onTextureLoaded?.Invoke(_spritesLoadedInMemory[checksum]);
            else
            {
                yield return LoadOrCreateTextureAsync(filePath, checksum, texture2D =>
                    {
                        if (texture2D != null && onTextureLoaded != null)
                        {
                            tmp = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0, 0), pixelPerUnit);
                            if (tmp != null && !string.IsNullOrEmpty(checksum))
                                AddNewlyLoadedSprite(checksum, tmp);
                            onTextureLoaded(tmp);
                        }
                    }
                );
            }
        }

        /// <summary>
        /// Loads a texture at a given file path.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="checksum"></param>
        /// <param name="onTextureLoaded"></param>
        public static IEnumerator LoadOrCreateTextureAsync(string filePath, string checksum = "", Action<Texture2D> onTextureLoaded = null)
        {
            Texture2D tex = TryToFindLoadedTexture(checksum);
            
            if (tex == null)
            {
                string fullPath = filePath;

                if (Application.platform == RuntimePlatform.Android)
                    fullPath = string.Format("file:///{0}", fullPath);
                else
                    fullPath = string.Format("file://{0}", fullPath);

                var textureRequest = UnityWebRequestTexture.GetTexture(fullPath, false);
                yield return textureRequest.SendWebRequest();

                if (string.IsNullOrWhiteSpace(textureRequest.error))
                    tex = DownloadHandlerTexture.GetContent(textureRequest);
                else
                    Debug.LogError("Error when trying to load an image at path : " + filePath + ", error: " + textureRequest.error);

                if (!string.IsNullOrEmpty(checksum))
                    AddNewlyLoadedTexture(checksum, tex);

                //Whether success or failure, send it back.
                if (onTextureLoaded != null)
                    onTextureLoaded(tex);

                textureRequest.Dispose();
            }

            if (onTextureLoaded != null)
                onTextureLoaded(tex);
        }

        #endregion

        #region Download / Save Utilities

        /// <summary>
        /// Returns a valid picture path based on the original path (if any. If not, the default appearition will be used.) and name.
        /// </summary>
        /// <returns>The picture save path.</returns>
        /// <param name="url"></param>
        /// <param name="folderPath">Folder path.</param>
        /// <param name="fileName"></param>
        /// <param name="avoidExisting"></param>
        /// <param name="forceExtension"></param>
        public static string GetPictureSavePath(string url, string folderPath, string fileName, bool avoidExisting = true, bool forceExtension = false)
        {
            //Check for the picture's extension. Limit it to .JPG and .PNG
            string[] splitUrl = url.Split('.');
            if (forceExtension)
            {
                string loweredSplitUrl = splitUrl[splitUrl.Length - 1].ToLower();
                if (!loweredSplitUrl.Equals("jpg") && !loweredSplitUrl.Equals("png") && !loweredSplitUrl.Equals("jpeg") &&
                    !loweredSplitUrl.Equals("jpg:large") && !loweredSplitUrl.Equals("png:large"))
                {
                    Debug.LogError("The given URL is not an accepted picture type.");
                    return null;
                }
            }

            if (string.IsNullOrEmpty(folderPath))
            {
                //folderPath = AppearitionConstants.MEDIA_DIRECTORY + "/";
                Debug.LogError("No folder path was provided.");
                return "";
            }

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            //Get the file path
            if (string.IsNullOrEmpty(fileName))
            {
                splitUrl = url.Split('/');
                fileName = splitUrl[splitUrl.Length - 1];
            }

            fileName = fileName.Trim();


            //File with no extension
            if (fileName.Length < 3)
            {
                fileName += ".jpg";
            }
            else
            {
                string lastThreeChars = "";

                lastThreeChars += fileName[fileName.Length - 3];
                lastThreeChars += fileName[fileName.Length - 2];
                lastThreeChars += fileName[fileName.Length - 1];

                //Bad name found! Make it JPG
                if (lastThreeChars != "jpg" && lastThreeChars != "png" && lastThreeChars != "rge" && lastThreeChars != "peg")
                    fileName += ".jpg";
            }


            string fullPath = folderPath + fileName;

            //Fix the path
            if (File.Exists(fullPath) && avoidExisting)
            {
                string[] tmpSplit = fullPath.Split('.');

                //Check for (N) terminal
                string endPreFinalSplit = "";
                for (int i = tmpSplit[tmpSplit.Length - 2].Length - 1; i > 2; i--)
                    endPreFinalSplit += tmpSplit[tmpSplit.Length - 2][i];

                int counter = 0;
                if (endPreFinalSplit.Contains("(") && endPreFinalSplit.Contains(")"))
                    counter = int.Parse(endPreFinalSplit[1].ToString());

                while (File.Exists(fullPath))
                {
                    string newCheck = "";

                    for (int i = 0; i < tmpSplit.Length; i++)
                    {
                        newCheck += tmpSplit[i];
                        if (i + 2 == tmpSplit.Length)
                            newCheck += "(" + counter + ").";
                    }

                    counter++;

                    fullPath = newCheck;
                }
            }

            return fullPath;
        }

        #endregion
    }
}