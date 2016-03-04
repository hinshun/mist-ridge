using UnityEngine;
using System;
using System.Collections.Generic;
using Zenject;

namespace MistRidge
{
    public class CheckpointFactory
    {
        private readonly DiContainer container;
        private readonly List<IChunkFeatureContainer> chunkFeatureContainers;
        private readonly ChunkFacadeFactory chunkFacadeFactory;
        private readonly IChunkFeatureContainerPickingStrategy chunkFeatureContainerPickingStrategy;

        public CheckpointFactory(
                DiContainer container,
                List<IChunkFeatureContainer> chunkFeatureContainers,
                IChunkFeatureContainerPickingStrategy chunkFeatureContainerPickingStrategy,
                ChunkFacadeFactory chunkFacadeFactory)
        {
            this.container = container;
            this.chunkFeatureContainers = chunkFeatureContainers;
            this.chunkFeatureContainerPickingStrategy = chunkFeatureContainerPickingStrategy;
            this.chunkFacadeFactory = chunkFacadeFactory;
        }

        public Checkpoint Create(ChunkRequest chunkRequest)
        {
            ChunkRequest checkpointRequest = new ChunkRequest()
            {
                chunkNum = chunkRequest.chunkNum,
                chunkCount = chunkRequest.chunkCount,
                heightChunkNum = chunkRequest.chunkNum,
                chunkFeatureContainer = chunkFeatureContainerPickingStrategy.Pick(chunkFeatureContainers)
            };

            ChunkFacade chunkFacade = chunkFacadeFactory.Create(checkpointRequest);
            Checkpoint checkpoint = container.Instantiate<Checkpoint>(chunkFacade);

            checkpoint.Initialize();

            return checkpoint;
        }
    }
}
