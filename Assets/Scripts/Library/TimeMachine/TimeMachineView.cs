using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class TimeMachineView : MonoView
    {
        public event Action GUI = delegate {};

        public void OnGUI()
        {
            GUI();
        }
    }
}
