using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class Generator : IInitializable
    {
        private readonly Settings settings;

        private System.Random random;

        public Generator(Settings settings)
        {
            this.settings = settings;
        }

        public System.Random Random
        {
            get
            {
                return random;
            }
        }

        public void Initialize()
        {
            this.random = new System.Random(settings.seed);
        }

        [Serializable]
        public class Settings
        {
            public int seed;
        }
    }
}
