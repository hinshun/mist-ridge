using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class PlayerJumpState : PlayerBaseState
    {
        public PlayerJumpState(
                Input input,
                Player player,
                PlayerView playerView,
                PlayerStateMachine stateMachine,
                PlayerController playerController)
            : base(input, player, stateMachine, playerView, playerController)
        {
            stateType = PlayerStateType.Jump;
        }

        public override void Update()
        {
            base.Update();

            Vector3 planarMoveDirection = Math3d.ProjectVectorOnPlane(playerView.Up, stateMachine.MoveDirection);
            Vector3 verticalMoveDirection = stateMachine.MoveDirection - planarMoveDirection;

            if (Vector3.Angle(verticalMoveDirection, playerView.Up) > 90f)
            {
                if (!playerView.Animator.GetBool("IsLanding")
                        && playerController.FallAcquiringGround())
                {
                    playerView.Animator.SetBool("IsLanding", true);
                }

                if (playerController.AcquiringGround())
                {
                    stateMachine.MoveDirection = planarMoveDirection;

                    if (!stateMachine.Enabled || input.Mapping.Direction.Vector == Vector2.zero)
                    {
                        JumpHalt();
                        stateMachine.ChangeState(PlayerStateType.Halt);
                    }
                    else
                    {
                        stateMachine.ChangeState(PlayerStateType.Walk);
                    }
                    return;
                }
            }

            if (stateMachine.Enabled)
            {
                planarMoveDirection = Vector3.MoveTowards(
                    planarMoveDirection,
                    planarMoveDirection + (stateMachine.LookDirection * player.CurrentJumpSpeed * input.Mapping.Direction.Vector.magnitude),
                    player.CurrentJumpAcceleration * playerController.DeltaTime
                );

                planarMoveDirection = Vector3.ClampMagnitude(
                    planarMoveDirection,
                    player.CurrentJumpSpeedLimit
                );
            }

            verticalMoveDirection -= playerView.Up * player.CurrentGravity * playerController.DeltaTime;

            stateMachine.MoveDirection = planarMoveDirection + verticalMoveDirection;
        }

        public override void EnterState()
        {
            playerView.Animator.SetBool("IsJumping", true);
            playerView.Animator.SetBool("IsLanding", false);

            playerView.CanJump = false;

            playerController.IsClamping = false;
            playerController.IsSlopeLimiting = false;

            stateMachine.MoveDirection += playerView.Up * SuperMath.CalculateJumpSpeed(player.CurrentJumpHeight, player.CurrentGravity);
        }

        public override void ExitState()
        {
            playerView.PlayerLand();

            playerView.Animator.SetBool("IsJumping", false);
            playerView.Animator.SetBool("IsLanding", true);
        }

        private void JumpHalt()
        {
            stateMachine.MoveDirection = Vector3.MoveTowards(
                stateMachine.MoveDirection,
                Vector3.zero,
                player.CurrentJumpHaltSpeed * playerController.DeltaTime
            );
        }
    }
}
