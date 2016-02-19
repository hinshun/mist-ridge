using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class GameReadyState : GameBaseState
    {
        public GameReadyState(
                GameStateMachine stateMachine)
            : base(stateMachine)
        {
            stateType = GameStateType.Ready;
        }

        public override void Update()
        {
            // Do Nothing
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
