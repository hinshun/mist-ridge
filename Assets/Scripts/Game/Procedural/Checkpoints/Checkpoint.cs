using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class Checkpoint
    {
        private readonly Settings settings;

        public Checkpoint(Settings settings)
        {
            this.settings = settings;
        }

        public class Factory : Factory<Checkpoint>
        {
        }

        [Serializable]
        public class Settings
        {
            public GameObject Prefab;
        }
    }
}
