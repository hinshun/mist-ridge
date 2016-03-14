using UnityEngine;
using System;
using System.Collections.Generic;

namespace MistRidge
{
    [CreateAssetMenu(menuName = "Chunks/Biome")]
    public class Biome : ScriptableObject, IBiome
    {
        [SerializeField]
        new private string name;

        [SerializeField]
        private float startAltitude;

        [SerializeField]
        private float endAltitude;

        [SerializeField]
        private List<ChunkFeature> chunkFeatures;

        public float StartAltitude()
        {
            return startAltitude;
        }

        public float EndAltitude()
        {
            return endAltitude;
        }

        public List<ChunkFeature> ChunkFeatures()
        {
            return chunkFeatures;
        }
    }
}
