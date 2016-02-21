using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class ChunkView : MonoBehaviour
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
    }
}
