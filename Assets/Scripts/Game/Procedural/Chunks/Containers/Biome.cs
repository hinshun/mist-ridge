using UnityEngine;
using System;
using System.Collections.Generic;

namespace MistRidge
{
    [CreateAssetMenu(menuName = "Chunks/Biome")]
    public class Biome : ScriptableObject, IChunkFeatureContainer
    {
        [SerializeField]
        new private string name;

        [SerializeField]
        private float startAltitude;

        [SerializeField]
        private float endAltitude;

        [SerializeField]
        private List<ChunkFeature> chunkFeatures;

        public List<ChunkFeature> ChunkFeatures()
        {
            return chunkFeatures;
        }
    }
}
