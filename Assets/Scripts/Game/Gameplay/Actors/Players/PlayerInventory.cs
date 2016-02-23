using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class PlayerInventory : IInitializable
    {
        private ItemManager itemManager;
        private ItemPickupSignal itemPickupSignal;

        private Item itemHeld;

        public PlayerInventory(
                ItemManager itemManager,
                ItemPickupSignal itemPickupSignal)
        {
            this.itemManager = itemManager;
            this.itemPickupSignal = itemPickupSignal;
        }

        public void Initialize()
        {
            itemPickupSignal.Event += OnItemPickup;
        }

        private void OnItemPickup(ItemType itemType)
        {
            Item item = itemManager.GetItem(itemType);
            Debug.Log("Picked up: " + item.Name);
        }
    }
}
