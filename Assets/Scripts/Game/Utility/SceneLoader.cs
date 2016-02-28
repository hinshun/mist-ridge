using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using Zenject;

namespace MistRidge
{
    public class SceneLoader
    {
        [SerializeField]
        private readonly Settings settings;

        public SceneLoader(
                Settings settings,
                UnityFixGI unityFixGI)
        {
            this.settings = settings;
        }

        public void Load(string sceneName)
        {
            SceneManager.LoadSceneAsync(sceneName);
        }

        [Serializable]
        public class Settings
        {
            public string loadingSceneName;
        }
    }
}
