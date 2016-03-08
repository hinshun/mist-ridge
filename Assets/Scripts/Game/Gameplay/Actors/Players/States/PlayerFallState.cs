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

            if (playerController.AcquiringGround()) {
                stateMachine.MoveDirection = Math3d.ProjectVectorOnPlane(playerView.Up, stateMachine.MoveDirection);
                stateMachine.ChangeState(PlayerStateType.Idle);
                return;
            }

            stateMachine.MoveDirection -= playerView.Up * player.CurrentGravity * playerController.DeltaTime;
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
    }
}
