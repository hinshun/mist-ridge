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
        private readonly CharacterSelectDisplayView characterSelectDisplayView;

        private Canvas gameDisplayCanvas;
        private CanvasScaler gameDisplayScaler;

        public DisplayManager(
                Settings settings,
                GameDisplayView gameDisplayView,
                CharacterSelectDisplayView characterSelectDisplayView)
        {
            this.settings = settings;
            this.gameDisplayView = gameDisplayView;
            this.characterSelectDisplayView = characterSelectDisplayView;
        }

        public void Initialize()
        {
            gameDisplayCanvas = gameDisplayView.GetComponent<Canvas>();
            gameDisplayScaler = gameDisplayView.GetComponent<CanvasScaler>();

            UpdateSprint(false);

            foreach (PlayerDisplayView playerDisplay in gameDisplayView.PlayerDisplays)
            {
                playerDisplay.SetActive(false);
                playerDisplay.Pointer.enabled = false;
            }

            DisplayCharacterSelect(false);
            DisplayCharacterStart(false);

            for (int i = 0; i < 4; ++i)
            {
                DisplayCharacter(i, CharacterType.None);
                DisplayCharacterArrows(i, false);
                DisplayCharacterJoin(i, false);
                DisplayCharacterSelect(i, false);
                DisplayCharacterPlayerTag(i, false);
            }
        }

        public void Display(int deviceNum, CharacterType characterType)
        {
            UpdateBackdrop(deviceNum, BackdropHealth.Alive);
            UpdateNameTag(deviceNum, characterType);
            UpdatePortraitImage(deviceNum, characterType, PortraitEmotion.Neutral);
            UpdateItem(deviceNum, null);
            UpdateRank(deviceNum, -1);
            UpdateAether(deviceNum, 0);
            PlayerDisplay(deviceNum).SetActive(true);
            UpdatePointer(deviceNum, Vector2.zero);
        }

        public void UpdateCamera(Camera camera)
        {
            gameDisplayCanvas.worldCamera = camera;
            gameDisplayCanvas.planeDistance = 1f;
        }

        public void UpdateSprint(bool show)
        {
            SprintDisplayView sprintDisplay = gameDisplayView.SprintDisplay;
            sprintDisplay.SetActive(show);
        }

        public void UpdateSprintText(int current, int total)
        {
            Text sprintText = gameDisplayView.SprintDisplay.SprintText;
            sprintText.text = current + " / " + total;
        }

        public void UpdateBackdrop(int deviceNum, BackdropHealth backdropHealth)
        {
            Image background = PlayerDisplay(deviceNum).Background;
            Image itemCircle = PlayerDisplay(deviceNum).ItemCircle;

            switch (backdropHealth)
            {
                case BackdropHealth.Alive:
                    background.sprite = settings.backdrops[deviceNum].background;
                    itemCircle.sprite = settings.backdrops[deviceNum].itemCircle;
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

        public void UpdateNameTag(int deviceNum, CharacterType characterType)
        {
            Image nameTag = PlayerDisplay(deviceNum).NameTag;
            nameTag.sprite = GetPortrait(characterType).nameTag;
        }

        public void UpdatePortraitImage(int deviceNum, CharacterType characterType, PortraitEmotion portraitEmotion)
        {
            Image portraitImage = PlayerDisplay(deviceNum).PortraitImage;
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

        public void UpdateItem(int deviceNum, ItemDrop itemDrop)
        {
            Image itemSlot = PlayerDisplay(deviceNum).ItemSlot;

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

        public void UpdateRank(int deviceNum, int rank)
        {
            Image rankImage = PlayerDisplay(deviceNum).RankImage;

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

        public void UpdateAether(int deviceNum, int aetherCount)
        {
            Text aetherText = PlayerDisplay(deviceNum).AetherText;
            aetherText.text = aetherCount.ToString();
        }

        public void UpdatePointer(int deviceNum, Vector2 position)
        {
            Image pointer = PlayerDisplay(deviceNum) .Pointer;

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
                    angle = 90;
                }
                else if (position.x > 1f)
                {
                    angle = 270f;
                }

                if (position.y < 0)
                {
                    angle = 180f;
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

        public void DisplayCharacterSelect(bool show)
        {
            characterSelectDisplayView.SetActive(show);
        }

        public void DisplayCharacter(int deviceNum, CharacterType characterType)
        {
            Image portrait = PlayerCharacterDisplay(deviceNum).Portrait;
            Image nameTag = PlayerCharacterDisplay(deviceNum).NameTag;

            if (characterType == CharacterType.None)
            {
                portrait.enabled = false;
                nameTag.enabled = false;
                return;
            }
            portrait.enabled = true;
            nameTag.enabled = true;

            CharacterPortrait characterPortrait = GetCharacterPortrait(characterType);
            portrait.sprite = characterPortrait.portrait;
            nameTag.sprite = characterPortrait.nameTag;
        }

        public void DisplayCharacterArrows(int deviceNum, bool show)
        {
            Image arrowLeft = PlayerCharacterDisplay(deviceNum).ArrowLeft;
            Image arrowRight = PlayerCharacterDisplay(deviceNum).ArrowRight;

            arrowLeft.enabled = show;
            arrowRight.enabled = show;
        }

        public void DisplayCharacterJoin(int deviceNum, bool show)
        {
            Image join = PlayerCharacterDisplay(deviceNum).Join;
            join.enabled = show;
        }

        public void DisplayCharacterSelect(int deviceNum, bool show)
        {
            Image select = PlayerCharacterDisplay(deviceNum).Select;
            select.enabled = show;
        }

        public void DisplayCharacterPlayerTag(int deviceNum, bool show)
        {
            Image playerTag = PlayerCharacterDisplay(deviceNum).PlayerTag;
            playerTag.enabled = show;
        }

        public void DisplayCharacterStart(bool show)
        {
            Image start = characterSelectDisplayView.Start;
            start.enabled = show;
        }

        private PlayerDisplayView PlayerDisplay(int deviceNum)
        {
            return gameDisplayView.PlayerDisplays[deviceNum];
        }

        private PlayerCharacterDisplayView PlayerCharacterDisplay(int deviceNum)
        {
            return characterSelectDisplayView.PlayerCharacterDisplays[deviceNum];
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

            Debug.LogError("Failed to find valid character type for portrait");
            return new Portrait();
        }

        private CharacterPortrait GetCharacterPortrait(CharacterType characterType)
        {
            switch (characterType)
            {
                case CharacterType.Jack:
                    return settings.jackCharacter;

                case CharacterType.Jill:
                    return settings.jillCharacter;
            }

            Debug.LogError("Failed to find valid character type for character portrait");
            return new CharacterPortrait();
        }

        [Serializable]
        public class Settings
        {
            public Portrait jack;
            public CharacterPortrait jackCharacter;
            public Portrait jill;
            public CharacterPortrait jillCharacter;

            public List<Sprite> rankSprites;

            public Backdrop deadBackdrop;
            public List<Backdrop> backdrops;
        }
    }
}
