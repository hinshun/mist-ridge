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
        private readonly DeathManager deathManager;

        private int aetherPosition;

        public Checkpoint(
                ChunkFacade chunkFacade,
                SpawnManager spawnManager,
                CheckpointManager checkpointManager,
                AetherManager aetherManager,
                DeathManager deathManager)
        {
            this.chunkFacade = chunkFacade;
            this.spawnManager = spawnManager;
            this.checkpointManager = checkpointManager;
            this.aetherManager = aetherManager;
            this.deathManager = deathManager;
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

            List<PlayerFacade> deadPlayerFacades = deathManager.DeadPlayerFacades;
            foreach (PlayerFacade playerFacade in deadPlayerFacades)
            {
                playerFacade.Position = spawnManager.CurrentSpawnView.SpawnPoint(playerFacade.Input.DeviceNum);
                deathManager.Respawn(playerFacade);
            }

            chunkFacade.CheckpointWallView.Open();
        }

        public void OnCheckpointArrival(PlayerView playerView)
        {
            aetherManager.AddAether(playerView, AetherAward);
            aetherPosition += 1;

            if (aetherPosition == deathManager.AlivePlayerCount)
            {
                checkpointManager.FinishCheckpoint(this);
            }
        }
    }
}
