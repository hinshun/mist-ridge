using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class GameReadyState : GameBaseState
    {
        private readonly CameraView cameraView;
        private readonly InputManager inputManager;
        private readonly PlayerManager playerManager;

        public GameReadyState(
                CameraView cameraView,
                InputManager inputManager,
                PlayerManager playerManager,
                GameStateMachine stateMachine)
            : base(stateMachine)
        {
            this.cameraView = cameraView;
            this.inputManager = inputManager;
            this.playerManager = playerManager;

            stateType = GameStateType.Ready;
        }

        public override void Update()
        {
            foreach(Input input in inputManager.Inputs)
            {
                if (input.Mapping.AnyButton.WasPressed)
                {
                    PlayerFacade playerFacade = playerManager.SpawnPlayer(input);

                    if (playerFacade != null)
                    {
                        playerFacade.Freefall();
                    }
                }
            }
        }

        public override void EnterState()
        {
            cameraView.IsActive = false;
        }

        public override void ExitState()
        {
            cameraView.IsActive = true;
        }
    }
}
