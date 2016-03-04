using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

namespace MistRidge
{
    public class PlayerDisplayView : MonoView
    {
        [SerializeField]
        private Image portrait;

        [SerializeField]
        private Text aetherText;

        public Image Portrait
        {
            get
            {
                return portrait;
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
