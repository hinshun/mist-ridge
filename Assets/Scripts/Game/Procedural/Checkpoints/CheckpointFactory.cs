using UnityEngine;
using System;
using System.Collections.Generic;
using Zenject;

namespace MistRidge
{
    public class CheckpointFactory
    {
        private readonly DiContainer container;
        private readonly List<IBiome> biomes;
        private readonly ChunkFacadeFactory chunkFacadeFactory;
        private readonly BiomePickingStrategy biomePickingStrategy;

        public CheckpointFactory(
                DiContainer container,
                List<IBiome> biomes,
                BiomePickingStrategy biomePickingStrategy,
                ChunkFacadeFactory chunkFacadeFactory)
        {
            this.container = container;
            this.biomes = biomes;
            this.biomePickingStrategy = biomePickingStrategy;
            this.chunkFacadeFactory = chunkFacadeFactory;
        }

        public Checkpoint Create(ChunkRequest chunkRequest, float altitude)
        {
            ChunkRequest checkpointRequest = new ChunkRequest()
            {
                chunkNum = chunkRequest.chunkNum,
                chunkCount = chunkRequest.chunkCount,
                heightChunkNum = chunkRequest.chunkNum,
                sprintEndChunkNum = chunkRequest.sprintEndChunkNum,
                biome = biomePickingStrategy.Pick(biomes, altitude),
            };

            ChunkFacade chunkFacade = chunkFacadeFactory.Create(checkpointRequest);
            Checkpoint checkpoint = container.Instantiate<Checkpoint>(chunkFacade);

            checkpoint.Initialize();

            return checkpoint;
        }
    }
}
