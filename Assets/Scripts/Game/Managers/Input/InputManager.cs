using UnityEngine;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Zenject;

namespace MistRidge
{
    public class InputManager : IInitializable, ITickable
    {
        public ReadOnlyCollection<Input> inputs;
        private readonly Input.Factory inputFactory;

        public InputManager(Input.Factory inputFactory)
        {
            this.inputFactory = inputFactory;
        }

        public void Initialize()
        {
            List<Input> viableInputs = new List<Input>();

            for (int deviceNum = 0; deviceNum < InControl.InputManager.Devices.Count; ++deviceNum)
            {
                InControl.InputDevice device = InControl.InputManager.Devices[deviceNum];

                if (device != null)
                {
                    Input input = inputFactory.Create(device);
                    viableInputs.Add(input);
                }
            }

            inputs = new ReadOnlyCollection<Input>(viableInputs);
        }

        public void Tick()
        {
            foreach(Input input in inputs)
            {
                input.Tick();
            }
        }
    }
}
