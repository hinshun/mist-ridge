using UnityEngine;
using System;

namespace MistRidge
{
    public interface IChunkPlacingStrategy
    {
        void Placement(ChunkView chunkView, ChunkConfig chunkConfig);
    }
}
