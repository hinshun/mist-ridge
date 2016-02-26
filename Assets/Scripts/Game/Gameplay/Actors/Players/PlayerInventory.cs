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
                if (input.Mapping.UseItem.WasPressed && item.IsUsable())
                {
                    item.Use();
                }
            }
        }

        private void OnItemPickup(ItemType itemType)
        {
            playerView.CanPickupItems = false;
            item = itemManager.NewItem(itemType, player);
        }
    }
}
