using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class ChunkFacade : Facade
    {
        private readonly Chunk chunk;
        private readonly ChunkView chunkView;

        public ChunkFacade(
                Chunk chunk,
                ChunkView chunkView)
        {
            this.chunk = chunk;
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
            set
            {
                chunkView.Position = value;
            }
        }

        public Quaternion Rotation
        {
            get
            {
                return chunkView.Rotation;
            }
            set
            {
                chunkView.Rotation = value;
            }
        }

        public CheckpointWallView CheckpointWallView
        {
            get
            {
                return chunk.CheckpointWallView;
            }
        }

        public CheckpointView CheckpointView
        {
            get
            {
                return chunk.CheckpointView;
            }
        }

        public ChunkView ChunkView
        {
            get
            {
                return chunkView;
            }
        }

        public SpawnView SpawnView
        {
            get
            {
                return chunk.SpawnViews[0];
            }
        }
    }
}
