using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

namespace MistRidge
{
    public class ScoreDisplayView : MonoView
    {
        [SerializeField]
        private List<PlayerScoreDisplayView> playerScoreDisplayViews;

        [SerializeField]
        private Text time;

        [SerializeField]
        private Image back;

        public List<PlayerScoreDisplayView> PlayerScoreDisplays
        {
            get
            {
                return playerScoreDisplayViews;
            }
        }

        public Text Time
        {
            get
            {
                return time;
            }
        }

        public Image Back
        {
            get
            {
                return back;
            }
        }
    }
}
