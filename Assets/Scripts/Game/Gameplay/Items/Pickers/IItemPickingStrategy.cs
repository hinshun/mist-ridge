using UnityEngine;
using System.Collections.Generic;

namespace MistRidge
{
    public interface IItemPickingStrategy
    {
        Item Pick(Dictionary<ItemType, Item> itemMapping);
    }
}
