using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Zenject;

namespace MistRidge
{
    public class StandardChunkFeaturePickingStrategy : IChunkFeaturePickingStrategy
    {
        private readonly Generator generator;

        private Dictionary<IChunkFeatureContainer, List<int>> containerCDF;
        private HashSet<ChunkFeature> pickedUniqueChunkFeatures;

        public StandardChunkFeaturePickingStrategy(Generator generator)
        {
            this.generator = generator;
        }

        public void Initialize()
        {
            containerCDF = new Dictionary<IChunkFeatureContainer, List<int>>();
            pickedUniqueChunkFeatures = new HashSet<ChunkFeature>();
        }

        public ChunkFeature Pick(IChunkFeatureContainer chunkFeatureContainer)
        {
            List<ChunkFeature> chunkFeatures = chunkFeatureContainer.ChunkFeatures();
            chunkFeatures = chunkFeatures
                .Where(chunkFeature => !chunkFeature.IsUnique || (chunkFeature.IsUnique && !pickedUniqueChunkFeatures.Contains(chunkFeature)))
                .ToList();

            if (!containerCDF.ContainsKey(chunkFeatureContainer))
            {
                containerCDF.Add(chunkFeatureContainer, CreateCDF(chunkFeatures));
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

            ChunkFeature pickedChunkFeature = chunkFeatures[randomIndex];

            if (pickedChunkFeature.IsUnique)
            {
                pickedUniqueChunkFeatures.Add(pickedChunkFeature);
                containerCDF[chunkFeatureContainer] = CreateCDF(chunkFeatures);
            }

            return pickedChunkFeature;
        }

        private List<int> CreateCDF(List<ChunkFeature> chunkFeatures)
        {
            List<int> featuresCDF = new List<int>();
            int sum = 0;

            foreach (ChunkFeature chunkFeature in chunkFeatures)
            {
                sum += chunkFeature.Rarity;
                featuresCDF.Add(sum);
            }

            return featuresCDF;
        }
    }
}
