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
        private readonly CinematicDisplayView cinematicDisplayView;
        private readonly DialogueDisplayView dialogueDisplayView;
        private readonly ReadySetGoDisplayView readySetGoDisplayView;
        private readonly ScoreDisplayView scoreDisplayView;

        private Canvas gameDisplayCanvas;
        private CanvasScaler gameDisplayScaler;

        public DisplayManager(
                Settings settings,
                GameDisplayView gameDisplayView,
                CharacterSelectDisplayView characterSelectDisplayView,
                CinematicDisplayView cinematicDisplayView,
                DialogueDisplayView dialogueDisplayView,
                ReadySetGoDisplayView readySetGoDisplayView,
                ScoreDisplayView scoreDisplayView)
        {
            this.settings = settings;
            this.gameDisplayView = gameDisplayView;
            this.characterSelectDisplayView = characterSelectDisplayView;
            this.cinematicDisplayView = cinematicDisplayView;
            this.dialogueDisplayView = dialogueDisplayView;
            this.readySetGoDisplayView = readySetGoDisplayView ;
            this.scoreDisplayView = scoreDisplayView;
        }

        public void Initialize()
        {
            gameDisplayCanvas = gameDisplayView.GetComponent<Canvas>();
            gameDisplayScaler = gameDisplayView.GetComponent<CanvasScaler>();

            ResetVariables();
        }

        public void ResetVariables()
        {
            UpdateSprint(false);

            foreach (PlayerDisplayView playerDisplay in gameDisplayView.PlayerDisplays)
            {
                playerDisplay.SetActive(false);
                playerDisplay.Pointer.enabled = false;
            }

            UpdateCharacterSelect(false);
            UpdateCharacterStart(false);

            for (int i = 0; i < 4; ++i)
            {
                UpdateCharacter(i, CharacterType.None);
                UpdateCharacterArrows(i, false);
                UpdateCharacterJoin(i, false);
                UpdateCharacterSelect(i, false);
                UpdateCharacterPlayerTag(i, false);
            }

            UpdateDialogue(false);

            readySetGoDisplayView.Reset();

            UpdateScoreTime(false, 0);
            UpdateScoreMenu(false);
            UpdateScorePlayer(0, 0, ScorePlacementType.First, CharacterType.None);
            UpdateScorePlayer(0, 0, ScorePlacementType.Second, CharacterType.None);
            UpdateScorePlayer(0, 0, ScorePlacementType.Third, CharacterType.None);
            UpdateScorePlayer(0, 0, ScorePlacementType.Fourth, CharacterType.None);
        }

        public void Display(int deviceNum, CharacterType characterType)
        {
            if (characterType == CharacterType.None)
            {
                PlayerDisplay(deviceNum).SetActive(false);
                return;
            }

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

        public void UpdateLayout(float y)
        {
            RectTransform layoutTransform = gameDisplayView.LayoutTransform;
            layoutTransform.anchoredPosition3D = new Vector3(
                layoutTransform.anchoredPosition3D.x,
                y,
                layoutTransform.anchoredPosition3D.z
            );
        }

        public void UpdateSprint(bool show)
        {
            SprintDisplayView sprintDisplay = gameDisplayView.SprintDisplay;
            sprintDisplay.SetActive(show);
        }

        public void UpdateSprintLayout(float y)
        {
            RectTransform layoutTransform = gameDisplayView.SprintDisplay.LayoutTransform;

            layoutTransform.anchoredPosition3D = new Vector3(
                layoutTransform.anchoredPosition3D.x,
                y,
                layoutTransform.anchoredPosition3D.z
            );
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

        public void UpdateCharacterSelect(bool show)
        {
            characterSelectDisplayView.SetActive(show);
        }

        public void UpdateCharacter(int deviceNum, CharacterType characterType)
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

        public void UpdateCharacterArrows(int deviceNum, bool show)
        {
            Image arrowLeft = PlayerCharacterDisplay(deviceNum).ArrowLeft;
            Image arrowRight = PlayerCharacterDisplay(deviceNum).ArrowRight;

            arrowLeft.enabled = show;
            arrowRight.enabled = show;
        }

        public void UpdateCharacterJoin(int deviceNum, bool show)
        {
            Image join = PlayerCharacterDisplay(deviceNum).Join;
            join.enabled = show;
        }

        public void UpdateCharacterSelect(int deviceNum, bool show)
        {
            Image select = PlayerCharacterDisplay(deviceNum).Select;
            select.enabled = show;
        }

        public void UpdateCharacterPlayerTag(int deviceNum, bool show)
        {
            Image playerTag = PlayerCharacterDisplay(deviceNum).PlayerTag;
            playerTag.enabled = show;
        }

        public void UpdateCharacterStart(bool show)
        {
            Image start = characterSelectDisplayView.Start;
            start.enabled = show;
        }

        public void UpdateCinematic(bool show)
        {
            cinematicDisplayView.SetActive(show);
        }

        public void UpdateDialogue(bool show)
        {
            dialogueDisplayView.SetActive(show);
        }

        public void UpdateDialogueText(string text)
        {
            dialogueDisplayView.Dialogue.text = text;
        }

        public void UpdateDialogueNext(bool show)
        {
            dialogueDisplayView.Next.enabled = show;
        }

        public void UpdateScorePlayer(int deviceNum, int aetherCount, ScorePlacementType scorePlacementType, CharacterType characterType)
        {
            if (characterType == CharacterType.None)
            {
                PlayerScoreDisplay(scorePlacementType).SetActive(false);
                return;
            }

            Image portrait = PlayerScoreDisplay(scorePlacementType).Portrait;
            Image playerTag = PlayerScoreDisplay(scorePlacementType).PlayerTag;
            Text aetherText = PlayerScoreDisplay(scorePlacementType).AetherText;

            portrait.sprite = GetScorePortrait(characterType);
            playerTag.sprite = GetPlayerTag(deviceNum);
            aetherText.text = aetherCount.ToString();

            PlayerScoreDisplay(scorePlacementType).SetActive(true);
        }

        public void UpdateScoreTime(bool show, int seconds)
        {
            ScoreTimeDisplayView scoreTimeDisplayView = scoreDisplayView.ScoreTimeDisplayView;

            int minutes = seconds / 60;
            scoreTimeDisplayView.Time.text = minutes + "m " + (seconds % 60) + "s";

            scoreDisplayView.SetActive(show);
        }

        public void UpdateScoreMenu(bool show)
        {
            scoreDisplayView.ScoreMenuDisplayView.SetActive(show);
        }

        private PlayerDisplayView PlayerDisplay(int deviceNum)
        {
            return gameDisplayView.PlayerDisplays[deviceNum];
        }

        private PlayerCharacterDisplayView PlayerCharacterDisplay(int deviceNum)
        {
            return characterSelectDisplayView.PlayerCharacterDisplays[deviceNum];
        }

        private PlayerScoreDisplayView PlayerScoreDisplay(ScorePlacementType scorePlacementType)
        {
            switch(scorePlacementType)
            {
                case ScorePlacementType.First:
                    return scoreDisplayView.PlayerScoreDisplays[0];

                case ScorePlacementType.Second:
                    return scoreDisplayView.PlayerScoreDisplays[1];

                case ScorePlacementType.Third:
                    return scoreDisplayView.PlayerScoreDisplays[2];

                case ScorePlacementType.Fourth:
                    return scoreDisplayView.PlayerScoreDisplays[3];
            }

            return null;
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

        private Sprite GetScorePortrait(CharacterType characterType)
        {
            switch (characterType)
            {
                case CharacterType.Jack:
                    return settings.jackScore;

                case CharacterType.Jill:
                    return settings.jillScore;
            }

            Debug.LogError("Failed to find valid character type for score portrait");
            return new Sprite();
        }

        private Sprite GetPlayerTag(int deviceNum)
        {
            return settings.playerTagSprites[deviceNum];
        }

        [Serializable]
        public class Settings
        {
            public Portrait jack;
            public CharacterPortrait jackCharacter;
            public Sprite jackScore;

            public Portrait jill;
            public CharacterPortrait jillCharacter;
            public Sprite jillScore;

            public List<Sprite> rankSprites;
            public List<Sprite> crownSprites;
            public List<Sprite> playerTagSprites;

            public Backdrop deadBackdrop;
            public List<Backdrop> backdrops;
        }
    }
}
