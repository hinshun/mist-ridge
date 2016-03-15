using UnityEngine;
using System;
using System.Collections.Generic;

namespace MistRidge
{
    public class GameDisplayView : MonoView
    {
        [SerializeField]
        private SprintDisplayView sprintDisplay;

        [SerializeField]
        private List<PlayerDisplayView> playerDisplays;

        public SprintDisplayView SprintDisplay
        {
            get
            {
                return sprintDisplay;
            }
        }

        public List<PlayerDisplayView> PlayerDisplays
        {
            get
            {
                return playerDisplays;
            }
        }
    }
}
