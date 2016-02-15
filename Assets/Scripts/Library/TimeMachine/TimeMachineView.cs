using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class TimeMachineView : MonoBehaviour
    {
        public event Action GUI = delegate {};

        public void OnGUI()
        {
            GUI();
        }
    }
}
