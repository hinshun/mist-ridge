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
        private readonly DeathManager deathManager;
        private readonly CameraView cameraView;
        private readonly CameraManager cameraManager;
        private readonly CameraAnchorManager cameraAnchorManager;

        public LevelManager(
                GameStateSignal.Trigger gameStateTrigger,
                SpawnManager spawnManager,
                ChunkManager chunkManager,
                InputManager inputManager,
                PlayerManager playerManager,
                DeathManager deathManager,
                CameraView cameraView,
                CameraManager cameraManager,
                CameraAnchorManager cameraAnchorManager)
        {
            this.gameStateTrigger = gameStateTrigger;
            this.spawnManager = spawnManager;
            this.chunkManager = chunkManager;
            this.inputManager = inputManager;
            this.playerManager = playerManager;
            this.deathManager = deathManager;
            this.cameraView = cameraView;
            this.cameraManager = cameraManager;
            this.cameraAnchorManager = cameraAnchorManager;
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

            cameraView.IsActive = true;
            cameraAnchorManager.ResetAnchor();
            cameraManager.ResetCamera();
            deathManager.IsActive = true;
        }
    }
}
