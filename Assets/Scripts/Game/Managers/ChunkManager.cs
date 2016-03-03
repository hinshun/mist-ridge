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

        private List<Sprint> sprints;
        private int chunkCount;

        public ChunkManager(
                Settings settings,
                SprintFactory sprintFactory)
        {
            this.settings = settings;
            this.sprintFactory = sprintFactory;
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

        public void Initialize()
        {
            chunkCount = CountChunks();
            sprints = SpawnSprints();
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
                };

                currentChunkNum -= sprintRequest.chunkCount;

                Sprint sprint = sprintFactory.Create(currentSprintRequest);
                sprints.Add(sprint);
            }

            return sprints;
        }

        [Serializable]
        public class Settings
        {
            public ChunkReference chunkReference;
            public List<SprintRequest> sprintRequests;
        }
    }
}
