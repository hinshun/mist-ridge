using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public abstract class PlayerBaseState : State<PlayerStateMachine, PlayerBaseState, PlayerStateType, PlayerStateFactory>
    {
        protected readonly Input input;
        protected readonly PlayerView playerView;
        protected readonly PlayerController playerController;

        public PlayerBaseState(
                Input input,
                PlayerStateMachine stateMachine,
                PlayerView playerView,
                PlayerController playerController)
            : base(stateMachine)
        {
            this.input = input;
            this.playerView = playerView;
            this.playerController = playerController;
        }

        public override void Update()
        {
            playerView.MeshTransform.rotation = Quaternion.LookRotation(stateMachine.LookDirection, playerView.Up);
        }
    }
}
