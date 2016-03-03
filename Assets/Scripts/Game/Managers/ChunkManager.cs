using UnityEngine;
using System;
using System.Collections.Generic;
using Zenject;

namespace MistRidge
{
    public class ChunkManager : IInitializable
    {
        private readonly Settings settings;
        private readonly SprintFactory sprintFactory;
        private readonly ChunkFacadeFactory chunkFacadeFactory;

        private int chunkCount;
        private List<Sprint> sprints;
        private ChunkFacade startingChunkFacade;

        public ChunkManager(
                Settings settings,
                SprintFactory sprintFactory,
                ChunkFacadeFactory chunkFacadeFactory)
        {
            this.settings = settings;
            this.sprintFactory = sprintFactory;
            this.chunkFacadeFactory = chunkFacadeFactory;
        }

        public int ChunkCount
        {
            get
            {
                return chunkCount;
            }
        }

        public List<Sprint> Sprints
        {
            get
            {
                return sprints;
            }
        }

        public ChunkFacade StartingChunkFacade
        {
            get
            {
                return startingChunkFacade;
            }
        }

        public void Initialize()
        {
            chunkCount = CountChunks();
            sprints = SpawnSprints();
            startingChunkFacade = SpawnStartingChunkFacade();
        }

        private int CountChunks()
        {
            int chunkSum = 0;

            foreach(SprintRequest sprintRequest in settings.sprintRequests)
            {
                chunkSum += sprintRequest.chunkCount;
            }

            return chunkSum;
        }

        private List<Sprint> SpawnSprints()
        {
            List<Sprint> sprints = new List<Sprint>();

            int currentChunkNum = chunkCount - 1;

            foreach(SprintRequest sprintRequest in settings.sprintRequests)
            {
                SprintRequest currentSprintRequest = new SprintRequest()
                {
                    chunkCount = sprintRequest.chunkCount,
                    startChunkNum = currentChunkNum,
                    totalChunkCount = chunkCount,
                };

                currentChunkNum -= sprintRequest.chunkCount;

                Sprint sprint = sprintFactory.Create(currentSprintRequest);
                sprints.Add(sprint);
            }

            return sprints;
        }

        private ChunkFacade SpawnStartingChunkFacade()
        {
            ChunkRequest chunkRequest = new ChunkRequest()
            {
                chunkNum = chunkCount,
                chunkCount = chunkCount,
                chunkFeatureContainer = settings.startingZoneFeatureContainer,
            };

            ChunkFacade chunkFacade = chunkFacadeFactory.Create(chunkRequest);
            chunkFacade.Name = "Starting Zone";

            return chunkFacade;
        }

        [Serializable]
        public class Settings
        {
            public GenericChunkFeatureContainer startingZoneFeatureContainer;
            public List<SprintRequest> sprintRequests;
        }
    }
}
