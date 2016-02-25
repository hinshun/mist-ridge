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

        private IItem item;

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
            if (item != null)
            {
                if (input.Mapping.UseItem.WasPressed)
                {
                    UseItem(item);

                    if (!item.IsUsable())
                    {
                        item.Dispose();
                        item = null;
                    }
                }
            }
        }

        private void OnItemPickup(ItemType itemType)
        {
            playerView.CanPickupItems = false;
            item = itemManager.NewItem(itemType);
        }

        private void UseItem(IItem item)
        {
            item.Use();
            playerView.CanPickupItems = true;
        }
    }
}
