using UnityEngine;
using System;
using System.Collections.Generic;

namespace MistRidge
{
    [CreateAssetMenu(menuName = "Chunks/Biome")]
    public class Biome : GenericChunkFeatureContainer
    {
        [SerializeField]
        new private string name;

        [SerializeField]
        private float startAltitude;

        [SerializeField]
        private float endAltitude;
    }
}
