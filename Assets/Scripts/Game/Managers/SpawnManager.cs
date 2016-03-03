using UnityEngine;
using Zenject;

namespace MistRidge
{
    public class SpawnManager
    {
        private SpawnView currentSpawn;

        public SpawnView CurrentSpawnView
        {
            get
            {
                return currentSpawn;
            }
            set
            {
                currentSpawn = value;
            }
        }
    }
}
