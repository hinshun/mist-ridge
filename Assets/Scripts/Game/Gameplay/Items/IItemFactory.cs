using UnityEngine;
using Zenject;

namespace MistRidge
{
    public class IItemFactory
    {
        private readonly DiContainer container;

        public IItemFactory(DiContainer container)
        {
            this.container = container;
        }

        public IItem Create(ItemType itemType, params object[] constructorArgs)
        {
            IItem item = null;

            switch (itemType)
            {
                case ItemType.Star:
                    item = container.Instantiate<StarItem>(constructorArgs);
                    break;

                case ItemType.Speed:
                    item = container.Instantiate<SpeedItem>(constructorArgs);
                    break;
            }

            if (item == null)
            {
                Debug.LogError("Failed to create valid IItem");
            }
            else
            {
                item.Initialize();
            }

            return item;
        }
    }
}
