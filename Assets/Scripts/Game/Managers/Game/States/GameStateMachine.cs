using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class GameStateMachine : StateMachine<GameStateMachine, GameBaseState, GameStateType, GameStateFactory>
    {
        private GameReadyState gameReadyState;
        private GameEndState gameEndState;
        private ChunkManager chunkManager;

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

        public GameEndState GameEndState
        {
            get
            {
                return gameEndState;
            }
        }

        public ChunkManager ChunkManager
        {
            get
            {
                return chunkManager;
            }
            set
            {
                chunkManager = value;
            }
        }

        protected override void ChangedState(GameStateType stateType)
        {
            switch(stateType)
            {
                case GameStateType.Ready:
                    gameReadyState = state as GameReadyState;
                    break;

                case GameStateType.End:
                    gameEndState = state as GameEndState;
                    break;
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
