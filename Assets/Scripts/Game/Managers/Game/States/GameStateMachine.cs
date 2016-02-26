using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class GameStateMachine : StateMachine<GameStateMachine, GameBaseState, GameStateType, GameStateFactory>
    {
        public GameStateMachine(
                GameStateFactory stateFactory)
            : base(stateFactory)
        {
        }

        [PostInject]
        public void InitializeState()
        {
            ChangeState(GameStateType.Start);
        }

        protected override void EarlyGlobalUpdate() {
            // Do Nothing
        }

        protected override void LateGlobalUpdate() {
            // Do Nothing
        }
    }
}
