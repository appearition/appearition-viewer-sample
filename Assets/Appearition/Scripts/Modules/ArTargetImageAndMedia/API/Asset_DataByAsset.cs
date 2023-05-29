using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using Appearition.API;
using Appearition.Common;
using Appearition.Common.TypeExtensions;
using UnityEngine.Serialization;

namespace Appearition.ArTargetImageAndMedia.API
{
    /// <summary>
    /// https://api.appearition.com/TenantName/api/Asset/DataByAsset/0 , where 0 is Channel ID.
    /// </summary>
    [System.Serializable]
    public class Asset_DataByAsset : BaseApiPost
    {
        public override int ApiVersion => 2;
        public override TypeOfApiStorage ResponseSaveType => TypeOfApiStorage.None;

        public string Data;

        /// <summary>
        /// Post ApiData
        /// </summary>
        [System.Serializable]
        public class PostData
        {
            public string assetId;
            public int arMediaId;
            public List<Setting> parameters;
        }

        public override Dictionary<string, string> JsonReplaceKvp => new Dictionary<string, string> {{"parameters", "params"}};
    }
}