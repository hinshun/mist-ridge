using UnityEngine;
using System.Collections.Generic;

namespace MistRidge
{
    public interface IChunkFeaturePickingStrategy
    {
        ChunkFeature Pick(IChunkFeatureContainer chunkFeatureContainer);
    }
}
