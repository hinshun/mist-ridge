using UnityEngine;
using System;
using System.Collections.Generic;
using Zenject;

namespace MistRidge
{
    public class ItemManager : IInitializable
    {
        private readonly Settings settings;
        private readonly IItemPickingStrategy itemPickingStrategy;

        private Dictionary<ItemType, Item> itemMapping;

        public ItemManager(
                Settings settings,
                IItemPickingStrategy itemPickingStrategy)
        {
            this.settings = settings;
            this.itemPickingStrategy = itemPickingStrategy;
        }

        public void Initialize()
        {
            itemMapping = new Dictionary<ItemType, Item>();

            foreach(Item item in settings.itemList.Items)
            {
                if (item.ItemType == ItemType.Random)
                {
                    Debug.LogError("Item was not added because it is of type Random.");
                    continue;
                }

                if (!itemMapping.ContainsKey(item.ItemType))
                {
                    itemMapping[item.ItemType] = item;
                }
                else
                {
                    Debug.LogError("Item " + item.ItemType + " was not added because it has a duplicated type.");
                    continue;
                }
            }
        }

        public Item GetItem(ItemType itemType)
        {
            if (itemType == ItemType.Random)
            {
                return itemPickingStrategy.Pick(itemMapping);
            }

            return itemMapping[itemType];
        }

        [Serializable]
        public class Settings
        {
            public ItemList itemList;
        }
    }
}
