using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class PlayerIdleState : PlayerBaseState
    {
        public PlayerIdleState(
                Input input,
                Player player,
                PlayerStateMachine stateMachine,
                PlayerView playerView,
                PlayerController playerController)
            : base(input, player, stateMachine, playerView, playerController)
        {
            stateType = PlayerStateType.Idle;
        }

        public override void Update()
        {
            if (input.Mapping.Jump.WasPressed) {
                stateMachine.ChangeState(PlayerStateType.Jump);
                return;
            }

            if (!playerController.MaintainingGround()) {
                stateMachine.ChangeState(PlayerStateType.Fall);
                return;
            }

            if (input.Mapping.Direction.Vector.magnitude > player.CurrentWalkThreshold)
            {
                stateMachine.ChangeState(PlayerStateType.Walk);
            }
        }

        public override void EnterState()
        {
            playerView.Animator.SetBool("IsIdling", true);

            playerController.IsClamping = true;
            playerController.IsSlopeLimiting = true;
        }

        public override void ExitState()
        {
            playerView.Animator.SetBool("IsIdling", false);
        }
    }
}
