using UnityEngine;

namespace MistRidge
{
    public class ReadySetGoManagerView : MonoView
    {
        private ReadySetGoManager readySetGoManager;

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

        public void OnReadyFade(float alpha)
        {
            readySetGoManager.OnReadyFade(alpha);
        }

        public void OnSetFade(float alpha)
        {
            readySetGoManager.OnSetFade(alpha);
        }

        public void OnGoFade(float alpha)
        {
            readySetGoManager.OnGoFade(alpha);
        }

        public void OnSet()
        {
            readySetGoManager.OnSet();
        }

        public void OnGo()
        {
            readySetGoManager.OnGo();
        }
    }
}
