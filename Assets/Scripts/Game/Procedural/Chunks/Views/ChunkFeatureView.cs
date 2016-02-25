using UnityEngine;
using System;
using System.Collections.Generic;
using Zenject;

namespace MistRidge
{
    public class ChunkFeatureView : ChunkChildView
    {
        [SerializeField]
        private ChunkReference chunkReference;

        [SerializeField]
        private PlatformBaseConfig platformBaseConfig = new PlatformBaseConfig()
        {
            spawnForwardLeft = true,
            spawnForwardRight = true,
            spawnLeft = true,
            spawnCenter = true,
            spawnRight = true,
            spawnBackwardLeft = true,
            spawnBackwardRight = true,
        };

        public PlatformBaseConfig PlatformBaseConfig
        {
            get
            {
                return platformBaseConfig;
            }
        }

        public void OnDrawGizmosSelected()
        {
            DrawForwardDirectionGizmos();
            DrawSpawnablePlatformBaseGizmos();
        }

        private void DrawForwardDirectionGizmos()
        {
            Gizmos.color = Color.yellow - new Color(0, 0, 0, 0.4f);

            Vector3 arrowHeight = Vector3.up * (chunkReference.LocalScale.x + chunkReference.LocalScale.z);

            Vector3 backward = arrowHeight + ((chunkReference.BackwardLeft + chunkReference.BackwardRight) / 2);
            Vector3 forward = arrowHeight + ((chunkReference.ForwardLeft + chunkReference.ForwardRight) / 2);

            Gizmos.DrawLine(
                transform.position + (transform.rotation * backward),
                transform.position + (transform.rotation * forward)
            );

            Gizmos.DrawLine(
                transform.position + (transform.rotation * forward),
                transform.position + (transform.rotation * (arrowHeight + chunkReference.Left))
            );

            Gizmos.DrawLine(
                transform.position + (transform.rotation * forward),
                transform.position + (transform.rotation * (arrowHeight + chunkReference.Right))
            );
        }

        private void DrawSpawnablePlatformBaseGizmos()
        {
            Gizmos.color = Color.cyan - new Color(0, 0, 0, 0.8f);

            if (chunkReference == null)
            {
                return;
            }

            DrawPlatformBaseGizmos(
                platformBaseConfig.spawnForwardLeft,
                chunkReference.ForwardLeft
            );

            DrawPlatformBaseGizmos(
                platformBaseConfig.spawnForwardRight,
                chunkReference.ForwardRight
            );

            DrawPlatformBaseGizmos(
                platformBaseConfig.spawnLeft,
                chunkReference.Left
            );

            DrawPlatformBaseGizmos(
                platformBaseConfig.spawnCenter,
                chunkReference.Center
            );

            DrawPlatformBaseGizmos(
                platformBaseConfig.spawnRight,
                chunkReference.Right
            );

            DrawPlatformBaseGizmos(
                platformBaseConfig.spawnBackwardLeft,
                chunkReference.BackwardLeft
            );

            DrawPlatformBaseGizmos(
                platformBaseConfig.spawnBackwardRight,
                chunkReference.BackwardRight
            );
        }

        private void DrawPlatformBaseGizmos(bool isSpawnable, Vector3 scaledPosition)
        {
            if (isSpawnable)
            {
                Gizmos.DrawWireMesh(
                    chunkReference.PlatformBaseMesh,
                    Position + (Rotation * (scaledPosition + (Down * chunkReference.LocalScale.y))),
                    Rotation,
                    chunkReference.LocalScale
                );
            }
        }
    }
}
