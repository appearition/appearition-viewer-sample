#pragma warning disable 0649
using System;
using System.Collections;
using System.Collections.Generic;
using Appearition.ArTargetImageAndMedia;
using UnityEngine;

namespace Appearition.Promotions
{
    [System.Serializable]
    public class Promotion 
    {
        public int id;
        public string text;
        public string moreInfo;
        [SerializeField] long dateStartUtc;
        public DateTime DateStart => DateTimeOffset.FromUnixTimeSeconds(dateStartUtc).DateTime; 
        [SerializeField] long dateEndUtc;
        public DateTime DateEnd => DateTimeOffset.FromUnixTimeSeconds(dateEndUtc).DateTime; 
        public List<MediaFile> mediaFiles;
    }
}