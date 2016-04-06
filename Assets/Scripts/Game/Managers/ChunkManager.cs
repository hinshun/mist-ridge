using UnityEngine;
using System;
using System.Collections.Generic;
using Zenject;

namespace MistRidge
{
    public class ChunkManager : IInitializable
    {
        private readonly DiContainer container;
        private readonly Settings settings;
        private readonly CinematicManager cinematicManager;
        private readonly CheckpointManager checkpointManager;
        private readonly SprintManager sprintManager;
        private readonly SprintFactory sprintFactory;
        private readonly ChunkFacadeFactory chunkFacadeFactory;
        private readonly GameStateMachine gameStateMachine;

        private int chunkCount;

        private List<Sprint> sprints;
        private ChunkFacade startingChunkFacade;
        private List<ChunkFacade> peakChunkFacades;
        private ChunkFacade peakChunkFacade;
        private PeakZoneView peakZoneView;
        private Checkpoint peakCheckpoint;

        private Sprint lastSprint;
        private Sprint secondLastSprint;

        public ChunkManager(
                DiContainer container,
                Settings settings,
                CinematicManager cinematicManager,
                CheckpointManager checkpointManager,
                SprintManager sprintManager,
                SprintFactory sprintFactory,
                ChunkFacadeFactory chunkFacadeFactory,
                GameStateMachine gameStateMachine)
        {
            this.container = container;
            this.settings = settings;
            this.cinematicManager = cinematicManager;
            this.checkpointManager = checkpointManager;
            this.sprintManager = sprintManager;
            this.sprintFactory = sprintFactory;
            this.chunkFacadeFactory = chunkFacadeFactory;
            this.gameStateMachine = gameStateMachine;
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

        public ChunkFacade PeakChunkFacade
        {
            get
            {
                return peakChunkFacade;
            }
        }

        public PeakZoneView PeakZoneView
        {
            get
            {
                return peakZoneView;
            }
        }

        public void Initialize()
        {
            ResetVariables();
        }

        public void ResetVariables()
        {
            chunkCount = CountChunks() + settings.peakChunkCount;
            startingChunkFacade = SpawnStartingChunkFacade();
            cinematicManager.StartingZoneView = startingChunkFacade.ChunkView.GetComponentInChildren<StartingZoneView>();

            sprints = SpawnSprints();
            sprintManager.SetSprintCount(sprints.Count);

            peakChunkFacades = SpawnPeakChunkFacades();
            peakChunkFacade = peakChunkFacades[peakChunkFacades.Count - 1];
            peakZoneView = peakChunkFacade.ChunkView.GetComponentInChildren<PeakZoneView>();

            cinematicManager.PeakZoneView = peakZoneView;
            peakCheckpoint = SpawnPeakCheckpoint();
            checkpointManager.LastCheckpoint = peakCheckpoint;

            lastSprint.Checkpoint = peakCheckpoint;
            secondLastSprint.Checkpoint.NextCheckpoint = lastSprint.Checkpoint;

            LinkChunks();

            gameStateMachine.ChunkManager = this;
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
            lastSprint = null;

            int currentChunkNum = chunkCount - 1;

            foreach (SprintRequest sprintRequest in settings.sprintRequests)
            {
                SprintRequest currentSprintRequest = new SprintRequest()
                {
                    chunkCount = sprintRequest.chunkCount,
                    startChunkNum = currentChunkNum,
                    totalChunkCount = chunkCount,
                    checkpointOverride = false
                };

                if (sprints.Count == settings.sprintRequests.Count - 1)
                {
                    currentSprintRequest.checkpointOverride = true;
                }

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

                if (sprints.Count == settings.sprintRequests.Count - 1)
                {
                    secondLastSprint = lastSprint;
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

        private Checkpoint SpawnPeakCheckpoint()
        {
            int checkpointNum = settings.sprintRequests.Count - 1;
            Checkpoint checkpoint = container.Instantiate<Checkpoint>(checkpointNum, peakChunkFacade);
            checkpoint.Initialize();

            checkpoint.CheckpointView.Position = peakZoneView.PeakCheckpointTransform.position;
            checkpoint.CheckpointView.Parent = peakZoneView.PeakCheckpointTransform;

            checkpointManager.AddCheckpoint(checkpoint);

            return checkpoint;
        }

        private void LinkChunks()
        {
            ChunkFacade previousChunkFacade = startingChunkFacade;

            foreach (Sprint sprint in sprints)
            {
                foreach (ChunkFacade chunkFacade in sprint.ChunkFacades)
                {
                    chunkFacade.PreviousChunkFacade = previousChunkFacade;
                    previousChunkFacade = chunkFacade;
                }
            }

            foreach (ChunkFacade chunkFacade in peakChunkFacades)
            {
                chunkFacade.PreviousChunkFacade = previousChunkFacade;
                previousChunkFacade = chunkFacade;
            }
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
