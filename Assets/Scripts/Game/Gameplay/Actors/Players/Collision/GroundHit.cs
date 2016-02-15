using UnityEngine;
using System;

namespace MistRidge
{
    public struct GroundHit
    {
        public bool isFound;
        public Vector3 point;
        public Vector3 normal;
        public float distance;

        public GroundHit(RaycastHit hit)
        {
            isFound = true;
            point = hit.point;
            normal = hit.normal;
            distance = hit.distance;
        }
    }
}
