using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class PlayerFloatingState : PlayerBaseState
    {
        public PlayerFloatingState(
                Input input,
                Player player,
                PlayerStateMachine stateMachine,
                PlayerView playerView,
                PlayerController playerController)
            : base(input, player, stateMachine, playerView, playerController)
        {
            stateType = PlayerStateType.Floating;
        }

        public override void Update()
        {
            // Do Nothing
        }

        public override void EnterState()
        {
            playerView.Animator.SetTrigger("StartFloating");

            stateMachine.MoveDirection = Vector3.zero;

            playerController.IsClamping = false;
            playerController.IsSlopeLimiting = false;

            playerView.CanJump = false;
            stateMachine.Enabled = false;
        }

        public override void ExitState()
        {
            playerView.CanJump = true;
            stateMachine.Enabled = true;
        }
    }
}
