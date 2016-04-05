using UnityEngine;
using System;
using System.Collections.Generic;
using Zenject;

namespace MistRidge
{
    public class GameEndState : GameBaseState
    {
        private readonly Settings settings;
        private readonly SceneLoadSignal sceneLoadSignal;
        private readonly SceneLoader sceneLoader;
        private readonly InputManager inputManager;
        private readonly AetherManager aetherManager;
        private readonly PlayerManager playerManager;
        private readonly DeathManager deathManager;
        private readonly CameraManager cameraManager;
        private readonly CameraRigManager cameraRigManager;
        private readonly CinematicManager cinematicManager;
        private readonly DisplayManager displayManager;

        public GameEndState(
                Settings settings,
                SceneLoadSignal sceneLoadSignal,
                SceneLoader sceneLoader,
                InputManager inputManager,
                AetherManager aetherManager,
                PlayerManager playerManager,
                DeathManager deathManager,
                CameraManager cameraManager,
                CameraRigManager cameraRigManager,
                CinematicManager cinematicManager,
                DisplayManager displayManager,
                GameStateMachine stateMachine)
            : base(stateMachine)
        {
            this.settings = settings;
            this.sceneLoadSignal = sceneLoadSignal;
            this.sceneLoader = sceneLoader;
            this.inputManager = inputManager;
            this.aetherManager = aetherManager;
            this.playerManager = playerManager;
            this.deathManager = deathManager;
            this.cameraManager = cameraManager;
            this.cameraRigManager = cameraRigManager;
            this.cinematicManager = cinematicManager;
            this.displayManager = displayManager;

            stateType = GameStateType.End;
        }

        public override void Initialize()
        {
            sceneLoadSignal.Event += OnSceneLoad;
        }

        public override void Update()
        {
            // Do Nothing
        }

        public override void EnterState()
        {
            deathManager.IsActive = false;
            sceneLoader.OnlyFade();
        }

        public override void ExitState()
        {
            cinematicManager.CinematicType = CinematicType.None;
        }

        private void OnSceneLoad()
        {
            displayManager.UpdateSprint(false);
            DisablePlayerDisplays();
            cameraManager.ZoomOverride = settings.zoomOverride;
            cameraManager.ZoomOverrideEnabled = true;
            cameraRigManager.RigPosition = settings.rigPosition;
            cinematicManager.CinematicType = CinematicType.PeakZone;

            Dictionary<PlayerView, ScorePlacementType> placements = aetherManager.Placements;

            foreach (Input input in inputManager.Inputs)
            {
                if (!playerManager.HasPlayerFacade(input))
                {
                    continue;
                }

                PlayerFacade playerFacade = playerManager.PlayerFacade(input);
                PlayerView playerView = playerFacade.PlayerView;
                ScorePlacementType scorePlacementType = placements[playerView];

                displayManager.UpdateScorePlayer(
                    input.DeviceNum,
                    aetherManager.Aethers(playerView),
                    scorePlacementType,
                    playerFacade.CharacterType
                );
            }

            displayManager.UpdateScoreTime(true, Mathf.FloorToInt(Time.time - aetherManager.GameTimer));
            displayManager.UpdateScoreMenu(true);
        }

        private void DisablePlayerDisplays()
        {
            foreach (Input input in inputManager.Inputs)
            {
                if (!playerManager.HasPlayerFacade(input))
                {
                    continue;
                }

                PlayerFacade playerFacade = playerManager.PlayerFacade(input);
                displayManager.Display(input.DeviceNum, CharacterType.None);
            }
        }

        [Serializable]
        public class Settings
        {
            public float zoomOverride;
            public Vector3 rigPosition;
        }
    }
}
