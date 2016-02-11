using UnityEngine;
using System;
using System.Collections.Generic;
using Zenject;

namespace MistRidge
{
    public class StartMenuInputHandler : IInitializable, ITickable
    {
        private readonly StartMenuManager startMenuManager;
        private readonly InputManager inputManager;

        private IDictionary<Input, bool> moved;

        public StartMenuInputHandler(
                StartMenuManager startMenuManager,
                InputManager inputManager)
        {
            this.startMenuManager = startMenuManager;
            this.inputManager = inputManager;
        }

        public void Initialize()
        {
            moved = new Dictionary<Input, bool>();
            foreach(Input input in inputManager.inputs)
            {
                moved.Add(input, false);
            }
        }

        public void Tick()
        {
            foreach(Input input in inputManager.inputs)
            {
                if (moved[input]) {
                    if (input.current.move.z == 0) {
                        moved[input] = false;
                    }
                } else {
                    if (input.current.move.z > 0)
                    {
                        moved[input] = true;
                        startMenuManager.SelectMenuItem(1);
                    } else if (input.current.move.z < 0)
                    {
                        moved[input] = true;
                        startMenuManager.SelectMenuItem(-1);
                    }
                }
            }
        }
    }
}
