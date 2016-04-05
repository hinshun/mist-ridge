using UnityEngine;
using System;
using System.Collections;
using Zenject;

namespace MistRidge
{
    public class SceneLoader : IInitializable
    {
        private readonly SceneLoadSignal.Trigger sceneLoadTrigger;
        private readonly SceneLoaderView sceneLoaderView;

        private Hashtable fadeInHashtable;
        private Hashtable fadeOutHashtable;

        public SceneLoader(
                SceneLoadSignal.Trigger sceneLoadTrigger,
                SceneLoaderView sceneLoaderView,
                UnityFixGI unityFixGI)
        {
            this.sceneLoadTrigger = sceneLoadTrigger;
            this.sceneLoaderView = sceneLoaderView;
        }

        public void Initialize()
        {
            sceneLoaderView.SceneLoader = this;

            fadeInHashtable = new Hashtable();
            fadeInHashtable.Add("from", Color.clear);
            fadeInHashtable.Add("to", Color.white);
            fadeInHashtable.Add("onupdate", "UpdateColor");
            fadeInHashtable.Add("oncompletetarget", sceneLoaderView.gameObject);

            fadeOutHashtable = new Hashtable();
            fadeOutHashtable.Add("from", Color.white);
            fadeOutHashtable.Add("to", Color.clear);
            fadeOutHashtable.Add("onupdate", "UpdateColor");
            fadeOutHashtable.Add("time", 1);
        }

        public void Load(string sceneName)
        {
            fadeInHashtable["oncomplete"] = "LoadScene";
            fadeInHashtable["oncompleteparams"] = sceneName;
            fadeInHashtable["time"] = 1;
            iTween.ValueTo(sceneLoaderView.FadeObject, fadeInHashtable);
        }

        public void OnlyFade()
        {
            fadeInHashtable["oncomplete"] = "OnlyFadeOut";
            fadeInHashtable["oncompleteparams"] = new Hashtable();
            fadeInHashtable["time"] = 4;
            iTween.ValueTo(sceneLoaderView.FadeObject, fadeInHashtable);
        }

        public void OnlyFadeOut()
        {
            sceneLoadTrigger.Fire();
            fadeOutHashtable["time"] = 4;
            iTween.ValueTo(sceneLoaderView.FadeObject, fadeOutHashtable);
        }

        public void FadeOut()
        {
            fadeOutHashtable["time"] = 1;
            iTween.ValueTo(sceneLoaderView.FadeObject, fadeOutHashtable);
        }
    }
}
