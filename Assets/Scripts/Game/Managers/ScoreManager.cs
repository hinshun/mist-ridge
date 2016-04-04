using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class ScoreManager : IInitializable
    {
        private readonly DisplayManager displayManager;

        public ScoreManager(DisplayManager displayManager)
        {
            this.displayManager = displayManager;
        }

        public void Initialize()
        {
            // Do Nothing
        }
    }
}
