// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: MediaFile.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using UnityEngine;

namespace Appearition.ArTargetImageAndMedia
{
    /// <summary>
    /// Container of an Appearition Media File JSON ApiData model.
    /// </summary>
    [System.Serializable]
    public class MediaFile
    {
        //Variables
        public int arMediaId;
        public string animationName;
        public string checksum;
        public string contentItemProviderName;
        public string contentItemKey;
        public string custom;
        public DataTransform dataTransform;
        public string fileName;
        public bool isAutoPlay;
		public bool isDataQuery;
        public bool isInteractive;
        public bool isPreDownload;
        public bool isPrivate;
        public bool isTracking;
        public string language;
        public string lastModified;
        public int mediaGridOffset;
        public string mediaType;
        public string mimeType;
        public float quaternionX;
        public float quaternionY;
        public float quaternionZ;
        public float quaternionW;
        public int resolution;
        public float rotationX;
        public float rotationY;
        public float rotationZ;
        public float scaleX;
        public float scaleY;
        public float scaleZ;
        public string text;
        public float translationX;
        public float translationY;
        public float translationZ;
        public string url;

        public bool IsContentLibraryItem => !string.IsNullOrEmpty(contentItemProviderName) && !string.IsNullOrEmpty(contentItemKey);

        #region 3D Data

        public Vector3 GetPosition => new Vector3(translationX, translationY, translationZ);

        //public Quaternion GetRotation => 
        //    (Mathf.Approximately(quaternionX, 0) && Mathf.Approximately(quaternionY, 0) && Mathf.Approximately(quaternionZ, 0) && Mathf.Approximately(quaternionW, 0)
        //    ? Quaternion.Euler(rotationX, rotationY, rotationZ)
        //    : new Quaternion(-quaternionX, -quaternionZ, -quaternionY, quaternionW));
        public Quaternion GetRotation
        {
            get
            {
                if (Mathf.Approximately(quaternionX, 0) && Mathf.Approximately(quaternionY, 0) && Mathf.Approximately(quaternionZ, 0) && Mathf.Approximately(quaternionW, 0))
                    return Quaternion.Euler(rotationX, rotationY, rotationZ);

                return new Quaternion(quaternionX, quaternionY, -quaternionZ, -quaternionW);
            }
        }

        public Vector3 GetScale => new Vector3(scaleX, scaleY, scaleZ);


        public void SetPosition(Vector3 pos)
        {
            translationX = pos.x;
            translationY = pos.y;
            translationZ = pos.z;
        }

        public void SetRotation(Quaternion rot)
        {
            rotationX = rot.eulerAngles.x;
            rotationY = rot.eulerAngles.y;
            rotationZ = rot.eulerAngles.z;
            quaternionX = rot.x;
            quaternionY = rot.z;
            quaternionZ = -rot.y;
            quaternionW = -rot.w;
        }

        public void SetScale(Vector3 sca)
        {
            scaleX = sca.x;
            scaleY = sca.y;
            scaleZ = sca.z;
        }

        #endregion

        public MediaFile()
        {
        }

        /// <summary>
        /// Copy Constructor
        /// </summary>
        /// <param name="cc">C.</param>
        public MediaFile(MediaFile cc)
        {
            CopyValuesFrom(cc);
        }

        public void CopyValuesFrom(MediaFile cc)
        {
            arMediaId = cc.arMediaId;
            animationName = cc.animationName;
            checksum = cc.checksum;
            contentItemKey = cc.contentItemKey;
            contentItemProviderName = cc.contentItemProviderName;
            custom = cc.custom;
            dataTransform = cc.dataTransform == null ? null : new DataTransform(cc.dataTransform);
            fileName = cc.fileName;
            isAutoPlay = cc.isAutoPlay;
            isDataQuery = cc.isDataQuery;
            isInteractive = cc.isInteractive;
            isPreDownload = cc.isPreDownload;
            isPrivate = cc.isPrivate;
            isTracking = cc.isTracking;
            language = cc.language;
            lastModified = cc.lastModified;
            mediaGridOffset = cc.mediaGridOffset;
            mediaType = cc.mediaType;
            mimeType = cc.mimeType;
            quaternionX = cc.quaternionX;
            quaternionY = cc.quaternionY;
            quaternionZ = cc.quaternionZ;
            quaternionW = cc.quaternionW;
            resolution = cc.resolution;
            rotationX = cc.rotationX;
            rotationY = cc.rotationY;
            rotationZ = cc.rotationZ;
            scaleX = cc.scaleX;
            scaleY = cc.scaleY;
            scaleZ = cc.scaleZ;
            text = cc.text;
            translationX = cc.translationX;
            translationY = cc.translationY;
            translationZ = cc.translationZ;
            url = cc.url;
        }
    }
}