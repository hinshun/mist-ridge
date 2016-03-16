using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

namespace MistRidge
{
    public class SceneLoaderView : MonoView
    {
        private SceneLoader sceneLoader;

        public SceneLoader SceneLoader
        {
            set
            {
                sceneLoader = value;
            }
        }

        private IEnumerator LoadScene(string sceneName)
        {
            AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);
            yield return async;
            sceneLoader.FadeOut();
        }
    }
}
