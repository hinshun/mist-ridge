using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Zenject;

namespace MistRidge
{
    public class ParticleTargetView : MonoView
    {
        [SerializeField]
        private float targetDelay;

        [SerializeField]
        private float targetAttraction;

        private Dictionary<ParticleSystem, ParticleTargetFulfillment> targetMapping;
        private Hashtable targetHashtable;

        private AetherGainSignal.Trigger aetherGainTrigger;
        private RespawnSignal.Trigger respawnTrigger;
        private ItemGainSignal.Trigger itemGainTrigger;

        [PostInject]
        public void Init(
                AetherGainSignal.Trigger aetherGainTrigger,
                RespawnSignal.Trigger respawnTrigger,
                ItemGainSignal.Trigger itemGainTrigger)
        {
            this.aetherGainTrigger = aetherGainTrigger;
            this.respawnTrigger = respawnTrigger;
            this.itemGainTrigger = itemGainTrigger;
        }

        public void Target(ParticleTargetRequest particleTargetRequest)
        {
            ParticleSystem particleSystem = particleTargetRequest.particleSystem;

            if (!targetMapping.ContainsKey(particleSystem))
            {
                targetMapping.Add(particleSystem, new ParticleTargetFulfillment()
                {
                    tweening = false
                });
            }

            ParticleTargetFulfillment particleTargetFulfillment = targetMapping[particleSystem];

            if (particleTargetFulfillment.tweening)
            {
                return;
            }

            particleTargetFulfillment = new ParticleTargetFulfillment()
            {
                tweening = true,
                particles = new ParticleSystem.Particle[particleTargetRequest.particleCount],
                particleCount = particleTargetRequest.particleCount,
                targetTransform = particleTargetRequest.targetTransform,
                particleTargetType = particleTargetRequest.particleTargetType,
                playerView = particleTargetRequest.playerView,
                playerFacade = particleTargetRequest.playerFacade,
                itemType = particleTargetRequest.itemType,
            };

            particleSystem.Emit(particleTargetRequest.particleCount);
            particleSystem.GetParticles(particleTargetFulfillment.particles);

            targetMapping[particleSystem] = particleTargetFulfillment;

            targetHashtable["time"] = particleTargetRequest.targetTime;
            targetHashtable["onupdate"] = (Action<object>) (attractionForce => {
                OnAttractParticles(particleSystem, (float)attractionForce);
            });
            targetHashtable["oncompleteparams"] = particleSystem;

            iTween.ValueTo(gameObject, targetHashtable);
        }

        public void OnAttractParticles(ParticleSystem particleSystem, float attractionForce)
        {
            ParticleTargetFulfillment particleTargetFulfillment = targetMapping[particleSystem];

            int particleCount = particleTargetFulfillment.particleCount;
            ParticleSystem.Particle[] particles = particleTargetFulfillment.particles;
            ParticleTargetType particleTargetType = particleTargetFulfillment.particleTargetType;
            Transform targetTransform = particleTargetFulfillment.targetTransform;
            PlayerView playerView = particleTargetFulfillment.playerView;
            PlayerFacade playerFacade = particleTargetFulfillment.playerFacade;

            for (int i = 0; i < particleCount; ++i)
            {
                particles[i].position += attractionForce * (targetTransform.position - particles[i].position).normalized + ((1f - (attractionForce / targetAttraction)) * particles[i].velocity * Time.deltaTime);
            }

            switch(particleTargetType)
            {
                case ParticleTargetType.Aether:
                    OnAttractAether(particles, particleCount, targetTransform, playerView);
                    break;

                case ParticleTargetType.Respawn:
                    OnAttractRespawn(particles, particleCount, targetTransform, playerFacade);
                    break;
            }

            particleSystem.SetParticles(particles, particles.Length);
        }

        public void OnTargetEnd(ParticleSystem particleSystem)
        {
            ParticleTargetFulfillment particleTargetFulfillment = targetMapping[particleSystem];

            targetMapping[particleSystem] = new ParticleTargetFulfillment()
            {
                tweening = false
            };
            particleSystem.Clear();

            ParticleTargetType particleTargetType = particleTargetFulfillment.particleTargetType;

            switch(particleTargetType)
            {
                case ParticleTargetType.Respawn:
                    OnRespawnEnd(particleTargetFulfillment.playerFacade);
                    break;
            }
        }

        private void Awake()
        {
            targetMapping = new Dictionary<ParticleSystem, ParticleTargetFulfillment>();

            targetHashtable = new Hashtable();
            targetHashtable.Add("from", 0);
            targetHashtable.Add("to", targetAttraction);
            targetHashtable.Add("delay", targetDelay);
            targetHashtable.Add("easetype", iTween.EaseType.easeInExpo);
            targetHashtable.Add("oncomplete", "OnTargetEnd");
        }

        private void OnAttractAether(ParticleSystem.Particle[] particles, int particleCount, Transform targetTransform, PlayerView playerView)
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

        private void OnAttractRespawn(ParticleSystem.Particle[] particles, int particleCount, Transform targetTransform, PlayerFacade playerFacade)
        {
            for (int i = 0; i < particleCount; ++i)
            {
                if (particles[i].lifetime >= 0 && (particles[i].position - targetTransform.position).magnitude < 1)
                {
                    particles[i].lifetime = -1;
                    respawnTrigger.Fire(playerFacade);
                }
            }
        }

        private void OnRespawnEnd(PlayerFacade playerFacade)
        {
            respawnTrigger.Fire(playerFacade);
        }
    }
}
