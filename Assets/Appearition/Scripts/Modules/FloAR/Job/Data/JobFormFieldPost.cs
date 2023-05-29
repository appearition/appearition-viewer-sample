// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: JobFormFieldPost.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace Appearition.Job
{
    [System.Serializable]
    public class JobFormFieldPost
    {
        public string FieldKey;
        public string FormFieldValue;
        public long PartId;
        public string Barcode;
        public string SerialNo;
        public int RowNo;
        public string ReviewerComments;
        public int AssetAction;

        static Dictionary<int, string> _assetActionTypeNameDictionary = new Dictionary<int, string>
            {{1401, "Check In"}, {1402, "Check Out"}};

        /// <summary>
        /// Fetches the name of an asset action from a given id.
        /// </summary>
        /// <param name="assetAction"></param>
        /// <returns></returns>
        public static string GetAssetActionTypeName(int assetAction)
        {
            if (_assetActionTypeNameDictionary.ContainsKey(assetAction))
                return _assetActionTypeNameDictionary[assetAction];
            return "";
        }
    }
}