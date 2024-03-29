using UnityEngine;
using System;
using System.Collections.Generic;
using Zenject;

namespace MistRidge
{
    public class SprintFactory : IInitializable
    {
        private readonly DiContainer container;
        private readonly CheckpointManager checkpointManager;
        private readonly List<IBiome> biomes;
        private readonly BiomePickingStrategy biomePickingStrategy;
        private readonly ChunkFacadeFactory chunkFacadeFactory;
        private readonly CheckpointFactory checkpointFactory;

        private float altitude;

        public SprintFactory(
                DiContainer container,
                CheckpointManager checkpointManager,
                List<IBiome> biomes,
                BiomePickingStrategy biomePickingStrategy,
                ChunkFacadeFactory chunkFacadeFactory,
                CheckpointFactory checkpointFactory)
        {
            this.container = container;
            this.checkpointManager = checkpointManager;
            this.biomes = biomes;
            this.biomePickingStrategy = biomePickingStrategy;
            this.chunkFacadeFactory = chunkFacadeFactory;
            this.checkpointFactory = checkpointFactory;
        }

        public void Initialize()
        {
            altitude = 0;
        }

        public Sprint Create(SprintRequest sprintRequest)
        {
            List<ChunkFacade> chunkFacades = SpawnChunkFacades(sprintRequest);

            Checkpoint checkpoint = null;

            if (!sprintRequest.checkpointOverride)
            {
                checkpoint = SpawnCheckpoint(sprintRequest);
            }

            List<TypeValuePair> args = new List<TypeValuePair>();
            args.Add(InstantiateUtil.CreateTypePair<List<ChunkFacade>>(chunkFacades));
            args.Add(InstantiateUtil.CreateTypePair<Checkpoint>(checkpoint));
            Sprint sprint = container.InstantiateExplicit<Sprint>(args);

            sprint.Initialize();

            return sprint;
        }

        private List<ChunkFacade> SpawnChunkFacades(SprintRequest sprintRequest)
        {
            List<ChunkFacade> chunkFacades = new List<ChunkFacade>();

            int chunkNum = sprintRequest.startChunkNum;
            int endChunkNum = chunkNum - sprintRequest.chunkCount;

            int offset = sprintRequest.checkpointOverride ? 0 : 1;

            for (; chunkNum > endChunkNum + offset; --chunkNum)
            {
                ChunkRequest chunkRequest = new ChunkRequest()
                {
                    chunkNum = chunkNum,
                    chunkCount = sprintRequest.totalChunkCount,
                    sprintEndChunkNum = endChunkNum + 1,
                    biome = biomePickingStrategy.Pick(biomes, altitude),
                };

                ChunkFacade chunkFacade = chunkFacadeFactory.Create(chunkRequest);
                altitude = chunkFacade.Position.y;

                chunkFacades.Add(chunkFacade);
            }

            return chunkFacades;
        }

        private Checkpoint SpawnCheckpoint(SprintRequest sprintRequest)
        {
            ChunkRequest checkpointRequest = new ChunkRequest()
            {
                chunkNum = sprintRequest.startChunkNum - sprintRequest.chunkCount + 1,
                chunkCount = sprintRequest.totalChunkCount,
            };

            Checkpoint checkpoint = checkpointFactory.Create(checkpointRequest, altitude);
            altitude = checkpoint.Position.y;

            checkpointManager.AddCheckpoint(checkpoint);

            return checkpoint;
        }
    }
}
