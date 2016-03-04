using UnityEngine;
using System;
using System.Collections.Generic;
using Zenject;

namespace MistRidge
{
    public class CheckpointManager : IInitializable
    {
        private readonly SpawnManager spawnManager;
        private readonly CheckpointSignal checkpointSignal;
        private readonly GameStateSignal.Trigger gameStateTrigger;

        private Checkpoint lastCheckpoint;
        private Dictionary<CheckpointView, Checkpoint> checkpointMapping;

        public CheckpointManager(
                SpawnManager spawnManager,
                CheckpointSignal checkpointSignal,
                GameStateSignal.Trigger gameStateTrigger)
        {
            this.spawnManager = spawnManager;
            this.checkpointSignal = checkpointSignal;
            this.gameStateTrigger = gameStateTrigger;
        }

        public void Initialize()
        {
            checkpointMapping = new Dictionary<CheckpointView, Checkpoint>();
            checkpointSignal.Event += OnCheckpointArrival;
        }

        public void AddCheckpoint(Checkpoint checkpoint)
        {
            lastCheckpoint = checkpoint;
            checkpointMapping.Add(checkpoint.CheckpointView, checkpoint);
        }

        public void FinishCheckpoint(Checkpoint checkpoint)
        {
            spawnManager.CurrentSpawnView = checkpoint.SpawnView;
            checkpoint.Open();

            if (checkpoint == lastCheckpoint)
            {
                gameStateTrigger.Fire(GameStateType.End);
            }
        }

        private void OnCheckpointArrival(CheckpointView checkpointView, PlayerView playerView)
        {
            if (!checkpointMapping.ContainsKey(checkpointView))
            {
                return;
            }

            Checkpoint arrivedCheckpoint = checkpointMapping[checkpointView];
            arrivedCheckpoint.OnCheckpointArrival(playerView);
        }
    }
}
