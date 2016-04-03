using UnityEngine;
using System;
using System.Collections.Generic;
using Zenject;

namespace MistRidge
{
    public class GameReadyState : GameBaseState
    {
        private readonly Settings settings;
        private readonly SceneLoader sceneLoader;
        private readonly CameraView cameraView;
        private readonly InputManager inputManager;
        private readonly PlayerManager playerManager;
        private readonly DisplayManager displayManager;
        private readonly GameManager gameManager;

        private bool tweening;
        private List<CharacterType> characterTypes;
        private Dictionary<Input, CharacterType> inputTypeMapping;
        private Dictionary<Input, SelectType> inputSelectMapping;

        public GameReadyState(
                Settings settings,
                SceneLoader sceneLoader,
                CameraView cameraView,
                InputManager inputManager,
                PlayerManager playerManager,
                DisplayManager displayManager,
                GameManager gameManager,
                GameStateMachine stateMachine)
            : base(stateMachine)
        {
            this.settings = settings;
            this.sceneLoader = sceneLoader;
            this.cameraView = cameraView;
            this.inputManager = inputManager;
            this.playerManager = playerManager;
            this.displayManager = displayManager;
            this.gameManager = gameManager;

            stateType = GameStateType.Ready;
        }

        public List<Input> JoinedInputs
        {
            get
            {
                List<Input> joinedInputs = new List<Input>();

                foreach (KeyValuePair<Input, SelectType> inputSelect in inputSelectMapping)
                {
                    if (inputSelect.Value == SelectType.Select)
                    {
                        joinedInputs.Add(inputSelect.Key);
                    }
                }

                return joinedInputs;
            }
        }

        public override void Initialize()
        {
            characterTypes = new List<CharacterType>();
            inputTypeMapping = new Dictionary<Input, CharacterType>();
            inputSelectMapping = new Dictionary<Input, SelectType>();

            characterTypes.Add(CharacterType.Jack);
            characterTypes.Add(CharacterType.Jill);

            foreach (Input input in inputManager.Inputs)
            {
                if (input.DeviceNum % 2 == 0)
                {
                    inputTypeMapping[input] = CharacterType.Jack;
                }
                else
                {
                    inputTypeMapping[input] = CharacterType.Jill;
                }
                inputSelectMapping.Add(input, SelectType.None);
                displayManager.DisplayCharacterJoin(input.DeviceNum, true);
                displayManager.DisplayCharacterPlayerTag(input.DeviceNum, true);
            }

            if (gameManager.LastInput != null)
            {
                SubmitPlayer(gameManager.LastInput);
            }
        }

        private void ResetMappings()
        {
            foreach (Input input in inputManager.Inputs)
            {
                if (input.DeviceNum % 2 == 0)
                {
                    inputTypeMapping[input] = CharacterType.Jack;
                }
                else
                {
                    inputTypeMapping[input] = CharacterType.Jill;
                }
                inputSelectMapping[input] = SelectType.None;
            }
        }

        public override void Update()
        {
            if (tweening)
            {
                return;
            }

            foreach(Input input in inputManager.Inputs)
            {
                if (input.Mapping.Submit.WasPressed)
                {
                    SubmitPlayer(input);
                }
                else if (input.Mapping.Cancel.WasPressed)
                {
                    CancelPlayer(input);
                }
                else if (input.Mapping.MenuWasPressed)
                {
                    StartPlayer(input);
                }
                else if (input.Mapping.Direction.Left.WasPressed
                        || input.Mapping.Direction.Right.WasPressed)
                {
                    DirectionPlayer(input);
                }
            }
        }

        public override void EnterState()
        {
            tweening = false;
            displayManager.DisplayCharacterSelect(true);
            cameraView.IsActive = false;
        }

        public override void ExitState()
        {
            // Do Nothing
        }

        private int SelectPlayerCount
        {
            get
            {
                return JoinedInputs.Count;
            }
        }

        private void SubmitPlayer(Input input)
        {
            CharacterType characterType = inputTypeMapping[input];
            SelectType selectType = inputSelectMapping[input];

            switch (selectType)
            {
                case SelectType.None:
                    displayManager.DisplayCharacter(input.DeviceNum, characterType);
                    displayManager.DisplayCharacterArrows(input.DeviceNum, true);
                    displayManager.DisplayCharacterJoin(input.DeviceNum, false);
                    displayManager.DisplayCharacterSelect(input.DeviceNum, true);

                    inputSelectMapping[input] = SelectType.Join;
                    break;

                case SelectType.Join:
                    displayManager.DisplayCharacterArrows(input.DeviceNum, false);
                    displayManager.DisplayCharacterJoin(input.DeviceNum, false);
                    displayManager.DisplayCharacterSelect(input.DeviceNum, false);

                    inputSelectMapping[input] = SelectType.Select;

                    if (SelectPlayerCount >= settings.minStartPlayers)
                    {
                        displayManager.DisplayCharacterStart(true);
                    }

                    break;
            }
        }

        private void CancelPlayer(Input input)
        {
            SelectType selectType = inputSelectMapping[input];

            switch (selectType)
            {
                case SelectType.None:
                    tweening = true;

                    ResetMappings();
                    stateMachine.ChangeState(GameStateType.Start);
                    sceneLoader.Load(settings.startMenuSceneName);
                    break;

                case SelectType.Join:
                    displayManager.DisplayCharacter(input.DeviceNum, CharacterType.None);
                    displayManager.DisplayCharacterArrows(input.DeviceNum, false);
                    displayManager.DisplayCharacterJoin(input.DeviceNum, true);
                    displayManager.DisplayCharacterSelect(input.DeviceNum, false);

                    inputSelectMapping[input] = SelectType.None;
                    break;

                case SelectType.Select:
                    displayManager.DisplayCharacterArrows(input.DeviceNum, true);
                    displayManager.DisplayCharacterJoin(input.DeviceNum, true);
                    displayManager.DisplayCharacterSelect(input.DeviceNum, true);

                    inputSelectMapping[input] = SelectType.Join;

                    if (SelectPlayerCount < settings.minStartPlayers)
                    {
                        displayManager.DisplayCharacterStart(false);
                    }

                    break;
            }
        }

        private void StartPlayer(Input input)
        {
            SelectType selectType = inputSelectMapping[input];

            switch (selectType)
            {
                case SelectType.Select:
                    if (SelectPlayerCount < settings.minStartPlayers)
                    {
                        break;
                    }

                    stateMachine.ChangeState(GameStateType.Play);
                    sceneLoader.Load(settings.levelSceneName);
                    break;
            }
        }

        private void DirectionPlayer(Input input)
        {
            SelectType selectType = inputSelectMapping[input];

            if (selectType != SelectType.Join)
            {
                return;
            }

            CharacterType characterType = inputTypeMapping[input];

            switch (characterType)
            {
                case CharacterType.Jack:
                    inputTypeMapping[input] = CharacterType.Jill;
                    break;

                case CharacterType.Jill:
                    inputTypeMapping[input] = CharacterType.Jack;
                    break;
            }

            displayManager.DisplayCharacter(input.DeviceNum, inputTypeMapping[input]);
        }

        [Serializable]
        public class Settings
        {
            public int minStartPlayers;
            public string startMenuSceneName;
            public string levelSceneName;
        }
    }
}
