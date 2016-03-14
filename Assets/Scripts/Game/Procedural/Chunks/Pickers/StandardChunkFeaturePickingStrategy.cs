using UnityEngine;
using System.Collections.Generic;
using Zenject;

namespace MistRidge
{
    public class StandardChunkFeaturePickingStrategy : IChunkFeaturePickingStrategy
    {
        private readonly Generator generator;

        private Dictionary<IChunkFeatureContainer, List<int>> containerCDF;

        public StandardChunkFeaturePickingStrategy(Generator generator)
        {
            this.generator = generator;
        }

        public void Initialize()
        {
            containerCDF = new Dictionary<IChunkFeatureContainer, List<int>>();
        }

        public ChunkFeature Pick(IChunkFeatureContainer chunkFeatureContainer)
        {
            List<ChunkFeature> chunkFeatures = chunkFeatureContainer.ChunkFeatures();

            if (!containerCDF.ContainsKey(chunkFeatureContainer))
            {
                List<int> featuresCDF = new List<int>();
                int sum = 0;

                foreach (ChunkFeature chunkFeature in chunkFeatures)
                {
                    sum += chunkFeature.Rarity;
                    featuresCDF.Add(sum);
                }

                containerCDF.Add(chunkFeatureContainer, featuresCDF);
            }

            List<int> cachedCDF = containerCDF[chunkFeatureContainer];
            int random = generator.Random.Next(cachedCDF[cachedCDF.Count - 1]);

            int randomIndex = 0;
            foreach (int probability in cachedCDF)
            {
                if (probability > random)
                {
                    break;
                }

                randomIndex++;
            }

            if (randomIndex > chunkFeatures.Count - 1)
            {
                randomIndex = chunkFeatures.Count - 1;
            }

            return chunkFeatures[randomIndex];
        }
    }
}
