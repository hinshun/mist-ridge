using UnityEngine;

namespace MistRidge
{
    public class StartingZoneView : MonoView
    {
        [SerializeField]
        private SpawnSpawnView initialSpawn;

        [SerializeField]
        private SpawnSpawnView normalSpawn;

        public SpawnSpawnView InitialSpawn
        {
            get
            {
                return initialSpawn;
            }
        }

        public SpawnSpawnView NormalSpawn
        {
            get
            {
                return normalSpawn;
            }
        }
    }
}
