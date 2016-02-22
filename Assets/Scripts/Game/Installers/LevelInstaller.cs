using UnityEngine;
using System;
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
            Container.Bind<IChunkFeatureContainer>().ToSingle<ChunkFeatureContainer>();
            Container.Bind<IChunkFeaturePickingStrategy>().ToSingle<RandomChunkFeaturePickingStrategy>();
            Container.Bind<IChunkPlacingStrategy>().ToSingle<SpiralChunkPlacingStrategy>();

            Container.BindGameObjectFactory<PlatformView.Factory>(settings.Chunk.PlatformPrefab);

            Container.Bind<ChunkManager>().ToSingle();
            Container.BindAllInterfacesToSingle<ChunkManager>();

            Container.BindFacadeFactory<ChunkConfig, ChunkFacade, ChunkFacadeFactory>(InstallChunkFacade);
        }

        private void InstallChunkFacade(DiContainer subContainer, ChunkConfig chunkConfig)
        {
            subContainer.BindInstance(chunkConfig);

            subContainer.Bind<ChunkView>().ToSinglePrefab(settings.Chunk.ChunkPrefab);
            subContainer.Bind<ChunkBaseView>().ToSinglePrefab(settings.Chunk.ChunkBasePrefab);

            subContainer.Bind<Chunk>().ToSingle();
            subContainer.BindAllInterfacesToSingle<Chunk>();

            subContainer.Bind<ChunkBase>().ToSingle();
            subContainer.BindAllInterfacesToSingle<ChunkBase>();
        }

        private void InstallSettings()
        {
            Container.Bind<ChunkFeatureContainer.Settings>().ToSingleInstance(settings.Chunk.ChunkFeatureContainer);
            Container.Bind<ChunkManager.Settings>().ToSingleInstance(settings.Chunk.ChunkManager);
            Container.Bind<Checkpoint.Settings>().ToSingleInstance(settings.Checkpoint);
        }

        [Serializable]
        public class Settings
        {
            public ChunkSettings Chunk;
            public Checkpoint.Settings Checkpoint;

            [Serializable]
            public class ChunkSettings
            {
                public GameObject ChunkPrefab;
                public GameObject ChunkBasePrefab;
                public GameObject PlatformPrefab;
                public ChunkFeatureContainer.Settings ChunkFeatureContainer;
                public ChunkManager.Settings ChunkManager;
            }
        }
    }
}
