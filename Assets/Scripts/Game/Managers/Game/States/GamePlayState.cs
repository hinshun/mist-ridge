using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class GamePlayState : GameBaseState
    {
        private readonly CameraManager cameraManager;
        private readonly PlayerManager playerManager;
        private readonly InputManager inputManager;

        public GamePlayState(
                CameraManager cameraManager,
                PlayerManager playerManager,
                InputManager inputManager,
                GameStateMachine stateMachine)
            : base(stateMachine)
        {
            this.cameraManager = cameraManager;
            this.playerManager = playerManager;
            this.inputManager = inputManager;

            stateType = GameStateType.Play;
        }

        public override void Update()
        {
            foreach(Input input in inputManager.Inputs)
            {
                if (input.Mapping.MenuWasPressed)
                {
                    playerManager.SpawnPlayer(input);
                    return;
                }
            }
        }

        public override void EnterState()
        {
            cameraManager.ResetCamera();
        }

        public override void ExitState()
        {
            // Do Nothing
        }
    }
}
