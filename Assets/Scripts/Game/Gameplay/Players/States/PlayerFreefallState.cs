using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class PlayerFreefallState : PlayerBaseState
    {
        public PlayerFreefallState(
                Input input,
                Player player,
                PlayerStateMachine stateMachine,
                PlayerView playerView,
                PlayerController playerController)
            : base(input, player, stateMachine, playerView, playerController)
        {
            stateType = PlayerStateType.Freefall;
        }

        public override void Update()
        {
            if (playerView.Animator.GetBool("IsFreefalling")
                    && playerController.FreefallAcquiringGround())
            {
                playerView.Animator.SetBool("IsFreefalling", false);
                playerView.CanControl = false;
            }

            if (playerController.AcquiringGround()) {
                stateMachine.ChangeState(PlayerStateType.Uncontrollable);
                return;
            }

            if (input.Mapping.Direction.Vector != Vector2.zero)
            {
                stateMachine.MoveDirection = Vector3.MoveTowards(
                    stateMachine.MoveDirection,
                    stateMachine.LookDirection * player.CurrentFreefallSpeed * input.Mapping.Direction.Vector.magnitude,
                    player.CurrentFreefallAcceleration * playerController.DeltaTime
                );
            }

            stateMachine.MoveDirection = Vector3.MoveTowards(
                stateMachine.MoveDirection,
                Vector3.zero,
                player.CurrentFreefallDrag * playerController.DeltaTime
            );

            stateMachine.MoveDirection -= playerView.Up * player.CurrentGravity * playerController.DeltaTime;

            UpdateTilt();
        }

        public override void EnterState()
        {
            playerView.Animator.SetTrigger("StartFreefall");
            playerView.Animator.SetBool("IsFreefalling", true);

            playerController.IsClamping = false;
            playerController.IsSlopeLimiting = false;

            stateMachine.MoveDirection += Vector3.MoveTowards(
                stateMachine.MoveDirection,
                stateMachine.MoveDirection + (-playerView.Position * player.CurrentFreefallSpeed),
                player.CurrentFreefallAcceleration
            );

        }

        public override void ExitState()
        {
            playerView.Animator.SetBool("IsFreefalling", false);

            playerView.LocalRotation = Quaternion.identity;
        }

        private void UpdateTilt()
        {
            playerView.MeshTransform.rotation = Quaternion.Euler(
                stateMachine.MoveDirection.z * -player.CurrentFreefallTilt,
                0,
                stateMachine.MoveDirection.x * -player.CurrentFreefallTilt
            );
        }
    }
}
