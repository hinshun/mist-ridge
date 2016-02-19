using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class GameStartState : GameBaseState
    {
        private readonly StartMenuManager startMenuManager;
        private readonly InputManager inputManager;

        public GameStartState(
                StartMenuManager startMenuManager,
                InputManager inputManager,
                GameStateMachine stateMachine)
            : base(stateMachine)
        {
            this.startMenuManager = startMenuManager;
            this.inputManager = inputManager;

            stateType = GameStateType.Start;
        }

        public override void Update()
        {
            foreach(Input input in inputManager.Inputs)
            {
                if (input.Current.submit.WasPressed)
                {
                    startMenuManager.Select();
                    return;
                }

                if (input.Current.direction.Up.WasPressed)
                {
                    startMenuManager.MoveSelection(1);
                } else if (input.Current.direction.Down.WasPressed)
                {
                    startMenuManager.MoveSelection(-1);
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
