using UnityEngine;
using System;
using System.Collections.Generic;
using Zenject;

namespace MistRidge
{
    [Serializable]
    public struct Biome
    {
        public string name;
        public List<GameObject> chunks;
        public float startAltitude;
        public float endAltitude;
    }
}
