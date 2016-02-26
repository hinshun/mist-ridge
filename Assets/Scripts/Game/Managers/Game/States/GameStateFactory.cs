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
            switch (stateType)
            {
                case GameStateType.Start:
                    return container.Instantiate<GameStartState>();

                case GameStateType.Ready:
                    return container.Instantiate<GameReadyState>();

                case GameStateType.Play:
                    return container.Instantiate<GamePlayState>();
            }

            Debug.LogError("Failed to create valid game state");
            return null;
        }
    }
}
