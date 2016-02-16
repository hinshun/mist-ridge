using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class Chunk
    {
        private readonly ChunkView chunkView;

        public Chunk(ChunkView chunkView)
        {
            this.chunkView = chunkView;
        }
    }
}
