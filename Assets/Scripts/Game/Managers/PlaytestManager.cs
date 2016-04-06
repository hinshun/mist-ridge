using UnityEngine;
using Zenject;

namespace MistRidge
{
    public class PlaytestManager : IInitializable, ITickable
    {
        private readonly SpawnView spawnView;
        private readonly SpawnManager spawnManager;
        private readonly InputManager inputManager;
        private readonly PlayerManager playerManager;
        private readonly DisplayManager displayManager;
        private readonly DeathManager deathManager;
        private readonly CameraView cameraView;
        private readonly GameStateSignal.Trigger gameStateTrigger;

        private bool updated;
        private float timer;

        public PlaytestManager(
                SpawnView spawnView,
                SpawnManager spawnManager,
                InputManager inputManager,
                PlayerManager playerManager,
                DisplayManager displayManager,
                DeathManager deathManager,
                CameraView cameraView,
                GameStateSignal.Trigger gameStateTrigger)
        {
            this.spawnView = spawnView;
            this.spawnManager = spawnManager;
            this.inputManager = inputManager;
            this.playerManager = playerManager;
            this.displayManager = displayManager;
            this.deathManager = deathManager;
            this.cameraView = cameraView;
            this.gameStateTrigger = gameStateTrigger;
        }

        public void Initialize()
        {
            spawnManager.CurrentSpawnView = spawnView;
            gameStateTrigger.Fire(GameStateType.Play);

            cameraView.IsActive = true;

            foreach (Input input in inputManager.Inputs)
            {
                PlayerFacade playerFacade = playerManager.SpawnPlayer(input, CharacterType.Jack);

                deathManager.AddPlayer(input);
                displayManager.Display(input.DeviceNum, playerFacade.CharacterType);
            }

            updated = false;
            timer = Time.time;
        }

        public void Tick()
        {
            if (updated)
            {
                return;
            }

            if (Time.time - timer > 2)
            {
                updated = true;
                displayManager.UpdateCinematic(false);
            }
        }
    }
}
