using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Zenject;

namespace MistRidge
{
    public class RandomItemDropPickingStrategy : IItemDropPickingStrategy
    {
        private readonly Generator generator;

        public RandomItemDropPickingStrategy(Generator generator)
        {
            this.generator = generator;
        }

        public ItemDrop Pick(Dictionary<ItemType, ItemDrop> itemDropMapping)
        {
            List<ItemDrop> itemDrops = Enumerable.ToList(itemDropMapping.Values);
            int randomIndex = generator.Random.Next(itemDrops.Count);
            return itemDrops[randomIndex];
        }
    }
}
