using UnityEngine;
using System;
using System.Collections;
using Zenject;

namespace MistRidge
{
    public class ParticleTargetView : MonoView
    {
        [SerializeField]
        private float targetDelay;

        [SerializeField]
        private float targetAttraction;

        [SerializeField]
        private float targetTerminalVelocityFactor;

        private bool tweening;
        private Hashtable targetHashtable;

        private ParticleTargetType particleTargetType;
        private ParticleSystem particleSystem;
        private ParticleSystem.Particle[] particles;
        private int particleCount;
        private Transform targetTransform;

        private PlayerView playerView;
        private ItemType itemType;

        private AetherGainSignal.Trigger aetherGainTrigger;
        private ItemGainSignal.Trigger itemGainTrigger;

        [PostInject]
        public void Init(
                AetherGainSignal.Trigger aetherGainTrigger,
                ItemGainSignal.Trigger itemGainTrigger)
        {
            this.aetherGainTrigger = aetherGainTrigger;
            this.itemGainTrigger = itemGainTrigger;
        }

        public void Target(ParticleTargetRequest particleTargetRequest)
        {
            if (tweening)
            {
                return;
            }

            tweening = true;
            particleTargetType = particleTargetRequest.particleTargetType;
            particleSystem = particleTargetRequest.particleSystem;
            particleCount = particleTargetRequest.particleCount;
            targetTransform = particleTargetRequest.targetTransform;

            playerView = particleTargetRequest.playerView;
            itemType = particleTargetRequest.itemType;

            particleSystem.Emit(particleCount);
            particles = new ParticleSystem.Particle[particleSystem.particleCount];
            particleCount = particleSystem.GetParticles(particles);

            targetHashtable["time"] = particleSystem.startLifetime * targetTerminalVelocityFactor;
            iTween.ValueTo(gameObject, targetHashtable);
        }

        public void OnAttractParticles(float attractionForce)
        {
            for (int i = 0; i < particleCount; ++i)
            {
                particles[i].position += attractionForce * (targetTransform.position - particles[i].position).normalized + ((1f - (attractionForce / targetAttraction)) * particles[i].velocity * Time.deltaTime);
            }

            switch(particleTargetType)
            {
                case ParticleTargetType.Aether:
                    OnAttractAether();
                    break;
            }

            particleSystem.SetParticles(particles, particles.Length);
        }

        private void OnAttractAether()
        {
            for (int i = 0; i < particleCount; ++i)
            {
                if (particles[i].lifetime >= 0 && (particles[i].position - targetTransform.position).magnitude < 1)
                {
                    particles[i].lifetime = -1;
                    aetherGainTrigger.Fire(playerView);
                }
            }
        }

        public void OnTargetEnd()
        {
            tweening = false;
            particleSystem.Clear();
        }

        private void Awake()
        {
            tweening = false;

            targetHashtable = new Hashtable();
            targetHashtable.Add("from", 0);
            targetHashtable.Add("to", targetAttraction);
            targetHashtable.Add("delay", targetDelay);
            targetHashtable.Add("easetype", iTween.EaseType.easeInExpo);
            targetHashtable.Add("onupdate", "OnAttractParticles");
            targetHashtable.Add("oncomplete", "OnTargetEnd");
        }
    }
}
