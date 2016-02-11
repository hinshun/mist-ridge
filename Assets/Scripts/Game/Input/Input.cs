using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class Input : ITickable
    {
        public InputSnapshot current;
        private InControl.InputDevice device;

        public Input(InControl.InputDevice device)
        {
            this.device = device;
        }

        public void Tick()
        {
            float horizontal = device.Direction.X;
            float vertical = device.Direction.Y;

            current = new InputSnapshot()
            {
                move = new Vector3(horizontal, 0, vertical),
                submitPressed = device.Action1,
                cancelPressed = device.Action2,
                startPressed = device.MenuWasPressed,
                jumpPressed = device.Action1,
            };
        }

        public class Factory : Factory<InControl.InputDevice, Input>
        {
        }
    }
}
