using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Zenject;

namespace MistRidge
{
    public class RandomItemPickingStrategy : IItemPickingStrategy
    {
        private readonly Generator generator;

        public RandomItemPickingStrategy(Generator generator)
        {
            this.generator = generator;
        }

        public Item Pick(Dictionary<ItemType, Item> itemMapping)
        {
            List<Item> items = Enumerable.ToList(itemMapping.Values);
            int randomIndex = generator.Random.Next(items.Count);
            return items[randomIndex];
        }
    }
}
