using UnityEngine;
using System.Collections.Generic;

namespace MistRidge
{
    public interface IChunkFeaturePickingStrategy
    {
        GameObject Pick(List<GameObject> chunkFeatures);
    }
}
