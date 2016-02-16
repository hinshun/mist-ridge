using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class LevelManager
    {
        private readonly Settings settings;

        public LevelManager(
                Settings settings)
        {
            this.settings = settings;
        }

        public void Generate()
        {
        }

        [Serializable]
        public class Settings
        {
        }
    }
}
