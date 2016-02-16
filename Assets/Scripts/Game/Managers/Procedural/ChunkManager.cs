using UnityEngine;
using System;
using System.Collections.Generic;
using Zenject;

namespace MistRidge
{
    public class ChunkManager
    {
        private readonly Settings settings;

        public ChunkManager(
                Settings settings)
        {
            this.settings = settings;
        }

        [Serializable]
        public class Settings
        {
            public List<GameObject> chunks;
        }
    }
}
