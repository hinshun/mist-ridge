using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class ChunkChildView : MonoBehaviour
    {
        public Transform Parent
        {
            get
            {
                return transform.parent;
            }
            set
            {
                transform.parent = value;
            }
        }
    }
}
