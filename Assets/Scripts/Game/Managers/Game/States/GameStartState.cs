using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class GameStartState : GameBaseState
    {
        private readonly MenuSignal.Trigger menuSignalTrigger;
        private readonly InputManager inputManager;
        private readonly CameraView cameraView;

        public GameStartState(
                MenuSignal.Trigger menuSignalTrigger,
                InputManager inputManager,
                CameraView cameraView,
                GameStateMachine stateMachine)
            : base(stateMachine)
        {
            this.menuSignalTrigger = menuSignalTrigger;
            this.inputManager = inputManager;
            this.cameraView = cameraView;

            stateType = GameStateType.Start;
        }

        public override void Update()
        {
            foreach(Input input in inputManager.Inputs)
            {
                if (input.Mapping.Submit.WasPressed)
                {
                    menuSignalTrigger.Fire(MenuSignalType.Submit);
                    return;
                }

                if (input.Mapping.Direction.Up.WasPressed)
                {
                    menuSignalTrigger.Fire(MenuSignalType.Up);
                }
                else if (input.Mapping.Direction.Down.WasPressed)
                {
                    menuSignalTrigger.Fire(MenuSignalType.Down);
                }
            }
        }

        public override void EnterState()
        {
            cameraView.AudioListener.enabled = false;
        }

        public override void ExitState()
        {
            cameraView.AudioListener.enabled = true;
        }
    }
}
