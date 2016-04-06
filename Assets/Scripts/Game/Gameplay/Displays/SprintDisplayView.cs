using UnityEngine;
using UnityEngine.UI;
using System;

namespace MistRidge
{
    public class SprintDisplayView : MonoView
    {
        [SerializeField]
        private Text sprintText;

        private RectTransform layoutTransform;

        public Text SprintText
        {
            get
            {
                return sprintText;
            }
        }

        public RectTransform LayoutTransform
        {
            get
            {
                return layoutTransform;
            }
        }

        private void Awake()
        {
            layoutTransform = GetComponent<RectTransform>();
        }
    }
}
