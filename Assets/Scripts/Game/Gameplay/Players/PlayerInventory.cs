using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class PlayerInventory : IInitializable, ITickable
    {
        private readonly Settings settings;
        private readonly Input input;
        private readonly Player player;
        private readonly PlayerView playerView;
        private readonly ItemManager itemManager;
        private readonly DisplayManager displayManager;
        private readonly ItemPickupSignal itemPickupSignal;

        private IItem item;
        private ItemType itemType;
        private ParticleSystem itemParticleSystem;
        private ParticleSystemRenderer itemRenderer;

        public PlayerInventory(
                Settings settings,
                Input input,
                Player player,
                PlayerView playerView,
                ItemManager itemManager,
                DisplayManager displayManager,
                ItemPickupSignal itemPickupSignal)
        {
            this.settings = settings;
            this.input = input;
            this.player = player;
            this.playerView = playerView;
            this.itemManager = itemManager;
            this.displayManager = displayManager;
            this.itemPickupSignal = itemPickupSignal;
        }

        public void Initialize()
        {
            itemPickupSignal.Event += OnItemPickup;
            this.playerView.CanPickupItems = true;
            itemParticleSystem = playerView.ItemUse;
            itemRenderer = itemParticleSystem.GetComponent<ParticleSystemRenderer>();
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
                    if (playerView.CanUseItems && input.Mapping.UseItem.WasPressed)
                    {
                        SpawnParticle();
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

        private void SpawnParticle()
        {
            switch (itemType)
            {
                case ItemType.Aether:
                    itemRenderer.material = settings.aetherItem;
                    break;

                case ItemType.Quickness:
                    itemRenderer.material = settings.quicknessItem;
                    break;

                case ItemType.BubbleTrap:
                    itemRenderer.material = settings.bubbleTrapItem;
                    break;
            }

            itemParticleSystem.Emit(1);
        }

        private void OnItemPickup(ItemType itemType)
        {
            this.itemType = itemType;
            playerView.CanPickupItems = false;

            ItemDrop itemDrop = itemManager.PickItemDrop(itemType, player);
            displayManager.UpdateItem(input.DeviceNum, itemDrop);

            item = itemManager.NewItem(itemDrop, player);
        }

        [Serializable]
        public class Settings
        {
            public Material aetherItem;
            public Material quicknessItem;
            public Material bubbleTrapItem;
        }
    }
}
