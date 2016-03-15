using UnityEngine;
using UnityEngine.UI;
using System;

namespace MistRidge
{
    public class SprintDisplayView : MonoView
    {
        [SerializeField]
        private Text sprintText;

        public Text SprintText
        {
            get
            {
                return sprintText;
            }
        }
    }
}
