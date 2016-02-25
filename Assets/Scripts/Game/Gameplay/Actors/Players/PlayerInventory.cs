using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class PlayerInventory : IInitializable, ITickable
    {
        private readonly Input input;
        private readonly Player player;
        private readonly PlayerView playerView;
        private readonly ItemManager itemManager;
        private readonly ItemPickupSignal itemPickupSignal;

        private Item itemHeld;

        public PlayerInventory(
                Input input,
                Player player,
                PlayerView playerView,
                ItemManager itemManager,
                ItemPickupSignal itemPickupSignal)
        {
            this.input = input;
            this.player = player;
            this.playerView = playerView;
            this.itemManager = itemManager;
            this.itemPickupSignal = itemPickupSignal;
        }

        public void Initialize()
        {
            this.playerView.CanPickupItems = true;
            itemPickupSignal.Event += OnItemPickup;
        }

        public void Tick()
        {
            if (itemHeld != null)
            {
                if (input.Mapping.UseItem.WasPressed)
                {
                    UseItem();
                }
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
            playerView.CanPickupItems = false;
            itemHeld = item;
        }

        private void UseItem()
        {
            if (itemHeld != null)
            {
                Debug.Log("Used: " + itemHeld.ItemType);
                playerView.CanPickupItems = true;
            }
        }
    }
}
