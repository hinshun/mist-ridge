using UnityEngine;
using System.Collections.Generic;
using Zenject;

namespace MistRidge
{
    public interface IChunkFeaturePickingStrategy : IInitializable
    {
        ChunkFeature Pick(IChunkFeatureContainer chunkFeatureContainer);
    }
}
