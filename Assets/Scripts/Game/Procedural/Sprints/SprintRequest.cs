using UnityEngine;
using System;

namespace MistRidge
{
    [Serializable]
    public struct SprintRequest
    {
        public int chunkCount;

        [HideInInspector]
        public int startChunkNum;
    }
}
