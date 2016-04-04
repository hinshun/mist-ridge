using UnityEngine;
using UnityEngine.UI;
using System;

namespace MistRidge
{
    public class PlayerCharacterDisplayView : MonoView
    {
        [SerializeField]
        private Image portrait;

        [SerializeField]
        private Image nameTag;

        [SerializeField]
        private Image arrowLeft;

        [SerializeField]
        private Image arrowRight;

        [SerializeField]
        private Image join;

        [SerializeField]
        private Image select;

        [SerializeField]
        private Image playerTag;

        public Image Portrait
        {
            get
            {
                return portrait;
            }
        }

        public Image NameTag
        {
            get
            {
                return nameTag;
            }
        }


        public Image ArrowLeft
        {
            get
            {
                return arrowLeft;
            }
        }

        public Image ArrowRight
        {
            get
            {
                return arrowRight;
            }
        }


        public Image Join
        {
            get
            {
                return join;
            }
        }

        public Image Select
        {
            get
            {
                return select;
            }
        }

        public Image PlayerTag
        {
            get
            {
                return playerTag;
            }
        }
    }
}
