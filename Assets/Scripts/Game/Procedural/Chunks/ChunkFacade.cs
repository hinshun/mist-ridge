using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class ChunkFacade : Facade
    {
        private readonly ChunkView chunkView;
        private readonly ChunkFeatureView chunkFeatureView;

        public ChunkFacade(
                ChunkView chunkView,
                ChunkFeatureView chunkFeatureView)
        {
            this.chunkView = chunkView;
            this.chunkFeatureView = chunkFeatureView;
        }

        public ChunkView ChunkView
        {
            get
            {
                return chunkView;
            }
        }

        public ChunkFeatureView ChunkFeatureView
        {
            get
            {
                return chunkFeatureView;
            }
        }

        public Vector3 Position
        {
            get
            {
                return chunkView.Position;
            }
        }
    }
}
