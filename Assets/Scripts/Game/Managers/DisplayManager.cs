using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using Zenject;

namespace MistRidge
{
    public class DisplayManager : IInitializable
    {
        private readonly Settings settings;
        private readonly GameDisplayView gameDisplayView;

        public DisplayManager(
                Settings settings,
                GameDisplayView gameDisplayView)
        {
            this.settings = settings;
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
            UpdateItem(input, null);
        }

        public void UpdateAether(Input input, int aetherCount)
        {
            Text aetherText = PlayerDisplay(input).AetherText;
            aetherText.text = "Aether x" + aetherCount;
        }

        public void UpdateItem(Input input, ItemDrop itemDrop)
        {
            Image itemImage = PlayerDisplay(input).ItemImage;

            if (itemDrop == null)
            {
                itemImage.sprite = settings.emptyItem;
            }
            else
            {
                itemImage.sprite = itemDrop.ItemSprite;
            }
        }

        private PlayerDisplayView PlayerDisplay(Input input)
        {
            return gameDisplayView.PlayerDisplays[input.DeviceNum];
        }

        [Serializable]
        public class Settings
        {
            public Sprite emptyItem;
        }
    }
}
