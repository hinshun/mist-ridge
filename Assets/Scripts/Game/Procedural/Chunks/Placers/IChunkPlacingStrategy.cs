using UnityEngine;
using System;

namespace MistRidge
{
    public interface IChunkPlacingStrategy
    {
        void Place(ChunkView chunkView, ChunkRequest chunkRequest);
    }
}
