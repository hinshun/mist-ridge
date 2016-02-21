using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class ChunkBaseView : ChunkChildView
    {
        public Vector3 LocalPosition
        {
            get
            {
                return transform.localPosition;
            }
            set
            {
                transform.localPosition = value;
            }
        }

        public Vector3 LocalScale
        {
            get
            {
                return transform.localScale;
            }
            set
            {
                transform.localScale = value;
            }
        }
    }
}
