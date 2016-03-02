using UnityEngine;
using UnityEngine.UI;
using System;
using Zenject;

namespace MistRidge
{
    public class ReadyManager : IInitializable
    {
        private readonly Settings settings;
        private readonly Spawn spawn;
        private readonly SpawnManager spawnManager;
        private readonly CameraManager cameraManager;
        private readonly GameStateSignal.Trigger gameStateTrigger;

        public ReadyManager(
                Settings settings,
                Spawn spawn,
                SpawnManager spawnManager,
                CameraManager cameraManager,
                GameStateSignal.Trigger gameStateTrigger)
        {
            this.settings = settings;
            this.spawn = spawn;
            this.spawnManager = spawnManager;
            this.cameraManager = cameraManager;
            this.gameStateTrigger = gameStateTrigger;
        }

        public void Initialize()
        {
            gameStateTrigger.Fire(GameStateType.Ready);
            spawnManager.CurrentSpawn = spawn;
            cameraManager.CurrentCamera = settings.camera;
        }

        [Serializable]
        public class Settings
        {
            public Camera camera;
        }
    }
}
