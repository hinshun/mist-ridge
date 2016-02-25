using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class PlayerInventory : IInitializable, ITickable
    {
        private readonly Player player;
        private readonly ItemManager itemManager;
        private readonly ItemPickupSignal itemPickupSignal;

        private Item itemHeld;
        private bool itemUsed;

        public PlayerInventory(
                Player player,
                ItemManager itemManager,
                ItemPickupSignal itemPickupSignal)
        {
            this.player = player;
            this.itemManager = itemManager;
            this.itemPickupSignal = itemPickupSignal;
        }

        public void Initialize()
        {
            itemUsed = false;
            itemPickupSignal.Event += OnItemPickup;
        }

        public void Tick()
        {
            if (itemHeld != null && itemUsed)
            {
                itemHeld.Tick();
            }
        }

        private void OnItemPickup(ItemType itemType)
        {
            Item item = itemManager.GetItem(itemType);
            Debug.Log("Picked up: " + item.ItemType);
            HoldItem(item);
        }

        private void HoldItem(Item item)
        {
            itemUsed = false;
            itemHeld = item;
        }

        private void UseItem()
        {
            if (itemHeld != null)
            {
                itemHeld.Use(player);
                itemUsed = true;
            }
        }
    }
}
