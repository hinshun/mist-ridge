using UnityEngine;
using UnityEngine.UI;
using System;

namespace MistRidge
{
    public class PlayerScoreDisplayView : MonoView
    {
        [SerializeField]
        private Image portrait;

        [SerializeField]
        private Image playerTag;

        [SerializeField]
        private Text aetherText;

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
