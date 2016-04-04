using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class GameStartState : GameBaseState
    {
        private readonly MenuSignal.Trigger menuSignalTrigger;
        private readonly InputManager inputManager;
        private readonly DisplayManager displayManager;
        private readonly CameraView cameraView;

        private bool tweening;

        public GameStartState(
                MenuSignal.Trigger menuSignalTrigger,
                InputManager inputManager,
                DisplayManager displayManager,
                CameraView cameraView,
                GameStateMachine stateMachine)
            : base(stateMachine)
        {
            this.menuSignalTrigger = menuSignalTrigger;
            this.inputManager = inputManager;
            this.displayManager = displayManager;
            this.cameraView = cameraView;

            stateType = GameStateType.Start;
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
                    tweening = true;
                    menuSignalTrigger.Fire(input, MenuSignalType.Submit);
                    return;
                }

                if (input.Mapping.Direction.Up.WasPressed)
                {
                    menuSignalTrigger.Fire(input, MenuSignalType.Up);
                }
                else if (input.Mapping.Direction.Down.WasPressed)
                {
                    menuSignalTrigger.Fire(input, MenuSignalType.Down);
                }
            }
        }

        public override void EnterState()
        {
            tweening = false;
            cameraView.IsActive = false;
            displayManager.UpdateCharacterSelect(false);
        }

        public override void ExitState()
        {
            cameraView.IsActive = true;
        }
    }
}
