using UnityEngine;
using System;
using System.Collections.Generic;
using Zenject;

namespace MistRidge
{
    public class ChunkFacadeFactory : FacadeFactory<ChunkConfig, ChunkFacade>
    {
        private readonly Bounds.Settings boundsSettings;
        private readonly IChunkFeaturePickingStrategy chunkFeaturePickingStrategy;

        public ChunkFacadeFactory(
                Bounds.Settings boundsSettings,
                IChunkFeaturePickingStrategy chunkFeaturePickingStrategy)
        {
            this.boundsSettings = boundsSettings;
            this.chunkFeaturePickingStrategy = chunkFeaturePickingStrategy;
        }

        public override ChunkFacade Create(ChunkConfig chunkConfig)
        {
            DiContainer subContainer = CreateSubContainer(chunkConfig);

            ChunkFeature chunkFeature = chunkFeaturePickingStrategy.Pick(chunkConfig.chunkFeatureContainer);

            GameObject prefab = chunkFeature.ChunkFeatureView.gameObject;
            subContainer.Bind<ChunkFeatureView>().ToSinglePrefab(prefab);

            ChunkFacade chunkFacade = subContainer.Resolve<ChunkFacade>();
            chunkFacade.Initialize();

            if (boundsSettings.enabled)
            {
                foreach(MeshRenderer meshRenderer in chunkFacade.ChunkFeatureView.GetComponentsInChildren<MeshRenderer>())
                {
                    meshRenderer.enabled = false;
                }
            }

            return chunkFacade;
        }
    }
}
