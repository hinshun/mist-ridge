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

        private Checkpoint checkpoint;

        public Sprint(
                SprintView sprintView,
                List<ChunkFacade> chunkFacades,
                Checkpoint checkpoint)
        {
            this.sprintView = sprintView;
            this.chunkFacades = chunkFacades;
            this.checkpoint = checkpoint;
        }

        public Checkpoint Checkpoint
        {
            get
            {
                return checkpoint;
            }
            set
            {
                checkpoint = value;
            }
        }

        public List<ChunkFacade> ChunkFacades
        {
            get
            {
                return chunkFacades;
            }
        }

        public void Initialize()
        {
            foreach (ChunkFacade chunkFacade in chunkFacades)
            {
                chunkFacade.Parent = sprintView.transform;
            }

            if (checkpoint != null)
            {
                checkpoint.Parent = sprintView.transform;
            }
        }
    }
}
