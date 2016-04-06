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
            ResetVariables();
        }

        public void ResetVariables()
        {
            int nextSeed = 0;

            if (settings.randomizeSeed || settings.seedPool.Seeds.Count == 0)
            {
                nextSeed = Mathf.FloorToInt(UnityEngine.Random.Range(0, 10000));
            }
            else
            {
                int randomIndex = Mathf.FloorToInt(UnityEngine.Random.Range(0, settings.seedPool.Seeds.Count));
                nextSeed = settings.seedPool.Seeds[randomIndex];
            }

            Debug.Log("Seed: " + nextSeed);
            this.random = new System.Random(nextSeed);
            UnityEngine.Random.seed = nextSeed;
        }

        [Serializable]
        public class Settings
        {
            public bool randomizeSeed;
            public SeedPool seedPool;
        }
    }
}
