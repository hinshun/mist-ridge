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
        private Image itemImage;

        [SerializeField]
        private Text aetherText;

        public Image Portrait
        {
            get
            {
                return portrait;
            }
        }

        public Image ItemImage
        {
            get
            {
                return itemImage;
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
