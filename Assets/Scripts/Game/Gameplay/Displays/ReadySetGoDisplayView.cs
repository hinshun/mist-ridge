using UnityEngine;
using UnityEngine.UI;
using System;

namespace MistRidge
{
    public class ReadySetGoDisplayView : MonoView
    {
        [SerializeField]
        private Image ready;

        [SerializeField]
        private Image set;

        [SerializeField]
        private Image go;

        private ReadySetGoManager readySetGoManager;

        public Image Ready
        {
            get
            {
                return ready;
            }
        }

        public Image Set
        {
            get
            {
                return set;
            }
        }

        public Image Go
        {
            get
            {
                return go;
            }
        }

        public void Reset()
        {
            ready.color = Color.clear;
            set.color = Color.clear;
            go.color = Color.clear;
        }

        public ReadySetGoManager ReadySetGoManager
        {
            get
            {
                return readySetGoManager;
            }
            set
            {
                readySetGoManager = value;
            }
        }

        public void ReadyFade(float alpha)
        {
            ready.color = new Color(1, 1, 1, alpha);
        }

        public void SetFade(float alpha)
        {
            set.color = new Color(1, 1, 1, alpha);
        }

        public void GoFade(float alpha)
        {
            go.color = new Color(1, 1, 1, alpha);
        }

        public void OnReadyInFadeComplete()
        {
            readySetGoManager.OnReadyInFadeComplete();
        }

        public void OnReadyOutFadeComplete()
        {
            readySetGoManager.OnReadyOutFadeComplete();
        }

        public void OnSetInFadeComplete()
        {
            readySetGoManager.OnSetInFadeComplete();
        }

        public void OnSetOutFadeComplete()
        {
            readySetGoManager.OnSetOutFadeComplete();
        }

        public void OnGoInFadeComplete()
        {
            readySetGoManager.OnGoInFadeComplete();
        }
    }
}
