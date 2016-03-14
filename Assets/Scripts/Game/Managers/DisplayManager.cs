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
        private readonly PlayerManager playerManager;

        private Canvas gameDisplayCanvas;
        private CanvasScaler gameDisplayScaler;

        public DisplayManager(
                Settings settings,
                GameDisplayView gameDisplayView,
                PlayerManager playerManager)
        {
            this.settings = settings;
            this.gameDisplayView = gameDisplayView;
            this.playerManager = playerManager;
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
            UpdateBackdrop(input, BackdropHealth.Alive);
            UpdateNameTag(input, playerManager.GetCharacterType(input));
            UpdatePortraitImage(input, playerManager.GetCharacterType(input), PortraitEmotion.Neutral);
            UpdateItem(input, null);
            UpdateRank(input, -1);
            UpdateAether(input, 0);
            PlayerDisplay(input).SetActive(true);
            UpdatePointer(input, Vector2.zero);
        }

        public void UpdateCamera(Camera camera)
        {
            gameDisplayCanvas.worldCamera = camera;
            gameDisplayCanvas.planeDistance = 1f;
        }

        public void UpdateBackdrop(Input input, BackdropHealth backdropHealth)
        {
            Image background = PlayerDisplay(input).Background;
            Image itemCircle = PlayerDisplay(input).ItemCircle;

            switch (backdropHealth)
            {
                case BackdropHealth.Alive:
                    background.sprite = settings.backdrops[input.DeviceNum].background;
                    itemCircle.sprite = settings.backdrops[input.DeviceNum].itemCircle;
                    return;

                case BackdropHealth.Dead:
                    background.sprite = settings.deadBackdrop.background;
                    itemCircle.sprite = settings.deadBackdrop.itemCircle;
                    return;
            }

            Debug.LogError("Failed to find valid backdrop health");
            background.sprite = null;
            itemCircle.sprite = null;
        }

        public void UpdateNameTag(Input input, CharacterType characterType)
        {
            Image nameTag = PlayerDisplay(input).NameTag;
            nameTag.sprite = GetPortrait(characterType).nameTag;
        }

        public void UpdatePortraitImage(Input input, CharacterType characterType, PortraitEmotion portraitEmotion)
        {
            Image portraitImage = PlayerDisplay(input).PortraitImage;
            Portrait portrait = GetPortrait(characterType);

            switch (portraitEmotion)
            {
                case PortraitEmotion.Neutral:
                    portraitImage.sprite = portrait.neutral;
                    return;

                case PortraitEmotion.Hit:
                    portraitImage.sprite = portrait.hit;
                    return;

                case PortraitEmotion.Joy:
                    portraitImage.sprite = portrait.joy;
                    return;

                case PortraitEmotion.Dead:
                    portraitImage.sprite = portrait.dead;
                    return;
            }

            Debug.LogError("Failed to find valid portrait emotion");
            portraitImage.sprite = null;
        }

        public void UpdateItem(Input input, ItemDrop itemDrop)
        {
            Image itemSlot = PlayerDisplay(input).ItemSlot;

            if (itemDrop == null)
            {
                itemSlot.enabled = false;
            }
            else
            {
                itemSlot.sprite = itemDrop.ItemSprite;
                itemSlot.enabled = true;
            }
        }

        public void UpdateRank(Input input, int rank)
        {
            Image rankImage = PlayerDisplay(input).RankImage;

            if (rank >= 0)
            {
                rankImage.sprite = settings.rankSprites[rank];
                rankImage.enabled = true;
            }
            else
            {
                rankImage.enabled = false;
            }
        }

        public void UpdateAether(Input input, int aetherCount)
        {
            Text aetherText = PlayerDisplay(input).AetherText;
            aetherText.text = aetherCount.ToString();
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

        private Portrait GetPortrait(CharacterType characterType)
        {
            switch (characterType)
            {
                case CharacterType.Jack:
                    return settings.jack;

                case CharacterType.Jill:
                    return settings.jill;
            }

            Debug.LogError("Failed to find valid character type");
            return new Portrait();
        }

        [Serializable]
        public class Settings
        {
            public Portrait jack;
            public Portrait jill;

            public List<Sprite> rankSprites;

            public Backdrop deadBackdrop;
            public List<Backdrop> backdrops;
        }
    }
}
