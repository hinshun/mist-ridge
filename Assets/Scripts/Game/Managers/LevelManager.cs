using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class LevelManager : IInitializable
    {
        private readonly GameStateSignal.Trigger gameStateTrigger;
        private readonly SpawnManager spawnManager;
        private readonly ChunkManager chunkManager;
        private readonly InputManager inputManager;
        private readonly PlayerManager playerManager;

        public LevelManager(
                GameStateSignal.Trigger gameStateTrigger,
                SpawnManager spawnManager,
                ChunkManager chunkManager,
                InputManager inputManager,
                PlayerManager playerManager)
        {
            this.gameStateTrigger = gameStateTrigger;
            this.spawnManager = spawnManager;
            this.chunkManager = chunkManager;
            this.inputManager = inputManager;
            this.playerManager = playerManager;
        }

        public void Initialize()
        {
            ChunkFacade startingChunkFacade = chunkManager.StartingChunkFacade;
            spawnManager.CurrentSpawnView = startingChunkFacade.SpawnView;

            gameStateTrigger.Fire(GameStateType.Play);

            foreach (Input input in inputManager.Inputs)
            {
                if (playerManager.HasPlayerFacade(input))
                {
                    PlayerFacade playerFacade = playerManager.PlayerFacade(input);
                    playerFacade.Position = spawnManager.CurrentSpawnView.SpawnPoint(input.DeviceNum);
                }
            }
        }
    }
}
