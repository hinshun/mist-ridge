using UnityEngine;

namespace MistRidge
{
    public struct InputMapping
    {
        public InControl.TwoAxisInputControl direction;
        public bool menuWasPressed;
        public InControl.InputControl submit;
        public InControl.InputControl cancel;
        public InControl.InputControl jump;
    }
}
