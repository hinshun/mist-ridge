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

        private List<ItemContainerView> itemContainerViews;
        private CheckpointWallView checkpointWallView;
        private CheckpointView checkpointView;
        private SpawnView spawnView;

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

        public CheckpointWallView CheckpointWallView
        {
            get
            {
                return checkpointWallView;
            }
        }

        public CheckpointView CheckpointView
        {
            get
            {
                return checkpointView;
            }
        }

        public SpawnView SpawnView
        {
            get
            {
                return spawnView;
            }
        }

        public void Initialize()
        {
            chunkFeatureView.Parent = chunkView.transform;
            PlaceChunk();
            itemContainerViews = SpawnItemContainers();
            checkpointWallView = chunkView.GetComponentInChildren<CheckpointWallView>();
            checkpointView = Spawn<CheckpointSpawnView, CheckpointView>(settings.checkpointPrefab);
            spawnView = Spawn<SpawnSpawnView, SpawnView>(settings.spawnPrefab);
        }

        private void PlaceChunk()
        {
            chunkPlacingStrategy.Place(chunkView, chunkRequest, chunkFeatureView);
        }

        private List<ItemContainerView> SpawnItemContainers()
        {
            List<ItemContainerView> itemContainerViews = new List<ItemContainerView>();
            ItemContainerSpawnView[] itemContainerSpawnViews = chunkView.GetComponentsInChildren<ItemContainerSpawnView>();

            foreach(ItemContainerSpawnView itemContainerSpawnView in itemContainerSpawnViews)
            {
                ItemContainerView itemContainerView = GameObject.Instantiate(settings.itemContainerPrefab).GetComponent<ItemContainerView>();

                itemContainerView.Parent = itemContainerSpawnView.transform;
                itemContainerView.LocalPosition = Vector3.zero;

                itemContainerViews.Add(itemContainerView);
            }

            return itemContainerViews;
        }

        private TComponent Spawn<TSpawner, TComponent>(GameObject prefab)
            where TSpawner : MonoBehaviour
            where TComponent : MonoView
        {
            TSpawner spawner = chunkView.GetComponentInChildren<TSpawner>();
            if (spawner == null)
            {
                return null;
            }

            TComponent component = GameObject.Instantiate(prefab).GetComponent<TComponent>();

            component.Parent = spawner.transform;
            component.LocalPosition = Vector3.zero;

            return component;
        }

        [Serializable]
        public class Settings
        {
            public GameObject itemContainerPrefab;
            public GameObject checkpointPrefab;
            public GameObject spawnPrefab;
        }
    }
}
