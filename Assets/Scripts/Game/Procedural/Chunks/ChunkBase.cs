using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Zenject;

namespace MistRidge
{
    public class ChunkBase : IInitializable
    {
        private readonly ChunkView chunkView;
        private readonly ChunkBaseView chunkBaseView;
        private readonly ChunkFeatureView chunkFeatureView;
        private readonly PlatformView.Factory platformViewFactory;

        private ReadOnlyCollection<PlatformView> platformViews;

        public ChunkBase(
                ChunkView chunkView,
                ChunkBaseView chunkBaseView,
                ChunkFeatureView chunkFeatureView,
                PlatformView.Factory platformViewFactory)
        {
            this.chunkView = chunkView;
            this.chunkBaseView = chunkBaseView;
            this.chunkFeatureView = chunkFeatureView;
            this.platformViewFactory = platformViewFactory;
        }

        public void Initialize()
        {
            chunkBaseView.Parent = chunkView.transform;
            SpawnPlatforms();
            chunkBaseView.LocalPosition = Vector3.zero;
        }

        private void SpawnPlatforms()
        {
            PlatformBaseConfig platformBaseConfig = chunkFeatureView.PlatformBaseConfig;
            List<PlatformView> spawnedPlatformViews = new List<PlatformView>();

            SpawnPlatform(
                spawnedPlatformViews,
                platformBaseConfig.spawnCenter,
                Vector2.zero
            );

            SpawnPlatform(
                spawnedPlatformViews,
                platformBaseConfig.spawnTop,
                new Vector3(
                    0,
                    -2f
                )
            );

            SpawnPlatform(
                spawnedPlatformViews,
                platformBaseConfig.spawnTopLeft,
                new Vector3(
                    1.5f,
                    -1f
                )
            );

            SpawnPlatform(
                spawnedPlatformViews,
                platformBaseConfig.spawnTopRight,
                new Vector3(
                    -1.5f,
                    -1f
                )
            );

            SpawnPlatform(
                spawnedPlatformViews,
                platformBaseConfig.spawnBottom,
                new Vector3(
                    0,
                    2f
                )
            );

            SpawnPlatform(
                spawnedPlatformViews,
                platformBaseConfig.spawnBottomLeft,
                new Vector3(
                    1.5f,
                    1f
                )
            );

            SpawnPlatform(
                spawnedPlatformViews,
                platformBaseConfig.spawnBottomRight,
                new Vector3(
                    -1.5f,
                    1f
                )
            );

            platformViews = new ReadOnlyCollection<PlatformView>(spawnedPlatformViews);
        }

        private void SpawnPlatform(List<PlatformView> spawnedPlatformViews, bool spawn, Vector2 position)
        {
            if (spawn)
            {
                PlatformView platformView = platformViewFactory.Create();
                platformView.Position = new Vector3(
                    position.x * platformView.LocalScale.x,
                    0,
                    position.y * platformView.LocalScale.z * Mathf.Sqrt(3) / 2
                );
                platformView.Parent = chunkBaseView.transform;

                spawnedPlatformViews.Add(platformView);
            }
        }
    }
}
