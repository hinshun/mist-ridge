using UnityEngine;
using System;
using System.Collections.Generic;
using Zenject;

namespace MistRidge
{
    public class Chunk : IInitializable
    {
        private readonly Settings settings;
        private readonly ChunkRequest chunkRequest;
        private readonly ChunkView chunkView;
        private readonly ChunkFeatureView chunkFeatureView;
        private readonly IChunkPlacingStrategy chunkPlacingStrategy;

        public Chunk(
                Settings settings,
                ChunkRequest chunkRequest,
                ChunkView chunkView,
                ChunkFeatureView chunkFeatureView,
                IChunkPlacingStrategy chunkPlacingStrategy)
        {
            this.settings = settings;
            this.chunkRequest = chunkRequest;
            this.chunkView = chunkView;
            this.chunkFeatureView = chunkFeatureView;
            this.chunkPlacingStrategy = chunkPlacingStrategy;
        }

        public void Initialize()
        {
            chunkFeatureView.Parent = chunkView.transform;
            PlaceChunk();
            SpawnItemContainers();
            SpawnCheckpoints();
        }

        private void PlaceChunk()
        {
            chunkPlacingStrategy.Placement(chunkView, chunkRequest);
        }

        private void SpawnItemContainers()
        {
            ItemContainerSpawnView[] itemContainerSpawnViews = chunkView.GetComponentsInChildren<ItemContainerSpawnView>();
            foreach(ItemContainerSpawnView itemContainerSpawnView in itemContainerSpawnViews)
            {
                ItemContainerView itemContainerView = GameObject.Instantiate(settings.itemContainerPrefab).GetComponent<ItemContainerView>();
                itemContainerView.Parent = itemContainerSpawnView.transform;
                itemContainerView.LocalPosition = Vector3.zero;
            }
        }

        private void SpawnCheckpoints()
        {
            CheckpointSpawnView[] checkpointSpawnViews = chunkView.GetComponentsInChildren<CheckpointSpawnView>();
            foreach(CheckpointSpawnView checkpointSpawnView in checkpointSpawnViews)
            {
                CheckpointView checkpointView = GameObject.Instantiate(settings.checkpointPrefab).GetComponent<CheckpointView>();
                checkpointView.Parent = checkpointSpawnView.transform;
                checkpointView.LocalPosition = Vector3.zero;
            }
        }

        [Serializable]
        public class Settings
        {
            public GameObject itemContainerPrefab;
            public GameObject checkpointPrefab;
        }
    }
}
