using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class Input : ITickable
    {
        private readonly InControl.InputDevice device;

        public InputSnapshot current;

        public Input(InControl.InputDevice device)
        {
            this.device = device;
        }

        public void Tick()
        {
            current = new InputSnapshot()
            {
                move = device.Direction,
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
