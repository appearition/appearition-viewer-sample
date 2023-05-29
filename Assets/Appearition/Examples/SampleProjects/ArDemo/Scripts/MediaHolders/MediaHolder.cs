using UnityEngine;

namespace Appearition.ArDemo
{
    public class MediaHolder : MonoBehaviour
    {
        public BaseMedia Media { get; protected set; }

        public virtual void Setup(Experience experience, BaseMedia media)
        {
            Media = media;
            Media.Setup(this, experience, media);
        }

        void Update()
        {
            Media.OnUpdate();
        }

        void OnDestroy()
        {
            if(Media != null)
                Media.Dispose();
        }
    }
}