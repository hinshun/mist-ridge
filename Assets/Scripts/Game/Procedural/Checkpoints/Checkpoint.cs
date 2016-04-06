using UnityEngine;
using System;
using System.Collections.Generic;
using Zenject;

namespace MistRidge
{
    public class Checkpoint : IInitializable, ITickable
    {
        private readonly int checkpointNum;
        private readonly ChunkFacade chunkFacade;
        private readonly SpawnManager spawnManager;
        private readonly CheckpointManager checkpointManager;
        private readonly AetherManager aetherManager;
        private readonly InputManager inputManager;
        private readonly PlayerManager playerManager;
        private readonly ReadySetGoManager readySetGoManager;
        private readonly DeathManager deathManager;

        private bool waitingForRespawn;
        private bool finished;
        private int aetherPosition;
        private Checkpoint nextCheckpoint;

        public Checkpoint(
                int checkpointNum,
                ChunkFacade chunkFacade,
                SpawnManager spawnManager,
                CheckpointManager checkpointManager,
                AetherManager aetherManager,
                InputManager inputManager,
                PlayerManager playerManager,
                ReadySetGoManager readySetGoManager,
                DeathManager deathManager)
        {
            this.checkpointNum = checkpointNum;
            this.chunkFacade = chunkFacade;
            this.spawnManager = spawnManager;
            this.checkpointManager = checkpointManager;
            this.aetherManager = aetherManager;
            this.inputManager = inputManager;
            this.playerManager = playerManager;
            this.readySetGoManager = readySetGoManager;
            this.deathManager = deathManager;
        }

        public bool WaitingForRespawn
        {
            get
            {
                return waitingForRespawn;
            }
            set
            {
                waitingForRespawn = value;
            }
        }

        public int CheckpointNum
        {
            get
            {
                return checkpointNum;
            }
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
            waitingForRespawn = false;
            finished = false;
            aetherPosition = 0;
            chunkFacade.Name = "Checkpoint";
        }

        public void Tick()
        {
            if (!waitingForRespawn)
            {
                return;
            }

            if (deathManager.DeadPlayerCount == 0)
            {
                waitingForRespawn = false;

                if (checkpointManager.FinishedLastCheckpoint)
                {
                    checkpointManager.FinishGame();
                    return;
                }

                Open();
            }
        }

        public void Open()
        {
            readySetGoManager.Countdown();
            chunkFacade.CheckpointWallView.SetActive(false);
        }

        public void RespawnPlayers()
        {
            if (waitingForRespawn)
            {
                return;
            }

            foreach (Input input in inputManager.Inputs)
            {
                if (!playerManager.HasPlayerFacade(input))
                {
                    continue;
                }

                PlayerFacade playerFacade = playerManager.PlayerFacade(input);
                deathManager.Respawn(playerFacade);

                if (!deathManager.IsTutorial)
                {
                    waitingForRespawn = true;
                }
            }
        }

        public void OnCheckpointArrival(PlayerView playerView)
        {
            aetherManager.AddAether(CheckpointView, playerView, AetherAward);
            aetherPosition += 1;

            Input input = playerManager.Input(playerView);
            PlayerFacade playerFacade = playerManager.PlayerFacade(input);
            playerFacade.Victory();

            Finish();
        }

        public void Finish()
        {
            if (!finished)
            {
                checkpointManager.CurrentCheckpoint = this;

                if (aetherPosition == deathManager.AlivePlayerCount)
                {
                    finished = true;
                    checkpointManager.FinishCheckpoint(this);
                }
            }
        }
    }
}
