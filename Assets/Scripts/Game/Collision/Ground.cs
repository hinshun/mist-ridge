using UnityEngine;

namespace MistRidge
{
    public struct Ground
    {
        public RaycastHit hit;
        public RaycastHit nearHit;
        public RaycastHit farHit;
        public RaycastHit secondaryHit;
        public Collidable collidable;
        public Transform transform;
    }
}
