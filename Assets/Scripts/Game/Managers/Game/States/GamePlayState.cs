using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class GamePlayState : GameBaseState
    {
        private readonly PlayerManager playerManager;
        private readonly DeathManager deathManager;
        private readonly InputManager inputManager;

        public GamePlayState(
                PlayerManager playerManager,
                DeathManager deathManager,
                InputManager inputManager,
                GameStateMachine stateMachine)
            : base(stateMachine)
        {
            this.playerManager = playerManager;
            this.deathManager = deathManager;
            this.inputManager = inputManager;

            stateType = GameStateType.Play;
        }

        public override void Update()
        {
            foreach(Input input in inputManager.Inputs)
            {
                if (input.Mapping.AnyButton.WasPressed)
                {
                    playerManager.SpawnPlayer(input);
                    deathManager.AddPlayer(input);
                }
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
