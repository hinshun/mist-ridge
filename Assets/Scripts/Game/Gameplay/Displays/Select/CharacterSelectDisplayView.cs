using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

namespace MistRidge
{
    public class CharacterSelectDisplayView : MonoView
    {
        [SerializeField]
        private List<PlayerCharacterDisplayView> playerCharacterDisplays;

        [SerializeField]
        private Image start;

        public List<PlayerCharacterDisplayView> PlayerCharacterDisplays
        {
            get
            {
                return playerCharacterDisplays;
            }
        }

        public Image Start
        {
            get
            {
                return start;
            }
        }
    }
}
