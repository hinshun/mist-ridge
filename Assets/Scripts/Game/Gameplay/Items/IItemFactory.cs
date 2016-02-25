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
            switch (itemType)
            {
                case ItemType.Star:
                    return container.Instantiate<StarItem>(constructorArgs);

                case ItemType.Speed:
                    return container.Instantiate<SpeedItem>(constructorArgs);
            }

            Debug.LogError("Failed to create valid IItem");
            return null;
        }
    }
}
