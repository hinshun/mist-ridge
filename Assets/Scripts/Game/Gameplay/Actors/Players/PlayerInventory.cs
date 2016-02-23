using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class PlayerInventory : IInitializable
    {
        private ItemPickupSignal itemPickupSignal;

        private Item itemHeld;

        public PlayerInventory(ItemPickupSignal itemPickupSignal)
        {
            this.itemPickupSignal = itemPickupSignal;
        }

        public void Initialize()
        {
            itemPickupSignal.Event += OnItemPickup;
        }

        private void OnItemPickup(ItemType itemType)
        {
            Debug.Log("Picked up: " + itemType);
        }
    }
}
