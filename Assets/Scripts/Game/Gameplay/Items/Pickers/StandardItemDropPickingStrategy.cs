using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Zenject;

namespace MistRidge
{
    public class StandardItemDropPickingStrategy : IItemDropPickingStrategy
    {
        private readonly Generator generator;

        public StandardItemDropPickingStrategy(Generator generator)
        {
            this.generator = generator;
        }

        public ItemDrop Pick(Dictionary<ItemType, ItemDrop> itemDropMapping, Player player)
        {
            int currentRank = player.CurrentRank;
            List<ItemDrop> itemDrops = Enumerable.ToList(itemDropMapping.Values);

            itemDrops = itemDrops
                .Where(itemDrop => (currentRank == 0 && itemDrop.ItemCollectability.firstCollectable)
                        || (currentRank == 1 && itemDrop.ItemCollectability.secondCollectable)
                        || (currentRank == 2 && itemDrop.ItemCollectability.thirdCollectable)
                        || (currentRank == 3 && itemDrop.ItemCollectability.fourthCollectable))
                .ToList();

            List<int> cdf = CreateCDF(itemDrops);
            int randomIndex = GetRandomIndex(cdf, itemDrops);
            return itemDrops[randomIndex];
        }

        private List<int> CreateCDF(List<ItemDrop> itemDrops)
        {
            List<int> dropsCDF = new List<int>();
            int sum = 0;

            foreach (ItemDrop itemDrop in itemDrops)
            {
                sum += itemDrop.Rarity;
                dropsCDF.Add(sum);
            }

            return dropsCDF;
        }

        private int GetRandomIndex(List<int> cdf, List<ItemDrop> itemDrops)
        {
            if (cdf.Count == 0)
            {
                Debug.LogError("Failed to pick item drop because item drops is empty");
                return 0;
            }

            int random = generator.Random.Next(cdf[cdf.Count - 1]);

            int randomIndex = 0;
            foreach (int probability in cdf)
            {
                if (probability > random)
                {
                    break;
                }

                randomIndex++;
            }

            if (randomIndex > itemDrops.Count - 1)
            {
                randomIndex = itemDrops.Count - 1;
            }

            return randomIndex;
        }
    }
}
