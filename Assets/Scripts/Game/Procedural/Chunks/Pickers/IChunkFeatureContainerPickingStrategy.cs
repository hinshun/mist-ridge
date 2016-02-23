using UnityEngine;
using System.Collections.Generic;

namespace MistRidge
{
    public interface IChunkFeatureContainerPickingStrategy
    {
        IChunkFeatureContainer Pick(List<IChunkFeatureContainer> chunkFeatureContainers);
    }
}
