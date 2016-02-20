using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class PlayerWalkState : PlayerBaseState
    {
        private readonly Settings settings;

        public PlayerWalkState(
                Settings settings,
                Input input,
                PlayerStateMachine stateMachine,
                PlayerView playerView,
                PlayerController playerController)
            : base(input, stateMachine, playerView, playerController)
        {
            this.settings = settings;
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
                    stateMachine.LookDirection * settings.walkSpeed,
                    settings.walkAcceleration * playerController.DeltaTime
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

        [Serializable]
        public class Settings
        {
            public float walkSpeed;
            public float walkAcceleration;
        }
    }
}
