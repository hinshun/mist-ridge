using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Zenject;

namespace MistRidge
{
    public class ChunkManager : IInitializable
    {
        private readonly Settings settings;
        private readonly ChunkFacadeFactory chunkFacadeFactory;

        private ReadOnlyCollection<ChunkFacade> chunkFacades;

        public ChunkManager(
                Settings settings,
                ChunkFacadeFactory chunkFacadeFactory)
        {
            this.settings = settings;
            this.chunkFacadeFactory = chunkFacadeFactory;
        }

        public ChunkReference ChunkReference
        {
            get
            {
                return settings.chunkReference;
            }
        }

        public int ChunkCount
        {
            get
            {
                return settings.chunkCount;
            }
        }

        public ReadOnlyCollection<ChunkFacade> ChunkFacades
        {
            get
            {
                return chunkFacades;
            }
        }

        public void Initialize()
        {
            List<ChunkFacade> spawnedChunkFacades = new List<ChunkFacade>();

            for (int chunkNum = 0; chunkNum < settings.chunkCount; ++chunkNum)
            {
                ChunkConfig chunkConfig = new ChunkConfig()
                {
                    chunkNum = chunkNum,
                };

                ChunkFacade chunkFacade = chunkFacadeFactory.Create(chunkConfig);
                spawnedChunkFacades.Add(chunkFacade);
            }

            chunkFacades = new ReadOnlyCollection<ChunkFacade>(spawnedChunkFacades);
        }

        [Serializable]
        public class Settings
        {
            public int chunkCount;
            public ChunkReference chunkReference;
        }
    }
}
