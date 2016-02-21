using UnityEngine;
using System;
using System.Collections.Generic;
using Zenject;

namespace MistRidge
{
    public class ChunkFacadeFactory : FacadeFactory<ChunkConfig, ChunkFacade>
    {
        private readonly IChunkFeatureContainer chunkFeatureContainer;
        private readonly IChunkFeaturePickingStrategy chunkFeaturePickingStrategy;

        public ChunkFacadeFactory(
            IChunkFeatureContainer chunkFeatureContainer,
            IChunkFeaturePickingStrategy chunkFeaturePickingStrategy)
        {
            this.chunkFeatureContainer = chunkFeatureContainer;
            this.chunkFeaturePickingStrategy = chunkFeaturePickingStrategy;
        }

        public override ChunkFacade Create(ChunkConfig chunkConfig)
        {
            DiContainer subContainer = CreateSubContainer(chunkConfig);

            List<GameObject> chunkFeatures = chunkFeatureContainer.ChunkFeatures();
            GameObject chunkFeature = chunkFeaturePickingStrategy.Pick(chunkFeatures);

            subContainer.Bind<ChunkFeatureView>().ToSinglePrefab(chunkFeature);

            ChunkFacade chunkFacade = subContainer.Resolve<ChunkFacade>();
            chunkFacade.Initialize();

            return chunkFacade;
        }
    }
}
