using UnityEngine;
using System;
using System.Collections.Generic;
using Zenject;

namespace MistRidge
{
    public class ItemManager : IInitializable, ITickable
    {
        private readonly Settings settings;
        private readonly IItemDropPickingStrategy itemDropPickingStrategy;
        private readonly IItemFactory itemFactory;

        private List<IItem> items;
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
            items = new List<IItem>();
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

        public void Tick()
        {
            List<IItem> itemsToDispose = new List<IItem>();

            foreach(IItem item in items)
            {
                if (item.IsDisposable())
                {
                    itemsToDispose.Add(item);
                    continue;
                }

                if (item.IsActive())
                {
                    item.Tick();
                }
            }

            foreach(IItem item in itemsToDispose)
            {
                item.Dispose();
                items.Remove(item);
            }
        }

        public IItem NewItem(ItemType itemType, Player player)
        {
            ItemDrop itemDrop = PickItemDrop(itemType);

            IItem item = itemFactory.Create(itemDrop.ItemType, player);
            items.Add(item);
            return item;
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
