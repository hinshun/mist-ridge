using UnityEngine;
using UnityStandardAssets.ImageEffects;
using System;
using Zenject;

namespace MistRidge
{
    public class MistManager : IInitializable, ITickable
    {
        private readonly Settings settings;
        private readonly Camera camera;
        private readonly MistView mistView;
        private readonly MistContainerView mistContainerView;

        private bool isActive;
        private Vector3 targetPosition;

        public MistManager(
                Settings settings,
                Camera camera,
                MistView mistView,
                MistContainerView mistContainerView)
        {
            this.settings = settings;
            this.camera = camera;
            this.mistView = mistView;
            this.mistContainerView = mistContainerView;
        }

        public bool IsActive
        {
            get
            {
                return isActive;
            }
            set
            {
                isActive = value;
                mistView.Collider.enabled = value;
            }
        }

        public void Initialize()
        {
            IsActive = false;
            targetPosition = mistContainerView.Position;
        }

        public void Tick()
        {
            if (!isActive)
            {
                return;
            }

            mistContainerView.Position = Vector3.Lerp(
                mistContainerView.Position,
                targetPosition - (Vector3.up * settings.mistOffset),
                settings.mistSpeed * Time.deltaTime
            );

            GlobalFog globalFog = camera.GetComponent<GlobalFog>();
            globalFog.height = Mathf.Lerp(
                globalFog.height,
                mistContainerView.Position.y + settings.cameraFogOffset,
                settings.mistSpeed * Time.deltaTime
            );

            mistContainerView.LocalScale = new Vector3(
                mistContainerView.LocalScale.x,
                mistContainerView.Position.y,
                mistContainerView.LocalScale.z
            );
        }

        public void UpdateMistPosition(float altitude)
        {
            targetPosition = new Vector3(0, altitude, 0);
        }

        [Serializable]
        public class Settings
        {
            public GameObject mistContainerPrefab;
            public float mistSpeed;
            public float mistOffset;
            public float cameraFogOffset;
        }
    }
}
