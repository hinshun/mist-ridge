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

        public Quaternion Rotation
        {
            get
            {
                return transform.rotation;
            }
            set
            {
                transform.rotation = value;
            }
        }

        public Vector3 Up
        {
            get
            {
                return transform.up;
            }
        }

    }
}
