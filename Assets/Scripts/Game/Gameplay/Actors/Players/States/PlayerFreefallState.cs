using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class PlayerFreefallState : PlayerBaseState
    {
        private readonly Player player;

        public PlayerFreefallState(
                Input input,
                Player player,
                PlayerStateMachine stateMachine,
                PlayerView playerView,
                PlayerController playerController)
            : base(input, stateMachine, playerView, playerController)
        {
            this.player = player;
            stateType = PlayerStateType.Freefall;
        }

        public override void Update()
        {
            if (playerController.AcquiringGround()) {
                stateMachine.ChangeState(PlayerStateType.Idle);
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

            UpdateTilt();
        }

        public override void EnterState()
        {
            playerView.Animator.SetTrigger("StartFreefall");

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
            playerView.Animator.SetTrigger("EndFreefall");
            playerView.LocalRotation = Quaternion.identity;
        }

        private void UpdateTilt()
        {
            playerView.LocalRotation = Quaternion.Euler(
                stateMachine.MoveDirection.x * -player.CurrentFreefallTilt,
                0,
                stateMachine.MoveDirection.z * -player.CurrentFreefallTilt
            );
        }
    }
}
