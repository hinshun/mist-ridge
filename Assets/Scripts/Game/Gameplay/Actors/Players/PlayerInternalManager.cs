using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class PlayerInternalManager : IInitializable
    {
        private readonly Settings settings;
        private readonly Input input;
        private readonly PlayerView playerView;

        public PlayerInternalManager(
            Settings settings,
            Input input,
            PlayerView playerView)
        {
            this.settings = settings;
            this.input = input;
            this.playerView = playerView;
        }

        public void Initialize()
        {
            SetupPlayerMaterials(input.DeviceNum);
        }

        private void SetupPlayerMaterials(int deviceNum)
        {
            playerView.MeshRenderer.material = settings.playerMaterials[deviceNum];
        }

        [Serializable]
        public class Settings
        {
            public Material[] playerMaterials;
        }
    }
}
