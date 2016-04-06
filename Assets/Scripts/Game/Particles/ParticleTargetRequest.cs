using UnityEngine;

namespace MistRidge
{
    public struct ParticleTargetRequest
    {
        public ParticleSystem particleSystem;
        public int particleCount;
        public float targetTime;
        public Transform targetTransform;

        public ParticleTargetType particleTargetType;
        public PlayerView playerView;
        public PlayerFacade playerFacade;
        public ItemType itemType;
    }
}
