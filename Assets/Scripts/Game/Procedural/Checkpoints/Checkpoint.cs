using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class Checkpoint : IInitializable
    {
        private readonly ChunkFacade chunkFacade;
        private readonly CheckpointManager checkpointManager;
        private readonly AetherManager aetherManager;
        private readonly PlayerManager playerManager;

        private int aetherPosition;

        public Checkpoint(
                ChunkFacade chunkFacade,
                CheckpointManager checkpointManager,
                AetherManager aetherManager,
                PlayerManager playerManager)
        {
            this.chunkFacade = chunkFacade;
            this.checkpointManager = checkpointManager;
            this.aetherManager = aetherManager;
            this.playerManager = playerManager;
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
            chunkFacade.CheckpointWallView.Open();
        }

        public void OnCheckpointArrival(PlayerView playerView)
        {
            aetherManager.AddAether(playerView, AetherAward);
            aetherPosition += 1;

            if (aetherPosition == playerManager.PlayerCount)
            {
                checkpointManager.FinishCheckpoint(this);
            }
        }
    }
}
