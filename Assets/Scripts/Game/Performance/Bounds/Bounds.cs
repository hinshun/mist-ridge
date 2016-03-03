using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class Bounds : IInitializable
    {
        private readonly Settings settings;
        private readonly CollisionSignal collisionSignal;

        public Bounds(
                Settings settings,
                CollisionSignal collisionSignal)
        {
            this.settings = settings;
            this.collisionSignal = collisionSignal;
        }

        public void Initialize()
        {
            collisionSignal.Event += OnCollision;
        }

        private void OnCollision(CollisionType collisionType, Collider other)
        {
            if (!settings.enabled)
            {
                return;
            }

            switch (collisionType)
            {
                case CollisionType.TriggerEnter:
                    OnTriggerEnter(other);
                    break;

                case CollisionType.TriggerExit:
                    OnTriggerExit(other);
                    break;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            SetComponents(other, true);
        }

        private void OnTriggerExit(Collider other)
        {
            SetComponents(other, false);
        }

        private void SetComponents(Collider other, bool active)
        {
            ChunkFeatureView chunkFeatureView = other.GetComponent<ChunkFeatureView>();

            if (chunkFeatureView != null)
            {
                foreach(MeshRenderer meshRenderer in chunkFeatureView.GetComponentsInChildren<MeshRenderer>())
                {
                    meshRenderer.enabled = active;
                }
            }
        }

        [Serializable]
        public class Settings
        {
            public bool enabled;
        }
    }
}
