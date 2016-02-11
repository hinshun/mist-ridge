using UnityEngine;
using System.Collections;
using Zenject;
using ModestTree;

namespace MistRidge
{
    public class GameStateFactory : StateFactory<GameStateMachine, GameBaseState, GameState, GameStateFactory>
    {
        public GameStateFactory(DiContainer container)
            : base(container) {}

        public override GameBaseState Create(GameState stateReference, params object[] constructorArgs)
        {
            switch (stateReference)
            {
                case GameState.Start:
                    return container.Instantiate<GameStartState>(constructorArgs);

                case GameState.Play:
                    return container.Instantiate<GamePlayState>(constructorArgs);
            }

            Assert.That(false);
            return null;
        }
    }
}
