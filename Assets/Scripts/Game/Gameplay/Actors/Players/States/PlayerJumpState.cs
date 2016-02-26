using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class PlayerJumpState : PlayerBaseState
    {
        private readonly Player player;

        public PlayerJumpState(
                Input input,
                Player player,
                PlayerView playerView,
                PlayerStateMachine stateMachine,
                PlayerController playerController)
            : base(input, stateMachine, playerView, playerController)
        {
            this.player = player;
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
                planarMoveDirection + (stateMachine.LookDirection * player.JumpSpeed * input.Mapping.Direction.Vector.magnitude),
                player.JumpAcceleration * playerController.DeltaTime
            );

            verticalMoveDirection -= playerView.Up * player.Gravity * playerController.DeltaTime;
            stateMachine.MoveDirection = planarMoveDirection + verticalMoveDirection;
        }

        public override void EnterState()
        {
            playerController.IsClamping = false;
            playerController.IsSlopeLimiting = false;

            stateMachine.MoveDirection += playerView.Up * SuperMath.CalculateJumpSpeed(player.JumpHeight, player.Gravity);
        }

        public override void ExitState()
        {
            // Do Nothing
        }
    }
}
