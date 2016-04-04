using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class GamePlayState : GameBaseState
    {
        private readonly PlayerManager playerManager;
        private readonly DisplayManager displayManager;
        private readonly DeathManager deathManager;
        private readonly InputManager inputManager;

        public GamePlayState(
                PlayerManager playerManager,
                DisplayManager displayManager,
                DeathManager deathManager,
                InputManager inputManager,
                GameStateMachine stateMachine)
            : base(stateMachine)
        {
            this.playerManager = playerManager;
            this.displayManager = displayManager;
            this.deathManager = deathManager;
            this.inputManager = inputManager;

            stateType = GameStateType.Play;
        }

        public override void EnterState()
        {
            displayManager.UpdateCharacterSelect(false);
            displayManager.UpdateCinematic(true);
        }

        public override void ExitState()
        {
            // Do Nothing
        }
    }
}
