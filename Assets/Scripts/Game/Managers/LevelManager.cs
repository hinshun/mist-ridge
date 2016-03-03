using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class LevelManager : IInitializable
    {
        private readonly GameStateSignal.Trigger gameStateTrigger;
        private readonly SpawnManager spawnManager;
        private readonly InputManager inputManager;
        private readonly PlayerManager playerManager;

        public LevelManager(
                GameStateSignal.Trigger gameStateTrigger,
                SpawnManager spawnManager,
                InputManager inputManager,
                PlayerManager playerManager)
        {
            this.gameStateTrigger = gameStateTrigger;
            this.spawnManager = spawnManager;
            this.inputManager = inputManager;
            this.playerManager = playerManager;
        }

        public void Initialize()
        {
            gameStateTrigger.Fire(GameStateType.Play);

            foreach (Input input in inputManager.Inputs)
            {
                if (playerManager.HasPlayerFacade(input))
                {
                    PlayerFacade playerFacade = playerManager.PlayerFacade(input);
                    playerFacade.Position = spawnManager.CurrentSpawn.SpawnPoint(input.DeviceNum);
                }
            }
        }
    }
}
