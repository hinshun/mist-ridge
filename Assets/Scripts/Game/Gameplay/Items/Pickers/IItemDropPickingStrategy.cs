using UnityEngine;
using System.Collections.Generic;

namespace MistRidge
{
    public interface IItemDropPickingStrategy
    {
        ItemDrop Pick(Dictionary<ItemType, ItemDrop> itemDropMapping, Player player);
    }
}
