using UnityEngine;
using System;
using System.Collections.Generic;

namespace MistRidge
{
    [CreateAssetMenu(menuName = "Chunks/Generic Chunk Feature Container")]
    public class GlobalBiome : ScriptableObject, IBiome
    {
        [SerializeField]
        private List<ChunkFeature> chunkFeatures;

        public float StartAltitude()
        {
            return float.MinValue;
        }

        public float EndAltitude()
        {
            return float.MaxValue;
        }

        public List<ChunkFeature> ChunkFeatures()
        {
            return chunkFeatures;
        }
    }
}
