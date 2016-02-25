using UnityEngine;
using System;

namespace MistRidge
{
    [Serializable]
    public abstract class ItemEffect : ScriptableObject
    {
        [SerializeField]
        private int maxUses = 1;

        public int MaxUses
        {
            get
            {
                return maxUses;
            }
        }
    }
}
