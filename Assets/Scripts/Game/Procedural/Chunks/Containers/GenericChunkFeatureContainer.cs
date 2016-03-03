using UnityEngine;
using System;
using System.Collections.Generic;

namespace MistRidge
{
    [CreateAssetMenu(menuName = "Chunks/Generic Chunk Feature Container")]
    public class GenericChunkFeatureContainer : ScriptableObject, IChunkFeatureContainer
    {
        [SerializeField]
        private List<ChunkFeature> chunkFeatures;

        public List<ChunkFeature> ChunkFeatures()
        {
            return chunkFeatures;
        }
    }
}
