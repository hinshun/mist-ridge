using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public abstract class GameBaseState : State<GameStateMachine, GameBaseState, GameState, GameStateFactory>
    {
        public GameBaseState(GameStateMachine stateMachine)
            : base(stateMachine) {}
    }
}
