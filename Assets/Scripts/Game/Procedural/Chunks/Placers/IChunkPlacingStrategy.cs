using UnityEngine;
using System;

namespace MistRidge
{
    public interface IChunkPlacingStrategy
    {
        Vector3 Placement(ChunkConfig chunkConfig);
    }
}
