using UnityEngine;
using System;
using System.Collections;
using Zenject;

namespace MistRidge
{
    public class ReadySetGoManager : IInitializable
    {
        private readonly Settings settings;
        private ReadySetGoManagerView readySetGoManagerView;
        private readonly DisplayManager displayManager;

        private Hashtable readyHashtable;
        private Hashtable setHashtable;
        private Hashtable goHashtable;

        public ReadySetGoManager(
                Settings settings,
                ReadySetGoManagerView readySetGoManagerView,
                DisplayManager displayManager)
        {
            this.settings = settings;
            this.readySetGoManagerView = readySetGoManagerView;
            this.displayManager = displayManager;
        }

        public void Initialize()
        {
            readySetGoManagerView.ReadySetGoManager = this;

            readyHashtable = new Hashtable();

            setHashtable = new Hashtable();

            goHashtable = new Hashtable();
        }

        public void OnReady()
        {
            displayManager.UpdateCinematic(true);
        }

        public void OnSet()
        {
        }

        public void OnGo()
        {
            displayManager.UpdateCinematic(false);
        }

        public void OnReadyFade(float alpha)
        {
        }

        public void OnSetFade(float alpha)
        {
        }

        public void OnGoFade(float alpha)
        {
        }

        [Serializable]
        public class Settings
        {
        }
    }
}
