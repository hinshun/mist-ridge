using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class PlayerFallState : PlayerBaseState
    {
        private readonly Player player;

        public PlayerFallState(
                Input input,
                Player player,
                PlayerStateMachine stateMachine,
                PlayerView playerView,
                PlayerController playerController)
            : base(input, stateMachine, playerView, playerController)
        {
            this.player = player;
            stateType = PlayerStateType.Fall;
        }

        public override void Update()
        {
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
