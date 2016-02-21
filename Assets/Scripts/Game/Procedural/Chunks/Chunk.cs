using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class Chunk : IInitializable
    {
        private readonly ChunkConfig chunkConfig;
        private readonly ChunkView chunkView;
        private readonly ChunkFeatureView chunkFeatureView;
        private readonly IChunkPlacingStrategy chunkPlacingStrategy;

        public Chunk(
                ChunkConfig chunkConfig,
                ChunkView chunkView,
                ChunkFeatureView chunkFeatureView,
                IChunkPlacingStrategy chunkPlacingStrategy)
        {
            this.chunkConfig = chunkConfig;
            this.chunkView = chunkView;
            this.chunkFeatureView = chunkFeatureView;
            this.chunkPlacingStrategy = chunkPlacingStrategy;
        }

        public void Initialize()
        {
            chunkFeatureView.Parent = chunkView.transform;

            PositionChunk();
        }

        private void PositionChunk()
        {
            chunkView.Position = chunkPlacingStrategy.Placement(chunkConfig);
        }
    }
}
