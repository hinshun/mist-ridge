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
            Spawn<ItemContainerSpawnView, ItemContainerView>(settings.itemContainerPrefab);
            Spawn<CheckpointSpawnView, CheckpointView>(settings.checkpointPrefab);
            Spawn<SpawnSpawnView, SpawnView>(settings.spawnPrefab);
        }

        private void PlaceChunk()
        {
            chunkPlacingStrategy.Place(chunkView, chunkRequest);
        }

        private void Spawn<TSpawner, TComponent>(GameObject prefab)
            where TSpawner : MonoBehaviour
            where TComponent : MonoView
        {
            TSpawner[] spawners = chunkView.GetComponentsInChildren<TSpawner>();
            foreach(TSpawner spawner in spawners)
            {
                TComponent component = GameObject.Instantiate(prefab).GetComponent<TComponent>();
                component.Parent = spawner.transform;
                component.LocalPosition = Vector3.zero;
            }
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
