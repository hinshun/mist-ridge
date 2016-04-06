using UnityEngine;
using System;
using System.Collections.Generic;

namespace MistRidge
{
    [CreateAssetMenu(menuName = "Seeds/Seed Pool")]
    public class SeedPool : ScriptableObject
    {
        [SerializeField]
        private List<int> seeds;

        public List<int> Seeds
        {
            get
            {
                return seeds;
            }
        }
    }
}
