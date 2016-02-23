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

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                PlayerView playerView = other.GetComponent<PlayerView>();
                playerView.OnItemPickup(itemType);
            }
        }
    }
}
