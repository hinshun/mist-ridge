using UnityEngine;
using System;

namespace MistRidge
{
    public class Collidable : MonoView
    {
        [SerializeField]
        private float standAngle = 80f;

        [SerializeField]
        private float slopeLimit = 80f;

        [SerializeField]
        private float friction = 10f;

        public float StandAngle
        {
            get
            {
                return standAngle;
            }
        }

        public float SlopeLimit
        {
            get
            {
                return slopeLimit;
            }
        }

        public float Friction
        {
            get
            {
                return friction;
            }
        }
    }
}
