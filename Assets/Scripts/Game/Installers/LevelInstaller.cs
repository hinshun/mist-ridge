using UnityEngine;
using System;
using System.Collections.Generic;
using Zenject;
using Zenject.Commands;

namespace MistRidge
{
    public class LevelInstaller : MonoInstaller
    {
        [SerializeField]
        private Settings settings;

        public override void InstallBindings()
        {
            InitExecutionOrder();
            InstallLevel();
            InstallChunks();
            InstallCheckpoints();
            InstallSprints();
            InstallSettings();
        }

        private void InitExecutionOrder()
        {
            Container.Install<ExecutionOrderInstaller>(
                new List<Type>()
                {
                    typeof(ChunkManager),
                    typeof(LevelManager),
                }
            );
        }

        private void InstallLevel()
        {
            Container.Bind<LevelManager>().ToSingle();
            Container.BindAllInterfacesToSingle<LevelManager>();
        }

        private void InstallChunks()
        {
            Container.Bind<IChunkFeatureContainerPickingStrategy>().ToSingle<BiomePickingStrategy>();
            Container.Bind<IChunkFeaturePickingStrategy>().ToSingle<RandomChunkFeaturePickingStrategy>();
            Container.Bind<IChunkPlacingStrategy>().ToSingle<SpiralChunkPlacingStrategy>();

            Container.BindGameObjectFactory<PlatformView.Factory>(settings.chunk.platformPrefab);

            Container.Bind<ChunkManager>().ToSingle();
            Container.BindAllInterfacesToSingle<ChunkManager>();

            Container.BindFacadeFactory<ChunkRequest, ChunkFacade, ChunkFacadeFactory>(InstallChunkFacade);
        }

        private void InstallChunkFacade(DiContainer subContainer, ChunkRequest chunkRequest)
        {
            subContainer.BindInstance(chunkRequest);

            subContainer.Bind<ChunkView>().ToSinglePrefab(settings.chunk.chunkPrefab);
            subContainer.Bind<ChunkBaseView>().ToSinglePrefab(settings.chunk.chunkBasePrefab);

            subContainer.Bind<Chunk>().ToSingle();
            subContainer.BindAllInterfacesToSingle<Chunk>();

            subContainer.Bind<ChunkBase>().ToSingle();
            subContainer.BindAllInterfacesToSingle<ChunkBase>();
        }

        private void InstallCheckpoints()
        {
            foreach(Biome checkpointBiome in settings.chunk.checkpointBiomes)
            {
                Container.Bind<IChunkFeatureContainer>().ToInstance(checkpointBiome).WhenInjectedInto<CheckpointFactory>();
            }

            Container.Bind<CheckpointFactory>().ToSingle();

            Container.BindSignal<CheckpointSignal>();
            Container.BindTrigger<CheckpointSignal.Trigger>();
        }

        private void InstallSprints()
        {
            foreach(Biome biome in settings.chunk.biomes)
            {
                Container.Bind<IChunkFeatureContainer>().ToInstance(biome).WhenInjectedInto<SprintFactory>();
            }

            Container.Bind<SprintFactory>().ToSingle();
            Container.Bind<SprintView>().ToTransientPrefab(settings.chunk.sprintPrefab);
        }

        private void InstallSettings()
        {
            Container.Bind<Chunk.Settings>().ToSingleInstance(settings.chunk.chunkSettings);
            Container.Bind<ChunkManager.Settings>().ToSingleInstance(settings.chunk.chunkManagerSettings);
        }

        [Serializable]
        public class Settings
        {
            public ChunkSettings chunk;

            [Serializable]
            public class ChunkSettings
            {
                public GameObject chunkPrefab;
                public GameObject chunkBasePrefab;
                public GameObject platformPrefab;
                public GameObject sprintPrefab;
                public List<Biome> biomes;
                public List<Biome> checkpointBiomes;
                public Chunk.Settings chunkSettings;
                public ChunkManager.Settings chunkManagerSettings;
            }
        }
    }
}
