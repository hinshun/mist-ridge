using UnityEngine;
using System;
using System.Collections.Generic;
using Zenject;

namespace MistRidge
{
    public class CameraManager : IInitializable, ITickable
    {
        private readonly Settings settings;
        private readonly Camera camera;
        private readonly CameraView cameraView;
        private readonly CameraRigView cameraRigView;
        private readonly DeathManager deathManager;

        private Camera currentCamera;

        public CameraManager(
                Settings settings,
                Camera camera,
                CameraView cameraView,
                CameraRigView cameraRigView,
                DeathManager deathManager)
        {
            this.settings = settings;
            this.camera = camera;
            this.cameraView = cameraView;
            this.cameraRigView = cameraRigView;
            this.deathManager = deathManager;
        }

        public Camera CurrentCamera
        {
            get
            {
                return currentCamera;
            }
            set
            {
                currentCamera = value;
            }
        }

        public void Initialize()
        {
            CurrentCamera = camera;
        }

        public void Tick()
        {
            if (!cameraView.IsActive)
            {
                return;
            }

            cameraView.LocalPosition = Vector3.Lerp(
                cameraView.LocalPosition,
                ZoomPosition(),
                settings.zoomSpeed * Time.deltaTime
            );
        }

        public void ResetCamera()
        {
            CurrentCamera = camera;
            cameraView.LocalPosition = ZoomPosition();
        }

        private Vector3 ZoomPosition()
        {
            float zoom = CameraZoomForEncapsulation(deathManager.AliveRelevantPlayerGroundingPositions);
            float cappedZoom = Mathf.Max(zoom, settings.minZoom);

            return new Vector3(
                cameraView.LocalPosition.x,
                cameraView.LocalPosition.y,
                -cappedZoom
            );
        }

        private float CameraZoomForEncapsulation(List<Vector3> playerPositions)
        {
            if (playerPositions.Count == 0)
            {
                return 0f;
            }

            float zoom, xMax, yMax;
            zoom = xMax = yMax = 0f;
            foreach (Vector3 playerPosition in playerPositions)
            {
                Vector3 relativePosition = cameraRigView.transform.InverseTransformPoint(playerPosition);

                float xBound = Mathf.Abs(relativePosition.x);
                float yBound = Mathf.Abs(relativePosition.y);

                if (relativePosition.z < 0)
                {
                    xBound -= relativePosition.z * settings.horizontalPercentage * cameraView.HorizontalTanFov;
                    yBound -= relativePosition.z * settings.verticalPercentage * cameraView.VerticalTanFov;
                }

                xBound /= settings.horizontalPercentage * cameraView.HorizontalTanFov;
                yBound /= settings.verticalPercentage * cameraView.VerticalTanFov;

                if (settings.Debug.showBounds)
                {
                    xMax = Mathf.Max(xMax, xBound);
                    yMax = Mathf.Max(yMax, yBound);
                }

                zoom = Mathf.Max(zoom, Mathf.Max(xBound, yBound));
            }

            if (settings.Debug.showBounds)
            {
                Vector3 xBound = cameraRigView.transform.TransformPoint(new Vector3(xMax, 0f, 0f));
                Vector3 yBound = cameraRigView.transform.TransformPoint(new Vector3(0f, yMax, 0f));

                Debug.DrawLine(cameraRigView.transform.position, cameraView.transform.position);
                Debug.DrawLine(cameraView.transform.position, xBound);
                Debug.DrawLine(cameraRigView.transform.position, xBound);
                Debug.DrawLine(cameraView.transform.position, yBound);
                Debug.DrawLine(cameraRigView.transform.position, yBound);
            }

            return zoom;
        }

        [Serializable]
        public class Settings
        {
            public DebugSettings Debug;
            public float zoomSpeed;
            public float minZoom;
            public float verticalPercentage;
            public float horizontalPercentage;

            [Serializable]
            public class DebugSettings
            {
                public bool showBounds;
            }
        }
    }
}
