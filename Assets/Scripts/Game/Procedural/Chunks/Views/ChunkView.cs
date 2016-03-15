using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class ChunkView : MonoView
    {
        private ChunkFeatureView chunkFeatureView;

        public ChunkFeatureView ChunkFeatureView
        {
            get
            {
                if (chunkFeatureView == null)
                {
                    chunkFeatureView = GetComponentInChildren<ChunkFeatureView>();
                }

                return chunkFeatureView;
            }
        }

        private void Awake()
        {
            chunkFeatureView = null;
        }
    }
}
