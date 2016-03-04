using UnityEngine;
using System;
using System.Collections.Generic;

namespace MistRidge
{
    public class GameDisplayView : MonoView
    {
        [SerializeField]
        private List<PlayerDisplayView> playerDisplays;

        public List<PlayerDisplayView> PlayerDisplays
        {
            get
            {
                return playerDisplays;
            }
        }
    }
}
