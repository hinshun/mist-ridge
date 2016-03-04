using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using Zenject;

namespace MistRidge
{
    public class DisplayManager : IInitializable
    {
        private readonly GameDisplayView gameDisplayView;

        public DisplayManager(GameDisplayView gameDisplayView)
        {
            this.gameDisplayView = gameDisplayView;
        }

        public void Initialize()
        {
            foreach (PlayerDisplayView playerDisplay in gameDisplayView.PlayerDisplays)
            {
                playerDisplay.SetActive(false);
            }
        }

        public void Display(Input input)
        {
            PlayerDisplay(input).SetActive(true);
            UpdateAether(input, 0);
        }

        public void UpdateAether(Input input, int aetherCount)
        {
            Text aetherText = PlayerDisplay(input).AetherText;
            aetherText.text = "Aether x" + aetherCount;

            Debug.Log(aetherText.text);
        }

        private PlayerDisplayView PlayerDisplay(Input input)
        {
            return gameDisplayView.PlayerDisplays[input.DeviceNum];
        }
    }
}
