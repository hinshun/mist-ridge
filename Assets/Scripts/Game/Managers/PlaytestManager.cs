using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
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

        public void Generate()
        {
            // Do Nothing
        }
    }
}
