using UnityEngine;
using System;
using System.Collections.Generic;
using Zenject;

namespace MistRidge
{
    public class StartMenuInputHandler : ITickable
    {
        private readonly StartMenuManager startMenuManager;
        private readonly InputManager inputManager;

        public StartMenuInputHandler(
                StartMenuManager startMenuManager,
                InputManager inputManager)
        {
            this.startMenuManager = startMenuManager;
            this.inputManager = inputManager;
        }

        public void Tick()
        {
            foreach(Input input in inputManager.inputs)
            {
                if (input.Current.submit.WasPressed)
                {
                    startMenuManager.Select();
                    return;
                }

                if (input.Current.move.Up.WasPressed)
                {
                    startMenuManager.MoveSelection(1);
                } else if (input.Current.move.Down.WasPressed)
                {
                    startMenuManager.MoveSelection(-1);
                }
            }
        }
    }
}
