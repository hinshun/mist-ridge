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
                displayManager.UpdateCharacterJoin(input.DeviceNum, true);
                displayManager.UpdateCharacterPlayerTag(input.DeviceNum, true);
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
            displayManager.UpdateCharacterSelect(true);
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
                    displayManager.UpdateCharacter(input.DeviceNum, characterType);
                    displayManager.UpdateCharacterArrows(input.DeviceNum, true);
                    displayManager.UpdateCharacterJoin(input.DeviceNum, false);
                    displayManager.UpdateCharacterSelect(input.DeviceNum, true);

                    inputSelectMapping[input] = SelectType.Join;
                    break;

                case SelectType.Join:
                    displayManager.UpdateCharacterArrows(input.DeviceNum, false);
                    displayManager.UpdateCharacterJoin(input.DeviceNum, false);
                    displayManager.UpdateCharacterSelect(input.DeviceNum, false);

                    inputSelectMapping[input] = SelectType.Select;

                    if (SelectPlayerCount >= settings.minStartPlayers)
                    {
                        displayManager.UpdateCharacterStart(true);
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
                    displayManager.UpdateCharacter(input.DeviceNum, CharacterType.None);
                    displayManager.UpdateCharacterArrows(input.DeviceNum, false);
                    displayManager.UpdateCharacterJoin(input.DeviceNum, true);
                    displayManager.UpdateCharacterSelect(input.DeviceNum, false);

                    inputSelectMapping[input] = SelectType.None;
                    break;

                case SelectType.Select:
                    displayManager.UpdateCharacterArrows(input.DeviceNum, true);
                    displayManager.UpdateCharacterJoin(input.DeviceNum, true);
                    displayManager.UpdateCharacterSelect(input.DeviceNum, true);

                    inputSelectMapping[input] = SelectType.Join;

                    if (SelectPlayerCount < settings.minStartPlayers)
                    {
                        displayManager.UpdateCharacterStart(false);
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

            displayManager.UpdateCharacter(input.DeviceNum, inputTypeMapping[input]);
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
