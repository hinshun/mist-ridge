using UnityEngine;
using System;
using System.Collections.Generic;
using Zenject;

namespace MistRidge
{
    public class ChunkManager : IInitializable
    {
        private readonly Settings settings;
        private readonly CheckpointManager checkpointManager;
        private readonly SprintFactory sprintFactory;
        private readonly ChunkFacadeFactory chunkFacadeFactory;

        private int chunkCount;
        private List<Sprint> sprints;
        private ChunkFacade startingChunkFacade;
        private List<ChunkFacade> peakChunkFacades;

        public ChunkManager(
                Settings settings,
                CheckpointManager checkpointManager,
                SprintFactory sprintFactory,
                ChunkFacadeFactory chunkFacadeFactory)
        {
            this.settings = settings;
            this.checkpointManager = checkpointManager;
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
            chunkCount = CountChunks() + settings.peakChunkCount;
            sprints = SpawnSprints();
            startingChunkFacade = SpawnStartingChunkFacade();
            peakChunkFacades = SpawnPeakChunkFacades();
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
            Sprint lastSprint = null;

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

                if (lastSprint == null)
                {
                    checkpointManager.CurrentCheckpoint = sprint.Checkpoint;
                }
                else
                {
                    lastSprint.Checkpoint.NextCheckpoint = sprint.Checkpoint;
                }
                lastSprint = sprint;

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
                heightChunkNum = chunkCount,
                sprintEndChunkNum = chunkCount,
                chunkFeatureContainer = settings.startingZoneFeatureContainer,
            };

            ChunkFacade chunkFacade = chunkFacadeFactory.Create(chunkRequest);
            chunkFacade.Name = "Starting Zone";

            return chunkFacade;
        }

        private List<ChunkFacade> SpawnPeakChunkFacades()
        {
            List<ChunkFacade> peakChunkFacades = new List<ChunkFacade>();

            for (int chunkNum = settings.peakChunkCount - 1; chunkNum >= 0; --chunkNum)
            {
                ChunkRequest chunkRequest = new ChunkRequest()
                {
                    chunkNum = chunkNum,
                    chunkCount = chunkCount,
                    heightChunkNum = settings.peakChunkCount - 1,
                    sprintEndChunkNum = 0,
                    chunkFeatureContainer = settings.peakFeatureContainer,
                };

                ChunkFacade chunkFacade = chunkFacadeFactory.Create(chunkRequest);
                chunkFacade.Name = "Peak";

                peakChunkFacades.Add(chunkFacade);
            }

            return peakChunkFacades;
        }

        [Serializable]
        public class Settings
        {
            public int peakChunkCount;
            public GenericChunkFeatureContainer peakFeatureContainer;
            public GenericChunkFeatureContainer startingZoneFeatureContainer;
            public List<SprintRequest> sprintRequests;
        }
    }
}
