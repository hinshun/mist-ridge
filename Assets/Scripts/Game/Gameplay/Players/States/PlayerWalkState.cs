using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class PlayerWalkState : PlayerBaseState
    {
        public PlayerWalkState(
                Input input,
                Player player,
                PlayerView playerView,
                PlayerStateMachine stateMachine,
                PlayerController playerController)
            : base(input, player, stateMachine, playerView, playerController)
        {
            stateType = PlayerStateType.Walk;
        }

        public override void Update()
        {
            if (stateMachine.Enabled && playerView.CanJump && input.Mapping.Jump.WasPressed)
            {
                base.Update();
                stateMachine.ChangeState(PlayerStateType.Jump);
                return;
            }

            if (!playerController.MaintainingGround())
            {
                base.Update();
                stateMachine.ChangeState(PlayerStateType.Fall);
                return;
            }

            if (stateMachine.Enabled && input.Mapping.Direction.Vector.magnitude > player.CurrentWalkThreshold)
            {
                SetHorizontal();

                stateMachine.MoveDirection = Vector3.MoveTowards(
                    stateMachine.MoveDirection,
                    playerView.MeshTransform.forward * player.CurrentWalkSpeed * input.Mapping.Direction.Vector.magnitude,
                    player.CurrentWalkAcceleration * playerController.DeltaTime
                );

                playerView.Animator.SetFloat(
                    "Speed",
                    stateMachine.MoveDirection.magnitude
                );
            }
            else
            {
                if (stateMachine.MoveDirection.magnitude > player.CurrentHaltThreshold)
                {
                    stateMachine.ChangeState(PlayerStateType.Halt);
                }
                else
                {
                    stateMachine.MoveDirection = Vector3.MoveTowards(
                        stateMachine.MoveDirection,
                        Vector3.zero,
                        player.CurrentHaltSpeed * playerController.DeltaTime
                    );

                    stateMachine.ChangeState(PlayerStateType.Idle);
                }
            }
        }

        public override void EnterState()
        {
            playerView.Animator.SetBool("IsWalking", true);
        }

        public override void ExitState()
        {
            playerView.Animator.SetBool("IsWalking", false);
        }
    }
}
