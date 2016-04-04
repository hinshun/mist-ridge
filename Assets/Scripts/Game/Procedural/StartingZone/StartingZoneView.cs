using UnityEngine;

namespace MistRidge
{
    public class StartingZoneView : MonoView
    {
        [SerializeField]
        private SpawnSpawnView initialSpawn;

        [SerializeField]
        private SpawnSpawnView normalSpawn;

        [SerializeField]
        private TurnipView turnipView;

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

        public TurnipView TurnipView
        {
            get
            {
                return turnipView;
            }
        }
    }
}
