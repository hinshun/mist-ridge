using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class GameManager : ITickable
    {
        private readonly GameStateMachine stateMachine;

        public GameManager(
                GameStateMachine stateMachine)
        {
            this.stateMachine = stateMachine;
        }

        public void Tick()
        {
            stateMachine.Tick();
        }

        public void Play()
        {
            stateMachine.ChangeState(GameStateType.Play);
        }
    }
}
