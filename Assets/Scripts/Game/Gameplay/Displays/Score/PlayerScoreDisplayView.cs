using UnityEngine;
using UnityEngine.UI;
using System;

namespace MistRidge
{
    public class PlayerScoreDisplayView : MonoView
    {
        [SerializeField]
        private Image background;

        [SerializeField]
        private Image crown;

        [SerializeField]
        private Image portrait;

        [SerializeField]
        private Image playerTag;

        [SerializeField]
        private Text aetherText;

        public Image Background
        {
            get
            {
                return background;
            }
        }

        public Image Crown
        {
            get
            {
                return crown;
            }
        }

        public Image Portrait
        {
            get
            {
                return portrait;
            }
        }

        public Image PlayerTag
        {
            get
            {
                return playerTag;
            }
        }

        public Text AetherText
        {
            get
            {
                 return aetherText;
            }
        }
    }
}
