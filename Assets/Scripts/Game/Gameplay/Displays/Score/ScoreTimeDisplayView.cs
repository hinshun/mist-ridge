using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

namespace MistRidge
{
    public class ScoreTimeDisplayView : MonoView
    {
        [SerializeField]
        private Text time;

        public Text Time
        {
            get
            {
                return time;
            }
        }
    }
}
