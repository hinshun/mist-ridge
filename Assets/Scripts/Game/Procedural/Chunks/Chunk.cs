using UnityEngine;
using System;
using System.Collections.Generic;
using Zenject;

namespace MistRidge
{
    public class Chunk : IInitializable
    {
        private readonly DiContainer container;
        private readonly Settings settings;
        private readonly ChunkRequest chunkRequest;
        private readonly ChunkView chunkView;
        private readonly ChunkFeature chunkFeature;
        private readonly ChunkFeatureView chunkFeatureView;
        private readonly IChunkPlacingStrategy chunkPlacingStrategy;

        private List<ItemContainerView> itemContainerViews;
        private List<SpawnView> spawnViews;
        private CheckpointWallView checkpointWallView;
        private CheckpointView checkpointView;

        public Chunk(
                DiContainer container,
                Settings settings,
                ChunkRequest chunkRequest,
                ChunkView chunkView,
                ChunkFeature chunkFeature,
                ChunkFeatureView chunkFeatureView,
                IChunkPlacingStrategy chunkPlacingStrategy)
        {
            this.container = container;
            this.settings = settings;
            this.chunkRequest = chunkRequest;
            this.chunkView = chunkView;
            this.chunkFeature = chunkFeature;
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

        public List<SpawnView> SpawnViews
        {
            get
            {
                return spawnViews;
            }
        }

        public void Initialize()
        {
            chunkFeatureView.Parent = chunkView.transform;
            PlaceChunk();
            itemContainerViews = SpawnItemContainers();
            checkpointWallView = chunkView.GetComponentInChildren<CheckpointWallView>();
            checkpointView = SpawnCheckpoint();
            spawnViews = SpawnSpawns();
        }

        private void PlaceChunk()
        {
            chunkPlacingStrategy.Place(chunkView, chunkRequest, chunkFeature, chunkFeatureView);
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

        private CheckpointView SpawnCheckpoint()
        {
            CheckpointSpawnView checkpointSpawnView = chunkView.GetComponentInChildren<CheckpointSpawnView>();
            return Spawn<CheckpointSpawnView, CheckpointView>(checkpointSpawnView, settings.checkpointPrefab);
        }

        private List<SpawnView> SpawnSpawns()
        {
            List<SpawnView> spawnViews = new List<SpawnView>();
            SpawnSpawnView[] spawnSpawnViews = chunkView.GetComponentsInChildren<SpawnSpawnView>();

            foreach(SpawnSpawnView spawnSpawnView in spawnSpawnViews)
            {
                SpawnView spawnView = Spawn<SpawnSpawnView, SpawnView>(spawnSpawnView, settings.spawnPrefab);
                spawnViews.Add(spawnView);
            }

            return spawnViews;
        }

        private TComponent Spawn<TSpawner, TComponent>(TSpawner spawner, GameObject prefab)
            where TSpawner : MonoBehaviour
            where TComponent : MonoView
        {
            if (spawner == null)
            {
                return null;
            }

            TComponent component = container.InstantiatePrefab(prefab).GetComponent<TComponent>();

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
