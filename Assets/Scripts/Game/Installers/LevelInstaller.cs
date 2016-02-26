using UnityEngine;
using System;
using System.Collections.Generic;
using Zenject;

namespace MistRidge
{
    public class LevelInstaller : MonoInstaller
    {
        [SerializeField]
        private Settings settings;

        public override void InstallBindings()
        {
            InstallLevel();
            InstallCheckpoints();
            InstallChunks();
            InstallSettings();
        }

        private void InstallLevel()
        {
            Container.Bind<LevelManager>().ToSingle();
            Container.BindAllInterfacesToSingle<LevelManager>();
        }

        private void InstallCheckpoints()
        {
            /* Container.Bind<Checkpoint.Factory>().ToSingle(); */
            /* Container.Bind<CheckpointView>().ToTransientPrefab(settings.Checkpoint.Prefab); */
        }

        private void InstallChunks()
        {
            foreach(Biome biome in settings.chunk.biomes)
            {
                Container.Bind<IChunkFeatureContainer>().ToInstance(biome);
            }

            Container.Bind<IChunkFeatureContainerPickingStrategy>().ToSingle<BiomePickingStrategy>();
            Container.Bind<IChunkFeaturePickingStrategy>().ToSingle<RandomChunkFeaturePickingStrategy>();
            Container.Bind<IChunkPlacingStrategy>().ToSingle<SpiralChunkPlacingStrategy>();

            Container.BindGameObjectFactory<PlatformView.Factory>(settings.chunk.platformPrefab);

            Container.Bind<ChunkManager>().ToSingle();
            Container.BindAllInterfacesToSingle<ChunkManager>();

            Container.BindFacadeFactory<ChunkConfig, ChunkFacade, ChunkFacadeFactory>(InstallChunkFacade);
        }

        private void InstallChunkFacade(DiContainer subContainer, ChunkConfig chunkConfig)
        {
            subContainer.BindInstance(chunkConfig);

            subContainer.Bind<ChunkView>().ToSinglePrefab(settings.chunk.chunkPrefab);
            subContainer.Bind<ChunkBaseView>().ToSinglePrefab(settings.chunk.chunkBasePrefab);

            subContainer.Bind<Chunk>().ToSingle();
            subContainer.BindAllInterfacesToSingle<Chunk>();

            subContainer.Bind<ChunkBase>().ToSingle();
            subContainer.BindAllInterfacesToSingle<ChunkBase>();
        }

        private void InstallSettings()
        {
            Container.Bind<Checkpoint.Settings>().ToSingleInstance(settings.checkpointSettings);
            Container.Bind<ChunkManager.Settings>().ToSingleInstance(settings.chunk.chunkManagerSettings);
        }

        [Serializable]
        public class Settings
        {
            public ChunkSettings chunk;
            public Checkpoint.Settings checkpointSettings;

            [Serializable]
            public class ChunkSettings
            {
                public GameObject chunkPrefab;
                public GameObject chunkBasePrefab;
                public GameObject platformPrefab;
                public List<Biome> biomes;
                public ChunkManager.Settings chunkManagerSettings;
            }
        }
    }
}
