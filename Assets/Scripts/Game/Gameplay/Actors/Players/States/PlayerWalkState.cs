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
            if (input.Mapping.Jump.WasPressed)
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

            if (input.Mapping.Direction.Vector.magnitude > player.CurrentWalkThreshold)
            {
                Quaternion lookRotation = Quaternion.LookRotation(stateMachine.LookDirection, playerView.Up);

                playerView.MeshTransform.rotation = Quaternion.RotateTowards(
                    playerView.MeshTransform.rotation,
                    lookRotation,
                    playerController.DeltaTime * player.CurrentRotationSpeed
                );

                float turnAngle = Quaternion.Angle(playerView.MeshTransform.rotation, lookRotation);
                if (Vector3.Cross(playerView.MeshTransform.forward, stateMachine.LookDirection).y < 0)
                {
                    turnAngle = -turnAngle;
                }

                playerView.Animator.SetFloat(
                    "Horizontal",
                    turnAngle / 180
                );

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
                stateMachine.ChangeState(PlayerStateType.Idle);
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
