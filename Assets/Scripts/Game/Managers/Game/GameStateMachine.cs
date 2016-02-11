using UnityEngine;
using System.Collections.Generic;
using Zenject;

namespace MistRidge
{
    public class GameStateMachine : StateMachine<GameStateMachine, GameBaseState, GameState, GameStateFactory>
    {
        public GameStateMachine(
                GameStateFactory stateFactory)
            : base(stateFactory)
        {
        }

        public override void Initialize()
        {
            ChangeState(GameState.Start, this);
        }
    }
}
