using UnityEngine;

namespace Appearition.ContentLibrary
{
    [System.Serializable]
    public class ContentThumbnail : ContentFile
    {
        [System.NonSerialized] public Sprite thumbnail;
    }
}