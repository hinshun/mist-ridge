using UnityEngine;
using System;
using System.Collections.Generic;
using Zenject;

namespace MistRidge
{
    public class ChunkFacadeFactory : FacadeFactory<ChunkConfig, ChunkFacade>
    {
        private readonly IChunkFeaturePickingStrategy chunkFeaturePickingStrategy;

        public ChunkFacadeFactory(IChunkFeaturePickingStrategy chunkFeaturePickingStrategy)
        {
            this.chunkFeaturePickingStrategy = chunkFeaturePickingStrategy;
        }

        public override ChunkFacade Create(ChunkConfig chunkConfig)
        {
            DiContainer subContainer = CreateSubContainer(chunkConfig);

            ChunkFeature chunkFeature = chunkFeaturePickingStrategy.Pick(chunkConfig.chunkFeatureContainer);
            subContainer.Bind<ChunkFeatureView>().ToSinglePrefab(chunkFeature.ChunkFeatureView.gameObject);

            ChunkFacade chunkFacade = subContainer.Resolve<ChunkFacade>();
            chunkFacade.Initialize();

            return chunkFacade;
        }
    }
}
