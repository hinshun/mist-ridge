using UnityEngine;
using System;
using System.Collections;
using Zenject;

namespace MistRidge
{
    public class SceneLoader : IInitializable
    {
        private readonly SceneLoaderView sceneLoaderView;

        private Hashtable fadeInHashtable;
        private Hashtable fadeOutHashtable;

        public SceneLoader(
                SceneLoaderView sceneLoaderView,
                UnityFixGI unityFixGI)
        {
            this.sceneLoaderView = sceneLoaderView;
        }

        public void Initialize()
        {
            sceneLoaderView.SceneLoader = this;

            fadeInHashtable = new Hashtable();
            fadeInHashtable.Add("from", Color.clear);
            fadeInHashtable.Add("to", Color.white);
            fadeInHashtable.Add("time", 1f);
            fadeInHashtable.Add("onupdate", "UpdateColor");
            fadeInHashtable.Add("oncomplete", "LoadScene");
            fadeInHashtable.Add("oncompletetarget", sceneLoaderView.gameObject);

            fadeOutHashtable = new Hashtable();
            fadeOutHashtable.Add("from", Color.white);
            fadeOutHashtable.Add("to", Color.clear);
            fadeOutHashtable.Add("onupdate", "UpdateColor");
            fadeOutHashtable.Add("time", 1f);
        }

        public void Load(string sceneName)
        {
            fadeInHashtable["oncompleteparams"] = sceneName;
            iTween.ValueTo(sceneLoaderView.FadeObject, fadeInHashtable);
        }

        public void FadeOut()
        {
            iTween.ValueTo(sceneLoaderView.FadeObject, fadeOutHashtable);
        }
    }
}
