using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class LevelManager : IInitializable
    {
        private readonly GameStateSignal.Trigger gameStateTrigger;
        private readonly GameStateMachine gameStateMachine;
        private readonly SpawnManager spawnManager;
        private readonly ChunkManager chunkManager;
        private readonly InputManager inputManager;
        private readonly PlayerManager playerManager;
        private readonly DeathManager deathManager;
        private readonly RankManager rankManager;
        private readonly MistManager mistManager;
        private readonly CheckpointManager checkpointManager;
        private readonly CameraView cameraView;
        private readonly CameraManager cameraManager;
        private readonly CameraAnchorManager cameraAnchorManager;

        public LevelManager(
                GameStateSignal.Trigger gameStateTrigger,
                GameStateMachine gameStateMachine,
                SpawnManager spawnManager,
                ChunkManager chunkManager,
                InputManager inputManager,
                PlayerManager playerManager,
                DeathManager deathManager,
                RankManager rankManager,
                MistManager mistManager,
                CheckpointManager checkpointManager,
                CameraView cameraView,
                CameraManager cameraManager,
                CameraAnchorManager cameraAnchorManager)
        {
            this.gameStateTrigger = gameStateTrigger;
            this.gameStateMachine = gameStateMachine;
            this.spawnManager = spawnManager;
            this.chunkManager = chunkManager;
            this.inputManager = inputManager;
            this.playerManager = playerManager;
            this.deathManager = deathManager;
            this.rankManager = rankManager;
            this.mistManager = mistManager;
            this.checkpointManager = checkpointManager;
            this.cameraView = cameraView;
            this.cameraManager = cameraManager;
            this.cameraAnchorManager = cameraAnchorManager;
        }

        public void Initialize()
        {
            ChunkFacade startingChunkFacade = chunkManager.StartingChunkFacade;
            StartingZoneView startingZoneView = startingChunkFacade.ChunkView.GetComponentInChildren<StartingZoneView>();;

            spawnManager.CurrentSpawnView = startingZoneView.InitialSpawn.GetComponentInChildren<SpawnView>();

            gameStateTrigger.Fire(GameStateType.Play);

            foreach (Input input in gameStateMachine.GameReadyState.JoinedInputs)
            {
                PlayerFacade playerFacade = playerManager.SpawnPlayer(input);
                deathManager.AddPlayer(input);

                playerFacade.Freefall();
                playerFacade.Player.Gravity = 1f;
                playerFacade.MoveDirection = Vector3.zero;

                playerFacade.ProbeGround();
            }

            spawnManager.CurrentSpawnView = startingZoneView.NormalSpawn.GetComponentInChildren<SpawnView>();

            cameraView.IsActive = true;
            cameraAnchorManager.ResetAnchor();
            cameraManager.ResetCamera();

            deathManager.IsActive = true;

            mistManager.IsActive = true;
            mistManager.UpdateMistPosition(checkpointManager.CurrentCheckpoint.CheckpointView.Position.y);

            rankManager.IsActive = true;
        }
    }
}
