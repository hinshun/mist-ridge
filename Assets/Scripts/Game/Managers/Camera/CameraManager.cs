using UnityEngine;
using System;
using System.Collections.Generic;
using Zenject;

namespace MistRidge
{
    public class CameraManager : ITickable
    {
        private readonly Settings settings;
        private readonly CameraView cameraView;
        private readonly CameraRigView cameraRigView;
        private readonly PlayerManager playerManager;

        public CameraManager(
                Settings settings,
                CameraView cameraView,
                CameraRigView cameraRigView,
                PlayerManager playerManager)
        {
            this.settings = settings;
            this.cameraView = cameraView;
            this.cameraRigView = cameraRigView;
            this.playerManager = playerManager;
        }

        public void Tick()
        {
            float zoom = CameraZoomForEncapsulation(playerManager.PlayerViews);
            float cappedZoom = Mathf.Max(zoom, settings.minZoom);
            cameraView.transform.localPosition = new Vector3(0f, 0f, -cappedZoom);
        }

        private float CameraZoomForEncapsulation(List<PlayerView> playerViews)
        {
            if (playerViews.Count == 0)
            {
                return 0f;
            }

            float zoom, xMax, yMax;
            zoom = xMax = yMax = 0f;
            foreach (PlayerView playerView in playerViews)
            {
                Vector3 relativePosition = cameraRigView.transform.InverseTransformPoint(playerView.transform.position);

                float xBound = Mathf.Abs(relativePosition.x);
                float yBound = Mathf.Abs(relativePosition.y);

                if (relativePosition.z < 0)
                {
                    xBound -= relativePosition.z * cameraView.HorizontalTanFov;
                    yBound -= relativePosition.z * cameraView.VerticalTanFov;
                }

                xBound /= cameraView.HorizontalTanFov;
                yBound /= cameraView.VerticalTanFov;

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
            public float minZoom;

            [Serializable]
            public class DebugSettings
            {
                public bool showBounds;
            }
        }
    }
}
