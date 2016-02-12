using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class PlayerController : ITickable
    {
        private readonly PlayerView view;

        public PlayerController(
                PlayerView view)
        {
            this.view = view;
        }

        public void Tick()
        {
            // Do Nothing
        }
    }
}
