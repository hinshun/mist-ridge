using UnityEngine;
using System;

namespace MistRidge
{
    public class BloomView : MonoView
    {
        [SerializeField]
        private Transform targetBloomable;

        public Transform TargetBloomable
        {
            get
            {
                return targetBloomable;
            }
        }
    }
}
