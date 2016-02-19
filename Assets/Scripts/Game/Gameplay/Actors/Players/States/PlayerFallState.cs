using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class PlayerFallState : PlayerBaseState
    {
        private readonly Settings settings;

        public PlayerFallState(
                Settings settings,
                Input input,
                PlayerStateMachine stateMachine,
                PlayerView playerView,
                PlayerController playerController)
            : base(input, stateMachine, playerView, playerController)
        {
            this.settings = settings;
            stateType = PlayerStateType.Fall;
        }

        public override void Update()
        {
            if (playerController.AcquiringGround()) {
                stateMachine.MoveDirection = Math3d.ProjectVectorOnPlane(playerView.Up, stateMachine.MoveDirection);
                stateMachine.ChangeState(PlayerStateType.Idle);
                return;
            }

            stateMachine.MoveDirection -= playerView.Up * settings.gravity * playerController.DeltaTime;
        }

        public override void EnterState()
        {
            playerController.IsClamping = false;
            playerController.IsSlopeLimiting = false;
        }

        public override void ExitState()
        {
            // Do Nothing
        }

        [Serializable]
        public class Settings
        {
            public float gravity;
        }
    }
}
