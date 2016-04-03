using UnityEngine;
using UnityEngine.UI;
using System;
using Zenject;

namespace MistRidge
{
    public class ReadyManager : IInitializable
    {
        private readonly GameStateSignal.Trigger gameStateTrigger;

        public ReadyManager(GameStateSignal.Trigger gameStateTrigger)
        {
            this.gameStateTrigger = gameStateTrigger;
        }

        public void Initialize()
        {
            gameStateTrigger.Fire(GameStateType.Ready);
        }
    }
}
