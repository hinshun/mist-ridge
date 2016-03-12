using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public abstract class PlayerBaseState : State<PlayerStateMachine, PlayerBaseState, PlayerStateType, PlayerStateFactory>
    {
        protected readonly Input input;
        protected readonly Player player;
        protected readonly PlayerView playerView;
        protected readonly PlayerController playerController;

        public PlayerBaseState(
                Input input,
                Player player,
                PlayerStateMachine stateMachine,
                PlayerView playerView,
                PlayerController playerController)
            : base(stateMachine)
        {
            this.input = input;
            this.player = player;
            this.playerView = playerView;
            this.playerController = playerController;
        }

        public override void Update()
        {
            playerView.MeshTransform.rotation = Quaternion.LookRotation(stateMachine.LookDirection, playerView.Up);
        }

        protected void SetHorizontal()
        {
            if (input.Mapping.Direction.Vector.magnitude == 0) {
                return;
            }

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
        }
    }
}
