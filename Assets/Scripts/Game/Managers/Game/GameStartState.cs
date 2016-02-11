using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class GameStartState : GameBaseState
    {

        public GameStartState(GameStateMachine stateMachine)
            : base(stateMachine)
        {
            stateReference = GameState.Start;
        }

        public override void Update()
        {
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
