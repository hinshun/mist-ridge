using UnityEngine;
using System;
using System.Collections.Generic;
using Zenject;

namespace MistRidge
{
    public class CheckpointManager : IInitializable, IDisposable, ITickable
    {
        private readonly SpawnManager spawnManager;
        private readonly MistManager mistManager;
        private readonly SprintManager sprintManager;
        private readonly CheckpointSignal checkpointSignal;
        private readonly CheckpointActionSignal checkpointActionSignal;
        private readonly GameStateSignal.Trigger gameStateTrigger;

        private bool finishedLastCheckpoint;
        private Checkpoint lastCheckpoint;
        private Checkpoint currentCheckpoint;
        private Dictionary<CheckpointView, Checkpoint> checkpointMapping;

        public CheckpointManager(
                SpawnManager spawnManager,
                MistManager mistManager,
                SprintManager sprintManager,
                CheckpointSignal checkpointSignal,
                CheckpointActionSignal checkpointActionSignal,
                GameStateSignal.Trigger gameStateTrigger)
        {
            this.spawnManager = spawnManager;
            this.mistManager = mistManager;
            this.sprintManager = sprintManager;
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

        public Checkpoint LastCheckpoint
        {
            get
            {
                return lastCheckpoint;
            }
            set
            {
                lastCheckpoint = value;
            }
        }

        public bool FinishedLastCheckpoint
        {
            get
            {
                return finishedLastCheckpoint;
            }
        }

        public void Initialize()
        {
            ResetVariables();
            checkpointSignal.Event += OnCheckpointArrival;
            checkpointActionSignal.Event += OnCheckpointAction;
        }

        public void Dispose()
        {
            checkpointSignal.Event -= OnCheckpointArrival;
            checkpointActionSignal.Event -= OnCheckpointAction;
        }

        public void ResetVariables()
        {
            finishedLastCheckpoint = false;
            checkpointMapping = new Dictionary<CheckpointView, Checkpoint>();
        }

        public void Tick()
        {
            if (currentCheckpoint != null)
            {
                currentCheckpoint.Tick();
            }
        }

        public void AddCheckpoint(Checkpoint checkpoint)
        {
            checkpointMapping.Add(checkpoint.CheckpointView, checkpoint);
        }

        public void FinishCheckpoint(Checkpoint checkpoint)
        {
            if (finishedLastCheckpoint)
            {
                return;
            }

            mistManager.UpdateMistPosition(currentCheckpoint.CheckpointView.Position.y);

            sprintManager.SetSprintNum(currentCheckpoint.CheckpointNum + 1);
            sprintManager.UpdateSprintText();

            spawnManager.CurrentSpawnView = currentCheckpoint.SpawnView;
            currentCheckpoint.RespawnPlayers();

            if (checkpoint == lastCheckpoint)
            {
                finishedLastCheckpoint = true;
            }
        }

        public void FinishGame()
        {
            gameStateTrigger.Fire(GameStateType.End);
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
                    CurrentCheckpoint.Finish();

                    return;

                case CheckpointAction.Respawn:
                    CurrentCheckpoint.RespawnPlayers();
                    return;
            }
        }
    }
}
