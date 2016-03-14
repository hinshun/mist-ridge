using UnityEngine;
using System.Collections.Generic;

namespace MistRidge
{
    public interface IBiome
    {
        float StartAltitude();
        float EndAltitude();
        List<ChunkFeature> ChunkFeatures();
    }
}
