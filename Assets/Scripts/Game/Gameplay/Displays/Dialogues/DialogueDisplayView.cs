using UnityEngine;
using UnityEngine.UI;
using System;

namespace MistRidge
{
    public class DialogueDisplayView : MonoView
    {
        [SerializeField]
        private Image background;

        [SerializeField]
        private Text dialogue;

        [SerializeField]
        private Image next;

        public Image Background
        {
            get
            {
                return background;
            }
        }

        public Text Dialogue
        {
            get
            {
                return dialogue;
            }
        }

        public Image Next
        {
            get
            {
                return next;
            }
        }
    }
}
