using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using Zenject;

namespace MistRidge
{
    public class LoadingManager : IInitializable
    {
        private readonly Settings settings;
        private readonly SceneLoader sceneLoader;

        public LoadingManager(
            Settings settings,
            SceneLoader sceneLoader)
        {
            this.settings = settings;
            this.sceneLoader = sceneLoader;
        }

        public void Initialize()
        {
            sceneLoader.Load(settings.newGameSceneName);
        }

        [Serializable]
        public class Settings
        {
            public string newGameSceneName;
        }
    }
}
