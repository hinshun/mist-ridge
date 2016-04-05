using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

namespace MistRidge
{
    public class SceneLoaderView : MonoView
    {
        [SerializeField]
        private GameObject fadeObject;

        private SceneLoader sceneLoader;

        public GameObject FadeObject
        {
            get
            {
                return fadeObject;
            }
        }

        public SceneLoader SceneLoader
        {
            set
            {
                sceneLoader = value;
            }
        }

        public void OnlyFadeOut()
        {
            sceneLoader.OnlyFadeOut();
        }

        private IEnumerator LoadScene(string sceneName)
        {
            AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);
            yield return async;
            sceneLoader.FadeOut();
        }
    }
}
