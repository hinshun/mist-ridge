using UnityEngine;
using System;
using Zenject;

namespace MistRidge {
    public class GamePlayState : GameBaseState
    {
        public GamePlayState(GameStateMachine stateMachine) : base(stateMachine)
        {
            stateReference = GameState.Play;
        }

        public override void Update()
        {
            // Do Nothing
        }

        public override void EnterState()
        {
            Debug.Log("Game started");
        }

        public override void ExitState()
        {
            // Do Nothing
        }
    }
}
