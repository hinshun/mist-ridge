using UnityEngine;
using System;
using System.Collections.Generic;

namespace MistRidge
{
    public class CheckpointView : MonoView
    {
        [SerializeField]
        private List<ParticleSystem> aetherAwards;

        [SerializeField]
        private ParticleTargetView particleTargetView;

        private Queue<ParticleSystem> aetherAwardQueue;

        public ParticleSystem AetherAward
        {
            get
            {
                ParticleSystem aetherAward = aetherAwardQueue.Dequeue();
                aetherAwardQueue.Enqueue(aetherAward);

                return aetherAward;
            }
        }

        public ParticleTargetView ParticleTargetView
        {
            get
            {
                return particleTargetView;
            }
        }

        private void Awake()
        {
            aetherAwardQueue = new Queue<ParticleSystem>(aetherAwards);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                PlayerView playerView = other.GetComponent<PlayerView>();
                playerView.OnCheckpoint(this);
            }
        }
    }
}
