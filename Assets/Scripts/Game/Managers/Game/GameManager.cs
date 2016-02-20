using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class GameManager : IInitializable, IDisposable, ITickable
    {
        private readonly GameStateSignal gameStateSignal;
        private readonly GameStateMachine stateMachine;

        public GameManager(
                GameStateSignal gameStateSignal,
                GameStateMachine stateMachine)
        {
            this.gameStateSignal = gameStateSignal;
            this.stateMachine = stateMachine;
        }

        public void Initialize()
        {
            gameStateSignal.Event += OnStateChange;
        }

        public void Dispose()
        {
            gameStateSignal.Event -= OnStateChange;
        }

        public void Tick()
        {
            stateMachine.Tick();
        }

        public void OnStateChange(GameStateType gameStateType)
        {
            stateMachine.ChangeState(gameStateType);
        }
    }
}
