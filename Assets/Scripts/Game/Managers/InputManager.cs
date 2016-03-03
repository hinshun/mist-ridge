using UnityEngine;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Zenject;
using InControl;

namespace MistRidge
{
    public class InputManager : IInitializable, ITickable
    {
        private readonly Input.Factory inputFactory;

        private ReadOnlyCollection<Input> inputs;

        public InputManager(
                Input.Factory inputFactory,
                InControl.InControlManager inControlManager)
        {
            this.inputFactory = inputFactory;
        }

        public ReadOnlyCollection<Input> Inputs
        {
            get
            {
                return inputs;
            }
        }

        public void Initialize()
        {
            List<Input> viableInputs = new List<Input>();

            for (int deviceNum = 0; deviceNum < InControl.InputManager.Devices.Count; ++deviceNum)
            {
                InControl.InputDevice device = InControl.InputManager.Devices[deviceNum];

                if (device != null)
                {
                    Input input = inputFactory.Create(deviceNum, device);
                    viableInputs.Add(input);
                }
            }

            if (viableInputs.Count == 0)
            {
                InControl.InputDevice device = new UnityInputDevice(new KeyboardAndMouseProfile());
                InControl.InputManager.AttachDevice(device);

                if (device != null)
                {
                    Input input = inputFactory.Create(0, device);
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
