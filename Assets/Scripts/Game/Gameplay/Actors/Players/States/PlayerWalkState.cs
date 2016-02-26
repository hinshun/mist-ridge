using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class PlayerWalkState : PlayerBaseState
    {
        private readonly Player player;

        public PlayerWalkState(
                Input input,
                Player player,
                PlayerView playerView,
                PlayerStateMachine stateMachine,
                PlayerController playerController)
            : base(input, stateMachine, playerView, playerController)
        {
            this.player = player;
            stateType = PlayerStateType.Walk;
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

            if (input.Mapping.Direction.Vector != Vector2.zero) {
                stateMachine.MoveDirection = Vector3.MoveTowards(
                    stateMachine.MoveDirection,
                    stateMachine.LookDirection * player.CurrentWalkSpeed * input.Mapping.Direction.Vector.magnitude,
                    player.CurrentWalkAcceleration * playerController.DeltaTime
                );
            } else {
                stateMachine.ChangeState(PlayerStateType.Idle);
            }
        }

        public override void EnterState()
        {
            // Do Nothing
        }

        public override void ExitState()
        {
            // Do Nothing
        }
    }
}
