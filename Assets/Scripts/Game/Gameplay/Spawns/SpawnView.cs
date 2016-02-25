using UnityEngine;
using System.Collections.Generic;

namespace MistRidge
{
    public class SpawnView : MonoView
    {
        [SerializeField]
        private List<SpawnPointView> spawnPointViews;

        public List<SpawnPointView> SpawnPointViews
        {
            get
            {
                return spawnPointViews;
            }
        }
    }
}
