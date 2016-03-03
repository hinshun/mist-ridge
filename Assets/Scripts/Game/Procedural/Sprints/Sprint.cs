using UnityEngine;
using System;
using System.Collections.Generic;
using Zenject;

namespace MistRidge
{
    public class Sprint : IInitializable
    {
        private readonly SprintView sprintView;
        private readonly List<ChunkFacade> chunkFacades;
        private readonly Checkpoint checkpoint;

        public Sprint(
                SprintView sprintView,
                List<ChunkFacade> chunkFacades,
                Checkpoint checkpoint)
        {
            this.sprintView = sprintView;
            this.chunkFacades = chunkFacades;
            this.checkpoint = checkpoint;
        }

        public void Initialize()
        {
            foreach (ChunkFacade chunkFacade in chunkFacades)
            {
                chunkFacade.Parent = sprintView.transform;
            }

            checkpoint.Parent = sprintView.transform;
        }
    }
}
