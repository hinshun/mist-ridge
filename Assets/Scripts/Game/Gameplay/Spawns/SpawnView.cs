using UnityEngine;
using System.Collections.Generic;

namespace MistRidge
{
    public class SpawnView : MonoView
    {
        [SerializeField]
        private List<SpawnPointView> spawnPointViews;

        [SerializeField]
        private ParticleTargetView particleTargetView;

        public ParticleTargetView ParticleTargetView
        {
            get
            {
                return particleTargetView;
            }
        }

        public Vector3 SpawnPoint(int index)
        {
            return spawnPointViews[index].Position;
        }

        public Transform SpawnTransform(int index)
        {
            return spawnPointViews[index].transform;
        }
    }
}
