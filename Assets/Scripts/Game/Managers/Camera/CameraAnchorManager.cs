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
        private readonly CinematicManager cinematicManager;

        public CameraAnchorManager(
                Settings settings,
                CameraView cameraView,
                CameraAnchorView cameraAnchorView,
                CameraOriginView cameraOriginView,
                CinematicManager cinematicManager)
        {
            this.settings = settings;
            this.cameraView = cameraView;
            this.cameraAnchorView = cameraAnchorView;
            this.cameraOriginView = cameraOriginView;
            this.cinematicManager = cinematicManager;
        }

        public void Tick()
        {
            if (!cameraView.IsActive)
            {
                return;
            }

            Vector3 anchorPosition = AnchorPosition();

            cameraAnchorView.Position = Vector3.Lerp(
                cameraAnchorView.Position,
                anchorPosition,
                settings.centeringSpeed * Time.deltaTime
            );

            cameraAnchorView.Rotation = Quaternion.Lerp(
                cameraAnchorView.Rotation,
                AnchorRotation(anchorPosition),
                settings.rotationSpeed * Time.deltaTime
            );
        }

        public void ResetAnchor()
        {
            Vector3 anchorPosition = AnchorPosition();
            cameraAnchorView.Position = anchorPosition;
            cameraAnchorView.Rotation = AnchorRotation(anchorPosition);
        }

        private Vector3 AnchorPosition()
        {
            return CenterPoint(cinematicManager.Positions);
        }

        private Quaternion AnchorRotation(Vector3 anchorPosition)
        {
            Vector3 originProjection = new Vector3(
                cameraOriginView.Position.x,
                cameraAnchorView.Position.y,
                cameraOriginView.Position.z
            );

            Vector3 lookDirection = originProjection - anchorPosition;
            if (lookDirection == Vector3.zero)
            {
                return Quaternion.identity;
            }

            return Quaternion.LookRotation(originProjection - anchorPosition);
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
