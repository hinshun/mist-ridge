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
        private readonly FinishCheckpointSignal finishCheckpointSignal;
        private readonly GameStateSignal.Trigger gameStateTrigger;

        private Checkpoint lastCheckpoint;
        private Checkpoint currentCheckpoint;
        private Dictionary<CheckpointView, Checkpoint> checkpointMapping;

        public CheckpointManager(
                SpawnManager spawnManager,
                CheckpointSignal checkpointSignal,
                FinishCheckpointSignal finishCheckpointSignal,
                GameStateSignal.Trigger gameStateTrigger)
        {
            this.spawnManager = spawnManager;
            this.checkpointSignal = checkpointSignal;
            this.finishCheckpointSignal = finishCheckpointSignal;
            this.gameStateTrigger = gameStateTrigger;
        }

        public Checkpoint CurrentCheckpoint
        {
            get
            {
                return currentCheckpoint;
            }
            set
            {
                currentCheckpoint = value;
            }
        }

        public void Initialize()
        {
            checkpointMapping = new Dictionary<CheckpointView, Checkpoint>();
            checkpointSignal.Event += OnCheckpointArrival;
            finishCheckpointSignal.Event += OnCheckpointFinish;
        }

        public void AddCheckpoint(Checkpoint checkpoint)
        {
            lastCheckpoint = checkpoint;
            checkpointMapping.Add(checkpoint.CheckpointView, checkpoint);
        }

        public void FinishCheckpoint(Checkpoint checkpoint)
        {
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

        private void OnCheckpointFinish()
        {
            if (CurrentCheckpoint != null)
            {
                CurrentCheckpoint.Finish();
            }
        }
    }
}
