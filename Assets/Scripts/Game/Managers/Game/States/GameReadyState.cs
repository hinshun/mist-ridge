using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class GameReadyState : GameBaseState
    {
        private readonly Settings settings;
        private readonly SceneLoader sceneLoader;
        private readonly CameraView cameraView;
        private readonly InputManager inputManager;
        private readonly PlayerManager playerManager;
        private readonly DeathManager deathManager;
        private readonly DisplayManager displayManager;
        private readonly GameManager gameManager;

        public GameReadyState(
                Settings settings,
                SceneLoader sceneLoader,
                CameraView cameraView,
                InputManager inputManager,
                PlayerManager playerManager,
                DeathManager deathManager,
                DisplayManager displayManager,
                GameManager gameManager,
                GameStateMachine stateMachine)
            : base(stateMachine)
        {
            this.settings = settings;
            this.sceneLoader = sceneLoader;
            this.cameraView = cameraView;
            this.inputManager = inputManager;
            this.playerManager = playerManager;
            this.deathManager = deathManager;
            this.displayManager = displayManager;
            this.gameManager = gameManager;

            stateType = GameStateType.Ready;
        }

        public override void Initialize()
        {
            if (gameManager.LastInput != null)
            {
                SpawnPlayer(gameManager.LastInput);
            }
        }

        public override void Update()
        {
            foreach(Input input in inputManager.Inputs)
            {
                if (input.Mapping.AnyButton.WasPressed)
                {
                    SpawnPlayer(input);
                }

                if (input.Mapping.MenuWasPressed && playerManager.HasPlayerFacade(input))
                {
                    stateMachine.ChangeState(GameStateType.Play);
                    sceneLoader.Load(settings.levelSceneName);
                }
            }
        }

        public override void EnterState()
        {
            cameraView.IsActive = false;
        }

        public override void ExitState()
        {
            deathManager.PopulatePlayerDeaths();
        }

        private void SpawnPlayer(Input input)
        {
            PlayerFacade playerFacade = playerManager.SpawnPlayer(input);

            if (playerFacade != null)
            {
                playerFacade.Freefall();
                displayManager.Display(input);
            }
        }

        [Serializable]
        public class Settings
        {
            public string levelSceneName;
        }
    }
}
