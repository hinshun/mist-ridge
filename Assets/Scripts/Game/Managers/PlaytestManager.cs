using UnityEngine;
using Zenject;

namespace MistRidge
{
    public class PlaytestManager : IInitializable
    {
        private readonly SpawnView spawnView;
        private readonly SpawnManager spawnManager;
        private readonly CameraView cameraView;
        private readonly GameStateSignal.Trigger gameStateTrigger;

        public PlaytestManager(
                SpawnView spawnView,
                SpawnManager spawnManager,
                CameraView cameraView,
                GameStateSignal.Trigger gameStateTrigger)
        {
            this.spawnView = spawnView;
            this.spawnManager = spawnManager;
            this.cameraView = cameraView;
            this.gameStateTrigger = gameStateTrigger;
        }

        public void Initialize()
        {
            spawnManager.CurrentSpawnView = spawnView;
            gameStateTrigger.Fire(GameStateType.Play);

            cameraView.IsActive = true;
        }
    }
}
