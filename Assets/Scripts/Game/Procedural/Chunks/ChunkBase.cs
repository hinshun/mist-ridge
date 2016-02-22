using UnityEngine;
using System;
using System.Collections.Generic;
using Zenject;

namespace MistRidge
{
    public class ChunkBase : IInitializable
    {
        private readonly ChunkManager chunkManager;
        private readonly ChunkView chunkView;
        private readonly ChunkBaseView chunkBaseView;
        private readonly ChunkFeatureView chunkFeatureView;
        private readonly PlatformView.Factory platformViewFactory;

        private PlatformView forwardLeftPlatformView;
        private PlatformView forwardRightPlatformView;
        private PlatformView leftPlatformView;
        private PlatformView centerPlatformView;
        private PlatformView rightPlatformView;
        private PlatformView backwardLeftPlatformView;
        private PlatformView backwardRightPlatformView;

        public ChunkBase(
                ChunkManager chunkManager,
                ChunkView chunkView,
                ChunkBaseView chunkBaseView,
                ChunkFeatureView chunkFeatureView,
                PlatformView.Factory platformViewFactory)
        {
            this.chunkManager = chunkManager;
            this.chunkView = chunkView;
            this.chunkBaseView = chunkBaseView;
            this.chunkFeatureView = chunkFeatureView;
            this.platformViewFactory = platformViewFactory;
        }

        public PlatformView ForwardLeftPlatformView
        {
            get
            {
                return forwardLeftPlatformView;
            }
        }

        public PlatformView ForwardRightPlatformView
        {
            get
            {
                return forwardRightPlatformView;
            }
        }

        public PlatformView LeftPlatformView
        {
            get
            {
                return leftPlatformView;
            }
        }

        public PlatformView CenterPlatformView
        {
            get
            {
                return centerPlatformView;
            }
        }

        public PlatformView RightPlatformView
        {
            get
            {
                return rightPlatformView;
            }
        }

        public PlatformView BackwardLeftPlatformView
        {
            get
            {
                return backwardLeftPlatformView;
            }
        }

        public PlatformView BackwardRightPlatformView
        {
            get
            {
                return backwardRightPlatformView;
            }
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

            SpawnPlatform(
                "Forward Left Platform",
                platformBaseConfig.spawnForwardLeft,
                chunkManager.ChunkReference.ForwardLeft,
                out forwardLeftPlatformView
            );

            SpawnPlatform(
                "Forward Right Platform",
                platformBaseConfig.spawnForwardRight,
                chunkManager.ChunkReference.ForwardRight,
                out forwardRightPlatformView
            );

            SpawnPlatform(
                "Left Platform",
                platformBaseConfig.spawnLeft,
                chunkManager.ChunkReference.Left,
                out leftPlatformView
            );

            SpawnPlatform(
                "Center Platform",
                platformBaseConfig.spawnCenter,
                chunkManager.ChunkReference.Center,
                out centerPlatformView
            );

            SpawnPlatform(
                "Right Platform",
                platformBaseConfig.spawnRight,
                chunkManager.ChunkReference.Right,
                out rightPlatformView
            );

            SpawnPlatform(
                "Backward Left Platform",
                platformBaseConfig.spawnBackwardLeft,
                chunkManager.ChunkReference.BackwardLeft,
                out backwardLeftPlatformView
            );

            SpawnPlatform(
                "Backward Right Platform",
                platformBaseConfig.spawnBackwardRight,
                chunkManager.ChunkReference.BackwardRight,
                out backwardRightPlatformView
            );
        }

        private void SpawnPlatform(string name, bool isSpawnable, Vector3 scaledPosition, out PlatformView platformView)
        {
            if (isSpawnable)
            {
                platformView = platformViewFactory.Create();
                platformView.name = name;
                platformView.Position = scaledPosition;
                platformView.Parent = chunkBaseView.transform;
            }
            else
            {
                platformView = null;
            }
        }
    }
}
