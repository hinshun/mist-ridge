using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class LevelManager : IInitializable
    {
        private readonly GameStateSignal.Trigger gameStateTrigger;

        public LevelManager(
                GameStateSignal.Trigger gameStateTrigger)
        {
            this.gameStateTrigger = gameStateTrigger;
        }

        public void Initialize()
        {
            gameStateTrigger.Fire(GameStateType.Play);
        }

        public void Generate()
        {
            // Do Nothing
        }
    }
}
