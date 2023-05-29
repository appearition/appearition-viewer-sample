// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: Channel.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

#pragma warning disable 0649
using System.Collections.Generic;
using Appearition.Common;
using UnityEngine;

namespace Appearition.ChannelManagement
{
    /// <summary>
    /// Container of an Appearition Channel JSON ApiData model.
    /// </summary>
    [System.Serializable]
    public class Channel
    {
        //Variables
        public int channelId;
        public string name;
        public ChannelImage[] images;
        [SerializeField] Setting[] settings;
        Dictionary<string, string> _settings = new Dictionary<string, string>();

        public Dictionary<string, string> Settings
        {
            get
            {
                if (settings != null && _settings.Count != settings.Length)
                {
                    _settings.Clear();
                    for (int i = 0; i < settings.Length; i++)
                        _settings.Add(settings[i].key, settings[i].value);
                }

                return _settings;
            }
        }

        public Channel()
        {
        }

        /// <summary>
        /// Copy Constructor    
        /// </summary>
        /// <param name="cc">C.</param>
        public Channel(Channel cc)
        {
            channelId = cc.channelId;
            name = cc.name;
            cc.images.CopyTo(images, 0);
            cc.settings.CopyTo(settings, 0);
        }
    }
}