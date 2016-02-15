using UnityEngine;
using System;
using System.Collections.Generic;
using Zenject;

namespace MistRidge
{
    public class PlayerInputHandler : ITickable
    {
        private readonly PlayerManager playerManager;
        private readonly InputManager inputManager;

        public PlayerInputHandler(
                PlayerManager playerManager,
                InputManager inputManager)
        {
            this.playerManager = playerManager;
            this.inputManager = inputManager;
        }

        public void Tick()
        {
            foreach(Input input in inputManager.inputs)
            {
                if (input.Current.cancel.WasPressed)
                {
                    playerManager.SpawnPlayer(input);
                    return;
                }
            }
        }
    }
}
