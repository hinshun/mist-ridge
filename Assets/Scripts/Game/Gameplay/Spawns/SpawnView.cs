using UnityEngine;
using System.Collections.Generic;

namespace MistRidge
{
    public class SpawnView : MonoView
    {
        [SerializeField]
        private List<SpawnPointView> spawnPointViews;

        public Vector3 SpawnPoint(int index)
        {
            return spawnPointViews[index].Position;
        }
    }
}
