using UnityEngine;
using System;
using System.Collections.Generic;
using Zenject;

namespace MistRidge
{
    public class CameraAnchorManager : ITickable
    {
        private readonly Settings settings;
        private readonly CameraView cameraView;
        private readonly CameraAnchorView cameraAnchorView;
        private readonly CameraOriginView cameraOriginView;
        private readonly PlayerManager playerManager;

        public CameraAnchorManager(
                Settings settings,
                CameraView cameraView,
                CameraAnchorView cameraAnchorView,
                CameraOriginView cameraOriginView,
                PlayerManager playerManager)
        {
            this.settings = settings;
            this.cameraView = cameraView;
            this.cameraAnchorView = cameraAnchorView;
            this.cameraOriginView = cameraOriginView;
            this.playerManager = playerManager;
        }

        public void Tick()
        {
            if (!cameraView.IsActive)
            {
                return;
            }

            Vector3 center = CenterPoint(playerManager.PlayerPositions);

            cameraAnchorView.transform.position = Vector3.Lerp(
                cameraAnchorView.transform.position,
                center,
                settings.centeringSpeed * Time.deltaTime
            );

            Vector3 originProjection = new Vector3(
                cameraOriginView.transform.position.x,
                cameraAnchorView.transform.position.y,
                cameraOriginView.transform.position.z
            );

            Vector3 lookDirection = originProjection - center;
            if (lookDirection == Vector3.zero)
            {
                return;
            }

            Quaternion rotation = Quaternion.LookRotation(originProjection - center);

            cameraAnchorView.transform.rotation = Quaternion.Lerp(
                cameraAnchorView.transform.rotation,
                rotation,
                settings.rotationSpeed * Time.deltaTime
            );
        }

        private Vector3 CenterPoint(List<Vector3> playerPositions)
        {
            if (playerPositions.Count == 0)
            {
                return Vector3.zero;
            }
            else if (playerPositions.Count == 1)
            {
                return playerPositions[0];
            }

            Vector3 localCenter = Vector3.zero;
            foreach (Vector3 playerPosition in playerPositions)
            {
                localCenter += playerPosition;
            }

            return localCenter / playerPositions.Count;
        }

        [Serializable]
        public class Settings
        {
            public float centeringSpeed;
            public float rotationSpeed;
        }
    }
}
