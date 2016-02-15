using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class PlayerJumpState : PlayerBaseState
    {
        Settings settings;

        public PlayerJumpState(
                Settings settings,
                Input input,
                PlayerStateMachine stateMachine,
                PlayerView playerView,
                PlayerController playerController)
            : base(input, stateMachine, playerView, playerController)
        {
            this.settings = settings;
            stateType = PlayerStateType.Jump;
        }

        public override void Update()
        {
            Vector3 planarMoveDirection = Math3d.ProjectVectorOnPlane(playerView.Up, stateMachine.MoveDirection);
            Vector3 verticalMoveDirection = stateMachine.MoveDirection - planarMoveDirection;

            if (Vector3.Angle(verticalMoveDirection, playerView.Up) > 90f
                    && playerController.AcquiringGround()) {
                stateMachine.MoveDirection = planarMoveDirection;
                stateMachine.ChangeState(PlayerStateType.Idle);
                return;
            }

            planarMoveDirection = Vector3.MoveTowards(
                planarMoveDirection,
                stateMachine.LookDirection * settings.moveSpeed,
                settings.jumpAcceleration * playerController.DeltaTime
            );

            verticalMoveDirection -= playerView.Up * settings.gravity * playerController.DeltaTime;
            stateMachine.MoveDirection = planarMoveDirection + verticalMoveDirection;
        }

        public override void EnterState()
        {
            playerController.IsClamping = false;
            playerController.IsSlopeLimiting = false;

            stateMachine.MoveDirection += playerView.Up * SuperMath.CalculateJumpSpeed(settings.jumpHeight, settings.gravity);
        }

        public override void ExitState()
        {
            // Do Nothing
        }

        [Serializable]
        public class Settings
        {
            public float moveSpeed;
            public float jumpHeight;
            public float jumpAcceleration;
            public float gravity;
        }
    }
}
