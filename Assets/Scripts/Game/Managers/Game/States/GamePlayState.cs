using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class GamePlayState : GameBaseState
    {
        private readonly PlayerManager playerManager;
        private readonly InputManager inputManager;

        public GamePlayState(
                PlayerManager playerManager,
                InputManager inputManager,
                GameStateMachine stateMachine)
            : base(stateMachine)
        {
            this.playerManager = playerManager;
            this.inputManager = inputManager;

            stateType = GameStateType.Play;
        }

        public override void Update()
        {
            foreach(Input input in inputManager.Inputs)
            {
                if (input.Mapping.MenuWasPressed)
                {
                    playerManager.SpawnPlayer(input);
                    return;
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
