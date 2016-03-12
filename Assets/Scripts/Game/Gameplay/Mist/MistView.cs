using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class MistView : MonoView
    {
        private PlayerManager playerManager;
        private DeathManager deathManager;
        private BoxCollider collider;

        [PostInject]
        public void Init(
                PlayerManager playerManager,
                DeathManager deathManager)
        {
            this.playerManager = playerManager;
            this.deathManager = deathManager;
        }

        public BoxCollider Collider
        {
            get
            {
                return collider;
            }
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                PlayerView playerView = other.GetComponent<PlayerView>();
                Input input = playerManager.Input(playerView);
                PlayerFacade playerFacade = playerManager.PlayerFacade(input);

                deathManager.Kill(input, playerFacade);
            }
        }

        private void Awake()
        {
            collider = GetComponent<BoxCollider>();
        }
    }
}
