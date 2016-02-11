using UnityEngine;

namespace MistRidge
{
    public struct InputSnapshot
    {
        public InControl.TwoAxisInputControl move;
        public bool menuPressed;
        public InControl.InputControl submit;
        public InControl.InputControl cancel;
        public InControl.InputControl jump;
    }
}
