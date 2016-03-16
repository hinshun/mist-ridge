using UnityEngine;
using System;
using System.Collections;
using Zenject;

namespace MistRidge
{
    public class SceneLoader : IInitializable
    {
        private readonly Settings settings;
        private readonly SceneLoaderView sceneLoaderView;

        private Hashtable fadeInHashtable;
        private Hashtable fadeOutHashtable;

        public SceneLoader(
                Settings settings,
                SceneLoaderView sceneLoaderView,
                UnityFixGI unityFixGI)
        {
            this.settings = settings;
            this.sceneLoaderView = sceneLoaderView;
        }

        public void Initialize()
        {
            sceneLoaderView.SceneLoader = this;

            GameObject fadeObject = iTween.CameraFadeAdd(settings.fadeTexture);
            fadeObject.transform.parent = sceneLoaderView.transform;

            fadeInHashtable = new Hashtable();
            fadeInHashtable.Add("amount", 1f);
            fadeInHashtable.Add("time", 1f);
            fadeInHashtable.Add("oncomplete", "LoadScene");
            fadeInHashtable.Add("oncompletetarget", sceneLoaderView.gameObject);

            fadeOutHashtable = new Hashtable();
            fadeOutHashtable.Add("amount", 0f);
            fadeOutHashtable.Add("time", 1f);
        }

        public void Load(string sceneName)
        {
            fadeInHashtable["oncompleteparams"] = sceneName;
            iTween.CameraFadeTo(fadeInHashtable);
        }

        public void FadeOut()
        {
            iTween.CameraFadeTo(fadeOutHashtable);
        }

        [Serializable]
        public class Settings
        {
            public Texture2D fadeTexture;
        }
    }
}
