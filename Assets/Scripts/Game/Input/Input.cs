using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class Input : ITickable
    {
        private readonly int deviceNum;
        private readonly InControl.InputDevice device;
        private InputMapping current;

        public Input(
                int deviceNum,
                InControl.InputDevice device)
        {
            this.deviceNum = deviceNum;
            this.device = device;
        }

        public int DeviceNum
        {
            get
            {
                return deviceNum;
            }
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
                menuWasPressed = device.MenuWasPressed,
                submit = device.Action1,
                cancel = device.Action2,
                jump = device.Action1,
            };
        }

        public class Factory : Factory<int, InControl.InputDevice, Input>
        {
        }
    }
}
