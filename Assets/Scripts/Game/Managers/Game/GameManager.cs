using UnityEngine;
using Zenject;

namespace MistRidge
{
    public class GameManager : ITickable
    {
        GameStateMachine stateMachine;

        public GameManager(
                GameStateMachine stateMachine)
        {
            this.stateMachine = stateMachine;
        }

        public void Tick()
        {
            stateMachine.Step();
        }
    }
}
