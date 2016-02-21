using UnityEngine;
using Zenject;

namespace MistRidge
{
    public class PlatformView : ChunkChildView
    {
        public Vector3 Position
        {
            get
            {
                return transform.position;
            }
            set
            {
                transform.position = value;
            }
        }

        public Vector3 LocalScale
        {
            get
            {
                return transform.localScale;
            }
        }

        public class Factory : GameObjectFactory<PlatformView>
        {
        }
    }
}
