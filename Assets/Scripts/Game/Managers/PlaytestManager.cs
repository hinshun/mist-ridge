using UnityEngine;
using Zenject;

namespace MistRidge
{
    public class PlaytestManager : IInitializable
    {
        private readonly GameStateSignal.Trigger gameStateTrigger;

        public PlaytestManager(
                GameStateSignal.Trigger gameStateTrigger)
        {
            this.gameStateTrigger = gameStateTrigger;
        }

        public void Initialize()
        {
            gameStateTrigger.Fire(GameStateType.Play);
        }
    }
}
