using UnityEngine;
using System.Collections;
using Zenject;

namespace MistRidge
{
    public class GameStateFactory : StateFactory<GameStateMachine, GameBaseState, GameStateType, GameStateFactory>
    {
        public GameStateFactory(DiContainer container)
            : base(container) {}

        public override GameBaseState Create(GameStateType stateType)
        {
            GameBaseState gameState = null;

            switch (stateType)
            {
                case GameStateType.Start:
                    gameState = container.Instantiate<GameStartState>();
                    break;

                case GameStateType.Ready:
                    gameState = container.Instantiate<GameReadyState>();
                    break;

                case GameStateType.Play:
                    gameState = container.Instantiate<GamePlayState>();
                    break;
            }

            if (gameState == null)
            {
                Debug.LogError("Failed to create valid game state");
                return null;
            }

            gameState.Initialize();
            return gameState;
        }
    }
}
