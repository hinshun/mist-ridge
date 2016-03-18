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
                case ItemType.Aether:
                    item = container.Instantiate<AetherItem>(constructorArgs);
                    break;

                case ItemType.Quickness:
                    item = container.Instantiate<QuicknessItem>(constructorArgs);
                    break;

                case ItemType.BubbleTrap:
                    item = container.Instantiate<BubbleTrapItem>(constructorArgs);
                    break;

                case ItemType.TimeSlow:
                    item = container.Instantiate<TimeSlowItem>(constructorArgs);
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
