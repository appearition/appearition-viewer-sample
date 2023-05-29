// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: DataStore.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using System.Collections.Generic;
using Appearition.Common;

namespace Appearition.ImageRecognition
{
    [System.Serializable]
    public class DataStore
    {
        public int channelId;
        public string channelName;
        public string provider;
        public List<ConfigSetting> configSettings;

        public DataStore()
        {
        }

        /// <summary>
        /// Copy Constructor
        /// </summary>
        /// <param name="cc"></param>
        public DataStore(DataStore cc)
        {
            channelId = cc.channelId;
            channelName = cc.channelName;
            provider = cc.provider;
            configSettings = new List<ConfigSetting>(cc.configSettings);
        }
    }
}