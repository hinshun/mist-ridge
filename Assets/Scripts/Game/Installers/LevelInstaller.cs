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
                    typeof(IChunkFeaturePickingStrategy),
                    typeof(CheckpointManager),
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
            Container.Bind<BiomePickingStrategy>().ToSingle();
            Container.Bind<IChunkFeaturePickingStrategy>().ToSingle<StandardChunkFeaturePickingStrategy>();
            Container.Bind<IInitializable>().ToSingle<StandardChunkFeaturePickingStrategy>();
            Container.Bind<IChunkPlacingStrategy>().ToSingle<SpiralChunkPlacingStrategy>();

            Container.BindGameObjectFactory<PlatformView.Factory>(settings.chunk.platformPrefab);

            Container.Bind<ChunkManager>().ToSingle();
            Container.BindAllInterfacesToSingle<ChunkManager>();

            Container.Bind<ChunkReference>().ToInstance(settings.chunk.chunkReference);

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
                Container.Bind<IBiome>().ToInstance(checkpointBiome).WhenInjectedInto<CheckpointFactory>();
            }

            Container.Bind<CheckpointFactory>().ToSingle();
            Container.BindAllInterfacesToSingle<CheckpointFactory>();

            Container.Bind<CheckpointManager>().ToSingle();
            Container.BindAllInterfacesToSingle<CheckpointManager>();
        }

        private void InstallSprints()
        {
            foreach(Biome biome in settings.chunk.biomes)
            {
                Container.Bind<IBiome>().ToInstance(biome).WhenInjectedInto<SprintFactory>();
            }

            Container.Bind<SprintFactory>().ToSingle();
            Container.BindAllInterfacesToSingle<SprintFactory>();
            Container.Bind<SprintView>().ToTransientPrefab(settings.chunk.sprintPrefab);

            Container.Bind<SprintManager>().ToSingle();
            Container.BindAllInterfacesToSingle<SprintManager>();
        }

        private void InstallSettings()
        {
            Container.Bind<Chunk.Settings>().ToSingleInstance(settings.chunk.chunkSettings);
            Container.Bind<ChunkManager.Settings>().ToSingleInstance(settings.chunk.chunkManagerSettings);

            Container.Bind<SpiralChunkPlacingStrategy.Settings>().ToSingleInstance(settings.spiralChunkPlacingStrategySettings);
        }

        [Serializable]
        public class Settings
        {
            public ChunkSettings chunk;
            public SpiralChunkPlacingStrategy.Settings spiralChunkPlacingStrategySettings;

            [Serializable]
            public class ChunkSettings
            {
                public GameObject chunkPrefab;
                public GameObject chunkBasePrefab;
                public GameObject platformPrefab;
                public GameObject sprintPrefab;
                public ChunkReference chunkReference;
                public List<Biome> biomes;
                public List<Biome> checkpointBiomes;
                public Chunk.Settings chunkSettings;
                public ChunkManager.Settings chunkManagerSettings;
            }
        }
    }
}
