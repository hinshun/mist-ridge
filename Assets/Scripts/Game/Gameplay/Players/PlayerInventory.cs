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
        private readonly DisplayManager displayManager;
        private readonly ItemPickupSignal itemPickupSignal;

        private IItem item;

        public PlayerInventory(
                Input input,
                Player player,
                PlayerView playerView,
                ItemManager itemManager,
                DisplayManager displayManager,
                ItemPickupSignal itemPickupSignal)
        {
            this.input = input;
            this.player = player;
            this.playerView = playerView;
            this.itemManager = itemManager;
            this.displayManager = displayManager;
            this.itemPickupSignal = itemPickupSignal;
        }

        public void Initialize()
        {
            this.playerView.CanPickupItems = true;
            itemPickupSignal.Event += OnItemPickup;
        }

        public void Dispose()
        {
            itemPickupSignal.Event -= OnItemPickup;
        }

        public void Tick()
        {
            if (item != null)
            {
                if (item.IsUsable())
                {
                    if (input.Mapping.UseItem.WasPressed)
                    {
                        item.Use();
                    }
                }
                else
                {
                    item = null;
                    playerView.CanPickupItems = true;
                    displayManager.UpdateItem(input.DeviceNum, null);
                    return;
                }

            }
        }

        private void OnItemPickup(ItemType itemType)
        {
            playerView.CanPickupItems = false;

            ItemDrop itemDrop = itemManager.PickItemDrop(itemType, player);
            displayManager.UpdateItem(input.DeviceNum, itemDrop);

            item = itemManager.NewItem(itemDrop, player);
        }
    }
}
