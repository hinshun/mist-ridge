using UnityEngine;
using System;
using System.Collections.Generic;
using Zenject;

namespace MistRidge
{
    public class CheckpointManager : IInitializable
    {
        private readonly SpawnManager spawnManager;
        private readonly MistManager mistManager;
        private readonly CheckpointSignal checkpointSignal;
        private readonly CheckpointActionSignal checkpointActionSignal;
        private readonly GameStateSignal.Trigger gameStateTrigger;

        private Checkpoint lastCheckpoint;
        private Checkpoint currentCheckpoint;
        private Dictionary<CheckpointView, Checkpoint> checkpointMapping;

        public CheckpointManager(
                SpawnManager spawnManager,
                MistManager mistManager,
                CheckpointSignal checkpointSignal,
                CheckpointActionSignal checkpointActionSignal,
                GameStateSignal.Trigger gameStateTrigger)
        {
            this.spawnManager = spawnManager;
            this.mistManager = mistManager;
            this.checkpointSignal = checkpointSignal;
            this.checkpointActionSignal = checkpointActionSignal;
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
            checkpointActionSignal.Event += OnCheckpointAction;
        }

        public void AddCheckpoint(Checkpoint checkpoint)
        {
            lastCheckpoint = checkpoint;
            checkpointMapping.Add(checkpoint.CheckpointView, checkpoint);
        }

        public void FinishCheckpoint(Checkpoint checkpoint)
        {
            checkpoint.Open();
            if (checkpoint.NextCheckpoint != null)
            {
                mistManager.UpdateMistPosition(checkpoint.NextCheckpoint.CheckpointView.Position.y);
            }

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

        private void OnCheckpointAction(CheckpointAction checkpointAction)
        {
            switch (checkpointAction)
            {
                case CheckpointAction.Finish:
                    CurrentCheckpoint.NextCheckpoint.Finish();
                    return;

                case CheckpointAction.Respawn:
                    CurrentCheckpoint.RespawnPlayers();
                    return;
            }
        }
    }
}
