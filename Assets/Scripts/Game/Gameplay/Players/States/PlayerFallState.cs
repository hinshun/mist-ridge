using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class PlayerFallState : PlayerBaseState
    {
        public PlayerFallState(
                Input input,
                Player player,
                PlayerStateMachine stateMachine,
                PlayerView playerView,
                PlayerController playerController)
            : base(input, player, stateMachine, playerView, playerController)
        {
            stateType = PlayerStateType.Fall;
        }

        public override void Update()
        {
            base.Update();

            if (!playerView.Animator.GetBool("IsLanding")
                    && playerController.FallAcquiringGround())
            {
                playerView.Animator.SetBool("IsLanding", true);
            }

            if (playerController.AcquiringGround()) {
                stateMachine.MoveDirection = Math3d.ProjectVectorOnPlane(playerView.Up, stateMachine.MoveDirection);
                stateMachine.ChangeState(PlayerStateType.Idle);
                return;
            }

            stateMachine.MoveDirection -= playerView.Up * player.CurrentGravity * playerController.DeltaTime;
        }

        public override void EnterState()
        {
            playerView.Animator.SetBool("IsFalling", true);
            playerView.Animator.SetBool("IsLanding", false);

            playerView.CanJump = false;

            playerController.IsClamping = false;
            playerController.IsSlopeLimiting = false;
        }

        public override void ExitState()
        {
            playerView.PlayerLand();

            playerView.Animator.SetBool("IsFalling", false);
            playerView.Animator.SetBool("IsLanding", true);
        }
    }
}
