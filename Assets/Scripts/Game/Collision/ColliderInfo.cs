using UnityEngine;
using System;

namespace MistRidge
{
    public struct ColliderInfo
    {
        public Collider collider;
        public int layer;

        public ColliderInfo(Collider collider)
        {
            this.collider = collider;
            this.layer = collider.gameObject.layer;
        }
    }
}
