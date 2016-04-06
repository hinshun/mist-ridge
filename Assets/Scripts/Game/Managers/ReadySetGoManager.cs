using UnityEngine;
using System;
using System.Collections;
using Zenject;

namespace MistRidge
{
    public class ReadySetGoManager : IInitializable
    {
        private readonly Settings settings;
        private readonly PlayerManager playerManager;
        private readonly DisplayManager displayManager;

        private ReadySetGoDisplayView readySetGoDisplayView;

        private Hashtable readyInHashtable;
        private Hashtable readyOutHashtable;
        private Hashtable setInHashtable;
        private Hashtable setOutHashtable;
        private Hashtable goInHashtable;
        private Hashtable goOutHashtable;

        public ReadySetGoManager(
                Settings settings,
                PlayerManager playerManager,
                ReadySetGoDisplayView readySetGoDisplayView,
                DisplayManager displayManager)
        {
            this.settings = settings;
            this.playerManager = playerManager;
            this.readySetGoDisplayView = readySetGoDisplayView;
            this.displayManager = displayManager;
        }

        public void Initialize()
        {
            readySetGoDisplayView.ReadySetGoManager = this;

            readyInHashtable = new Hashtable();
            readyInHashtable.Add("from", 0);
            readyInHashtable.Add("to", 1);
            readyInHashtable.Add("time", settings.readyInFadeTime);
            readyInHashtable.Add("delay", settings.readyInFadeDelay);
            readyInHashtable.Add("onupdate", "ReadyFade");
            readyInHashtable.Add("oncomplete", "OnReadyInFadeComplete");

            readyOutHashtable = new Hashtable();
            readyOutHashtable.Add("from", 1);
            readyOutHashtable.Add("to", 0);
            readyOutHashtable.Add("time", settings.readyOutFadeTime);
            readyOutHashtable.Add("delay", settings.readyOutFadeDelay);
            readyOutHashtable.Add("onupdate", "ReadyFade");
            readyOutHashtable.Add("oncomplete", "OnReadyOutFadeComplete");

            setInHashtable = new Hashtable();
            setInHashtable.Add("from", 0);
            setInHashtable.Add("to", 1);
            setInHashtable.Add("time", settings.setInFadeTime);
            setInHashtable.Add("delay", settings.setInFadeDelay);
            setInHashtable.Add("onupdate", "SetFade");
            setInHashtable.Add("oncomplete", "OnSetInFadeComplete");

            setOutHashtable = new Hashtable();
            setOutHashtable.Add("from", 1);
            setOutHashtable.Add("to", 0);
            setOutHashtable.Add("time", settings.setOutFadeTime);
            setOutHashtable.Add("delay", settings.setOutFadeDelay);
            setOutHashtable.Add("onupdate", "SetFade");
            setOutHashtable.Add("oncomplete", "OnSetOutFadeComplete");

            goInHashtable = new Hashtable();
            goInHashtable.Add("from", 0);
            goInHashtable.Add("to", 1);
            goInHashtable.Add("time", settings.goInFadeTime);
            goInHashtable.Add("delay", settings.goInFadeDelay);
            goInHashtable.Add("onupdate", "GoFade");
            goInHashtable.Add("oncomplete", "OnGoInFadeComplete");

            goOutHashtable = new Hashtable();
            goOutHashtable.Add("from", 1);
            goOutHashtable.Add("to", 0);
            goOutHashtable.Add("time", settings.goOutFadeTime);
            goOutHashtable.Add("delay", settings.goOutFadeDelay);
            goOutHashtable.Add("onupdate", "GoFade");
        }

        public void Countdown()
        {
            displayManager.UpdateCinematic(true);
            iTween.ValueTo(readySetGoDisplayView.gameObject, readyInHashtable);
        }

        public void OnReadyInFadeComplete()
        {
            iTween.ValueTo(readySetGoDisplayView.gameObject, readyOutHashtable);
        }

        public void OnReadyOutFadeComplete()
        {
            iTween.ValueTo(readySetGoDisplayView.gameObject, setInHashtable);
        }

        public void OnSetInFadeComplete()
        {
            iTween.ValueTo(readySetGoDisplayView.gameObject, setOutHashtable);
        }

        public void OnSetOutFadeComplete()
        {
            iTween.ValueTo(readySetGoDisplayView.gameObject, goInHashtable);
        }

        public void OnGoInFadeComplete()
        {
            playerManager.ChangePlayerControl(true);

            displayManager.UpdateCinematic(false);
            displayManager.UpdateSprint(false);
            iTween.ValueTo(readySetGoDisplayView.gameObject, goOutHashtable);
        }

        [Serializable]
        public class Settings
        {
            public float readyInFadeTime;
            public float readyInFadeDelay;
            public float readyOutFadeTime;
            public float readyOutFadeDelay;

            public float setInFadeTime;
            public float setInFadeDelay;
            public float setOutFadeTime;
            public float setOutFadeDelay;

            public float goInFadeTime;
            public float goInFadeDelay;
            public float goOutFadeTime;
            public float goOutFadeDelay;
        }
    }
}
