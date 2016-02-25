using UnityEngine;
using System;
using System.Collections.Generic;
using Zenject;

namespace MistRidge
{
    public class ItemManager : IInitializable
    {
        private readonly Settings settings;
        private readonly IItemDropPickingStrategy itemDropPickingStrategy;
        private readonly IItemFactory itemFactory;

        private Dictionary<ItemType, ItemDrop> itemDropMapping;

        public ItemManager(
                Settings settings,
                IItemDropPickingStrategy itemDropPickingStrategy,
                IItemFactory itemFactory)
        {
            this.settings = settings;
            this.itemDropPickingStrategy = itemDropPickingStrategy;
            this.itemFactory = itemFactory;
        }

        public void Initialize()
        {
            itemDropMapping = new Dictionary<ItemType, ItemDrop>();

            foreach(ItemDrop itemDrop in settings.itemDroplist.ItemDrops)
            {
                if (itemDrop.ItemType == ItemType.Random)
                {
                    Debug.LogError("Item drop was not added because it is of type Random.");
                    continue;
                }

                if (!itemDropMapping.ContainsKey(itemDrop.ItemType))
                {
                    itemDropMapping[itemDrop.ItemType] = itemDrop;
                }
                else
                {
                    Debug.LogError("Item drop " + itemDrop.ItemType + " was not added because it has a duplicated type.");
                    continue;
                }
            }
        }

        public IItem NewItem(ItemType itemType)
        {
            ItemDrop itemDrop = PickItemDrop(itemType);
            Debug.Log("Picked up: " + itemDrop.ItemType);
            return itemFactory.Create(itemDrop.ItemType);
        }

        private ItemDrop PickItemDrop(ItemType itemType)
        {
            if (itemType == ItemType.Random)
            {
                return itemDropPickingStrategy.Pick(itemDropMapping);
            }

            return itemDropMapping[itemType];
        }

        [Serializable]
        public class Settings
        {
            public ItemDroplist itemDroplist;
        }
    }
}
