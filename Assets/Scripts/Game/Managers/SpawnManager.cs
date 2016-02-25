using UnityEngine;
using Zenject;

namespace MistRidge
{
    public class SpawnManager
    {
        private Spawn currentSpawn;

        public Spawn CurrentSpawn
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
