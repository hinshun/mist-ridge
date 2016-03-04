using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class FloorBoundsView : MonoView
    {
        private PlayerManager playerManager;
        private DeathManager deathManager;

        [PostInject]
        public void Init(
                PlayerManager playerManager,
                DeathManager deathManager)
        {
            this.playerManager = playerManager;
            this.deathManager = deathManager;
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                PlayerView playerView = other.GetComponent<PlayerView>();
                Input input = playerManager.Input(playerView);
                PlayerFacade playerFacade = playerManager.PlayerFacade(input);

                deathManager.Kill(playerFacade);
            }
        }
    }
}
