using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class Input : ITickable
    {
        private readonly InControl.InputDevice device;
        private InputMapping current;

        public Input(InControl.InputDevice device)
        {
            this.device = device;
        }

        public InputMapping Current
        {
            get
            {
                return current;
            }
        }

        public void Tick()
        {
            current = new InputMapping()
            {
                direction = device.Direction,
                menuPressed = device.MenuWasPressed,
                submit = device.Action1,
                cancel = device.Action2,
                jump = device.Action1,
            };
        }

        public class Factory : Factory<InControl.InputDevice, Input>
        {
        }
    }
}
