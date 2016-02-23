using UnityEngine;
using System;
using System.Collections.Generic;
using Zenject;

namespace MistRidge
{
    public class ChunkFeatureContainer : IChunkFeatureContainer
    {
        private readonly Settings settings;

        public ChunkFeatureContainer(Settings settings)
        {
            this.settings = settings;
        }

        public List<ChunkFeature> ChunkFeatures()
        {
            return settings.chunkFeatures;
        }

        [Serializable]
        public class Settings
        {
            public List<ChunkFeature> chunkFeatures;
        }
    }
}
