using UnityEngine;
using System;
using System.Collections.Generic;
using Zenject;

namespace MistRidge
{
    public class ChunkManager : IInitializable
    {
        private readonly Settings settings;
        private readonly CinematicManager cinematicManager;
        private readonly CheckpointManager checkpointManager;
        private readonly SprintManager sprintManager;
        private readonly SprintFactory sprintFactory;
        private readonly ChunkFacadeFactory chunkFacadeFactory;

        private int chunkCount;
        private List<Sprint> sprints;
        private ChunkFacade startingChunkFacade;
        private List<ChunkFacade> peakChunkFacades;

        public ChunkManager(
                Settings settings,
                CinematicManager cinematicManager,
                CheckpointManager checkpointManager,
                SprintManager sprintManager,
                SprintFactory sprintFactory,
                ChunkFacadeFactory chunkFacadeFactory)
        {
            this.settings = settings;
            this.cinematicManager = cinematicManager;
            this.checkpointManager = checkpointManager;
            this.sprintManager = sprintManager;
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
            startingChunkFacade = SpawnStartingChunkFacade();
            cinematicManager.StartingZoneView = startingChunkFacade.ChunkView.GetComponentInChildren<StartingZoneView>();

            sprints = SpawnSprints();
            peakChunkFacades = SpawnPeakChunkFacades();

            sprintManager.SetSprintCount(sprints.Count);
            /* sprintManager.UpdateSprintText(); */
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
                sprintEndChunkNum = chunkCount,
                biome = settings.startingZoneBiome,
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
                    sprintEndChunkNum = 0,
                    biome = settings.peakBiome,
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
            public GlobalBiome peakBiome;
            public GlobalBiome startingZoneBiome;
            public List<SprintRequest> sprintRequests;
        }
    }
}
