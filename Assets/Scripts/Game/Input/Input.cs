using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class Input : ITickable
    {
        private readonly int deviceNum;
        private readonly InControl.InputDevice device;
        private InputMapping mapping;

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

        public InputMapping Mapping
        {
            get
            {
                return mapping;
            }
        }

        public void Tick()
        {
            mapping = new InputMapping()
            {
                Direction = device.Direction,
                MenuWasPressed = device.MenuWasPressed,
                Submit = device.Action1,
                Cancel = device.Action2,
                Jump = device.Action1,
                UseItem = device.LeftTrigger,
            };
        }

        public class Factory : Factory<int, InControl.InputDevice, Input>
        {
        }
    }
}
