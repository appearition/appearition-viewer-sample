// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: TypeExtensions.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Appearition.Common.TypeExtensions
{
    public static class TypeExtension
    {
        #region Float Extensions

        /// <summary>
        /// Returns the given int as a float
        /// </summary>
        /// <returns>The float.</returns>
        /// <param name="tmp">tmp.</param>
        public static float ToFloat(this int tmp)
        {
            return tmp;
        }


        /// <summary>
        /// Remaps the given value from two floats to two floats.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="minX"></param>
        /// <param name="maxX"></param>
        /// <param name="minY"></param>
        /// <param name="maxY"></param>
        /// <returns></returns>
        public static float Remap(this float value, float minX, float maxX, float minY, float maxY)
        {
            return (value - minX) / (maxX - minX) * (maxY - minY) + minY;
        }

        /// <summary>
        /// Remaps the given value from a vector2 minmax to a vector2 minmax
        /// </summary>
        /// <param name="value"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static float Remap(this float value, Vector2 from, Vector2 to)
        {
            return (value - from.x) / (from.y - from.x) * (to.y - to.x) + to.x;
        }

        #endregion

        /// <summary>
        /// Returns the given float as an int
        /// </summary>
        /// <returns>The int.</returns>
        /// <param name="tmp">tmp.</param>
        public static int ToInt(this float tmp)
        {
            return (int) tmp;
        }

        public static void DestroySelf(this GameObject tmp)
        {
            UnityEngine.Object.Destroy(tmp);
        }

        #region String Extensions

        /// <summary>
        /// Deserializes a comma separated value (CSV) as a string array.
        /// </summary>
        /// <returns>The CS.</returns>
        /// <param name="tmp">Tmp.</param>
        /// <param name="removeNullOrEmpty">If set to <c>true</c> remove null or empty.</param>
        public static string[] DeserializeCSV(this string tmp, bool removeNullOrEmpty = true)
        {
            string regex = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
            string[] deserializedCSV = System.Text.RegularExpressions.Regex.Split
                (tmp, regex);

            if (removeNullOrEmpty)
                deserializedCSV = deserializedCSV.Where(o => !string.IsNullOrEmpty(o)).ToArray();
            return deserializedCSV;
        }

        public static string FirstCharToUpper(this string input)
        {
            switch (input)
            {
                case null:
                case "":
                    throw new ArgumentException("The input cannot be null or empty cannot be empty");
                default:
                    return input.First().ToString().ToUpper() + input.Substring(1);
            }
        }

        /// <summary>
        /// Converts a binary data inside a string to a byte array.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static byte[] BinaryStringToByteArray(this string input)
        {
            //input = input.DecodeFromUtf8();
            //Debug.Log(input.Length);
            //Debug.Log(input);
            //int numOfBytes = input.Length / 8;
            //byte[] bytes = new byte[numOfBytes];
            //for (int i = 0; i < numOfBytes; ++i)
            //{
            //    bytes[i] = Convert.ToByte(input.Substring(8 * i, 8), 2);
            //}

            //return bytes;

            return Convert.FromBase64String(input);
        }

        public static string DecodeFromUtf8(this string utf8String)
        {
            ////// copy the string as UTF-8 bytes.
            //byte[] utf8Bytes = new byte[utf8String.Length];
            //for (int i = 0; i < utf8String.Length; ++i)
            //{
            //    //Debug.Assert( 0 <= utf8String[i] && utf8String[i] <= 255, "the char must be in byte's range");
            //    utf8Bytes[i] = (byte)utf8String[i];
            //}

            //return System.Text.Encoding.UTF8.GetString(utf8Bytes, 0, utf8Bytes.Length);//.Replace("\\u0000", "");
            //var bytes = Convert.FromBase64String(utf8String);
            //return Encoding.UTF8.GetString(bytes);
            //Convert.FromBase64String()
            //return Regex.Unescape(utf8String);
            //Regex regex = new Regex(@"\\U([0-9A-F]{4})", RegexOptions.IgnoreCase);
            //return regex.Replace(utf8String, match => ((char) int.Parse(match.Groups[1].Value,
            //    NumberStyles.HexNumber)).ToString());

            return Regex.Unescape(utf8String);
        }

        /// <summary>
        /// Deserializes a Key Value Pair string.
        /// eg "myKey1 : value;myKey2:value ;etc :etc"
        /// </summary>
        /// <param name="input"></param>
        /// <param name="equalChar"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static Dictionary<string, string> DeserializeDictionary(this string input, char equalChar = ':', char separator = ';')
        {
            Dictionary<string, string> output = new Dictionary<string, string>();

            string[] split = input.Split(new[] {separator}, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < split.Length; i++)
            {
                string tmp = split[i].Trim();
                string[] kvp = tmp.Split(new[] {equalChar});

                if (kvp.Length == 0 || kvp.Length > 2 || string.IsNullOrWhiteSpace(kvp[0]))
                    continue;

                string key = kvp[0].Trim();
                if (!output.ContainsKey(key))
                    output.Add(key, kvp[1].Trim());
            }

            return output;
        }

        /// <summary>
        /// Converts safely a HTML #000000 code to color.
        /// </summary>
        /// <param name="htmlColor"></param>
        /// <returns></returns>
        public static Color? TryConvertToColor(this string htmlColor)
        {
            if (string.IsNullOrWhiteSpace(htmlColor))
                return default;

            //if (htmlColor[0] == '#')
            //    htmlColor = htmlColor.Substring(1, htmlColor.Length - 1);

            if (ColorUtility.TryParseHtmlString(htmlColor, out Color col))
            {
                if (htmlColor.Length == 6)
                    col.a = 1.0f;
                return col;
            }

            return default;
        }

        #endregion
    }
}

namespace Appearition.Common.ObjectExtensions
{
    public static class ObjectExtension
    {
        #region Object And Stream Extensions

        /// <summary>
        /// Converts a Stream to a byte array.
        /// </summary>
        /// <returns>The byte array.</returns>
        /// <param name="stream">Stream.</param>
        public static byte[] ToByteArray(this System.IO.Stream stream)
        {
            byte[] output = new byte[stream.Length];
            for (int bytesCopied = 0; bytesCopied < stream.Length;)
                bytesCopied += stream.Read(output, bytesCopied, Convert.ToInt32(stream.Length) - bytesCopied);
            return output;
        }

        /// <summary>
        /// Deserializes a binary stream to an object.
        /// </summary>
        /// <returns>The object.</returns>
        /// <param name="data"></param>
        public static object ToObject(this byte[] data)
        {
            //Retardation test
            if (data == null)
                return null;

            System.Runtime.Serialization.IFormatter formatter =
                new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            System.IO.MemoryStream stream = new System.IO.MemoryStream(data);
            return formatter.Deserialize(stream);
        }

        /// <summary>
        /// Serializes an object to a binary stream.
        /// </summary>
        /// <returns>The stream.</returns>
        /// <param name="obj">Object.</param>
        public static System.IO.Stream ToStream(this object obj)
        {
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            System.Runtime.Serialization.IFormatter formatter =
                new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            formatter.Serialize(stream, obj);
            return stream;
        }

        public static string Checksum(this Stream stream)
        {
            string checksum;

            using (var md5 = MD5.Create())
            {
                checksum = BitConverter.ToString(md5.ComputeHash(stream));
            }

            return checksum.Replace("-", string.Empty);
        }

        public static byte[] ToByteArrayExtended(this object obj)
        {
            byte[] data = null;
            var paramStream = obj as Stream;

            if (paramStream != null)
            {
                //Write the stream to byte array
                data = paramStream.ToByteArray();
            }
            else if (obj is byte[])
            {
                data = (byte[]) obj;
            }
            else if (obj is Sprite)
            {
                data = ((Sprite) obj).texture.EncodeToJPG();
            }
            else if ((Texture2D) obj != null)
            {
                data = ((Texture2D) obj).EncodeToJPG();
            }

            return data;
        }

        #endregion
    }
}

namespace Appearition.Common.ListExtensions
{
    public static class ListExtension
    {
        #region List Extensions

        /// <summary>
        /// Sorts a list based on the given comparison delegate.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="thisList"></param>
        /// <param name="comparison"></param>
        /// <param name="isAscending"></param>
        public static void HiziSort<T>(this List<T> thisList, Comparison<T> comparison, bool isAscending)
        {
            while (true)
            {
                bool isSorted = true;

                for (int i = 0; i < thisList.Count; i++)
                {
                    //Don't check the last item
                    if (i + 1 < thisList.Count)
                    {
                        if (isAscending && comparison(thisList[i], thisList[i + 1]) > 0)
                        {
                            //Swap
                            T tmp = thisList[i];
                            thisList[i] = thisList[i + 1];
                            thisList[i + 1] = tmp;

                            isSorted = false;
                        }
                        else if (!isAscending && comparison(thisList[i], thisList[i + 1]) < 0)
                        {
                            //Swap
                            T tmp = thisList[i];
                            thisList[i] = thisList[i + 1];
                            thisList[i + 1] = tmp;

                            isSorted = false;
                        }
                    }
                }

                if (isSorted)
                    break;
            }
        }

        /// <summary>
        /// Whether or not any of the elements in the list matches the predicate.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="thisList"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static bool HiziAny<T>(this List<T> thisList, Predicate<T> predicate)
        {
            for (int i = 0; i < thisList.Count; i++)
            {
                if (predicate(thisList[i]))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Whether or not any of the elements in the list matches the predicate.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="thisList"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static bool HiziAll<T>(this List<T> thisList, Predicate<T> predicate)
        {
            for (int i = 0; i < thisList.Count; i++)
            {
                if (!predicate(thisList[i]))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Fetches all the indices of the items matching the predicate.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="thisList"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static List<int> HiziFindAllIndices<T>(this List<T> thisList, Predicate<T> predicate)
        {
            return HiziFindAllIndices(thisList, predicate, new List<int>());
        }

        /// <summary>
        /// Fetches all the indices of the items matching the predicate.
        /// Provides a reusable list to avoid creating a new one, but will clear its content.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="thisList"></param>
        /// <param name="predicate"></param>
        /// <param name="reusableList"></param>
        /// <returns></returns>
        public static List<int> HiziFindAllIndices<T>(this List<T> thisList, Predicate<T> predicate, List<int> reusableList)
        {
            if (reusableList == null)
                reusableList = new List<int>();
            else
                reusableList.Clear();

            for (int i = 0; i < thisList.Count; i++)
            {
                if (predicate(thisList[i]))
                    reusableList.Add(i);
            }

            return reusableList;
        }

        /// <summary>
        /// Fetches all items matching a given predicate.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="thisList"></param>
        /// <param name="predicate"></param>
        /// <param name="firstFoundFirstItem"></param>
        /// <returns></returns>
        public static List<T> HiziFindAll<T>(this List<T> thisList, Predicate<T> predicate, bool firstFoundFirstItem = true)
        {
            return HiziFindAll(thisList, predicate, new List<T>());
        }

        /// <summary>
        /// Fetches all items matching a given predicate.
        /// Provides a reusable list to avoid creating a new one, but will clear its content.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="thisList"></param>
        /// <param name="predicate"></param>
        /// <param name="reusableList"></param>
        /// <param name="firstFoundFirstItem"></param>
        /// <returns></returns>
        public static List<T> HiziFindAll<T>(this List<T> thisList, Predicate<T> predicate, List<T> reusableList, bool firstFoundFirstItem = true)
        {
            if (reusableList == null)
                reusableList = new List<T>();
            else
                reusableList.Clear();

            if (firstFoundFirstItem)
            {
                for (int i = 0; i < thisList.Count; i++)
                {
                    if (predicate(thisList[i]))
                        reusableList.Add(thisList[i]);
                }
            }
            else
            {
                for (int i = thisList.Count - 1; i >= 0; i--)
                {
                    if (predicate(thisList[i]))
                        reusableList.Add(thisList[i]);
                }
            }

            return reusableList;
        }

        /// <summary>
        /// Returns the first item or the default state of the given object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="thisList"></param>
        /// <returns></returns>
        public static T HiziFirstOrDefault<T>(this List<T> thisList)
        {
            if (thisList == null || thisList.Count == 0)
                return default(T);
            return thisList[0];
        }

        /// <summary>
        /// Returns the first item which matches the predicate, or the default state of the given object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="thisList"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static T HiziFirstOrDefault<T>(this List<T> thisList, Predicate<T> predicate)
        {
            if (thisList == null)
                return default(T);

            for (int i = 0; i < thisList.Count; i++)
            {
                if (predicate(thisList[i]))
                    return thisList[i];
            }

            return default(T);
        }

        /// <summary>
        /// Remove all objects from this list that the other list also has. Doesn't touch the other list's content.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="thisList"></param>
        /// <param name="otherList"></param>
        public static void HiziRemoveObjectsInCommon<T>(this List<T> thisList, List<T> otherList)
        {
            for (int i = 0; i < otherList.Count; i++)
            {
                if (thisList.Contains(otherList[i]))
                    thisList.Remove(otherList[i]);
            }
        }

        /// <summary>
        /// Gets a debug-friendly string including the list, as well as how many objects inside. Handles nullrefs.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="thisList"></param>
        /// <returns></returns>
        public static string ToStringWithCount<T>(this List<T> thisList)
        {
            if (thisList == null)
                throw new NullReferenceException();
            else
                return $"{thisList}, with {thisList.Count} objects inside.";
        }


        /// <summary>
        /// Removes the duplicates from a given list.
        /// </summary>
        /// <returns>The duplicates.</returns>
        /// <param name="items">Items.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static void HiziRemoveDuplicates<T>(this List<T> items)
        {
            List<T> result = new List<T>();
            for (int i = 0; i < items.Count; i++)
            {
                // Assume not duplicate.
                bool duplicate = false;
                for (int z = 0; z < i; z++)
                {
                    if (items[z].Equals(items[i]))
                    {
                        // This is a duplicate.
                        duplicate = true;
                        break;
                    }
                }

                // If not duplicate, add to result.
                if (!duplicate)
                {
                    result.Add(items[i]);
                }
            }

            items.Clear();
            items.AddRange(result);
        }

        /// <summary>
        /// Returns a shuffled list of given items.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        public static List<T> HiziShuffle<T>(this List<T> items)
        {
            List<T> outcome = new List<T>();

            while (items.Count > 0)
            {
                int itemIndex = UnityEngine.Random.Range(0, items.Count);
                outcome.Add(items[itemIndex]);
                items.RemoveAt(itemIndex);
            }

            return outcome;
        }

        [System.Serializable]
        public struct ManifestContainer<T>
        {
            public List<T> itemsRemoved;
            /// <summary>
            /// Always contains two items: 0 is the previous version, 1 is the newer version of the item.
            /// </summary>
            public List<T[]> itemsUpdated;
            public List<T> itemsAdded;
        }

        /// <summary>
        /// Creates a manifest showing what elements have been added, updated and removed from two given lists.
        /// </summary>
        /// <typeparam name="T">Item type.</typeparam>
        /// <param name="items">This new list of items.</param>
        /// <param name="previousItems">The old list of items.</param>
        /// <param name="equalPredicate">A way to determine whether two items are the same.</param>
        /// <param name="comparer">A way to compare which of two equal items is the most recent one.</param>
        public static ManifestContainer<T> HiziUpdateListWithManifest<T>(this List<T> items, List<T> previousItems, Func<T, T, bool> equalPredicate, Comparison<T> comparer)
        {
            List<T> itemsAdded = new List<T>();
            List<T[]> itemsUpdated = new List<T[]>();
            List<T> itemsRemoved = new List<T>();

            if (previousItems == null || previousItems.Count == 0)
                itemsAdded.AddRange(items);
            else if (items == null || items.Count == 0)
                itemsRemoved.AddRange(previousItems);
            //Find what's new and what's gone
            else
            {
                //Check removed/updated
                for (int i = 0; i < previousItems.Count; i++)
                {
                    var newMediaMatching = items.HiziFirstOrDefault(o => equalPredicate(o, previousItems[i]));
                    if (newMediaMatching == null)
                        itemsRemoved.Add(previousItems[i]);
                    else if (comparer(newMediaMatching, previousItems[i]) > 0)
                        itemsUpdated.Add(new T[2] {previousItems[i], newMediaMatching});
                }

                //Check new ones
                for (int i = 0; i < items.Count; i++)
                {
                    if (!previousItems.HiziAny(o => equalPredicate(o, items[i])))
                        itemsAdded.Add(items[i]);
                }
            }

            return new ManifestContainer<T> {
                itemsAdded = itemsAdded,
                itemsUpdated = itemsUpdated,
                itemsRemoved = itemsRemoved
            };
        }

        #endregion
    }
}