using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class ChunkFacade : Facade
    {
        private readonly ChunkView chunkView;

        public ChunkFacade(ChunkView chunkView)
        {
            this.chunkView = chunkView;
        }

        public Vector3 Position
        {
            get
            {
                return chunkView.Position;
            }
        }
    }
}
