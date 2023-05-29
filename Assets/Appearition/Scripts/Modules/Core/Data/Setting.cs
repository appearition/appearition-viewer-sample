// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: Setting.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

namespace Appearition.Common
{
    /// <summary>
    /// Container of an Appearition Setting JSON ApiData model.
    /// </summary>
    [System.Serializable]
    public class Setting
    {
        public string key;
        public string value;

        public Setting()
        {
        }

        public Setting(string newKey, string newValue)
        {
            key = newKey;
            value = newValue;
        }

        public Setting(Setting cc)
        {
            key = cc.key;
            value = cc.value;
        }
    }
}