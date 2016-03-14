using UnityEngine;
using UnityEngine.UI;
using System;

namespace MistRidge
{
    public class PlayerDisplayView : MonoView
    {
        [SerializeField]
        private Image background;

        [SerializeField]
        private Image itemCircle;

        [SerializeField]
        private Image nameTag;

        [SerializeField]
        private Image portraitImage;

        [SerializeField]
        private Image itemSlot;

        [SerializeField]
        private Image rankImage;

        [SerializeField]
        private Text aetherText;

        [SerializeField]
        private Image pointer;

        public Image Background
        {
            get
            {
                return background;
            }
        }

        public Image ItemCircle
        {
            get
            {
                return itemCircle;
            }
        }


        public Image NameTag
        {
            get
            {
                return nameTag;
            }
        }

        public Image PortraitImage
        {
            get
            {
                return portraitImage;
            }
        }


        public Image ItemSlot
        {
            get
            {
                return itemSlot;
            }
        }

        public Image RankImage
        {
            get
            {
                return rankImage;
            }
        }

        public Text AetherText
        {
            get
            {
                return aetherText;
            }
        }

        public Image Pointer
        {
            get
            {
                return pointer;
            }
        }
    }
}
