using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using Zenject;

namespace MistRidge
{
    public class SceneLoader
    {
        public SceneLoader(UnityFixGI unityFixGI)
        {
        }

        public void Load(string sceneName)
        {
            SceneManager.LoadSceneAsync(sceneName);
        }
    }
}
