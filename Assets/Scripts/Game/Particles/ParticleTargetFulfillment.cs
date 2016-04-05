using UnityEngine;

namespace MistRidge
{
    public struct ParticleTargetFulfillment
    {
        public bool tweening;

        public ParticleSystem.Particle[] particles;
        public int particleCount;
        public Transform targetTransform;

        public ParticleTargetType particleTargetType;
        public PlayerView playerView;
        public PlayerFacade playerFacade;
        public ItemType itemType;
    }
}
