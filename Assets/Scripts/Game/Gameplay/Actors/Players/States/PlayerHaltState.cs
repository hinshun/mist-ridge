using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class PlayerHaltState : PlayerBaseState
    {
        public PlayerHaltState(
                Input input,
                Player player,
                PlayerStateMachine stateMachine,
                PlayerView playerView,
                PlayerController playerController)
            : base(input, player, stateMachine, playerView, playerController)
        {
            stateType = PlayerStateType.Halt;
        }

        public override void Update()
        {

            if (playerView.CanJump && input.Mapping.Jump.WasPressed) {
                stateMachine.ChangeState(PlayerStateType.Jump);
                return;
            }

            if (!playerController.MaintainingGround()) {
                stateMachine.ChangeState(PlayerStateType.Fall);
                return;
            }
            else
            {
                stateMachine.MoveDirection = Vector3.MoveTowards(
                    stateMachine.MoveDirection,
                    Vector3.zero,
                    player.CurrentHaltSpeed * playerController.DeltaTime
                );
            }

            if (input.Mapping.Direction.Vector.magnitude > player.CurrentWalkThreshold)
            {
                stateMachine.ChangeState(PlayerStateType.Walk);
            }

            if (Mathf.Approximately(0, stateMachine.MoveDirection.magnitude))
            {
                stateMachine.ChangeState(PlayerStateType.Idle);
            }
        }

        public override void EnterState()
        {
            playerView.Animator.SetBool("IsHalting", true);

            playerController.IsClamping = true;
            playerController.IsSlopeLimiting = true;
        }

        public override void ExitState()
        {
            playerView.Animator.SetBool("IsHalting", false);
        }
    }
}
