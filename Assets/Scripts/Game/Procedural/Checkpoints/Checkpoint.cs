using UnityEngine;
using System;
using System.Collections.Generic;
using Zenject;

namespace MistRidge
{
    public class Checkpoint : IInitializable
    {
        private readonly ChunkFacade chunkFacade;
        private readonly SpawnManager spawnManager;
        private readonly CheckpointManager checkpointManager;
        private readonly AetherManager aetherManager;
        private readonly PlayerManager playerManager;
        private readonly DeathManager deathManager;

        private int aetherPosition;
        private Checkpoint nextCheckpoint;

        public Checkpoint(
                ChunkFacade chunkFacade,
                SpawnManager spawnManager,
                CheckpointManager checkpointManager,
                AetherManager aetherManager,
                PlayerManager playerManager,
                DeathManager deathManager)
        {
            this.chunkFacade = chunkFacade;
            this.spawnManager = spawnManager;
            this.checkpointManager = checkpointManager;
            this.aetherManager = aetherManager;
            this.playerManager = playerManager;
            this.deathManager = deathManager;
        }

        public Checkpoint NextCheckpoint
        {
            get
            {
                return nextCheckpoint;
            }
            set
            {
                nextCheckpoint = value;
            }
        }

        public Vector3 Position
        {
            get
            {
                return chunkFacade.Position;
            }
        }

        public Transform Parent
        {
            get
            {
                return chunkFacade.Parent;
            }
            set
            {
                chunkFacade.Parent = value;
            }
        }

        public CheckpointView CheckpointView
        {
            get
            {
                return chunkFacade.CheckpointView;
            }
        }

        public int AetherAward
        {
            get
            {
                switch (aetherPosition)
                {
                    case 0:
                        return 5;
                    case 1:
                        return 3;
                    case 2:
                        return 2;
                    case 3:
                        return 1;
                }

                return 0;
            }
        }

        public SpawnView SpawnView
        {
            get
            {
                return chunkFacade.SpawnView;
            }
        }

        public void Initialize()
        {
            chunkFacade.Name = "Checkpoint";
            aetherPosition = 0;
        }

        public void Open()
        {
            CheckpointView.SetActive(false);
            spawnManager.CurrentSpawnView = SpawnView;
            checkpointManager.CurrentCheckpoint = this;

            RespawnPlayers();

            List<PlayerFacade> alivePlayerFacades = deathManager.AlivePlayerFacades;
            foreach (PlayerFacade playerFacade in alivePlayerFacades)
            {
                playerFacade.StopDance();
            }

            chunkFacade.CheckpointWallView.SetActive(false);
        }

        public void RespawnPlayers()
        {
            List<PlayerFacade> deadPlayerFacades = deathManager.DeadPlayerFacades;
            foreach (PlayerFacade playerFacade in deadPlayerFacades)
            {
                playerFacade.Position = spawnManager.CurrentSpawnView.SpawnPoint(playerFacade.Input.DeviceNum);
                playerFacade.MoveDirection = Vector3.zero;
                deathManager.Respawn(playerFacade);
            }
        }

        public void OnCheckpointArrival(PlayerView playerView)
        {
            aetherManager.AddAether(playerView, AetherAward);
            aetherPosition += 1;

            Input input = playerManager.Input(playerView);
            PlayerFacade playerFacade = playerManager.PlayerFacade(input);
            playerFacade.Dance();

            Finish();
        }

        public void Finish()
        {
            if (aetherPosition == deathManager.AlivePlayerCount)
            {
                checkpointManager.FinishCheckpoint(this);
            }
        }
    }
}
