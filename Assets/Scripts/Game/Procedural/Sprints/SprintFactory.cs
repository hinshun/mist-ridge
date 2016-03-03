using UnityEngine;
using System;
using System.Collections.Generic;
using Zenject;

namespace MistRidge
{
    public class SprintFactory
    {
        private readonly DiContainer container;
        private readonly List<IChunkFeatureContainer> chunkFeatureContainers;
        private readonly IChunkFeatureContainerPickingStrategy chunkFeatureContainerPickingStrategy;
        private readonly ChunkFacadeFactory chunkFacadeFactory;
        private readonly CheckpointFactory checkpointFactory;

        public SprintFactory(
                DiContainer container,
                List<IChunkFeatureContainer> chunkFeatureContainers,
                IChunkFeatureContainerPickingStrategy chunkFeatureContainerPickingStrategy,
                ChunkFacadeFactory chunkFacadeFactory,
                CheckpointFactory checkpointFactory)
        {
            this.container = container;
            this.chunkFeatureContainers = chunkFeatureContainers;
            this.chunkFeatureContainerPickingStrategy = chunkFeatureContainerPickingStrategy;
            this.chunkFacadeFactory = chunkFacadeFactory;
            this.checkpointFactory = checkpointFactory;
        }

        public Sprint Create(SprintRequest sprintRequest)
        {
            List<ChunkFacade> chunkFacades = SpawnChunkFacades(sprintRequest);
            Checkpoint checkpoint = SpawnCheckpoint(sprintRequest);
            Sprint sprint = container.Instantiate<Sprint>(chunkFacades, checkpoint);
            sprint.Initialize();

            return sprint;
        }

        private List<ChunkFacade> SpawnChunkFacades(SprintRequest sprintRequest)
        {
            List<ChunkFacade> chunkFacades = new List<ChunkFacade>();

            int chunkNum = sprintRequest.startChunkNum;
            int endChunkNum = chunkNum - sprintRequest.chunkCount;

            for (; chunkNum > endChunkNum + 1; --chunkNum)
            {
                ChunkRequest chunkRequest = new ChunkRequest()
                {
                    chunkNum = chunkNum,
                    chunkFeatureContainer = chunkFeatureContainerPickingStrategy.Pick(chunkFeatureContainers),
                };

                ChunkFacade chunkFacade = chunkFacadeFactory.Create(chunkRequest);
                chunkFacades.Add(chunkFacade);
            }

            return chunkFacades;
        }

        private Checkpoint SpawnCheckpoint(SprintRequest sprintRequest)
        {
            ChunkRequest checkpointRequest = new ChunkRequest()
            {
                chunkNum = sprintRequest.startChunkNum - sprintRequest.chunkCount + 1,
            };

            return checkpointFactory.Create(checkpointRequest);
        }
    }
}
