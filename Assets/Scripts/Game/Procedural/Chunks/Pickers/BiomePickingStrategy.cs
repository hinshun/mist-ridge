using UnityEngine;
using System.Collections.Generic;
using Zenject;

namespace MistRidge
{
    public class BiomePickingStrategy : IChunkFeatureContainerPickingStrategy
    {
        private readonly Generator generator;

        public BiomePickingStrategy(Generator generator)
        {
            this.generator = generator;
        }

        public IChunkFeatureContainer Pick(List<IChunkFeatureContainer> chunkFeatureContainers)
        {
            int randomIndex = generator.Random.Next(chunkFeatureContainers.Count);
            return chunkFeatureContainers[randomIndex];
        }
    }
}
