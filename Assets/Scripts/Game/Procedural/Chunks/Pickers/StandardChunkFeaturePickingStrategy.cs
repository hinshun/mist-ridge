using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Zenject;

namespace MistRidge
{
    public class StandardChunkFeaturePickingStrategy : IChunkFeaturePickingStrategy
    {
        private readonly Generator generator;

        private HashSet<ChunkFeature> pickedUniqueChunkFeatures;
        private ChunkFeature chunkChain;

        public StandardChunkFeaturePickingStrategy(Generator generator)
        {
            this.generator = generator;
        }

        public void Initialize()
        {
            pickedUniqueChunkFeatures = new HashSet<ChunkFeature>();
            chunkChain = null;
        }

        public ChunkFeature Pick(ChunkRequest chunkRequest)
        {
            int depth = ChunkMath.Depth(chunkRequest);
            int sideChunkNum = ChunkMath.SideChunkNum(chunkRequest);

            if (chunkChain != null)
            {
                ChunkFeature chunkChainedFeature = chunkChain;
                chunkChain = chunkChain.ChunkChainNext;

                return chunkChainedFeature;
            }

            IChunkFeatureContainer chunkFeatureContainer = chunkRequest.chunkFeatureContainer;
            List<ChunkFeature> chunkFeatures = chunkFeatureContainer.ChunkFeatures();
            chunkFeatures = chunkFeatures
                .Where(chunkFeature => !chunkFeature.IsUnique || (chunkFeature.IsUnique && !pickedUniqueChunkFeatures.Contains(chunkFeature)))
                .ToList();

            List<int> unfittedChunkIndices = GetUnfittedChunkIndices(chunkRequest, chunkFeatures);
            foreach (int index in unfittedChunkIndices)
            {
                chunkFeatures.RemoveAt(index);
            }

            List<int> cdf = CreateCDF(chunkFeatures);
            int randomIndex = GetRandomIndex(cdf, chunkFeatures);
            ChunkFeature pickedChunkFeature = chunkFeatures[randomIndex];

            if (pickedChunkFeature.IsUnique)
            {
                pickedUniqueChunkFeatures.Add(pickedChunkFeature);
            }

            chunkChain = pickedChunkFeature.ChunkChainNext;

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

        private int GetRandomIndex(List<int> cdf, List<ChunkFeature> chunkFeatures)
        {
            if (cdf.Count == 0)
            {
                Debug.LogError("Failed to pick chunk feature because chunk feature container ran out of features");
                return 0;
            }

            int random = generator.Random.Next(cdf[cdf.Count - 1]);

            int randomIndex = 0;
            foreach (int probability in cdf)
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

            return randomIndex;
        }

        private List<int> GetUnfittedChunkIndices(ChunkRequest chunkRequest, List<ChunkFeature> chunkFeatures)
        {
            List<int> unfittedChunkIndices = new List<int>();
            for (int index = 0; index < chunkFeatures.Count; ++index)
            {
                ChunkFeature chunkFeature = chunkFeatures[index];
                ChunkFeature chunkChainedFeature = chunkFeature;

                int chunkNumOffset = 0;
                bool chained = false;
                while (chunkChainedFeature != null)
                {
                    ChunkRequest nextChunkRequest = new ChunkRequest()
                    {
                        chunkNum = chunkRequest.chunkNum - chunkNumOffset,
                    };


                    if (chained && nextChunkRequest.chunkNum == chunkRequest.sprintEndChunkNum)
                    {
                        unfittedChunkIndices.Add(index);
                        break;
                    }

                    int nextDepth = ChunkMath.Depth(nextChunkRequest);
                    int nextDepthEndChunkNum = ChunkMath.DepthEndChunkNum(nextChunkRequest);
                    int nextSideChunkNum = ChunkMath.SideChunkNum(nextChunkRequest);

                    // Mark chunk feature to be removed if it hits a corner and it is supposed to skip corners
                    if ((chained || nextChunkRequest.chunkNum != nextDepthEndChunkNum)
                        && (nextSideChunkNum == nextDepth - 1 && chunkChainedFeature.SkipCorners))
                    {
                        unfittedChunkIndices.Add(index);
                        break;
                    }

                    chunkChainedFeature = chunkChainedFeature.ChunkChainNext;
                    chunkNumOffset++;
                    chained = true;
                }
            }

            return unfittedChunkIndices;
        }
    }
}
