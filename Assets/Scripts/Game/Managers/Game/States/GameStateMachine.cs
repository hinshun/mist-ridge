using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class GameStateMachine : StateMachine<GameStateMachine, GameBaseState, GameStateType, GameStateFactory>
    {
        private GameReadyState gameReadyState;

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

        public GameReadyState GameReadyState
        {
            get
            {
                return gameReadyState;
            }
        }

        protected override void ChangedState(GameStateType stateType)
        {
            if (stateType == GameStateType.Ready)
            {
                gameReadyState = state as GameReadyState;
            }
        }

        protected override void EarlyGlobalUpdate() {
            // Do Nothing
        }

        protected override void LateGlobalUpdate() {
            // Do Nothing
        }
    }
}
