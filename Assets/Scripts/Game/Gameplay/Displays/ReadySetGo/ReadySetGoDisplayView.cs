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
    }
}
