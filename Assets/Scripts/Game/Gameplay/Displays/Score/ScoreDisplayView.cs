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
        private ScoreTimeDisplayView scoreTimeDisplayView;

        [SerializeField]
        private ScoreMenuDisplayView scoreMenuDisplayView;

        public List<PlayerScoreDisplayView> PlayerScoreDisplays
        {
            get
            {
                return playerScoreDisplayViews;
            }
        }

        public ScoreTimeDisplayView ScoreTimeDisplayView
        {
            get
            {
                return scoreTimeDisplayView;
            }
        }

        public ScoreMenuDisplayView ScoreMenuDisplayView
        {
            get
            {
                return scoreMenuDisplayView;
            }
        }
    }
}
