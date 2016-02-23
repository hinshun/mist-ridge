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
    }
}
