using UnityEngine;
using System;
using System.Collections.Generic;
using Zenject;

namespace MistRidge
{
    public class ChunkBase : IInitializable
    {
        private readonly ChunkReference chunkReference;
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
                ChunkReference chunkReference,
                ChunkView chunkView,
                ChunkBaseView chunkBaseView,
                ChunkFeatureView chunkFeatureView,
                PlatformView.Factory platformViewFactory)
        {
            this.chunkReference = chunkReference;
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
            chunkBaseView.LocalRotation = chunkView.Rotation;
            chunkBaseView.LocalScale = new Vector3(
                chunkBaseView.LocalScale.x,
                chunkBaseView.Position.y / 2,
                chunkBaseView.LocalScale.z
            );
        }

        private void SpawnPlatforms()
        {
            PlatformBaseConfig platformBaseConfig = chunkFeatureView.PlatformBaseConfig;

            SpawnPlatform(
                "Forward Left Platform",
                platformBaseConfig.spawnForwardLeft,
                chunkReference.ForwardLeft,
                out forwardLeftPlatformView
            );

            SpawnPlatform(
                "Forward Right Platform",
                platformBaseConfig.spawnForwardRight,
                chunkReference.ForwardRight,
                out forwardRightPlatformView
            );

            SpawnPlatform(
                "Left Platform",
                platformBaseConfig.spawnLeft,
                chunkReference.Left,
                out leftPlatformView
            );

            SpawnPlatform(
                "Center Platform",
                platformBaseConfig.spawnCenter,
                chunkReference.Center,
                out centerPlatformView
            );

            SpawnPlatform(
                "Right Platform",
                platformBaseConfig.spawnRight,
                chunkReference.Right,
                out rightPlatformView
            );

            SpawnPlatform(
                "Backward Left Platform",
                platformBaseConfig.spawnBackwardLeft,
                chunkReference.BackwardLeft,
                out backwardLeftPlatformView
            );

            SpawnPlatform(
                "Backward Right Platform",
                platformBaseConfig.spawnBackwardRight,
                chunkReference.BackwardRight,
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
