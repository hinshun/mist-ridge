using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class Checkpoint : IInitializable
    {
        private readonly ChunkFacade chunkFacade;

        public Checkpoint(ChunkFacade chunkFacade)
        {
            this.chunkFacade = chunkFacade;
        }

        public Transform Parent
        {
            get
            {
                return chunkFacade.Parent;
            }
            set
            {
                chunkFacade.Parent = value;
            }
        }

        public void Initialize()
        {
            chunkFacade.Name = "Checkpoint";
        }
    }
}
