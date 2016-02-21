using UnityEngine;
using System.Collections;
using Zenject;

namespace MistRidge
{
    public class GameStateFactory : StateFactory<GameStateMachine, GameBaseState, GameStateType, GameStateFactory>
    {
        public GameStateFactory(DiContainer container)
            : base(container) {}

        public override GameBaseState Create(GameStateType stateType, params object[] constructorArgs)
        {
            switch (stateType)
            {
                case GameStateType.Start:
                    return container.Instantiate<GameStartState>(constructorArgs);

                case GameStateType.Ready:
                    return container.Instantiate<GameReadyState>(constructorArgs);

                case GameStateType.Play:
                    return container.Instantiate<GamePlayState>(constructorArgs);
            }

            Debug.LogError("Failed to create valid game state");
            return null;
        }
    }
}
