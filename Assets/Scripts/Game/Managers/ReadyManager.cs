using UnityEngine;
using UnityEngine.UI;
using System;
using Zenject;

namespace MistRidge
{
    public class ReadyManager : IInitializable, IDisposable, ITickable
    {
        private readonly Settings settings;
        private readonly SpawnView spawnView;
        private readonly SpawnManager spawnManager;
        private readonly CameraManager cameraManager;
        private readonly PoolManager poolManager;
        private readonly GameStateSignal.Trigger gameStateTrigger;

        private float timer;

        public ReadyManager(
                Settings settings,
                SpawnView spawnView,
                SpawnManager spawnManager,
                CameraManager cameraManager,
                PoolManager poolManager,
                GameStateSignal.Trigger gameStateTrigger)
        {
            this.settings = settings;
            this.spawnView = spawnView;
            this.spawnManager = spawnManager;
            this.cameraManager = cameraManager;
            this.poolManager = poolManager;
            this.gameStateTrigger = gameStateTrigger;
        }

        public void Initialize()
        {
            timer = Time.time;
            spawnManager.CurrentSpawnView = spawnView;
            cameraManager.CurrentCamera = settings.camera;

            gameStateTrigger.Fire(GameStateType.Ready);
        }

        public void Dispose()
        {
            poolManager.ClearPool(settings.cloudView);
        }

        public void Tick()
        {
            if (Time.time - timer > settings.cloudSpawnSpeed)
            {
                timer = Time.time;

                Vector3 position = settings.cloudSpawn.position + (UnityEngine.Random.insideUnitSphere * settings.cloudSpawnRadius);

                poolManager.ReusePoolInstance(
                    settings.cloudView,
                    position,
                    Quaternion.identity
                );
            }
        }

        [Serializable]
        public class Settings
        {
            public CloudView cloudView;
            public Transform cloudSpawn;
            public float cloudSpawnRadius;
            public float cloudSpawnSpeed;
            public Camera camera;
        }
    }
}
