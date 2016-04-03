using UnityEngine;
using System;
using System.Collections.Generic;

namespace MistRidge
{
    public class CharacterSelectDisplayView : MonoView
    {
        [SerializeField]
        private List<PlayerCharacterDisplayView> playerCharacterDisplays;

        public List<PlayerCharacterDisplayView> PlayerCharacterDisplays
        {
            get
            {
                return playerCharacterDisplays;
            }
        }
    }
}
