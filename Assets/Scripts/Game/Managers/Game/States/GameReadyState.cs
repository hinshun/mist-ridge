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
        private bool starting;
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

        public List<Input> SelectedInputs
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

        public List<Input> JoinedInputs
        {
            get
            {
                List<Input> joinedInputs = new List<Input>();

                foreach (KeyValuePair<Input, SelectType> inputSelect in inputSelectMapping)
                {
                    if (inputSelect.Value == SelectType.Join)
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

            characterTypes.Add(CharacterType.Jack);
            characterTypes.Add(CharacterType.Jill);

            ResetVariables();
        }

        public void ResetVariables()
        {
            tweening = false;
            starting = false;

            inputTypeMapping = new Dictionary<Input, CharacterType>();
            inputSelectMapping = new Dictionary<Input, SelectType>();

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
                displayManager.UpdateCharacterJoin(input.DeviceNum, true);
                displayManager.UpdateCharacterPlayerTag(input.DeviceNum, true);
            }
        }

        public CharacterType ChosenCharacterType(Input input)
        {
            return inputTypeMapping[input];
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
            starting = false;
            displayManager.UpdateCharacterSelect(true);
            cameraView.IsActive = false;

            if (gameManager.LastInput != null)
            {
                SubmitPlayer(gameManager.LastInput);
            }
        }

        public override void ExitState()
        {
            // Do Nothing
        }

        private int SelectPlayerCount
        {
            get
            {
                return SelectedInputs.Count;
            }
        }

        private int JoinedPlayerCount
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

                    break;
            }

            if (JoinedPlayerCount > 0 || SelectPlayerCount < settings.minStartPlayers)
            {
                displayManager.UpdateCharacterStart(false);
            }
            else
            {
                displayManager.UpdateCharacterStart(true);
            }
        }

        private void CancelPlayer(Input input)
        {
            SelectType selectType = inputSelectMapping[input];

            switch (selectType)
            {
                case SelectType.None:
                    tweening = true;

                    ResetVariables();
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
                    displayManager.UpdateCharacterJoin(input.DeviceNum, false);
                    displayManager.UpdateCharacterSelect(input.DeviceNum, true);

                    inputSelectMapping[input] = SelectType.Join;

                    break;
            }

            if (JoinedPlayerCount > 0 || SelectPlayerCount < settings.minStartPlayers)
            {
                displayManager.UpdateCharacterStart(false);
            }
            else
            {
                displayManager.UpdateCharacterStart(true);
            }
        }

        private void StartPlayer(Input input)
        {
            SelectType selectType = inputSelectMapping[input];

            switch (selectType)
            {
                case SelectType.Select:
                    if (starting || JoinedPlayerCount > 0 || SelectPlayerCount < settings.minStartPlayers)
                    {
                        break;
                    }

                    starting = true;
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
