using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class ItemContainerView : MonoView
    {
        [SerializeField]
        private ItemType itemType;

        [SerializeField]
        private Collider pickupCollider;

        [SerializeField]
        private float spawnDelay = 5f;

        [SerializeField]
        private MeshRenderer meshRenderer;

        private float spawnTimer;
        private bool pickable;

        private void Awake()
        {
            spawnTimer = 0;
            DisablePickable();
        }

        private void Update()
        {
            if (!pickable)
            {
                if (spawnTimer >= spawnDelay)
                {
                    spawnTimer = 0;
                    EnablePickable();
                }
                else
                {
                    spawnTimer += Time.deltaTime;
                }
            }
        }

        private void EnablePickable()
        {
            pickable = true;
            meshRenderer.enabled = pickable;
            pickupCollider.enabled = pickable;
        }

        private void DisablePickable()
        {
            pickable = false;
            meshRenderer.enabled = pickable;
            pickupCollider.enabled = pickable;
        }

        private void OnTriggerStay(Collider other)
        {
            if (pickable && other.CompareTag("Player"))
            {
                PlayerView playerView = other.GetComponent<PlayerView>();
                if (!playerView.CanPickupItems)
                {
                    return;
                }

                DisablePickable();
                playerView.OnItemPickup(itemType);
            }
        }
    }
}
