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

        private Canvas gameDisplayCanvas;
        private CanvasScaler gameDisplayScaler;

        public DisplayManager(
                Settings settings,
                GameDisplayView gameDisplayView)
        {
            this.settings = settings;
            this.gameDisplayView = gameDisplayView;
        }

        public void Initialize()
        {
            gameDisplayCanvas = gameDisplayView.GetComponent<Canvas>();
            gameDisplayScaler = gameDisplayView.GetComponent<CanvasScaler>();

            foreach (PlayerDisplayView playerDisplay in gameDisplayView.PlayerDisplays)
            {
                playerDisplay.SetActive(false);
                playerDisplay.Pointer.enabled = false;
            }
        }

        public void Display(Input input)
        {
            PlayerDisplay(input).SetActive(true);
            UpdateAether(input, 0);
            UpdateItem(input, null);
            UpdatePointer(input, Vector2.zero);
        }

        public void UpdateCamera(Camera camera)
        {
            gameDisplayCanvas.worldCamera = camera;
            gameDisplayCanvas.planeDistance = 1f;
        }

        public void UpdateAether(Input input, int aetherCount)
        {
            Text aetherText = PlayerDisplay(input).AetherText;
            aetherText.text = aetherCount.ToString();
        }

        public void UpdateItem(Input input, ItemDrop itemDrop)
        {
            Image itemImage = PlayerDisplay(input).ItemImage;

            if (itemDrop == null)
            {
                /* itemImage.sprite = settings.emptyItem; */
            }
            else
            {
                itemImage.sprite = itemDrop.ItemSprite;
            }
        }

        public void UpdatePointer(Input input, Vector2 position)
        {
            Image pointer = PlayerDisplay(input) .Pointer;

            if (position == Vector2.zero)
            {
                pointer.enabled = false;
            }
            else
            {
                Vector2 referenceResolution = gameDisplayScaler.referenceResolution;
                RectTransform pointerTransform = pointer.rectTransform;
                float angle = 0f;

                if (position.x < 0)
                {
                    angle = 180f;
                }

                if (position.y < 0)
                {
                    angle = 270f;
                }
                else if (position.y > 1f)
                {
                    angle = 90f;
                }

                pointerTransform.localRotation = Quaternion.AngleAxis(angle, Vector3.forward);

                Vector2 anchoredPosition = new Vector2(
                    Mathf.Clamp(position.x * (referenceResolution.x), 0, referenceResolution.x - pointerTransform.rect.width),
                    Mathf.Clamp(position.y * (referenceResolution.y), 0, referenceResolution.y - pointerTransform.rect.height)
                );

                pointer.enabled = true;
                pointer.rectTransform.anchoredPosition = anchoredPosition;
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
