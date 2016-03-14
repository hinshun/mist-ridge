using UnityEngine;
using System.Collections.Generic;
using Zenject;

namespace MistRidge
{
    public class RandomChunkFeaturePickingStrategy : IChunkFeaturePickingStrategy
    {
        private readonly Generator generator;

        public RandomChunkFeaturePickingStrategy(Generator generator)
        {
            this.generator = generator;
        }

        public void Initialize()
        {
            // Do Nothing
        }

        public ChunkFeature Pick(IChunkFeatureContainer chunkFeatureContainer)
        {
            List<ChunkFeature> chunkFeatures = chunkFeatureContainer.ChunkFeatures();
            int randomIndex = generator.Random.Next(chunkFeatures.Count);
            return chunkFeatures[randomIndex];
        }
    }
}
