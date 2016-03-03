using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class ChunkFacade : Facade
    {
        private readonly ChunkView chunkView;

        public ChunkFacade(ChunkView chunkView)
        {
            this.chunkView = chunkView;
        }

        public Transform Parent
        {
            get
            {
                return chunkView.Parent;
            }
            set
            {
                chunkView.Parent = value;
            }
        }

        public string Name
        {
            get
            {
                return chunkView.Name;
            }
            set
            {
                chunkView.Name = value;
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
