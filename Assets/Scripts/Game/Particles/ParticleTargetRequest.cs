using UnityEngine;

namespace MistRidge
{
    public struct ParticleTargetRequest
    {
        public ParticleSystem particleSystem;
        public int particleCount;
        public Transform targetTransform;

        public ParticleTargetType particleTargetType;
        public PlayerView playerView;
        public ItemType itemType;
    }
}
