using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class PlayerUncontrollableState : PlayerBaseState
    {
        public PlayerUncontrollableState(
                Input input,
                Player player,
                PlayerStateMachine stateMachine,
                PlayerView playerView,
                PlayerController playerController)
            : base(input, player, stateMachine, playerView, playerController)
        {
            stateType = PlayerStateType.Uncontrollable;
        }

        public override void Update()
        {
            if (playerView.CanControl) {
                stateMachine.ChangeState(PlayerStateType.Idle);
                return;
            }
        }

        public override void EnterState()
        {
            playerView.CanJump = false;
            playerView.CanControl = false;
        }
    }
}
